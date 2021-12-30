using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    public class V_ResultModel
    {
         /// <summary>
         /// 返回数据信息
         /// </summary>
        public string data { get; set; }
        /// <summary>
        /// 标志
        /// </summary>
        public bool successMessage { get; set; }
    }

    public class V_ResultModel2
    {
        /// <summary>
        /// 返回数据信息提示
        /// </summary>
        public string data { get; set; }
        /// <summary>
        /// 标志
        /// </summary>
        public bool successMessage { get; set; }
             /// <summary>
             ///  附加信息
             /// </summary>
        public object AdditionalInformation { get; set; }
    }

    public class SelmealTypeDto: Information
    { 
       public List<SelmealType> types { get; set; }
    }

    public class SelmealType
    {
        public string OperatorName { get; set; }
        public string OperatorID { get; set; }
        //public string value { get; set; }
        public double value { get; set; }
        public string label { get; set; }
        public List<SelmealInfo> children { get; set; }
    }
    public class SelmealInfo
    {
        /// <summary>
        /// 套餐名称
        /// </summary>
        public string label { get; set; }
        /// <summary>
        /// 套餐编号
        /// </summary>
        //public string value { get; set; }
        public double value { get; set; }
        public string OperatorName { get; set; }
        public string CardTypeID { get; set; }
        public string CardTypeName { get; set; }
        public string CardXTID { get; set; }
        public string CardXTName { get; set; }
        public string Code { get; set; }
        public int Flow { get; set; }
        public string PartNumber { get; set; }
        public string Remark { get; set; }
    }

    ///<summary>
    ///修改套餐信息
    /// </summary>
    public class UpdateSetmealPara
    {
        ///<summary>
        ///卡类型编号
        /// </summary>
        public string CardTypeID { get; set; }
        ///<summary>
        ///卡形态编号
        /// </summary>
        public string CardXTID { get; set; }
        ///<summary>
        ///流量
        /// </summary>
        public string Flow { get; set; }
        ///<summary>
        ///运营商编码
        /// </summary>
        public string OperatorID { get; set; }
        ///<summary>
        ///套餐名称
        /// </summary>
        public string PackageDescribe { get; set; }
        ///<summary>
        ///物料编码
        /// </summary>
        public string PartNumber { get; set; }
        ///<summary>
        ///备注
        /// </summary>
        public string Remark { get; set; }
        ///<summary>
        ///唯一标识 套餐编码
        /// </summary>
        public string SetMealID { get; set; }
    }
}