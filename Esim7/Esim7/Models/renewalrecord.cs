using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    /// <summary>
    /// 续费记录表映射类
    /// </summary>
    public class renewalrecord
    {
        public int id { get; set; }
        //订单编号
        public string OrderNum { get; set; }
        public string Card_ICCID { get; set; }
        public string SN { get; set; }
        //OperatorsFlg 1：移动 2：电信 3：联通 4：全网通 5：漫游
        public string OperatorsFlg { get; set; }
        //续费客户的company_id
        public string CustomerCompany { get; set; }
        //续费类型 1 年 2月
        public string RenewType { get; set; }
        //续费时长
        public int RenewNum { get; set; }

    }
}