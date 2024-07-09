using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace HocTiengAnh.Helpers
{
    public static class HtmlHelper
    {
        public static string GetContent(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("gzip"));
                var response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                using (var responseStream = response.Content.ReadAsStreamAsync().Result)
                using (var decompressedStream = new GZipStream(responseStream, CompressionMode.Decompress))
                using (var reader = new StreamReader(decompressedStream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static void Download(string url,string SaveFilePath)
        {
            string dir = Path.GetDirectoryName(SaveFilePath);
            if(!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            using (HttpClient client = new HttpClient())
            {
                var bytes = client.GetByteArrayAsync(url).Result;
                File.WriteAllBytes(SaveFilePath, bytes);
            }
        }

        public static HtmlDocument Convert(string hmtlContent)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(hmtlContent);
            return document;
        }
    }
}
