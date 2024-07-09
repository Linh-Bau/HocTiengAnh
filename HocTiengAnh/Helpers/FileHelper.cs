using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HocTiengAnh.Helpers
{
    public static class FileHelper
    {
        public static void SaveObjectAsJson(string path, object data)
        {
            string context= JsonConvert.SerializeObject(data, Formatting.Indented);
            string dir = Path.GetDirectoryName(path)!;
            if(!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.WriteAllText(path, context);
        }
    }
}
