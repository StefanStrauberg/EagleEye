using EagleEye.Application.Contracts.Logger;
using EagleEye.Application.Contracts.TemporaryBuffer;
using EagleEye.TemporaryBuffer.Config;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using WebAPI.EagleEye.Application.Contracts.Persistence;

namespace EagleEye.TemporaryBuffer
{
    internal class BufferRepository : IBufferRepository
    {
        readonly IAppLogger<BufferRepository> _logger;
        readonly string _index;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        readonly List<BsonDocument> _buffer;
        readonly object _locker;
        int _cursor;

        public BufferRepository(IAppLogger<BufferRepository> logger,
                                IBufferConfiguration bufferConfiguration,
                                IServiceScopeFactory serviceScopeFactory)
        {
            _locker = new();
            _cursor = 0;
            _logger = logger;
            _index = bufferConfiguration.CollectionNameIndex;
            _buffer = new(bufferConfiguration.SizeOfBuffer);
            _serviceScopeFactory = serviceScopeFactory;
        }

        void IBufferRepository.PushToBuffer(byte[] bytes)
        {
            lock(_locker)
            {
                _buffer.Add(BsonSerializer.Deserialize<BsonDocument>(bytes));
                _cursor++;
                if (_cursor == _buffer.Capacity)
                    ((IBufferRepository)this).ClearBuffer();
            }
        }

        void IBufferRepository.ClearBuffer()
        {
            lock(_locker)
            {
                string index = $"{_index}_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}";
                _logger.LogInformation("Migrate buffer data to Database");
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var service = scope.ServiceProvider.GetService<ICollectionRepository>();
                    service.InsertMany(index, _buffer);
                }
                _logger.LogInformation("The buffer has been migrated");
                _cursor = 0;
                _buffer.Clear();
            }
        }

        int IBufferRepository.CountItems()
        {
            lock(_locker)
            {
                return _buffer.Count;
            }
        }
    }
}