using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    public class Model_Charing_Card
    {
        public string Card_ID { get; set; }
        public string Card_IMSI { get; set; }

        public string Card_Type { get; set; }

        public string Card_IMEI { get; set; }

        public string Card_State { get; set; }
        public string Card_WorkState { get; set; }
        public DateTime Card_OpenDate { get; set; }
        public DateTime Card_ActivationDate { get; set; }
        public string Card_Remarks { get; set; }
        public string status { get; set; }
        public string Card_CompanyID { get; set; }
        public string Card_ICCID { get; set; }
        public string Card_totalflow { get; set; }
        public string Card_userdflow { get; set; }
        public string Card_Residualflow { get; set; }
        public string Card_Monthlyusageflow { get; set; }
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
        public string RenewDate { get; set; }
        public int isout { get; set; }

        public string CopyID { get; set; }
        public string Platform { get; set; }
        /// <summary>
        /// 表 taocan
        /// </summary>
          public string SetMealID { get; set; }
        /// <summary>
        /// 表setmeal
        /// </summary>
        public string SetMealID2 { get; set; }
        public int num { get; set; }
        public string SN { get; set; }

    }
}