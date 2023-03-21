using System.Collections.Generic;
using EagleEye.Application.Contracts.Logger;
using EagleEye.Application.Contracts.TemporaryBuffer;
using EagleEye.TemporaryBuffer.Config;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace EagleEye.TemporaryBuffer
{
    internal class BufferRepository : IBufferRepository
    {
        readonly IAppLogger<BufferRepository> _logger;
        readonly IBufferConfiguration _bufferConfiguration;
        static List<byte[]> _buffer;
        object _locker;
        int _cursor;
        List<BsonDocument> _list;

        public BufferRepository(IAppLogger<BufferRepository> logger,
                                IBufferConfiguration bufferConfiguration)
        {
            _locker = new();
            _cursor = 0;
            _list = new(bufferConfiguration.SizeOfBuffer);
            _logger = logger;
            _bufferConfiguration = bufferConfiguration;
            _buffer = new(bufferConfiguration.SizeOfBuffer);
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

        void ClearBuffer()
        {
            foreach (var item in _buffer)
            {
                BsonDocument bsonDoc = BsonSerializer.Deserialize<BsonDocument>(item);
                _list.Add(bsonDoc);
            }
            _buffer.Clear();
            foreach (var item in _list)
            {
                _logger.LogInformation(item.ToJson());
            }
            _list.Clear();
            _cursor = 0;
        }
    }
}