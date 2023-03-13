using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace FGLogDog.FGLogDog.Application.Helper
{
    public delegate Task ParserDelegate(string message);
    public delegate Task<JsonObject> ProducerDelegate();
}