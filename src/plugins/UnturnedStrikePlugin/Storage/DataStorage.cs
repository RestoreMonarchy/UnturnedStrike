using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnturnedStrike.Plugin.Storage
{
    public class DataStorage<T> where T : class
    {
        public string DataPath { get; private set; }
        public DataStorage(string dir, string fileName)
        {
            DataPath = Path.Combine(dir, fileName);
        }

        public void Save(T obj)
        {
            string objData = JsonConvert.SerializeObject(obj, Formatting.Indented);

            using (StreamWriter stream = new StreamWriter(DataPath, false))
            {
                stream.Write(objData);
            }
        }

        public T Read()
        {
            if (File.Exists(DataPath))
            {
                string dataText;
                using (StreamReader stream = File.OpenText(DataPath))
                {
                    dataText = stream.ReadToEnd();
                }
                return JsonConvert.DeserializeObject<T>(dataText);
            }
            else
            {
                return null;
            }
        }
    }
}
