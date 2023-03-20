namespace FGLogDog.Application.Contracts.Buffer
{
    internal class BufferRepository : IBufferRepository
    {
        byte[] IBufferRepository.PullFromBuffer()
            => Models.Buffer.buffer.Take();

        void IBufferRepository.PushToBuffer(byte[] bytes)
            => Models.Buffer.buffer.Add(bytes);
    }
}