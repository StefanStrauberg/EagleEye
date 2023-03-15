using MongoDB.Bson;

namespace FGLogDog.FGLogDog.Application.Helper
{
    public delegate void ParserDelegate(string message);
    public delegate BsonDocument ProducerDelegate();
}