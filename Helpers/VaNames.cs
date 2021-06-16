using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using vaYolo.Ext;
using vaYolo.Helpers;
using vaYolo.Model;

namespace vaYolo
{
    public class VaNames
    {
        public static List<VaClass> Classes = new();

        public static void Load(string folder)
        {
            List<VaClass> result = new();
            try
            {
                if (!File.Exists(VaUtil.NamesPath(folder)))
                {
                    result.Add(new VaClass()
                    {
                        ObjectClass = 0,
                        Description = "Undefined"
                    });

                    Classes = result;
                    Save(folder);
                }
                else
                {
                    var lines = File.ReadAllLines(VaUtil.NamesPath(folder)).ToList();
                    int tmp = 0;
                    if (lines.Count > 0)
                        lines.ForEach((l) =>
                        {
                            result.Add(new VaClass()
                            {
                                ObjectClass = tmp++,
                                Description = l
                            });
                        });
                    else
                        result.Add(new VaClass()
                        {
                            ObjectClass = 0,
                            Description = "Undefined"
                        });

                    Classes = result;
                }
            }
            catch (Exception exc)
            {
            }
        }

        public static string Save(string folder)
        {
            return GetNames().Save(VaUtil.NamesPath(folder));
        }

        public static IEnumerable<string> GetNames()
        {
            return Classes.OrderBy(l => l.ObjectClass).Select(l => l.Description);
        }
    }
}