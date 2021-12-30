using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    /// <summary>
    /// 最新修改的实体类
    /// </summary>

    public class NewModel//查看续费预警信息(后台查看看)
    {
        public int Type { get; set; }
        public List<ForHoutai> Message_ForHoutai { get; set; }
        public List<Custom> Message_Custom { get; set; }
    }
    public class ForHoutai//后台
    {
        public string CompanyName { get; set; }
        public string CompanyID { get; set; }
        public string Number { get; set; }
        public string Phone { get; set; }
        public string Total { get; set; }
    }
    public class Custom
    {
        public string CompanyName { get; set; }
        public string CompanyID { get; set; }
        public string Number { get; set; }
        public string Phone { get; set; }
        public string Total { get; set; }
    }

    public class CustomMessage//查看续费预警信息(客户查看看)
    {
        public int Type { get; set; }
        public List<ForHoutai> Custom { get; set; }
    }

    ///<summary>
    ///续费预警卡信息（详情）
    /// </summary>
    public class WaringDetail
    {
        public string ICCID { get; set; }
        public string Card_ID { get; set; }
        
        public string CompanyID { get; set; }
        public string CustomName { get; set; }
        public string Custom_EndTime { get; set; }
    }
}