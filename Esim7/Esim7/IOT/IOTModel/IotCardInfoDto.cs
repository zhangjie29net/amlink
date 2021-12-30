using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.IOT.IOTModel
{
    /// <summary>
    /// 物联网卡信息接收类
    /// </summary>
    public class IotCardInfoDto: Information
    {
        public List<ItoCardInfo> cardInfos { get; set; }
    }

    public class ItoCardInfo
    {
        /// <summary>
        /// 卡号/接入号
        /// </summary>
        public string access_number { get; set; }

        public string iccid { get; set; }
        public string imei { get; set; }
        /// <summary>
        /// 月使用流量（KB）
        /// </summary>
        public decimal month_flow { get; set; }
        /// <summary>
        /// 卡状态
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 卡在线离线状态 00离线 01在线
        /// </summary>
        public string online_status { get; set; }

        ///<summary>
        ///卡激活日期
        /// </summary>
        public string activetime { get; set; }

        ///<summary>
        ///套餐名称
        /// </summary>
        public string SetmealName { get; set; }
        ///<summary>
        ///卡余额
        /// </summary>
        public string balance { get; set; }
    }

    #region 讯众API对接实体接收类
    ///<summary>
    ///卡号码信息接收类
    /// </summary>
    public class XunZhongCardInfoRoot
    {
        /// <summary>
        /// 
        /// </summary>
        public string errorCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DataCardInfo data { get; set; }
    }
    public class DataCardInfo
    {
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
        public string cardNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int cardType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int packageTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal flowsUsed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal flowsTotal { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal flowsRest { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int smsFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int gprsFlag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? openTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? startTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? endTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int carriType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int useType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string imei { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<PackageInfoListItem> packageInfoList { get; set; }
        public int isCertification { get; set; }
    }

    public class PackageInfoListItem
    {
        /// <summary>
        /// 移动13L-100M年
        /// </summary>
        public string packageName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int carriType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int packageKind { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int packageType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int quantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string unit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int month { get; set; }
    }

    ///<summary>
    ///单卡状态查询接收类
    /// </summary>
    public class XunZhongCardStatusRoot
    {
        /// <summary>
        /// 
        /// </summary>
        public string errorCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public StatusData data { get; set; }
    }
    public class StatusData
    {
        /// <summary>
        /// 
        /// </summary>
        public string iccid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int carriType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 待激活
        /// </summary>
        public string statusName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string openTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string startTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string endTime { get; set; }
    }

    ///<summary>
    ///基站定位接收类
    /// </summary>
    public class ZhongXunLocationRoot
    {
        /// <summary>
        /// 
        /// </summary>
        public string errorCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<LocationDataItem> data { get; set; }
    }
    public class LocationDataItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string lat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lon { get; set; }
    }
    #endregion 
}