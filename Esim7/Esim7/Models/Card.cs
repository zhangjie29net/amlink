using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Esim7.Action.CardAction;

namespace Esim7.Models
{
    public class Card
    {

        /// <summary>
        /// 卡号
        /// </summary>
        public string Card_ID { get; set; }
        /// <summary>
        /// IMSI
        /// </summary>
        public string Card_IMSI { get; set; }
        ///// <summary>
        ///// 类型
        ///// </summary>
        //public string  Card_Type { get; set; }
        /// <summary>
        /// IMEI
        /// </summary>
        public string Card_IMEI { get; set; }
        /// <summary>
        ///   
        /// </summary>
        /// 
        public string Card_Type { get; set; }
        public string Card_State { get; set; }
        public string Card_WorkState { get; set; }
        public string Card_OpenDate { get; set; }
        public string Card_ActivationDate { get; set; }
        public string Card_Remarks { get; set; }
        public string status { get; set; }
        public string Card_CompanyID { get; set; }
        public string Card_ICCID { get; set; }
        public string Card_totalflow { get; set; }
        public string Card_userdflow { get; set; }
        public string Card_Residualflow { get; set; }
        public string Card_Monthlyusageflow { get; set; }
        public string SetmealName { get; set; }
        public string CardTypeName { get; set; }
        public string CardXTName { get; set; }
        public string operatorsName { get; set; }
        public string cityName { get; set; }
        public string cardType { get; set; }
        public string Card_EndTime { get; set; }
        public string Card_ActivationDate_Custom { get; set; }
        public string RenewDate_custom { get; set; }
        public string RenewDate { get; set; }
        public string Flow { get; set; }
        /// <summary>
        /// 分配类型 1:未分配  2:已分配  card表没有这个字段 card_copy1表有该字段
        /// </summary>
        public int AssignType { get; set; }
        public string SetmealID2 { get; set; }
        public DateTime OpenCardTime { get; set; }
        public string Platform { get; set; }
        public string accountsID { get; set; }
        public int Total { get; set; }
        public string SN { get; set; }
        public string PackageDescribe { get; set; }
        ///<summary>
        ///分配给用户的公司名称
        /// </summary>
        public string CustomerCompany { get; set; }
        ///<summary>
        ///客户开卡时间
        /// </summary>
        public string CustomerActivationDate { get; set; }
        ///<summary>
        ///客户卡续费时间
        /// </summary>
        public string CustomerEndTime { get; set; }

        ///<summary>
        ///移动卡ICCID
        /// </summary>
        public string CMCC_CardICCID { get; set; }
        ///<summary>
        ///电信卡ICCID
        /// </summary>
        public string CT_CardICCID { get; set; }
        ///<summary>
        ///联通卡ICCID
        /// </summary>
        public string CUCC_CardICCID { get; set; }
        ///<summary>
        ///是否机卡分离 0:否 1:是
        /// </summary>
        public string IsSeparate { get; set; }
        ///<summary>
        ///所属运营商名称
        /// </summary>
        public string Remark { get; set; }
        ///<summary>
        ///标签
        /// </summary>
        public string RegionLabel { get; set; }
        ///<summary>
        ///区域管控状态 0未超出管控 1超出管控
        /// </summary>
        public string RegionLimitStatus { get; set; }
        ///<summary>
        ///
        /// </summary>
        public string Scene { get; set; }
        /// <summary>
        /// 年使用流量（月使用流量累计数据）
        /// </summary>
        public string YearFlow { get; set; }
        public string operatorsID { get; set;}
    }


    public class GetCardPara
    {
        ///<summary>
        ///运营商编码
        /// </summary>
        public string accountID { get; set; }
        /// <summary>
        /// 基本属相 ICCID  IMSI 等
        /// </summary>
        ///<summary>
        ///分配给客户的公司名称
        /// </summary>
        public string CustomerCompany { get; set; }
        /// <summary>
        /// 工作状态
        /// </summary>
        public string Card_WorkState { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Card_Remarks { get; set; }
        /// <summary>
        /// 公司内码
        /// </summary>
        public string Card_CompanyID { get; set; }
        /// <summary>
        /// 卡状态
        /// </summary>
        public string Card_State { get; set; }
        public string Card_OperatorsFlg { get; set; }
    }
}