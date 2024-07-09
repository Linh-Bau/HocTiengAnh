using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HocTiengAnh.Services
{
    public partial class LogService : ObservableObject
    {
        [ObservableProperty]
        private string log;

        public LogService() 
        {

        }

        public void Debug(string message)
        {
            AppendLog(message, "DEBUG");
        }

        public void Info(string message)
        {
            AppendLog(message, "INFOR");
        }

        public void Error(string message)
        {
            AppendLog(message, "ERROR");
        }

        private void AppendLog(string message, string type)
        {
            // 10:00:00 - DEBUG: abcde 
            string _log = string.Format("{0} - {1}: {2}",
                DateTime.Now.ToString("HH:mm:ss"),
                type,
                message);
            Log += _log + "\n";
        }
    }
}
