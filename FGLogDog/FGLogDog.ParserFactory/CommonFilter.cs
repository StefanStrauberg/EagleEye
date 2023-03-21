using System.Text;
using FGLogDog.Application.Contracts.Filter;
using Microsoft.Extensions.Configuration;

namespace FGLogDog.ParserFactory
{
    internal class CommonFilter : ICommonFilter
    {
        readonly byte[] _common;

        public CommonFilter(IConfiguration configuration)
            => _common = Encoding.UTF8.GetBytes(configuration.GetSection("ServiceConfiguration")
                                                             .GetSection("CommonFilter")
                                                             .Value);

        bool ICommonFilter.Contain(byte[] inputArray)
        {
            int i = 0, j = 0;
            while (i < inputArray.Length - 1 && j < _common.Length - 1)
            {
                if (inputArray[i] == _common[j])
                {
                    i++;
                    j++;
                    if (j == _common.Length - 1)
                        return true;
                }
                else
                {
                    i = i - j + 1;
                    j = 0;
                }
            }
            return false;
        }
    }
}