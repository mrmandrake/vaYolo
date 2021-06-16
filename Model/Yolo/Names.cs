using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using vaYolo.Ext;
using vaYolo.Helpers;
using vaYolo.Model.Yolo;

namespace vaYolo.Model.Yolo
{
    public class Names
    {
        public static List<ObjectClass> Classes = new();

        public static void Load(string folder)
        {
            List<ObjectClass> result = new();
            try
            {
                if (!File.Exists(Util.NamesPath(folder)))
                {
                    result.Add(new ObjectClass(0, "undefined"));
                    Classes = result;
                    Save(folder);
                }
                else
                {
                    var lines = File.ReadAllLines(Util.NamesPath(folder)).ToList();
                    int tmp = 0;
                    if (lines.Count > 0)
                        lines.ForEach((l) => result.Add(new ObjectClass(tmp++, l)));
                    else
                        result.Add(new ObjectClass(0, "undefined"));

                    Classes = result;
                }
            }
            catch (Exception exc)
            {
            }
        }

        public static string Save(string folder)
        {
            return GetNames().Save(Util.NamesPath(folder));
        }

        public static IEnumerable<string> GetNames()
        {
            return Classes.OrderBy(l => l._Class).Select(l => l.Description);
        }
    }
}