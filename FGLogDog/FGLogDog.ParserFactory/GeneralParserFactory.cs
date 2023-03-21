using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FGLogDog.Application.Contracts.Filter;
using FGLogDog.Application.Models;
using FGLogDog.ParserFactory.Helpers;
using MongoDB.Bson;

namespace FGLogDog.ParserFactory
{
    internal class GeneralParserFactory : IParserFactory
    {
        readonly IConfigurationFilters _filters;

        public GeneralParserFactory(IConfigurationFilters filters)
            => _filters = filters;

        byte[] IParserFactory.ParsingMessage(byte[] bytes)
        {
            string message = Encoding.UTF8.GetString(bytes);

            if(string.IsNullOrEmpty(message))
                return null;

            BsonDocument bsonDoc = new();
            MatchCollection matches;
            string inputString;

            for (int i = 0; i < _filters.SearchableSubStrings.Length; i++)
            {
                matches = Regex.Matches(message, _filters.SearchableSubStrings[i]);
                if (matches.Count > 0)
                    inputString = matches.First().Value;
                else
                    continue;

                switch (_filters.FilterPatterns[i])
                {
                    case ParserTypes.INT:
                        matches = Regex.Matches(inputString, RegexHelper._int);
                        if (matches.Count > 0)
                            if (int.TryParse(matches.First().Value, out var data))
                                bsonDoc.Add(new BsonElement(_filters.FilterKeys[i],
                                                            data));
                        break;
                    case ParserTypes.TIME:
                        matches = Regex.Matches(inputString, RegexHelper._time);
                        if (matches.Count > 0)
                            if (TimeOnly.TryParse(matches.First().Value, out var data))
                                bsonDoc.Add(new BsonElement(_filters.FilterKeys[i],
                                                            data.ToString()));
                        break;
                    case ParserTypes.DATE:
                        matches = Regex.Matches(inputString, RegexHelper._date);
                        if (matches.Count > 0)
                            if (DateOnly.TryParse(matches.First().Value, out var data))
                                bsonDoc.Add(new BsonElement(_filters.FilterKeys[i],
                                                            data.ToString()));
                        break;
                    case ParserTypes.STRING:
                        matches = Regex.Matches(inputString, RegexHelper._string);
                        if (matches.Count > 0)
                            bsonDoc.Add(new BsonElement(_filters.FilterKeys[i],
                                                        matches[2].Value));
                        break;
                    case ParserTypes.GUID:
                        matches = Regex.Matches(inputString, RegexHelper._guid);
                        if (matches.Count > 0)
                            if (Guid.TryParse(matches.First().Value, out var data))
                                bsonDoc.Add(new BsonElement(_filters.FilterKeys[i],
                                                            new BsonBinaryData(data,
                                                                               GuidRepresentation.Standard)));
                        break;
                    case ParserTypes.MAC:
                        matches = Regex.Matches(inputString, RegexHelper._mac);
                        if (matches.Count > 0)
                            bsonDoc.Add(new BsonElement(_filters.FilterKeys[i],
                                                        matches.First().Value));
                        break;
                    case ParserTypes.IP:
                        matches = Regex.Matches(inputString, RegexHelper._ip);
                        if (matches.Count > 0)
                            bsonDoc.Add(new BsonElement(_filters.FilterKeys[i],
                                                        matches.First().Value));
                        break;
                    case ParserTypes.DATETIME:
                        StringBuilder temp = new();
                        matches = Regex.Matches(inputString, RegexHelper._date);
                        if (matches.Count > 0)
                            if (DateOnly.TryParse(matches.First().Value, out var date))
                                temp.Append(date);
                        else
                            break;
                        temp.Append(' ');
                        matches = Regex.Matches(inputString, RegexHelper._time);
                        if (matches.Count > 0)
                            if (TimeOnly.TryParse(matches.First().Value, out var time))
                                temp.Append(time);
                        else
                            break;
                        if (DateTime.TryParse(temp.ToString(), out DateTime dateTime))
                            bsonDoc.Add(new BsonElement(_filters.FilterKeys[i],
                                                        dateTime));
                        break;
                    default:
                        throw new ArgumentException("Invalid incoming data of ParserTypes.");
                }
            }

            if (bsonDoc.ElementCount == 0)
                return null;

            return bsonDoc.ToBson();
        }
    }
}