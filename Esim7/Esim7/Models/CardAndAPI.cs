using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    public class CardAndAPI
    {





      
        public Card card { get; set; }

       
    }


    public class Card_API {

        public List<Card> CardAndAPI { get; set; }

       public string total { get; set; }
        //用于导出物联信息数据
        //public bool ExportState { get; set; }

    }


    public class Card_API2
    {

        public List<Card> Cards{ get; set; }

        public string Total { get; set; }


        public string Message { get; set; }

        public string status { get; set; }
    }


    /// <summary>
    /// 单卡IMEI6个月使用流量情况和七日流量使用情况接收类
    /// </summary>
    public class CardFlowImeiInfo:Information
    {
        public string imei { get; set; }
        public List<MonthFlow> monthFlows { get; set; }
        public List<DayFlow> dayFlows { get; set; }
    }

    public class MonthFlow
    {
        public List<string> flow { get; set; }
        public List<string> date { get; set; }
        //public string Month { get; set; }
        //public string Flow { get; set; }
    }
    public class DayFlow
    {
        public List<string> flow { get; set; }
        public List<string> date { get; set; }
        //public string Day { get; set; }
        //public string Flow { get; set; }
    }

}