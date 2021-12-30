using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    /// <summary>
    /// 卡类型套餐形态提成设置表映射类
    /// </summary>
    public class distributionratio
    {
        public int Id { get; set; }
        /// <summary>
        /// 设置人公司内码（当前登录的）
        /// </summary>
        public string Company_ID { get; set; }
        /// <summary>
        /// 运营商
        /// </summary>
        public string OperatorID { get; set; }
        ///<summary>
        ///卡形态
        /// </summary>
        public string CardXTID { get; set; }
        ///<summary>
        ///卡类型
        /// </summary>
        public string CardTypeID { get; set; }
        ///<summary>
        ///套餐编号
        /// </summary>
        public string SetmealID { get; set; }
        ///<summary>
        ///卡价格
        /// </summary>
        public decimal CardPrice { get; set; }
        ///<summary>
        ///卡销售价格
        /// </summary>
        public decimal SaleCardPrice { get; set; }
        ///<summary>
        ///提成比例
        /// </summary>
        public decimal tcbl { get; set; }
        ///<summary>
        ///手续费比例
        /// </summary>
        public decimal sxfbl { get; set; }
        ///<summary>
        ///设置的合伙人公司编码(子级用户)
        /// </summary>
        public string CustomerCompanyID { get; set; }
        ///<summary>
        ///添加时间
        /// </summary>
        public DateTime AddTime { get; set; }
    }


    ///<summary>
    ///生产抛料对比数据
    /// </summary>
    /// 
    public class ICCIDSS
    {
        public List<cardiccidsss> card1 { get; set; }
        public List<cardiccidsss> card2 { get; set; }
    }
    public class cardiccidsss
    {
        public string ICCID { get; set; }
    }

    
    ///<summary>
    ///生产抛料对比数据后返回的数据
    /// </summary>
    public class PaoLiaoDto : Information
    {
        public List<cardiccidsss> getpaoliaos { get; set; }
    }

    public class getpaoliao
    {
        public string ICCID { get; set; }
    }
}