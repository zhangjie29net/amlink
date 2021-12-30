using System.Collections.Generic;

namespace Esim7.Model_New_Onelink_API
{   /// <summary>
///      单卡基本信息查询 查询物联卡码号信息、开卡时间、首次激活时间。 CMIOT_API23S00
/// </summary>
    public class  Card_basic_Message
    {
        public class ResultItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string msisdn { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string imsi { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string iccid { get; set; }
            /// <summary>
            ///  激活日期    先开卡再激活
            /// </summary>
            public string activeDate { get; set; }
            /// <summary>
            /// 开卡日期
            /// </summary>
            public string openDate { get; set; }
        }

        public class Root
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


    }


    /// <summary>
    /// 单卡本月语音累计使用量实时查询  实时查询物联卡本月语音累计使用量。  API23U01 
    /// </summary>
    public class   VOice_Used_Mon{

        public class ResultItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string voiceAmount { get; set; }
        }

        public class Root
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









    }


    /// <summary>
    /// 单张卡 APN开通查询
    /// </summary>
    public class APN {
        public class ApnListItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string apnName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string status { get; set; }
        }

        public class ResultItem
        {
            /// <summary>
            /// 
            /// </summary>
            public List<ApnListItem> apnList { get; set; }
        }

        public class Root
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


    }

    /// <summary>
    /// 开关机状态查询    API25M00
    /// </summary>
    public class On_OFF
    {
        public class ResultItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string status { get; set; }
        }

        public class Root
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

    }
    /// <summary>
    ///  单卡在线信息实时查询 查询物联卡的在线信息，区分APN，返回APN信息、IP地址、会话开始时间。
    /// </summary>
    public class OnLine {

        public class SimSessionListItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string apnId { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string status { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ip { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string createDate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string rat { get; set; }
        }

        public class ResultItem
        {
            /// <summary>
            /// 
            /// </summary>
            public List<SimSessionListItem> simSessionList { get; set; }
        }

        public class Root
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
    }
    /// <summary>
    /// 获取IMEI
    /// </summary>
    public class GetIMEI {

        public class ResultItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string imei { get; set; }
        }

        public class Root
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

    }
    /// <summary>
    ///   功能开通查询   通信功能服务： 01 基础语音通信服务 08 短信基础服务 10 国际漫游服务 11 数据通信服务    API23M08
    /// </summary>
    public class FunctionalOpenQuery {

        public class ServiceTypeListItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string serviceType { get; set; }
            /// <summary>
            ///                                                                                              
            /// </summary>
            public string apnName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string serviceStatus { get; set; }
        }

        public class ResultItem
        {
            /// <summary>
            /// 
            /// </summary>
            public List<ServiceTypeListItem> serviceTypeList { get; set; }
        }

        public class Root
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



    }
    /// <summary>
    ///   资费订购实时查询 根据用户类型（企业、群组、sim卡）查询已订购的所有资费列表。   API23R00
    /// </summary>
    public class SubscribedFee {

        public class OfferingInfoListItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string offeringId { get; set; }
            /// <summary>
            /// 物联卡个人
            /// </summary>
            public string offeringName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string effectiveDate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string expiriedDate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string apnName { get; set; }
        }

        public class ResultItem
        {
            /// <summary>
            /// 
            /// </summary>
            public List<OfferingInfoListItem> offeringInfoList { get; set; }
        }

        public class Root
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


    }

     /// <summary>
     ///单张卡月使用流量
     /// </summary>
    public class Flow_Mom_One {

        public class ResultItem

        {
            /// <summary>
            /// 
            /// </summary>
            public string dataAmount { get; set; }
        }

        public class Root
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









    }



}