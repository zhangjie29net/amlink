using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    /// <summary>
    /// 销售单映射类
    /// </summary>
    public class saleorder
    {
        public int Id { get; set; }
        /// <summary>
        /// 登录用户的公司父级公司编码
        /// </summary>
        public string Company_ID { get; set; }
        /// <summary>
        /// 运营商编号
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// 卡形态编号
        /// </summary>
        public string CardXTName { get; set; }
        /// <summary>
        /// 卡类型id
        /// </summary>
        public string CardTypeName { get; set; }
        ///<summary>
        ///套餐编码
        /// </summary>
        public string SetmealID { get; set; }
        /// <summary>
        /// 套餐名称
        /// </summary>
        public string SetmealName { get; set; }
        /// <summary>
        /// 卡价格
        /// </summary>
        public decimal CardPrice { get; set; }
        /// <summary>
        ///  登录用户的公司编码
        /// </summary>
        public string CustomerCompany { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// 销售卡片数量
        /// </summary>
        public int CardNum { get; set; }
        ///<summary>
        ///购买月数
        /// </summary>
        public int MonthNum { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }
        ///<summary>
        ///设置套餐编码
        /// </summary>
        public string InstallNum { get; set; }
        ///<summary>
        ///省市区拼接串
        /// </summary>
        public string[] CityStr { get; set; }
        ///<summary>
        ///详细地址
        /// </summary>
        public string Address { get; set; }

    }
}