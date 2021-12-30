using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Dto
{
    /// <summary>
    /// 三合一卡信息
    /// </summary>
    public class ThreeCardInfoDto: Information
    {
        public int Total { get; set; }
        public List<ThreeCardInfo> Cards { get; set; }
    }
    public class ThreeCardInfo
    {
        public string SN { get; set; }
        /// <summary>
        /// 卡类型
        /// </summary>
        public string Card_Type { get; set; }
        /// <summary>
        /// 开卡日期
        /// </summary>
        public string Card_OpenDate { get; set; }
        /// <summary>
        /// 卡激活日期
        /// </summary>
        public string Card_ActivationDate { get; set; }
        ///<summary>
        ///续费日期
        /// </summary>
        public string RenewDate { get; set; }
        /// <summary>
        /// 套餐名称
        /// </summary>
        public string SetmealName { get; set; }
        /// <summary>
        /// 卡的总流量
        /// </summary>
        public string Card_totalflow { get; set; }
        ///<summary>
        ///
        /// </summary>
        public string Card_Remarks { get; set; }
        public DateTime AddTime { get; set; }
        ///<summary>
        ///分配给用户的公司名称
        /// </summary>
        public string CustomerCompany { get; set; }
        ///<summary>
        ///客户开卡时间
        /// </summary>
        public string CustomerActivationDate { get; set; }
        ///<summary>
        ///客户续费时间
        /// </summary>
        public string CustomerEndTime { get; set; }

    }
    /// <summary>
    /// 三合一卡详细信息
    /// </summary>
    public class ThreeDetail
    {
        public string flg { get; set; }
        public string Msg { get; set; }
        public List<ThreeCardInfos> Data { get; set; }
    }

    public class ThreeCardInfos
    {
        public string CMCC_CardId { get; set; }
        public string CUCC_CardId { get; set; }
        public string CT_CardId { get; set; }
        public string CMCC_CardICCID { get; set; }
        public string CUCC_CardICCID { get; set; }
        public string CT_CardICCID { get; set; }
        public string CMCC_CardState { get; set; }
        public string CUCC_CardState { get; set; }
        public string CT_CardState { get; set; }
        public string CMCC_WorkState { get; set; }
        public string CUCC_WorkState { get; set; }

        public string CT_WorkState { get; set; }
        public string CMCC_Flow { get; set; }
        public string CUCC_Flow { get; set; }
        public string CT_Flow { get; set; }
    }
}