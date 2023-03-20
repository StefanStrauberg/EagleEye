namespace FGLogDog.Application.Contracts.Parser
{
    /// <summary>
    /// Interface for parsing an incimong bytes to the bson bytes
    /// </summary>
    public interface IParserFactory
    {
        /// <summary>
        /// Parsing incoming bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        byte[] ParsingMessage(byte[] bytes);
    }
}