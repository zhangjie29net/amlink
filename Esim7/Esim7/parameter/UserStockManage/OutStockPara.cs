using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.parameter.UserStockManage
{
    /// <summary>
    /// 出库参数
    /// </summary>
    public class OutStockPara
    {
        public string Company_ID { get; set; }
        /// <summary>
        /// 出库用途
        /// </summary>
        public string OutPurpose { get; set; }
        ///<summary>
        ///出库数量
        /// </summary>
        public int CardOutNumber { get; set; }
        ///<summary>
        ///备注
        /// </summary>
        public string Remark { get; set; }
        ///<summary>
        ///价格
        /// </summary>
        public decimal CardPrice { get; set; }
        ///<summary>
        ///入库单号
        /// </summary>
        public string EnterCode { get; set; }
        ///<summary>
        ///出库人
        /// </summary>
        public string personnel { get; set; }
        ///<summary>
        ///卡集合
        /// </summary>
        public List<ICCIDS> EnterICCID { get; set; }
    }
}