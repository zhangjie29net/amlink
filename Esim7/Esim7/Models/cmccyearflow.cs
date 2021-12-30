using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Web;

namespace Esim7.Models
{
    public class CmccYearFlows:Information
    {
        public int Total { get; set; }
        public List<cmccyearflow> cmccyearflows { get; set; }
    }
    /// <summary>
    /// 年流量接收类
    /// </summary>
    public class cmccyearflow
    {
        public string Card_ID { get; set; }
        public string ICCID { get; set; }
        public string SetmealID { get; set; }
        public string SetmealName { get; set; }
        public string Years { get; set; }
        public string Months { get; set; }
        /// <summary>
        /// 月使用流量数据KB转MB
        /// </summary>
        public decimal MonthFlow { get; set; }
        //备注
        public string Card_Remarks { get; set; }
        //所属公司
        public string CustomerCompany { get; set; }
        //运营商
        public string OperatorName { get; set; }
    }


    ///<summary>
    ///单卡查询和批量查询导出的入参
    /// </summary>
    public class GetYearFlowPara
    {
        public int PagNumber { get; set; }
        public int Num { get; set; }
        public string Card_ICCID { get; set; }
        public string Card_CompanyID { get; set; }
        //public string Years { get; set; }
        public string Months { get; set; }

        public List<YearCardICCID> ListCard { get; set; }
    }

    public class YearCardICCID
    {
        public string ICCID { get; set; }
    }

    ///<summary>
    ///查看僵尸卡信息
    /// </summary>
    public class DieCardDto:Information
    {
        public List<diecard> diecards { get; set; }
    }

    public class diecard
    {
        public string Card_CompanyID { get; set; }
        public string Card_ICCID { get; set; }
        public string Card_ID { get; set; }
        public DateTime Card_ActivationDate { get; set; }
        public DateTime Card_EndTime { get; set; }
        public string LdleTime { get; set; }
        public string SetMealName { get; set; }
        public decimal MonthFlow { get; set; }
        //备注
        public string Card_Remarks { get; set; }       
        //运营商
        public string OperatorName { get; set; }
    }

    ///<summary>
    ///登录后看是否有僵尸卡
    /// </summary>
    public class DieCardCountInfo : Information
    {
        public bool IsDieCard { get; set; }
    }

    ///<summary>
    ///单卡近七次的会话信息
    /// </summary>
    public class GetCardHuihuaDto : Information
    {
        public List<CardHuiHuanInfo> CardActionInfos { get; set; }
    }

    public class CardHuiHuanInfo
    {
        public string ip { get; set; }
        public string Card_ID { get; set; }
        public string Card_ICCID { get; set; }
        public string Card_WorkState { get; set; }
        public string CreateDate { get; set; }
    }
}