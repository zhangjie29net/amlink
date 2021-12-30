using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Dto
{
    public class GetPayInfoDto: Information
    {
        public List<PayInfo> payInfos { get; set; }
    }
    public class PayInfo
    {
        public int Id { get; set; }
        public string PayUserName { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string PayCompany { get; set; }
        public string OrderNum { get; set; }
        public string Company_ID { get; set; }
        public string Phone { get; set; }
        public int PayType { get; set; }
        ///<summary>
        ///支付金额
        /// </summary>
        public decimal PayMoney { get; set; }
        /// <summary>
        /// 水单号
        /// </summary>
        public string WaterOrderNum { get; set; }

        public string Remark { get; set; }
        ///<summary>
        ///银行卡号
        /// </summary>
        public string BankCardNum { get; set; }
        ///<summary>
        ///开户行名称
        /// </summary>
        public string BankName { get; set; }
        ///<summary>
        ///收获地址
        /// </summary>
        public string Address { get; set; }
        ///<summary>
        ///支付上传图片
        /// </summary>
        public string fileName { get; set; }
        ///<summary>
        ///Status 0:未审核  1:已审核
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime AddTime { get; set; }
        ///<summary>
        ///接口标识 2:编辑
        /// </summary>
        public string ApiFlg { get; set; }
    }

    public class OrderPayInfoDto : Information
    {
        public List<OrderPay> orderPays { get; set; }
    }

    public class OrderPay
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNum { get; set; }
        ///<summary>
        ///地址
        /// </summary>
        public string Address { get; set; }
        ///<summary>
        ///支付人公司编码
        /// </summary>
        public string Company_ID { get; set; }
        ///<summary>
        ///支付总金额
        /// </summary>
        public string Company_Name { get; set; }
        ///<summary>
        ///支付总金额
        /// </summary>
        public decimal TotalPrice { get; set; }
        ///<summary>
        ///支付类型
        /// </summary>
        public string OrderType { get; set; }
        ///<summary>
        ///支付状态
        /// </summary>
        public string Status { get; set; }
        ///<summary>
        ///支付时间
        /// </summary>
        public DateTime AddTime { get; set; }
    }

}