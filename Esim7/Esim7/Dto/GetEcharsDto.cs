using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Dto
{
    /// <summary>
    /// 统计面板返回的实体类
    /// </summary>
    public class GetEcharsDto
    {
    }

    ///<summary>
    ///统计 移动 联通 电信 全网通卡数量
    /// </summary>
    public class CardTotalNumberTypeDto: Information
    {
        //public List<CardOperatorTypeNum> CMCCNUMTYPE { get; set; }
        //public List<CardOperatorTypeNum> CUCCNUMTYPE { get; set; }
        //public List<CardOperatorTypeNum> CTNUMTYPE { get; set; }
        //public List<CardOperatorTypeNum> THREECARDNUMTYPE { get; set; }
        public List<CardOperatorTypeNum> operatorTypeNums { get; set; }
        public List<CardOperatorDto> source { get; set; }
    }

    public class CardOperatorTypeNum
    {
        public string CardTypeName { get; set; }

        public string CardTypeID { get; set; }
        public int NUM { get; set; }

    }

    public class CardOperatorDto
    {
        public string OperatorName { get; set; }
        public int NB { get; set; }
        public int NoNB { get; set; }
    }

    ///<summary>
    ///获取月使用流量数据接收类
    /// </summary>
    public class CardMonthFlowDto:Information
    {
       public List<CardMonthFlowInfo> cardMonthFlows { get; set; }
    }

    public class CardMonthFlowInfo
    {
        public string CMCCFlow { get; set; }
        public string CUCCFlow { get; set; }
        public string CTFlow { get; set; }
        public string TotalFlow { get; set; }
    }

    ///<summary>
    ///获取移动电信联通卡数量和其他卡数量
    /// </summary>
    public class CardNumberDto:Information
    {
        public List<CardNumberInfo> cardNumbers { get; set; }
    }

    public class CardNumberInfo
    {
        public int CMCCNumber { get; set; }
        public int CUCCNumber { get; set; }
        public int CTNumber { get; set; }
        public int OtherNumber { get; set; }
    }

    ///<summary>
    ///统计近七天流量使用情况
    /// </summary>
    public class CardFlowDays: Information
    {
        public List<FlowDay> data { get; set; }        
    }

    public class FlowDay
    {
        public List<decimal> flow { get; set; }
        public List<string> date { get; set; }
    }
}