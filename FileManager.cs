using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace TaskList
{
    public static class FileManager
    {
        public static List<Objective> DeserializeData(string fileName)
        {
            if (File.Exists(fileName))
            {
                string fileData = File.ReadAllText(fileName);

                if(fileData.Length != 0)
                    return JsonConvert.DeserializeObject<List<Objective>>(fileData);
            }

            return new List<Objective>();
        }

        public static void SerializeData(List<Objective> data, string fileName)
        {
            if(!File.Exists(fileName))
            {
                File.WriteAllLines(fileName, new string[] {
                    "[",
                    "]"
                });
            }

            StreamWriter wr = new StreamWriter(fileName);
            wr.Write(JsonConvert.SerializeObject(data, Formatting.Indented));
            wr.Close();
        }
    }
}
