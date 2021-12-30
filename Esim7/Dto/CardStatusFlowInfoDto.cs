using Esim7.Models;
using Esim7.parameter.UserStockManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Esim7.Action.Action_haringGetExcel;

namespace Esim7.Dto
{
    /// <summary>
    /// 联通电信共用单卡查询卡状态工作状态月使用流量
    /// </summary>
    public class CardStatusFlowInfoDto
    { 
        /// <summary>
        /// 流量
        /// </summary>
        public string Card_Monthlyusageflow { get; set; }
        ///<summary>
        ///卡工作状态
        /// </summary>
        public string Card_WorkState { get; set; }
        ///<summary>
        ///卡的状态
        /// </summary>
        public string Card_State { get; set; }
        public string Message { get; set; }
        public string status { get; set; }
        public bool Success { get; set; }
    }


    public class CardOffOnPara
    {
        public string CompanyID { get; set; }
        //operType 0：开 1:停
        public string operType { get; set; }
        public string ICCID { get; set; }
        public List<SetRulesCardInfos> cards { get; set; }
    }

    public class CardDelPara
    {
        public string CompanyID { get; set; }
        //运营商  1：移动  2:电信 3:联通 4:全网通卡 5:漫游卡
        public int OperatorID { get; set; }
        public List<ListItem> Cards{ get; set; }
    }
   
}