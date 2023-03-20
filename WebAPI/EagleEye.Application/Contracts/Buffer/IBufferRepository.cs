namespace EagleEye.Application.Contracts.Buffer
{
    /// <summary>
    /// Main interface that defines methods for communicating with the buffer
    /// </summary>
    public interface IBufferRepository
    {
        /// <summary>
        /// Insert an item into the buffer
        /// </summary>
        /// <param name="bytes"></param>
        void PushToBuffer(byte[] bytes);
        /// <summary>
        /// Take an item from the buffer
        /// </summary>
        /// <returns></returns>
        byte[] PullFromBuffer();
    }
}