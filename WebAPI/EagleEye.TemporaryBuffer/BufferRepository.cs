using System;
using System.Collections.Generic;
using EagleEye.Application.Contracts.Logger;
using EagleEye.Application.Contracts.TemporaryBuffer;
using EagleEye.TemporaryBuffer.Config;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using WebAPI.EagleEye.Application.Contracts.Persistence;

namespace EagleEye.TemporaryBuffer
{
    internal class BufferRepository : IBufferRepository
    {
        readonly IAppLogger<BufferRepository> _logger;
        readonly IBufferConfiguration _bufferConfiguration;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        static List<byte[]> _buffer;
        object _locker;
        int _cursor;
        List<BsonDocument> _list;

        public BufferRepository(IAppLogger<BufferRepository> logger,
                                IBufferConfiguration bufferConfiguration,
                                IServiceScopeFactory serviceScopeFactory)
        {
            _locker = new();
            _cursor = 0;
            _list = new(bufferConfiguration.SizeOfBuffer);
            _logger = logger;
            _bufferConfiguration = bufferConfiguration;
            _buffer = new List<byte[]>(bufferConfiguration.SizeOfBuffer);
            _serviceScopeFactory = serviceScopeFactory;
        }

        void IBufferRepository.PushToBuffer(byte[] bytes)
        {
            lock(_locker)
            {
                _buffer.Add(bytes);
                _cursor++;
                if (_cursor == _buffer.Capacity)
                    ClearBuffer();
            }
        }

        async void ClearBuffer()
        {
            foreach (var item in _buffer)
            {
                BsonDocument bsonDoc = BsonSerializer.Deserialize<BsonDocument>(item);
                _list.Add(bsonDoc);
            }
            _buffer.Clear();
            string index = $"{_bufferConfiguration.CollectionNameIndex}_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}";
            _logger.LogInformation("Migrate buffer data to Database");
            using(var scope = _serviceScopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<ICollectionRepository>();
                await service.InsertManyAsync(index, _list);
            }
            _logger.LogInformation("The buffer has been migrated");
            _list.Clear();
            _cursor = 0;
        }
    }
}