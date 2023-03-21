using FGLogDog.Application.Contracts.TemporaryBuffer;
using FGLogDog.TemporaryBuffer.Models;

namespace FGLogDog.TemporaryBuffer
{
    public class BufferRepository : IBufferRepository 
    {
        byte[] IBufferRepository.PullFromBuffer()
            => GeneralBuffer.buffer.Take();

        void IBufferRepository.PushToBuffer(byte[] bytes)
            => GeneralBuffer.buffer.Add(bytes);
    }
}