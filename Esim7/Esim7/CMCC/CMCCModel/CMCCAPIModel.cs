using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.CMCC.CMCCModel
{
    public class CMCCAPIModel
    {
    }
    /// <summary>
    /// 移动token类
    /// </summary>
    public class CMCCRootToken
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
        public string token { get; set; }
    }


    /// <summary>
    /// 移动新平台卡的状态
    /// </summary>
    public class NewCMCCCardStatus
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
        public List<NewCMIOT_API25S04> result { get; set; }
    }
    public class NewCMIOT_API25S04
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

    /// <summary>
    /// 移动新平台卡的工作状态
    /// </summary>
    public class NewCMCCCardWorkStatus
    {

        public string status { get; set; }
        /// <summary>
        /// 正确
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<NewCmccCardResult> result { get; set; }
    }
    public class NewCmccCardResult
    {
        /// <summary>
        /// 
        /// </summary>
        public List<NewSimSessionListItem> simSessionList { get; set; }
    }
    public class NewSimSessionListItem
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

    public class NewCMCCCardMonthFlow
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
        public List<NewCMIOT_API25U04Item> result { get; set; }
    }
    public class NewCMIOT_API25U04Item
    {
        /// <summary>
        /// 
        /// </summary>
        public string dataAmount { get; set; }
    }

    ///<summary>
    ///新平台APN关停操作结果接收类
    ///</summary>
    public class CMIOT_API23M07
    {
        /// <summary>
        /// 
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 正确
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<CMIOT_API23M07ResultItem> result { get; set; }
    }
    
    public class CMIOT_API23M07ResultItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string iccid { get; set; }
    }

    ///<summary>
    ///单卡APN开通情况查询
    /// </summary>
    public class CMIOT_API23M03
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
        public List<CMIOT_API23M03ResultItem> result { get; set; }
    }

    public class CMIOT_API23M03ResultItem
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ApnListItem> apnList { get; set; }
    }
    public class ApnListItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string apnName { get; set; }
    }

    ///<summary>
    ///NB卡基站定位接收类
    /// </summary>
    public class CMIOT_API25L00Root
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
        public List<CMIOT_API25L00ResultItem> result { get; set; }
    }

    public class CMIOT_API25L00ResultItem
    {
        public string lat  { get; set; }
        public string lon { get; set; }
    }


    ///<summary>
    ///移动修改单卡状态API
    /// </summary>
    public class CMIOT_API23S03Root
    {
        /// <summary>
        /// 
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 正确
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<CMIOT_API23S03ResultItem> result { get; set; }
    }

    public class CMIOT_API23S03ResultItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string iccid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string orderNum { get; set; }
    }

    ///<summary>
    ///移动物联网卡区域限制状态查询
    /// </summary>
    public class CMIOT_API23A11
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
        public List<CMIOT_API23A11ResultItem> result { get; set; }
    }
    public class CMIOT_API23A11ResultItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string regionLimitStatus { get; set; }
    }


    #region 老平台卡
    /// <summary>
    /// 查看老平台卡的状态
    /// </summary>
    public class OldCmccCardStatus
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

    /// <summary>
    /// 移动老平台卡的工作状态
    /// </summary>
    public class OldCmccCardWorkStatus
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
    /// 移动老平台月使用流量
    /// </summary>
    public class OldCmccCardMonthFlow
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
    ///百度地图apiwgs84转百度地图坐标
    /// </summary>
    public class DTRoot
    {
        /// <summary>
        /// 
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DTResultItem> result { get; set; }
    }

    public class DTResultItem
    {
        /// <summary>
        /// 
        /// </summary>
        public double x{ get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double y{ get; set; }
    }


    /// <summary>
    /// 百度地图api地理位置逆解析接收类
    /// </summary>
    public class baidunjxRoot
    {
        /// <summary>
        /// 
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public baidunjxResult result { get; set; }
    }

    public class Location
    {
        /// <summary>
        /// 
        /// </summary>
        public double lng { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double lat { get; set; }
    }
    public class AddressComponent
    {
        /// <summary>
        /// 中国
        /// </summary>
        public string country { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int country_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string country_code_iso { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string country_code_iso2 { get; set; }
        /// <summary>
        /// 广东省
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 广州市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int city_level { get; set; }
        /// <summary>
        /// 南沙区
        /// </summary>
        public string district { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string town { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string town_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string adcode { get; set; }
        /// <summary>
        /// 庐前山东路
        /// </summary>
        public string street { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string street_number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string direction { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string distance { get; set; }
    }

    public class baidunjxResult
    {
        /// <summary>
        /// 
        /// </summary>
        public Location location { get; set; }
        /// <summary>
        /// 广东省广州市南沙区庐前山东路
        /// </summary>
        public string formatted_address { get; set; }
        /// <summary>
        /// 黄阁
        /// </summary>
        public string business { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public AddressComponent addressComponent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> pois { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> roads { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> poiRegions { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sematic_description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int cityCode { get; set; }
    }

 
    #endregion

}