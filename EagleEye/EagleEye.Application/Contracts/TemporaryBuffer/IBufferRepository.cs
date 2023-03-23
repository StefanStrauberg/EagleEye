namespace EagleEye.Application.Contracts.TemporaryBuffer
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
        /// Migrate buffer data to Database
        /// </summary>
        void ClearBuffer();
        /// <summary>
        /// Return of count items in the buffer
        /// </summary>
        /// <returns></returns>
        int CountItems();
    }
}