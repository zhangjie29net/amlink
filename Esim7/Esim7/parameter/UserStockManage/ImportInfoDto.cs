using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.parameter.UserStockManage
{
    /// <summary>
    /// 返回客户可以一键导入的卡信息列表
    /// </summary>

    public class Infos: Information
    {
        public List<ImportInfoDto> importInfos { get; set; }
    }
    public class ImportInfoDto
    {
        /// <summary>
        /// 属于用户
        /// </summary>
        public string Company_ID { get; set; }
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
        public int Flow { get; set; }
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
        ///起始ICCID
        /// </summary>
        public string StartICCID { get; set; }
        ///<summary>
        ///终止ICCID
        /// </summary>
        public string EndICCID { get; set; }
        ///<summary>
        ///起始Card_ID
        /// </summary>
        public string StartCardID { get; set; }
        ///<summary>
        ///终止CardID
        /// </summary>
        public string EndCardID { get; set; }
        ///<summary>
        ///卡数量
        /// </summary>
        public int CardNumber { get; set; }
        /// <summary>
        /// 上传的ICCID数据
        /// </summary>
        public List<ICCIDS> EnterICCID { get; set; }
    }
   

    ///<summary>
    ///用户卡分批次的数据接收
    /// </summary>
    public class CardGroup
    {
        public string days { get; set; }
    }
}