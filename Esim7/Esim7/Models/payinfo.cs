using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    /// <summary>
    /// 支付信息表映射类
    /// </summary>
    public class payinfo
    {
        public int Id { get; set; }
        /// <summary>
        /// 支付人名称
        /// </summary>
        public string PayUserName { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string PayCompany { get; set; }
        public string Company_ID { get; set; }
        public string Phone { get; set; }
        public PayTypes PayType { get; set; }
        ///<summary>
        ///支付金额
        /// </summary>
        public decimal PayMoney { get; set; }
        /// <summary>
        /// 水单号
        /// </summary>
        public string WaterOrderNum { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNum { get; set; }
        public string Remark { get; set; }
        /// <summary>
        /// 审核状态 0:未审核 1:已审核
        /// </summary>
        public int Status { get; set; }
        public DateTime AddTime { get; set; }

    }

    public enum PayTypes
    {
        zfb=1,//支付宝
        wx=2,//微信
        yl=3//银联
    }

    public  class bizcontent
    {
        public string out_trade_no { get; set; }
        public double total_amount { get; set; }
        public string product_code { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
    }

    ///<summary>
    ///在线续费入参
    /// </summary>
    public class OnlineRenewPara
    {
        /// <summary>
        /// 当前用户登录的
        /// </summary>
        public string Company_ID { get; set; }
        /// <summary>
        /// 选择的用户的 可以为空
        /// </summary>
        public string CustomCompany_ID { get; set; }
        ///<summary>
        ///运营商标识
        /// </summary>
        public string OperatorsFlg { get; set; }
        ///<summary>
        ///续费类型 1：年:2：月
        /// </summary>
        public string RenewType { get; set; }
        ///<summary>
        ///续费数量
        /// </summary>
        public int RenewNum { get; set; }
        ///<summary>
        ///续费的卡集合
        /// </summary>
        public List<RenewCardList> Cards { get; set; }
    }

    public class RenewCardList
    {
        public string ICCID { get; set; }
        public string SN { get; set; }
    }

    ///<summary>
    ///在线续费返回值
    /// </summary>
    public class OnlineRenewDto : Information
    {
        public string Body { get; set; }
       
    }
}