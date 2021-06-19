using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vaYolo.Ext
{
    public static class StringExtension
    {
        public static List<string> SplitByLength(this string str, int chunkSize = 200)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize)).ToList();
        }   

        public static IEnumerable<string> SplitBy(this string str, int chunkLength = 200)
        {
            if (String.IsNullOrEmpty(str)) throw new ArgumentException();
            if (chunkLength < 1) throw new ArgumentException();

            for (int i = 0; i < str.Length; i += chunkLength)
            {
                if (chunkLength + i > str.Length)
                    chunkLength = str.Length - i;

                yield return str.Substring(i, chunkLength);
            }
        }             
    }
}


    