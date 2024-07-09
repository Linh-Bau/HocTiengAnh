using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HocTiengAnh.Models
{
    public class EngWord
    {
        public string Word { get; set; }

        public string Ipa { get; set; }

        public string Type { get; set; }

        public string Meaning { get; set; }

        public bool IsLeaned { get; set; }
    }
}
