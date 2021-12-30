using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.CUCC.CUCCModel
{
    public class CUCCAPIModel
    {
    }

    public class CuccCardWorkStatus
    {
        public string iccid { get; set; }
        public string ipAddress { get; set; }
        public string dateSessionStarted { get; set; }
        public string dateSessionEnded { get; set; }
    }

    ///<summary>
    ///联通卡的状态
    /// </summary>
    public class CUCCCardStatus
    {
        /// <summary>
        /// 
        /// </summary>
        public string iccid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string imsi { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string msisdn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string imei { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ratePlan { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string communicationPlan { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string customer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string endConsumerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dateActivated { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dateAdded { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dateUpdated { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dateShipped { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string accountId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string fixedIPAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string simNotes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string euiccid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string deviceID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string modemID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string globalSimType { get; set; }
    }


    public class CuccCardFlow
    {
        public string iccid { get; set; }
        public string imsi { get; set; }
        public string msisdn { get; set; }
        public string imei { get; set; }
        public string status { get; set; }
        public decimal ctdDataUsage { get; set; }
        public int ctdSMSUsage { get; set; }
        public int ctdVoiceUsage { get; set; }
        public string ctdSessionCount { get; set; }
        public bool overageLimitReached { get; set; }
        public string overageLimitOverride { get; set; }
        public DateTime dateActivated { get; set; }
    }

    ///<summary>
    ///CMIOT实名认证接收类
    /// </summary>
    public class CMIOT_API23A12
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
        public List<ResultItem> result { get; set; }
    }

    public class ResultItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string busiSeq { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
    }

    ///<summary>
    ///移动物联卡实名登记状态查询接收类
    /// </summary>
    public class CMIOT_API23A10
    {
        public string status { get; set; }
        /// <summary>
        /// 正确
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<CMIOT_API23A10ResultItem> result { get; set; }
    }
    public class CMIOT_API23A10ResultItem
    {
        /// <summary>
        /// 实名状态： 1：已实名； 0：未实名；
        /// </summary>
        public string realNameStatus { get; set; }
        /// <summary>
        /// 实名原因： 01：登记到责任单位人和责任人信息 02：11位号码实名（除148号段） 05：订购语音或短信
        /// </summary>
        public string reason { get; set; }
    }
}