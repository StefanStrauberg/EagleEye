using System.Threading;
using System.Threading.Tasks;
using FGLogDog.Application.Queries;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FGLogDog.Application.Handlers
{
    public class ParseFGLogHandler : IRequestHandler<ParseFGLogQuery, Unit>
    {
        private readonly ILogger<ParseFGLogHandler> _logger;
        private readonly string _filter;

        public ParseFGLogHandler(ILogger<ParseFGLogHandler> logger,
                                 IConfiguration configuration)
        {
            _logger = logger;
            _filter = configuration.GetSection("ConfigurationString").GetSection("Filter").Value;
        }

        public async Task<Unit> Handle(ParseFGLogQuery request,
                                         CancellationToken cancellationToken)
        {
            _logger.LogInformation(_filter);
            _logger.LogInformation(request.message);
            return await Task.FromResult(Unit.Value);
        }
    }
}