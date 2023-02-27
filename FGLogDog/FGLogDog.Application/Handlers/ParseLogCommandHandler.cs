using System.Threading;
using System.Threading.Tasks;
using FGLogDog.Application.Commands;
using FGLogDog.Application.Helper;
using MediatR;
using FGLogDog.FGLogDog.Application.Helper;
using System.Text.Json.Nodes;
using System;

namespace FGLogDog.Application.Handlers
{
    public class ParseLogCommandHandler : IRequestHandler<ParseLogCommand, Unit>
    {
        private readonly IConfigurationFilters _filters;

        public ParseLogCommandHandler(IConfigurationFilters filters)
            => _filters = filters;

        public async Task<Unit> Handle(ParseLogCommand request,
                                       CancellationToken cancellationToken)
        {
            JsonObject outputObject = ParserFactory.GetParsedDictionary(request.inputLog, _filters);

            Console.WriteLine(outputObject);

            return await Task.FromResult(Unit.Value);
        }
    }
}