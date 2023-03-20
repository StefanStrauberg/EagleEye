namespace FGLogDog.Application.Contracts.Logger
{
    /// <summary>
    /// Logging interface 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAppLogger<T>
    {
        /// <summary>
        /// logging information event
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        void LogInformation(string message, params object[] args);
        /// <summary>
        /// logging warning event
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        void LogWarning(string message, params object[] args);
        /// <summary>
        /// logging error event
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        void LogError(string message, params object[] args);
    }
}