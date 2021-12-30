using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.IOT.IOTModel
{
    public class ApiModels
    {
    }
    /// <summary>
    /// CMIOT_API2001-在线信息实时查询 
    /// 集团客户根据所属物联卡的码号信息查询该卡的GPRS在线状态、IP地址、APN、RAT信息。
    /// </summary>
    public class OldRoot_CMIOT_API12001
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
        public List<OldCMIOT_API12001> result { get; set; }
    }
    public class OldCMIOT_API12001
    {
        /// <summary>
        /// 
        /// </summary>
        public string APN { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GPRSSTATUS { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RAT { get; set; }
    }

    /// <summary>
    ///  CMIOT_API2002-用户状态信息实时查询  集团客户可根据所属物联卡的码号信息实时查询该卡的状态信息。
    /// </summary>
    public class OldRoot_CMIOT_API2002
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
        public List<ResultItem_CMIOT_API2002> result { get; set; }
    }
    public class ResultItem_CMIOT_API2002
    {
        public string STATUS { get; set; }
    }

    public class ResultItem_CMIOT_API2105
    {
        /// <summary>
        /// 
        /// </summary>
        public string lifecycle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string opentime { get; set; }
    }
    /// <summary>
    /// CMIOT_API2105-物联卡生命周期查询
    /// 集团客户根据卡号（imsi、msisdn、iccid三个中任意一个），查询物联卡当前生命周期，生命周期包括：00:正式期，01:测试期，02:沉默期，03:其他。
    /// </summary>
    public class OldRoot_CMIOT_API2105
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
        public List<ResultItem_CMIOT_API2105> result { get; set; }
    }
    /// <summary>
    /// CMIOT_API2005-用户当月 GPRS 查询
    /// </summary>
    public class OldRootCMIOT_API2005
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
        public List<OldCMIOT_API2005Item> result { get; set; }
    }
    public class OldCMIOT_API2005Item
    {
        /// <summary>
        /// 
        /// </summary>
        public string total_gprs { get; set; }
    }

    ///<summary>
    ///老平台物联网卡资费套餐查询
    /// </summary>
    public class CMIOT_API2037
    {
        /// <summary>
        /// 正确
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<CMIOT_API2037ResultItem> result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
    }
    public class CMIOT_API2037ResultItem
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
        public List<ProdinfosItem> prodinfos { get; set; }
    }

    public class ProdinfosItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string prodid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string prodinstefftime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string prodinstexptime { get; set; }
        /// <summary>
        /// GPRS 中小流量新 3 元套餐
        /// </summary>
        public string prodname { get; set; }
    }

    ///<summary>
    ///老品台用户余额信息实时查询
    /// </summary>
    public class CMIOT_API2011
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
        public List<CMIOT_API2011ResultItem> result { get; set; }
    }
    public class CMIOT_API2011ResultItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string balance { get; set; }
    }

    

    #region 新平台返回查询的返回集合
    ///<summary>
    ///单卡在线信息查询
    /// </summary>
    public class Root_CMIOT_API25M01
    {

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

    /// <summary>
    /// 查询卡状态  如 正常 停机  等  每张卡   Card_State
    /// </summary>
    public class CMIOT_API25S04Item
    {
        /// <summary>
        /// 
        /// </summary>
        public string cardStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lastChangeDate { get; set; }
    }

    public class CMIOT_API25S04
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
        public List<CMIOT_API25S04Item> result { get; set; }
    }


    /// <summary>
    /// 单卡基本信息查询 API描述 查询物联卡码号信息、开卡时间、首次激活时间  Card_OpenDate
    /// </summary>
    public class CMIOT_API23S00
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
        public List<CMIOT_API23S00Item> result { get; set; }
    }
    public class CMIOT_API23S00Item
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
        /// 
        /// </summary>
        public string activeDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string openDate { get; set; }
    }

    ///<summary>
    ///CMIOT_API25U04-单卡本月流量累计使用量查询
    /// </summary>
    public class CMIOT_API25U04
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
        public List<CMIOT_API25U04Item> result { get; set; }
    }
    public class CMIOT_API25U04Item
    {
        /// <summary>
        /// 
        /// </summary>
        public string dataAmount { get; set; }
    }

    ///<summary>
    ///资费id
    /// </summary>
    public class CMIOT_API23R00
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
        public List<CMIOT_API23R00ResultItem> result { get; set; }
    }
    public class CMIOT_API23R00ResultItem
    {
        /// <summary>
        /// 
        /// </summary>
        public List<OfferingInfoListItem> offeringInfoList { get; set; }
    }
    public class OfferingInfoListItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string offeringId { get; set; }
        /// <summary>
        /// API服务订购
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
    }

    ///<summary>
    ///资费名称
    /// </summary>
    public class CMIOT_API23R01
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
        public List<CMIOT_API23R01ResultItem> result { get; set; }
    }
    public class CMIOT_API23R01ResultItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string offeringId { get; set; }
        /// <summary>
        /// 物品类促销商品
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
    }

    //卡余额
    public class CMIOT_API23B01
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
        public List<CMIOT_API23B01ResultItem> result { get; set; }
    }
    public class CMIOT_API23B01ResultItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string accountId { get; set; }
        /// <summary>
        /// 可口可乐有限公司
        /// </summary>
        public string accountName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string amount { get; set; }
    }
    #endregion
}