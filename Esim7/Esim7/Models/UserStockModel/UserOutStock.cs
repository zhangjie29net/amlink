using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models.UserStockModel
{
    /// <summary>
    /// 用户出库表
    /// </summary>
    public class UserOutStock
    {
        public int Id { get; set; }
        public string Card_ICCID { get; set; }
        //出库单号 关联UserStock表OutCode
        public string OutCode { get; set; }
        public DateTime AddTime { get; set; }
        public string Card_ID { get; set; }
    }
}