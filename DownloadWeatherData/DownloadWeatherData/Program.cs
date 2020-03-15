using System;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace DownloadWeatherData
{
    class Program
    {
        static string _parameterPath = ".\\Parameters\\Urls.txt";
        static string _clientRoot = "D:\\weather_data";
        static string _serverRoot = "http://data.wxbc.jp/basic_data/kansoku/";

        static void Main(string[] args)
        {
            foreach (var url in GetUrls())
            {
                if (!string.IsNullOrWhiteSpace(url) && !url.StartsWith("//"))
                {
                    if (Download(url))
                    {
                        var logText = $"{DateTime.Now.ToString("HH:mm:ss.fff")}   {url}";
                        Console.WriteLine(logText);
                    }
                }
            }
        }

        private static IEnumerable<string> GetUrls()
        {
            var allText = "";
            using (var reader = new StreamReader(_parameterPath))
            {
                allText = reader.ReadToEnd();
            }
            return allText.Split("\r\n");
        }

        private static bool Download(string url)
        {
            var subDir = url.Replace(_serverRoot, "").Replace("/", "\\");
            var localPath = Path.Combine(_clientRoot, subDir);
            var fileInfo = new FileInfo(localPath);
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }
            //有効なファイルがもうあればダウンロードしない
            if (fileInfo.Exists && fileInfo.Length > 0) { return false; }
            var wc = new WebClient();
            wc.DownloadFile(new Uri(url), localPath);

            return true;
        }
    }
}
