using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    public class FeedBackDto : Information
    {
       public List<feedback> feedbacks { get; set; }
    }


    /// <summary>
    /// 意见反馈表
    /// </summary>
    public class feedback
    {
        public int id { get; set; }
        public string Company_ID { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNum { get; set; }
        public string Content { get; set; }
        public string AddTime { get; set; }
        public DateTime AddTimes { get; set; }
    }
}