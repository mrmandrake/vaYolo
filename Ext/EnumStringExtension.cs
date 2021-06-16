using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vaYolo.Ext
{
    public static class EnumerableStringExtension
    {
        public static string Save(this IEnumerable<string> en, string path)
        {
            File.WriteAllLines(path, en);
            return path;
        }

        public static IEnumerable<string> Rebase(this IEnumerable<string> en, string oldDir, string newDir)
        {
            return (from e in en select Path.Combine(newDir, Path.GetRelativePath(oldDir, e).Replace(@"\", "/")));
        }
    }
}
