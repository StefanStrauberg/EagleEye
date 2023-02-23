using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FGLogDog.Application.Commands;
using FGLogDog.Application.Helper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FGLogDog.Application.Handlers
{
    public class ParseLogCommandHandler : IRequestHandler<ParseLogCommand, Unit>
    {
        private readonly ILogger<ParseLogCommandHandler> _logger;
        private readonly string _filter;

        public ParseLogCommandHandler(ILogger<ParseLogCommandHandler> logger,
                                 IConfiguration configuration)
        {
            _logger = logger;
            _filter = configuration.GetSection("ConfigurationString").GetSection("Filter").Value;
        }

        public async Task<Unit> Handle(ParseLogCommand request,
                                         CancellationToken cancellationToken)
        {
            var subStrings = _filter.Split(' ');
            IDictionary<string, object> neObj = new Dictionary<string, object>();
            
            for (int i = 0; i < subStrings.Length; i++)
            {
                var subValues = subStrings[i].Split('=');
                System.Console.WriteLine($"subValues1:{subValues[0]}, subValues2:{subValues[1]}");
                ParserTypes type = (ParserTypes) Enum.Parse(typeof(ParserTypes), subValues[1]);
                object value;
                switch (type)
                {
                    case ParserTypes.STRING:
                        value = ParserFactory.GetSubstringSTRING(request.message, subValues[0] + "=", type);
                        break;
                    case ParserTypes.INT:
                        value = ParserFactory.GetSubstringINT(request.message, subValues[0] + "=", type);
                        break;
                    case ParserTypes.IP:
                        value = ParserFactory.GetSubstringSTRING(request.message, subValues[0] + "=", type);
                        break;
                    case ParserTypes.DATE:
                        value = ParserFactory.GetSubstringSTRING(request.message, subValues[0] + "=", type);
                        break;
                    case ParserTypes.TIME:
                        value = ParserFactory.GetSubstringSTRING(request.message, subValues[0] + "=", type);
                        break;
                    case ParserTypes.MAC:
                        value = ParserFactory.GetSubstringSTRING(request.message, subValues[0] + "=", type);
                        break;
                    default:
                        throw new ArgumentException("Invalid type of ParserTypes.");
                }
                neObj.Add(subValues[0], value);
            }

            var ouput = JsonConvert.SerializeObject(neObj);
            File.WriteAllText("test.json", ouput);

            System.Console.WriteLine(ouput);

            return await Task.FromResult(Unit.Value);
        }
    }
}