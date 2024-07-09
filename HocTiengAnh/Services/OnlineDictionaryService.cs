using HocTiengAnh.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace HocTiengAnh.Services
{
    public class OnlineDictionaryService
    {
        private LogService logService;

        private readonly string xpathUSIPA = "//div[@class='fl word_tab_title word_tab_title_0']//span[@class='color-orange']";
        
        private readonly string xpathIPA = "//div[@class='fl word_tab_title word_tab_title_0']//span[@class='color-black']";

        public OnlineDictionaryService(LogService logService)
        {
            this.logService = logService;
        }

        public string GetIPA(string word)
        {
            string url = $"https://dict.laban.vn/find?type=1&query={word}";
            logService.Debug($"get IPA url: {url}");
            try
            {
                string htmlContent = HtmlHelper.GetContent(url);
                var html = HtmlHelper.Convert(htmlContent);
                var node = html.DocumentNode.SelectSingleNode(xpathUSIPA);
                if (!string.IsNullOrEmpty(node!.InnerText.Trim()))
                {
                    return node.InnerText.Trim();
                };
                node = html.DocumentNode.SelectSingleNode(xpathIPA);
                string ipa = node.InnerText.Trim();
                logService.Info($"{word} : {ipa}");
                return ipa;
            }
            catch (Exception ex)
            {
                logService.Error(ex.ToString());
                return null!;
            }
        }


        public bool DownloadSound(string word)
        {  
            try
            {
                string soundDataUrl = $"https://dict.laban.vn/ajax/getsound?accent=us&word={word}";
                logService.Debug($"download sound info url: {soundDataUrl}");
                string htmlContent = HtmlHelper.GetContent(soundDataUrl);
                logService.Info($"response: {htmlContent}");
                var jobject = JsonConvert.DeserializeObject<JObject>(htmlContent);
                if(jobject["error"].ToString().Trim() == "-1")
                {
                    logService.Error("try to get uk sound");
                    soundDataUrl = $"https://dict.laban.vn/ajax/getsound?accent=uk&word={word}";
                    logService.Debug($"download sound info url: {soundDataUrl}");
                    htmlContent = HtmlHelper.GetContent(soundDataUrl);
                    logService.Info($"response: {htmlContent}");
                    jobject = JsonConvert.DeserializeObject<JObject>(htmlContent);
                    if (jobject["error"].ToString().Trim() == "-1")
                    {
                        return false;
                    }
                }
                string downloadMp3Url = jobject["data"].ToString();
                string soundPath = Directory.GetCurrentDirectory() + $"\\Sounds\\{word}.mp3";
                HtmlHelper.Download(downloadMp3Url, soundPath);
                return true;
            }
            catch (Exception ex)
            {
                logService.Error(ex.ToString());
                return false;
            }
        }
    }
}
