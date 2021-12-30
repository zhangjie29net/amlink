using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models.UserStockModel
{
    /// <summary>
    /// 套餐表映射类
    /// </summary>
    public class setmeal
    {
        public int id { get; set; }
        /// <summary>
        /// 运营商 外联表operator
        /// </summary>
        public string OperatorID { get; set; }
        ///<summary>
        ///物料编码
        /// </summary>
        public string Code { get; set; }
        ///<summary>
        ///编码
        /// </summary>
        public string PartNumber { get; set; }
        ///<summary>
        ///备注
        /// </summary>
        public string Remark { get; set; }
        ///<summary>
        ///套餐描述
        /// </summary>
        public string PackageDescribe { get; set; }
        ///<summary>
        ///套餐内码
        /// </summary>
        public string SetmealID { get; set; }
        ///<summary>
        ///流量
        /// </summary>
        public int Flow { get; set; }
        public string CardTypeID { get; set; }
        public string CardXTID { get; set; }
    }
}