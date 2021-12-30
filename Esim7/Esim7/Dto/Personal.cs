using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Dto
{
    public class PersonalInfoDto: Information
    {
        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal Balance { get; set; }
        public List<OrderInfo> orderInfos { get; set; }

    }
    public class OrderInfo
    {
        public string Company_ID { get; set; }
        /// <summary>
        /// 购买公司名称
        /// </summary>
        public string Company_Name { get; set; }
        /// <summary>
        /// 卡运营商名称
        /// </summary>
        public string OperatorName { get; set; }
        ///<summary>
        ///卡形态名称
        /// </summary>
        public string CardXTName { get; set; }
        ///<summary>
        ///卡类型名称
        /// </summary>
        public string CardTypeName { get; set; }
        ///<summary>
        ///套餐名称
        /// </summary>
        public string SetmealName { get; set; }
        ///<summary>
        ///卡价格
        /// </summary>
        public decimal CardPrice { get; set; }
        ///<summary>
        ///数量
        /// </summary>
        public int CardNum { get; set; }
        ///<summary>
        ///购买套餐月份
        /// </summary>
        public int MonthNum { get; set; }
        ///<summary>
        ///总价格
        /// </summary>
        public decimal TotalPrice { get; set; }
        ///<summary>
        ///销售给子级用户的公司内码
        /// </summary>
        public string CustomerCompany { get; set; }
        /// <summary>
        /// 回扣
        /// </summary>
        public decimal Commission { get; set; }
        ///<summary>
        ///描述
        /// </summary>
        public string Remark { get; set; }
        ///<summary>
        ///订单状态  订单提交未支付  已支付 
        /// </summary>
        public int OrderStatus { get; set; }
        /// <summary>
        /// 订单类型 1:佣金  2:提现
        /// </summary>
        //public int OrderType { get; set; }
        public string OrderNum { get; set; }
        /// <summary>
        /// 提现金额
        /// </summary>
        public decimal Extract { get; set; }
        public DateTime AddTime { get; set; }
    }

    public class GetPersonalInfoPara
    {
        public string Company_ID { get; set; }
    }


    ///<summary>
    ///提现入参
    /// </summary>
    public class WithdrawalPara
    {
        public string Company_ID { get; set; }
        /// <summary>
        /// 提现金额
        /// </summary>
        public decimal Money { get; set; }
        //银行卡号
        public string BankCardNum { get; set; }
        public string BankName { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
    }


}