namespace FGLogDog.Application.Contracts.Filter
{
    /// <summary>
    /// Interface for converting inciming bytes to bson bytes
    /// </summary>
    public interface IParserFactory
    {
        /// <summary>
        /// Parsing incoming bytes
        /// </summary>
        /// <param name="bytes">Inciming bytes</param>
        /// <returns>Bson bytes</returns>
        byte[] ParsingMessage(byte[] bytes);
    }
}