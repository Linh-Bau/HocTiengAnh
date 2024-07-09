using HocTiengAnh.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HocTiengAnh.Services
{
    public class ConfigureService 
    {
        private readonly string configurePath = Directory.GetCurrentDirectory() + "\\AppConfig.json";
        public AppConfig AppConfig { get; set; }
        public ConfigureService() 
        {

        }

        public void Load()
        {
            try
            {
                string context = File.ReadAllText(configurePath);
                AppConfig = JsonConvert.DeserializeObject<AppConfig>(context) ?? throw new Exception();
            }
            catch
            {
                AppConfig = new AppConfig();
            }
        }

        public void Save()
        {
            string context = JsonConvert.SerializeObject(AppConfig, Formatting.Indented);
             File.WriteAllText(configurePath, context);
        }
    }
}
