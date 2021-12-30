using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.parameter.PayManage
{
    public class AddPayInfoPara
    {
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
        ///<summary>
        ///订单编号
        /// </summary>
        public string OrderNum { get; set; }
        /// <summary>
        /// 水单号
        /// </summary>
        public string WaterOrderNum { get; set; }

        public string Remark { get; set; }
        /// <summary>
        /// 图片名称
        /// </summary>
        public string fileName { get; set; }
        ///<summary>
        ///接口标识 2:编辑
        /// </summary>
        public string ApiFlg { get; set; }
    }

    /// <summary>
    /// 审核订单入参
    /// </summary>
    public class PayExaminePara
    {
        public string Company_ID { get; set; }
        public int id { get; set; }
        public int Status { get; set; }
        public string OrderNum { get; set; }
    }

    ///<summary>
    ///查看支付信息入参
    /// </summary>
    public class PayInfoPara
    {
        public string Company_ID { get; set; }
        public string Status { get; set; }
        public string WaterOrderNum { get; set; }
        public string[] AddTime { get; set; }
    }

    ///<summary>
    ///创建支付账单
    /// </summary>
    public class CreateOrderPara
    {
        ///<summary>
        ///当前用户登录companyid
        /// </summary>
        public string Company_ID { get; set; }
        ///<summary>
        ///分配的用户编码
        /// </summary>
        public string CustomerCompanyId { get; set; }
        ///<summary>
        ///价格
        /// </summary>
        public decimal CardTotalPrice { get; set; }
        ///<summary>
        ///购买月数
        /// </summary>
        public int MonthNum { get; set; }
        /// <summary>
        /// 套餐编码
        /// </summary>
        public string SetmealID { get; set; }
        public List<CardList> Cards { get; set; }

    }

    ///<summary>
    ///卡信息
    /// </summary>
    public class CardList
    {
        public string Card_ID { get; set; }
        public string ICCID { get; set; }
        public string SN { get; set; }
    }

    ///<summary>
    ///设置套餐价格和用户
    /// </summary>
    public class SetUpapackagePara
    {
        /// <summary>
        /// 1:添加  2:编辑
        /// </summary>
        public string setflg { get; set; }
        public int id { get; set; }
        /// <summary>
        /// 设置人公司内码（当前登录的）
        /// </summary>
        public string Company_ID { get; set; }
        ///<summary>
        ///套餐编号
        /// </summary>
        public string SetmealID { get; set; }
        ///<summary>
        ///卡价格
        /// </summary>
        public decimal Price { get; set; }
        ///<summary>
        ///设置的合伙人公司编码(子级用户)
        /// </summary>
        public string CustomerCompanyID { get; set; }
    }

    ///<summary>
    ///套餐列表类
    /// </summary>
    public class SetMealDto:Information
    {
       public List<SetMealDtos> Setmealdto { get; set; }
    }
    public class SetMealDtos
    {
        public string SetMealID { get; set; }
        public string SetMealName { get; set; }
    }


    ///<summary>
    ///设置套餐列表
    /// </summary>
    public class CustomerSetMealDto : Information
    {
        public List<CustomOperatorSetMeal> setmealinfo { get; set; }
    }

    public class CustomOperatorSetMeal
    {
        public string value { get; set; }
        public string label { get; set; }
        public List<CustomSetMeal> children { get; set; }
    }

    public class CustomSetMeal
    {
        public string value { get; set; }
        public string label { get; set; }
    }

    ///<summary>
    ///查看用户的套餐价格信息
    /// </summary>
    public class GetSetUpapackageDto:Information
    {
       public List<SetUpapackageDto> Dtos { get; set; }
    }
    public class SetUpapackageDto
    {
        public int id { get; set; }
        /// <summary>
        /// 套餐ID
        /// </summary>
        public string SetmealID { get; set; }
        /// <summary>
        /// 套餐名称
        /// </summary>
        public string SetmealName { get; set; }
        ///<summary>
        ///运营商编号
        /// </summary>
        public string OperatorID { get; set; }
        ///<summary>
        ///价格
        /// </summary>
        public decimal Price { get; set; }
    }
}