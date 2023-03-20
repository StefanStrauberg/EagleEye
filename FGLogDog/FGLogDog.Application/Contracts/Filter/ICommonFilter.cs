namespace FGLogDog.Application.Contracts.Filter
{
    /// <summary>
    /// Interface for checking the input array for pattern matching
    /// </summary>
    public interface ICommonFilter
    {
        /// <summary>
        /// Check if input string contain filter substring from CommonFilter.
        /// If contain return true. Otherwise return false. 
        /// </summary>
        /// <param name="inputArray">input byte array for checking</param>
        /// <returns></returns>
        bool Contain(byte[] inputArray);
    }
}