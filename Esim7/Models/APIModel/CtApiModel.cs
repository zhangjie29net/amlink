using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models.APIModel
{
    public class CtApiModel
    {
    }
    /// <summary>
    /// 电信近六个月流量
    /// </summary>
    public class CtFlow
    {
        /// <summary>
        /// 
        /// </summary>
        public string GROUP_TRANSACTIONID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string resultCode { get; set; }
        /// <summary>
        /// 处理成功！
        /// </summary>
        public string resultMsg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string amount { get; set; }
    }
}