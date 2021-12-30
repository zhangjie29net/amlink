using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    /// <summary>
    /// 电信卡奇迹物联
    /// </summary>
    public class ct_card
    {
        public int Id { get; set; }
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
        public string Card_State { get; set; }
        public string Card_WorkState { get; set; }
        public string Card_Opendate { get; set; }
        public string Card_ActivationDate { get; set; }
        public string Card_Remrarks { get; set; }
        public string status { get; set; }
        public string Card_CompanyID { get; set; }
        public string Card_ICCID { get; set; }
        public string Card_totalflow { get; set; }
        public string Card_userdflow { get; set; }
        public string Card_Residualflow { get; set; }
        public string Card_Monthlyusageflow { get; set; }
        public string RenewDate { get; set; }
        public string SetmealName { get; set; }
        public string CardTypeName { get; set; }
        public string CardXTName { get; set; }
        public string operatorsName { get; set; }
        public string cityName { get; set; }
        public string ICCID { get; set; }
        public string ActivateDate { get; set; }
        public string EndDate { get; set; }
        public DateTime Card_OpenDate { get; set; }
        public string Card_Remarks { get; set; }
        public string operatorsID { get; set; }
        public string accountsID { get; set; }
        public string pici { get; set; }
        public string Card_testTime { get; set; }
        public string Card_silentTime { get; set; }
        public DateTime Card_EndTime { get; set; }
        public string cardType { get; set; }
        public string OpeningYearsTime { get; set; }
        public string Card_jifei_status { get; set; }
        public string BatchID { get; set; }
        public string OutofstockID { get; set; }
        public int isout { get; set; }
        public string CopyID { get; set; }
        public string Platform { get; set; }
        public string Card_Type { get; internal set; }
    }
}