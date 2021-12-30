using Esim7.Models;
using Esim7.Models.ShortMessageModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.parameter.ShortMessage
{
    public class ShortMessageDto: Information
    {
        public List<shortmsgtemplate> ShortMessage { get; set; }
    }

    /// <summary>
    /// 下发短信结果接收
    /// </summary>
    public class CMIOT_API25C00
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
        public List<Result> result { get; set; }
    }
    public class Result
    {
        /// <summary>
        /// 
        /// </summary>
        public string succSvcNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string failSvcNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<FailSvcDetail> failSvcDetail { get; set; }
    }
    public class FailSvcDetail
    {
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// MSISDN号不是所查询的集团下的用户
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string msisdn { get; set; }
    }


    public class CMIOT_API25C02
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
        public List<CMIOT_API25C02Result> result { get; set; }
    }

    public class CMIOT_API25C02Result
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
        public string iccid { get; set; }
        public string msisdn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string successNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string failNum { get; set; }
    }

    /// <summary>
    /// 获取sim卡回复内容信息
    /// </summary>
    public class CMIOT_API25C03
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
        public List<CMIOT_API25C03Result> result { get; set; }
    }
    public class CMIOT_API25C03Result
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
        public string iccid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string successNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string failNum { get; set; }
    }

    ///<summary>
    ///平台下发短信接收内容
    /// </summary>
    public class ShortMessgeDto
    {
        public string transactionId { get; set; }
        public string iccid { get; set; }
        public string msisdn { get; set; }
        public string message { get; set; }
    }
}