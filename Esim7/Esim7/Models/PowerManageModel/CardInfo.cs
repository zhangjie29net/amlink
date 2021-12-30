using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models.PowerManageModel
{
    ///<summary>
    ///选择卡列表
    /// </summary>
    public class QueryCardInfo
    {
        public string flg { get; set; }
        public string Msg { get; set; }
        public int Total { get; set; }
        public List<CardInfo> cardInfos { get; set; }  
    }
    /// <summary>
    /// 用户选择卡信息
    /// </summary>
    public class CardInfo
    {
        public int id { get; set; }
        public string Card_ID { get; set; }
        public string Card_ICCID { get; set; }
        /// <summary>
        /// 分配类型 1:未分配  2:已分配
        /// </summary>
        public int AssignType { get; set; }
    }
}