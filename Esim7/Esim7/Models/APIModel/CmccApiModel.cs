using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models.APIModel
{
    public class CmccApiModel
    {
        #region 移动新平台
        /// <summary>
        /// 移动新平台日流量查询
        /// </summary>
        public class CMIOT_API25U01
        {
            /// <summary>
            /// 
            /// </summary>
            public string status { get; set; }
            /// <summary>
            /// 正确
            /// </summary>
            public string message { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<CMIOT_API25U01ResultItem> result { get; set; }
        }
        public class CMIOT_API25U01ResultItem
        {
            /// <summary>
            /// 
            /// </summary>
            public List<DataAmountListItem> dataAmountList { get; set; }
        }

        public class DataAmountListItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string msisdn { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string dataAmount { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<ApnDataAmountListItem> apnDataAmountList { get; set; }
        }
        public class ApnDataAmountListItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string apnName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string apnDataAmount { get; set; }
        }

        ///<summary>
        ///单卡停机原因接收类
        /// </summary>

        public class CMIOT_API25S02
        {
            /// <summary>
            /// 
            /// </summary>
            public string status { get; set; }
            /// <summary>
            /// 正确
            /// </summary>
            public string message { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<CMIOT_API25S02Item> result { get; set; }
        }

        public class CMIOT_API25S02Item
        {
            /// <summary>
            /// 
            /// </summary>
            public string platformType { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string stopReason { get; set; }
        }
        #endregion
    }
}