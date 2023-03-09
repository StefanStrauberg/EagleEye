using System.Threading;
using System.Threading.Tasks;
using FGLogDog.Application.Helper;
using MediatR;
using FGLogDog.FGLogDog.Application.Helper;
using System.Text.Json.Nodes;
using System;
using FGLogDog.Application.Features.Commands;

namespace FGLogDog.Application.Features.Handlers
{
    internal class ParseLogCommandHandler : IRequestHandler<ParseLogCommand, Unit>
    {
        private readonly IConfigurationFilters _filters;

        public ParseLogCommandHandler(IConfigurationFilters filters)
            => _filters = filters;

        public async Task<Unit> Handle(ParseLogCommand request,
                                       CancellationToken cancellationToken)
        {
            JsonObject outputObject = ParserFactory.GetJsonFromMessage(request.inputLog, _filters);

            if (outputObject is not null)
                Console.WriteLine(outputObject);

            return await Task.FromResult(Unit.Value);
        }
    }
}