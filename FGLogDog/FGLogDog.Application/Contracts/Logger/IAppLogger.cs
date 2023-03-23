namespace FGLogDog.Application.Contracts.Logger
{
    /// <summary>
    /// General logging interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAppLogger<T>
    {
        /// <summary>
        /// logging information event
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="args">Arguments for concatenate strings</param>
        void LogInformation(string message, params object[] args);
        /// <summary>
        /// logging warning event
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="args">Arguments for concatenate strings</param>
        void LogWarning(string message, params object[] args);
        /// <summary>
        /// logging error event
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="args">Arguments for concatenate strings</param>
        void LogError(string message, params object[] args);
    }
}