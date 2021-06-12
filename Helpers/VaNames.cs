using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using vaYolo.Models;

namespace vaYolo
{
    public class VaNames
    {
        private static List<vaClass> Classes = new ();

        private static string NamesPath(string folder) {
            return Path.Combine(folder, new DirectoryInfo(folder).Name + ".names");
        }

        public static void Load(string folder) {
            List<vaClass> result = new ();
            try {    
                if (!File.Exists(NamesPath(folder))) {
                    result.Add(new vaClass() { 
                            Class = 0,
                            Description = "Undefined"
                        });

                    Save(NamesPath(folder));
                }
                else {
                    var lines = File.ReadAllLines(NamesPath(folder)).ToList();
                    int tmp = 0;
                    lines.ForEach((l) => {
                        result.Add(new vaClass() {
                            Class = tmp++,
                            Description = l
                        });
                    });
                }
            }
            catch (Exception exc) {
            }

            Classes = result;            
        }

        public static IEnumerable<string> GetNames() {
            return Classes.OrderBy(l => l.Class).Select(l => l.Description);
        }

        public static void Save(string namesPath) {
            File.WriteAllLines(namesPath, GetNames());
        }
    }
}