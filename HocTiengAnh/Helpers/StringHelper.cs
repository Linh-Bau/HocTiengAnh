using HocTiengAnh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HocTiengAnh.Helpers
{
    public static class StringHelper
    {
        public static EngWord ConvertStringToEngWord(string data)
        {
            Regex r = new Regex(@"(?<word>.+)(?<type>\(.+\))\t(?<meaning>.+)");
            var match = r.Match(data);
            if(!match.Success)
            {
                throw new ArgumentException($"convert data to EngWord false, data: {data}");
            }
            var engWord = new EngWord();
            engWord.Word = match.Groups["word"].Value.ToLower().Trim();
            engWord.Type = match.Groups["type"].Value.ToLower().Trim(); ;
            engWord.Meaning = match.Groups["meaning"].Value.ToLower().Trim();
            return engWord;
        }
    }
}