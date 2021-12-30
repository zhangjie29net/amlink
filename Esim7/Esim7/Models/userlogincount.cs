using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    public class userlogincount
    {
        public int id { get; set; }
        public string CompanyID { get; set; }
        public string CompanyName { get; set; }
        public int WeekCount { get; set; }
        public int MonthCount { get; set; }
        public int TotalCount { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime WeekCountTime { get; set; }
        public DateTime MonthCountTime { get; set; }
       
    }
}