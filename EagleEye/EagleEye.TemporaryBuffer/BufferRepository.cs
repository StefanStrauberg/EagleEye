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
        readonly IBufferConfiguration _bufferConfiguration;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        readonly object _locker;
        readonly List<BsonDocument> _temp;

        public BufferRepository(IAppLogger<BufferRepository> logger,
                                IBufferConfiguration bufferConfiguration,
                                IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _index = bufferConfiguration.CollectionNameIndex;
            _serviceScopeFactory = serviceScopeFactory;
            _bufferConfiguration = bufferConfiguration;
            _locker = new();
            _temp = new(bufferConfiguration.SizeOfBuffer);
        }

        void IBufferRepository.PushToBuffer(byte[] bytes)
        {
            lock(_locker)
            {
                if (_temp.Count == _bufferConfiguration.SizeOfBuffer)
                    ((IBufferRepository)this).ClearBuffer();
                _temp.Add(BsonSerializer.Deserialize<BsonDocument>(bytes));
            }
        }

        void IBufferRepository.ClearBuffer()
        {
            lock(_locker)
            {
                string index = $"{_index}_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}";
                _logger.LogInformation("EagleEye started migration the buffer data to Database");
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var service = scope.ServiceProvider.GetService<ICollectionRepository>();
                    service.InsertMany(index, _temp);
                }
                _logger.LogInformation("EagleEye finished migration the buffer data to Database");
                _temp.Clear();
            }
        }

        int IBufferRepository.CountItems()
        {
            lock(_locker)
            {
                return _temp.Count;
            }
        }
    }
}