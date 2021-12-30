using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Dto
{
   /// <summary>
   /// 三合一卡入库信息
   /// </summary>
    public class ThreeCardStockPara
    {
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
        /// 库房ID
        /// </summary>
        public string StorageRoomID { get; set; }
        /// <summary>
        /// API 字段
        /// </summary>
        public string AccountID { get; set; }
        /// <summary>
        /// 平台 10 移动旧平台 11 移新 20 电信旧 21 电信新
        /// </summary>
        public string Platform { get; set; }
        /// <summary>
        /// ICCID
        /// </summary>
        public DateTime ActivateDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<CardSN> ICCIDS { get; set; }
    }

    public class CardSN
    {
        public string SN { get; set; }
        public string CMCC_ICCID { get; set; }
        public string CT_ICCID { get; set; }
        public string CUCC_ICCID { get; set; }
        
    }

    ///<summary>
    ///获取三合一卡信息入参
    /// </summary>
    public class GetThreeCardPara
    {
        public string Company_ID { get; set; }
        public string CardRemark { get; set; }
        public string SN { get; set; }
        //根据分配的公司名查找用户卡信息
        public string CustomerCompany { get; set; }
        public int Num { get; set; }
        public int PagNumber { get; set; }
    }

    ///<summary>
    ///给用户分配卡入参
    /// </summary>
    public class ThreeCardCopyPara
    {
        /// <summary>
        /// 用户的公司编码
        /// </summary>
        public string CompanyID { get; set; }
        /// <summary>
        /// 卡激活时间
        /// </summary>
        public DateTime ActivateDate { get; set; }
        /// <summary>
        /// 卡续费时间
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ThreeCardCopySN> Cards { get; set; }
    }
    public class ThreeCardCopySN
    {
        public string SN { get; set; }
    }

    public class UpdateThreeCardSetmeal
    {
        public string SetmealID { get; set; }
        public List<ThreeCardCopySN> Cards { get; set; }
    }


}