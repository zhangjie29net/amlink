using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models.UserStockModel
{
    /// <summary>
    /// 用户入库表
    /// </summary>
    public class UserWarehousing
    {
        public int Id { get; set; }
        public string Card_ICCID { get; set; }
        //是否出库 0否 1是
        public int Isout { get; set; }
        //入库单号 关联UserStock表EnterCode
        public string EnterCode { get; set; }
        //关联UserOutStock表OutCode
        public string OutCode { get; set; }
        public DateTime AddTime { get; set; }
        public string Card_ID { get; set; }
    }
}