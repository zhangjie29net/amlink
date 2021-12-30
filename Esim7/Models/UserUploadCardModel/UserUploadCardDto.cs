using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models.UserUploadCardModel
{
    public class UserUploadCardDto
    {
        ///<summary>
        ///当前登录的用户内码
        /// </summary>
        public string Company_ID { get; set; }
        /// <summary>
        /// 操作人员
        /// </summary>
        public string operatorID { get; set; }
        /// <summary>
        /// 套餐主键 字段
        /// </summary>
        public string SetmealID2 { get; set; }
        /// <summary>
        /// 测试期 月
        /// </summary>
        public string testDate { get; set; }
        /// <summary>
        ///  沉默期 月
        /// </summary>
        public string silentDate { get; set; }
        /// <summary>
        /// 开通年限 年
        /// </summary>
        public string OpeningDate { get; set; }
        /// <summary>
        /// API运营商  分地区  非 中国移动 等  （惠州移动）
        /// </summary>
        public string OperatorsID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 库房位置
        /// </summary>
        public string StockAdderss { get; set; }
        /// <summary>
        /// API 字段
        /// </summary>
        public string AccountID { get; set; }
        /// <summary>
        /// 平台 10 移动旧平台 11 移新 20 电信旧 21 电信新 
        /// </summary>
        public string Platform { get; set; }
        ///<summary>
        ///是否直接导入到库存  1:否  2:是  字段废弃
        /// </summary>
        public string IsUploadStock{ get; set; }
        ///<summary>
        ///卡价格
        /// </summary>
        public decimal CardPrice { get; set; }
        ///<summary>
        ///标签
        /// </summary>
        public string RegionLabel { get; set; }
        ///<summary>
        ///使用场景
        /// </summary>
        public string Scene { get; set; }
        ///<summary>
        ///起始时间
        /// </summary>
        public DateTime? ActivateDate { get; set; }
        ///<summary>
        ///终止时间
        /// </summary>
        public DateTime? EndDate { get; set; }
        ///<summary>
        ///采购单号
        /// </summary>
        public string PurchaseNo { get; set; }
        /// <summary>
        /// ICCID
        /// </summary>
        public List<Excel_Card> ICCIDS { get; set; }
    }

    public class Excel_Card
    {
        public string ICCID { get; set; }
        public string SN { get; set; }
        public string Card_ID { get; set; }
    }

    ///<summary>
    ///用户添加反馈信息
    /// </summary>
    public class feedbackInfo
    {
        public string Company_ID { get; set; }
        public string Content { get; set; }
    }
}