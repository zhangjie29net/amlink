using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.IOT.IOTModel
{
    public class IOTMonthFlow:Information
    {      
            /// <summary>
            /// 
            /// </summary>
            public List<UsageData> usageData { get; set; }

    }

    public class UsageData
    {
        /// <summary>
        /// 
        /// </summary>
        public string date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string path { get; set; }
    }

    public class IOTDayFlow : Information
    {
        public List<DayGprs> DayGprs { get; set; }
    }
    public class DayGprs
    {
        /// <summary>
        /// 上载
        /// </summary>
        public string tx { get; set; }
        /// <summary>
        /// 下载
        /// </summary>
        public string rx { get; set; }
    }


}