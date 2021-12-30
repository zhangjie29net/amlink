using System;
using System.Collections.Generic;

namespace Esim7.Model_Stock
{    /// <summary>
    /// Model 库存配置信息 如 
    /// </summary>
    public class Model_Stock_Config
    {
        /// <summary>
        /// 套餐    定义    对应 数据库 setmeal
        /// </summary>
        public class Package
        {
            /// <summary>
            /// 运营商
            /// </summary>
            public string Operator { get; set; }
            /// <summary>
            /// 物料编码
            /// </summary>
            public string Code { get; set; }
            /// <summary>
            /// Part Number
            /// </summary>
            public string PartNumber { get; set; }
            /// <summary>
            /// 套餐描述
            /// </summary>
            public string PackageDescribe { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            public string Remark { get; set; }
            /// <summary>
            /// 流量
            /// </summary>
            public string Flow { get; set; }
            /// <summary>
            /// 卡形态
            /// </summary>
            public string CardXTID { get; set; }
            /// <summary>
            /// 卡类型
            /// </summary>
            public string CardTypeID { get; set; }
            ///<summary>
            ///添加的公司内码
            /// </summary>
            public string Company_ID { get; set; }
        }
        /// <summary>
        /// 运营商
        /// </summary>
        public class Operator
        {
            /// <summary>
            /// 运营商名称
            /// </summary>
            public string OperatorName { get; set; }
        }
        /// <summary>
        /// 库房
        /// </summary>
        public class storageroom
        {
            /// <summary>
            /// 库房名称
            /// </summary>
            public string StorageRoomName { get; set; }

        }
        /// <summary>
        /// 卡形态  卡类型
        /// </summary>
        public class CardType
        {
            /// <summary>
            ///名称
            /// </summary>
            public string CardTypeName { get; set; }
        }
        /// <summary>
        /// 卡形态
        /// </summary>
        public class CardXingTai
        {
            /// <summary>
            /// 卡形态
            /// </summary>
            public string CardXTName { get; set; }
        }
        /// <summary>
        /// 入库时的字段  前台需上传的JSON 对象  PostBody解析
        /// </summary>
        public class WareHousing_Message
        {
            ///<summary>
            ///运营商标识  OperatorsFlg 1：移动 2：电信 3：联通 
            /// </summary>
            public string OperatorsFlg { get; set; }
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
            /// 区域管控标记
            /// </summary>
            public string RegionLabel { get; set; }

            public DateTime ActivateDate { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public DateTime EndDate { get; set; }
            /// <summary>
            /// ICCID
            /// </summary>
            public List<Excel_ICCID> ICCIDS { get; set; }
        }
        /// <summary>
        /// excel文件字段
        /// </summary>
        public class Excel_ICCID
        {
            /// <summary>
            /// ICCID
            /// </summary>
            //public string card { get; set; }

            public string ICCID { get; set; }
        }
        /// <summary>
        /// ICCID 集合
        /// </summary>
        public class List_ICCID
        {
            public List<Excel_ICCID> ICCIDS { get; set; }
        }
        /// <summary>
        /// 表accounts  配置
        /// </summary>
        public class Account
        {
            public string APPID { get; set; }
            public string Company_ID { get; set; }
            public string ECID { get; set; }
            public string PASSWORD { get; set; }
            public string cityID { get; set; }
            public string accountName { get; set; }
            public string accountID { get; set; }
            public string TOKEN { get; set; }
            public string TRANSID { get; set; }
            public string Remark { get; set; }
            public string operatorsID { get; set; }
            public string URL { get; set; }
            public string Platform { get; set; }
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string APIkey { get; set; }

            //{"APPID":"","ECID":"","PASSWORD":"","accountName":"","TOKEN ":"","TRANSID":"","Remark":"","URL":"","Platform":"","accountID":""}
        }
        /// <summary>
        /// 出库单  操作
        /// </summary>
        public class OutOfStock
        {   /// <summary>
            /// 库存剩余
            /// </summary>
            public string ProductNumber { get; set; }
            /// <summary>
            /// 入库时总数量
            /// </summary>
            public string ProductNumbers { get; set; }
            /// <summary>
            /// 出库数量
            /// </summary>
            public string OutNumber { get; set; }
            public string OutofstockID { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            public string remark { get; set; }
            /// <summary>
            /// 库存单号
            /// </summary>
            public string BatchID { get; set; }
            /// <summary>
            /// 操作人员
            /// </summary>
            public string Operator { get; set; }
            /// <summary>
            /// 用途
            /// </summary>
            public string purpose { get; set; }
            /// <summary>
            ///  API AccountID
            /// </summary>
            public string AccountID { get; set; }
            /// <summary>
            /// 套餐ID  
            /// </summary>
            public string SetmealID { get; set; }
            public List<Excel_ICCID> ICCIDS { get; set; }
        }

        /// <summary>
        /// 出库单  操作
        /// </summary>
        public class OutOfStock2
        {   /// <summary>
            /// 库存剩余
            /// </summary>
            public string ProductNumber { get; set; }
            /// <summary>
            /// 入库时总数量
            /// </summary>
            public string ProductNumbers { get; set; }
            /// <summary>
            /// 出库数量
            /// </summary>
            public string OutNumber { get; set; }
            public string OutofstockID { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            public string remark { get; set; }
            /// <summary>
            /// 库存单号
            /// </summary>
            public string BatchID { get; set; }
            /// <summary>
            /// 操作人员
            /// </summary>
            public string Operator { get; set; }
            /// <summary>
            /// 用途
            /// </summary>
            public string purpose { get; set; }
            /// <summary>
            ///  API AccountID
            /// </summary>
            public string AccountID { get; set; }
            /// <summary>
            /// 套餐ID  
            /// </summary>
            public string SetmealID { get; set; }        
        }
        public class UpdatePackage {
            /// <summary>
            /// 运营商卡标识
            /// </summary>
            public string OperatorsFlg { get; set; }
        public string SetmealID { get; set; }
            public List<Excel_ICCID> Cards { get; set; }
        }

    }
}