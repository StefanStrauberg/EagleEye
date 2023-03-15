using MongoDB.Bson;

namespace FGLogDog.Application.Contracts.Commands
{
    internal interface IParserManager
    {
        void ParseLogAndBufferize(string inputLog);
    }
}
