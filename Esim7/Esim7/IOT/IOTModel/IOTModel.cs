using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.IOT.IOTModel
{
   
    public class GetRoamCardPara
    {
        /// <summary>
        /// 基本属相 ICCID  IMSI 等
        /// </summary>
        public string Card_Mess { get; set; }
        ///<summary>
        ///分配给客户的公司名称
        /// </summary>
        public string CustomerCompany { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Card_Remarks { get; set; }
        /// <summary>
        /// 公司内码
        /// </summary>
        public string Card_CompanyID { get; set; }
        /// <summary>
        /// 卡状态
        /// </summary>
        public string Card_State { get; set; }
        
        public int PagNumber { get; set; }
        public int Num { get; set; }

    }

    /// <summary>
    /// 酷宅订单记录
    /// </summary>
    /// 
    public class KuZhaiOrderDtoS
    {
        public List<KuZhaiOrderDto> dtos { get; set; }
    }


    public class KuZhaiOrderDto
    {
        public string orderName { get; set; }
        public int totalLicenses { get; set; }
        public string productModel { get; set; }
    }





    //酷宅烧录数据接收类

    public class KuZhaiRoot
    {
        public List<GetKuZhaiDto> dtos { get; set; }
    } 
    public class GetKuZhaiDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string deviceid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string factory_apikey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sta_mac { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sap_mac { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string device_model { get; set; }
    }
}