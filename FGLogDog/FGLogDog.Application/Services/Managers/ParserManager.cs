using FGLogDog.Application.Contracts.Commands;
using FGLogDog.Application.Helper;
using FGLogDog.Application.Models;
using FGLogDog.FGLogDog.Application.Helper;

namespace FGLogDog.Application.Services.Managers
{
    internal class ParserManager : IParserManager
    {
        readonly IConfigurationFilters _filters;

        public ParserManager(IConfigurationFilters filters)
            => _filters = filters;

        void IParserManager.ParseLogAndBufferize(string inputLog)
        {
            var document = ParserFactory.GetJsonFromMessage(inputLog, _filters);
            while (!Buffer.buffer.TryAdd(document))
            { }
        }
    }
}
