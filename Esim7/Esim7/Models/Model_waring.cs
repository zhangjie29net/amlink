using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{

    /// <summary>
    /// 最新定义资费预警功能字段
    /// </summary>
    public class Model_waring
    {

        public string Card_ID { get; set; }
        public string Card_ICCID { get; set; }
        public string Card_CompanyID { get; set; }

        public string CompanyName { get; set; }
        public string SetMealName{ get; set; }
        public string operatorsName { get; set; }

        public string cityName { get; set; }
        public DateTime  CostomEdndate { get; set; }

        public DateTime RealEndDatetime { get; set; }

        public DateTime RealActivationDate { get; set; }

        public DateTime CostomActivationdate { get; set; }
       
      
       





    }
}