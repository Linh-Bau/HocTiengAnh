using HocTiengAnh.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HocTiengAnh.ViewModels.Pages
{
    public class AppLoggingViewModel 
    {
        public LogService LogService { get; }
        public AppLoggingViewModel(LogService logService) 
        {
            this.LogService = logService;
        }
    }
}
