using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.parameter.UserStockManage
{
    /// <summary>
    /// 一键导入入库参数
    /// </summary>
    public class EnterStockPara
    {
        /// <summary>
        /// 当前登录用户的
        /// </summary>
        public string Company_ID { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string personnel { get; set; }
        ///<summary>
        ///库存所在城市
        /// </summary>
        public string StockCity { get; set; }
        ///<summary>
        ///库存所在详细地址
        /// </summary>
        public string StockAdderss { get; set; }
        ///<summary>
        ///备注
        /// </summary>
        public string Remark { get; set; }
        ///<summary>
        ///卡的价格
        /// </summary>
        public decimal CardPrice { get; set; }
    }

    /// <summary>
    /// 用户上传EXCEL入库方式需要填写的参数
    /// </summary>
    public class ExeclEnterStockPara : EnterStockPara
    {
        ///<summary>
        ///选择的套餐信息  用户选择套餐信息则无需填写套餐信息用户不选择套餐信息则填写套餐信息
        /// </summary>
        public string SetmealID { get; set; }
        ///<summary>
        ///套餐名称
        /// </summary>
        public string SetmealName { get; set; }
        ///<summary>
        ///卡形态名称
        /// </summary>
        public string CardXTName { get; set; }
        ///<summary>
        ///卡类型名称
        /// </summary>
        public string CardTypeName { get; set; }
        ///<summary>
        ///运营商名称
        /// </summary>
        public string OperatorName { get; set; }
        ///<summary>
        ///流量
        /// </summary>
        public string Flow { get; set; }
        ///<summary>
        ///物料编码
        /// </summary>
        public string MaterielCode { get; set; }
        ///<summary>
        ///沉默期
        /// </summary>
        public int silentDate { get; set; }
        ///<summary>
        ///测试期
        /// </summary>
        public int TestDate { get; set; }
        ///<summary>
        ///标签
        /// </summary>
        public string RegionLabel { get; set; }
        /// <summary>
        /// 上传的ICCID数据
        /// </summary>
        public List<ICCIDS> EnterICCID { get; set; }
    }
    public class ICCIDS
    {
        public string ICCID { get; set; }
        public string Card_ID { get; set; }
        public string Card_IMEI { get; set; }
    }
}