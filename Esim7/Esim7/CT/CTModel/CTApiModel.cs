using Esim7.CT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/// <summary>
/// 对接电信接口的API返回类
/// </summary>
namespace Esim7.CT
{
    public class CTApiModel
    {
    }
    ///<summary>
    ///企业级月用量查询
    /// </summary>
    public class Monthlyflow
    {
        public string GROUP_TRANSACTIONID { get; set; }
        public string resultCode { get; set; }
        public string resultMsg { get; set; }
        public string amount { get; set; }
    }
    /// <summary>
    /// 三码互查返回信息
    /// </summary>
    public class Root
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
        public Info info { get; set; }
    }
    public class Info
    {
        /// <summary>
        /// 卡号
        /// </summary>
        public string access_number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ICCID { get; set; }
    }

   
    /// <summary>
    /// 电信卡状态
    /// </summary>
    public class RootCTStatus
    {
        /// <summary>
        /// 
        /// </summary>
        public string groupTransactionId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string resultCode { get; set; }
        /// <summary>
        /// 处理成功
        /// </summary>
        public string resultMsg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Description description { get; set; }
    }
    public class SimList
    {
        /// <summary>
        /// 
        /// </summary>
        public string accNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string iccid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string activationTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> simStatus { get; set; }
    }

    public class Description
    {
        /// <summary>
        /// 
        /// </summary>
        public string pageIndex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<SimList> simList { get; set; }
    }


   

    public class RootCTCardWorkStatus
    {
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
        public string groupTransactionId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DescriptionWorkState description { get; set; }
    }

    public class DescriptionWorkState
    {
        /// <summary>
        /// 
        /// </summary>
        public string result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public string msisdn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public string netModel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public string imsi { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public string acctStatusType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public DateTime eventTimestamp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public string framedIpAddress { get; set; }
        /// <summary>
        /// 河北
        /// </summary>
        //public string provname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public string duration { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public DateTime stopTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public string startTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public string framedIpv6Prefix { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public string framedInterfaceId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public string apn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public string rattype { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public string imei { get; set; }
    }


    #region 移远用
    public class LocationInfo
    {
        /// <summary>
        /// 定位结果值
        /// </summary>
        public int POSITIONRESULT { get; set; }
        /// <summary>
        /// 定位状态
        /// </summary>
        public int MSID_TYPE { get; set; }
        /// <summary>
        /// 定位号码
        /// </summary>
        public string MSID { get; set; }
        ///<summary>
        ///经度
        /// </summary>
        public string LONGITUDE { get; set; }
        ///<summary>
        ///纬度
        /// </summary>
        public string LATITUDE { get; set; }
        ///<summary>
        ///定位时间
        /// </summary>
        public string LOCALTIME { get; set; }
    }

    public class YiYuanLocationInfo : LocationInfo
    {
        public string resultCode { get; set; }
        public string errorMessage { get; set; }
    }




    /// <summary>
    /// 移远查看单月卡流量
    /// </summary>
    public class YiYuanRootFlow
    {
        /// <summary>
        /// 
        /// </summary>
        public List<FlowsItem> flows { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Page page { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int resultCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string errorMessage { get; set; }
    }
    public class FlowsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int msisdn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string iccid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int flow { get; set; }
    }

    public class Page
    {
        /// <summary>
        /// 
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int totalCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int totalPage { get; set; }
    }


    ///<summary>
    ///移远卡详情
    /// </summary>
    public class YiYuanCardDetailDto
    {
        /// <summary>
        /// 
        /// </summary>
        public int setmealmonths { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int flowratenumber { get; set; }
        /// <summary>
        /// NB-1年期
        /// </summary>
        public string setMeal { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal residueFlow { get; set; }
        /// <summary>
        /// 普通贴片MS0 2mm*2mm
        /// </summary>
        public string cardType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int resultCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string errorMessage { get; set; }
        /// <summary>
        /// 激活
        /// </summary>
        public string active { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string activateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string imsi { get; set; }
        /// <summary>
        /// QC-03-13-NB1年
        /// </summary>
        public string billingCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string expiryDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string iccid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string productCode { get; set; }
        /// <summary>
        /// 中国电信
        /// </summary>
        public string supplier { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string msisdn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string networkType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal flow { get; set; }
        /// <summary>
        /// 正常
        /// </summary>
        public string status { get; set; }
    }


    ///<summary>
    ///移远电信卡工作状态接收类
    /// </summary>
    public class YiYuanWorkStatusRoot
    {
        /// <summary>
        /// 
        /// </summary>
        public int resultCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string errorMessage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public RealTimeStatus realTimeStatus { get; set; }
    }
    public class RealTimeStatus
    {
        /// <summary>
        /// 
        /// </summary>
        public string issuccess { get; set; }
        /// <summary>
        /// 关机
        /// </summary>
        public string onlinestatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string usedFlow { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string surpFlow { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string totalFlow { get; set; }
        /// <summary>
        /// 离线
        /// </summary>
        public string gprsstatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ip { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal usedFlowb { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal surpFlowb { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal totalFlowb { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string apn { get; set; }
    }
    #endregion

    #region 电信机卡重绑接收类
    public class Rootjkcb
    {
        /// <summary>
        /// 
        /// </summary>
        public Response Response { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GROUP_TRANSACTIONID { get; set; }
    }

    public class Response
    {
        /// <summary>
        /// 
        /// </summary>
        public string RspType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RspCode { get; set; }
        /// <summary>
        /// 成功接收消息
        /// </summary>
        public string RspDesc { get; set; }
    }
   
    #endregion

}