using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterTrendsForm
{
    public class JsonFile
    {
        public static string path;

        public JsonFile(string fileName)
        {
            path = fileName;
        }

        public List<State> GetStates()
        {
            Dictionary<string, List<object>[,]> data = JsonConvert.DeserializeObject<Dictionary<string, List<object>[,]>>(File.ReadAllText(path));

            List<State> states = (data).Select(x => new State
            {
                Name = x.Key,
                Coordinates = ConvertCoordinates(x.Value)
            }).ToList();

            return states;
        }
        public List<object>[,] ConvertCoordinates(List<object>[,] xv)
        {
            foreach (var coos in xv)
            {
                for (int i = 0; i < coos.Count; i++)
                {
                    if (coos[i] is double)
                    {
                        return xv;
                    }
                    else
                    {
                        coos[i] = ((JArray)coos[i]).Select(c => (double)c).ToList();
                    }
                }
            }
            return xv;
        }
    }
}
