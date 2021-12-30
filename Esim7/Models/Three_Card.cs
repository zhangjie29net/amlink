using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    /// <summary>
    /// 奇迹物联三合一卡数据库映射类
    /// </summary>
    public class Three_Card
    {
        public int Id { get; set; }
        public string SN { get; set; }
        /// <summary>
        /// 开卡日期
        /// </summary>
        public DateTime Card_OpenDate { get; set; }
        /// <summary>
        /// 激活日期
        /// </summary>
        public DateTime Card_ActivationDate { get; set; }
        public string Card_Remarks { get; set; }
        public string Card_CompanyID { get; set; }
        public int status { get; set; }
        /// <summary>
        /// 三张卡的总流量
        /// </summary>
        public string Card_totalflow { get; set; }
        public string pici { get; set; }
        public DateTime Card_testTime { get; set; }
        public DateTime Card_silentTime { get; set; }
        public DateTime Card_EndTime { get; set; }
        public string cardType { get; set; }
        public int OpeningYearsTime { get; set; }
        public string Card_jifei_status { get; set; }
        public string OutofstockID { get; set; }
        public string RenewDate { get; set; }
        public int isout { get; set; }
        public string CopyID { get; set; }
        public string Platform { get; set; }
        public string SetMealID { get; set; }
        public string SetMealID2 { get; set; }
        public string AddTime { get; set; }
        public int OperatorsFlg { get; set; }
        public string accountsID { get; set; }
        public string CMCC_CardICCID { get; set; }
        public string CUCC_CardICCID { get; set; }
        public string CT_CardICCID { get; set; }
    }
}