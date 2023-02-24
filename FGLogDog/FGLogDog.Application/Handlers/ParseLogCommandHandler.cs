using System.Text;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FGLogDog.Application.Commands;
using FGLogDog.Application.Helper;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FGLogDog.FGLogDog.Application.Helper;

namespace FGLogDog.Application.Handlers
{
    public class ParseLogCommandHandler : IRequestHandler<ParseLogCommand, Unit>
    {
        private readonly ILogger<ParseLogCommandHandler> _logger;
        private readonly IFilters _filters;

        public ParseLogCommandHandler(ILogger<ParseLogCommandHandler> logger,
                                      IFilters filters)
        {
            _logger = logger;
            _filters = filters;
        }

        public async Task<Unit> Handle(ParseLogCommand request,
                                       CancellationToken cancellationToken)
        {
            IDictionary<string, object> newObj = new Dictionary<string, object>();
            
            newObj = ParserFactory.GetParsedDictionary(request.message, _filters.Filter, _filters.Patterns);
            //newObj = ParserFactory.GetParsingDictionry(request.message, _filters.Filter);

            // for (int i = 0; i < _filters.Filter.Length; i++)
            // {
            //     var subValues = _filters[i].Split('=');
            //     StringBuilder subType = new StringBuilder(subValues[1]);
            //     StringBuilder subValue = new StringBuilder(subValues[0] + "=");
            //     ParserTypes type = (ParserTypes) Enum.Parse(typeof(ParserTypes), subType.Replace(";", "").ToString());
            //     object value;
            //     switch (type)
            //     {
            //         case ParserTypes.STRING:
            //             value = ParserFactory.SearchSubstringSTRING(request.message, subValue.ToString(), type);
            //             break;
            //         case ParserTypes.INT:
            //             value = ParserFactory.SearchSubstringINT(request.message, subValue.ToString(), type);
            //             break;
            //         case ParserTypes.IP:
            //             value = ParserFactory.SearchSubstringSTRING(request.message, subValue.ToString(), type);
            //             break;
            //         case ParserTypes.DATE:
            //             value = ParserFactory.SearchSubstringSTRING(request.message, subValue.ToString(), type);
            //             break;
            //         case ParserTypes.TIME:
            //             value = ParserFactory.SearchSubstringSTRING(request.message, subValue.ToString(), type);
            //             break;
            //         case ParserTypes.MAC:
            //             value = ParserFactory.SearchSubstringSTRING(request.message, subValue.ToString(), type);
            //             break;
            //         case ParserTypes.GUID:
            //             value = ParserFactory.SearchSubstringSTRING(request.message, subValue.ToString(), type);
            //             break;
            //         default:
            //             throw new ArgumentException("Invalid type of ParserTypes.");
            //     }
            //     neObj.Add(subValues[0], value);
            // }

            var ouput = JsonConvert.SerializeObject(newObj);
            System.Console.WriteLine(ouput);
            // File.WriteAllText("test.json", ouput);

            return await Task.FromResult(Unit.Value);
        }
    }
}