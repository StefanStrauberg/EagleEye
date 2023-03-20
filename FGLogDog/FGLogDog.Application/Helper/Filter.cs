using System.Text;
namespace FGLogDog.Application.Helper
{
    public static class Filter
    {
        private static byte[] _common;

        public static void InitFilter(ICommonFilter filters)
            => _common = Encoding.UTF8.GetBytes(filters.Common);

        /// <summary>
        /// Check if input string contain filter substring from CommonFilter.
        /// If contain return true. Otherwise return false. 
        /// </summary>
        /// <param name="inputArray">input byte array for checking</param>
        /// <returns></returns>
        public static bool Contain(byte[] inputArray)
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