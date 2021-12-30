using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    ///<summary>
    ///查看订单
    /// </summary>
    public class GetContractorderDto:Information
    {
        public List<contractorder> orders { get; set; }
    }
    /// <summary>
    /// 合同订单实体类
    /// </summary>
    public class contractorder
    {
        public int Id { get; set; }
        /// <summary>
        /// 合同编号
        /// </summary>
        public string ContractNo { get; set; }
        /// <summary>
        /// 用户conpanyid
        /// </summary>
        public string CustomCompanyID { get; set; }
        ///<summary>
        ///客户公司名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 当前用户companyid
        /// </summary>
        public string Company_ID { get; set; }
        /// <summary>
        /// 物联编码
        /// </summary>
        public string MaterialCode { get; set; }
        /// <summary>
        /// 卡类型ID
        /// </summary>
        public string CardTypeID { get; set; }
        ///<summary>
        ///卡类型名称
        /// </summary>
        public string CardTypeName { get; set; }
        ///<summary>
        ///卡形态名称
        /// </summary>
        public string CardXTName { get; set; }
        /// <summary>
        /// 卡形态ID
        /// </summary>
        public string CardXTID { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// 税费
        /// </summary>
        public decimal Taxation { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remar { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string pic { get; set; }

        public DateTime AddTime { get; set; }
    }


    ///<summary>
    ///销售订单详细信息
    /// </summary>
    public class ContractorderDetailDto:Information
    {
        public List<ContractorderDetail> details { get; set; }
    }

    public class ContractorderDetail
    {
        public string Card_ICCID { get; set; }
        public string Card_ID { get; set; }
        public string ContractNo { get; set; }
        public decimal Price { get; set; }
        /// <summary>
        /// 客户公司名称
        /// </summary>
        public string CompanyName { get; set; }
    }

    ///<summary>
    ///账单管理集合
    /// </summary>
    public class OrderManageDto : Information
    {
        public List<OrderManage> orders { get; set; }
    }
    ///<summary>
    ///账单管理
    /// </summary>
    public class OrderManage
    {
        ///<summary>
        ///客户公司名称
        /// </summary>
        public string CompanyName { get; set; }
        ///<summary>
        ///合同编号
        /// </summary>
        public string ContractNo { get; set; }
        ///<summary>
        ///单价
        /// </summary>
        public decimal Price { get; set; }
        ///<summary>
        ///总价
        /// </summary>
        public decimal TotalPrice { get; set; }
        ///<summary>
        ///卡类型名称
        /// </summary>
        public string CardTypeName { get; set; }
        ///<summary>
        ///卡形态名称
        /// </summary>
        public string CardXTName { get; set; }
        ///<summary>
        ///卡数量
        /// </summary>
        public int CardNumber { get; set; }
        public string CardXTID { get; set; }
        public string CardTypeID { get; set; }
    }

    ///<summary>
    ///查看账单详细信息
    /// </summary>
    public class OrderDetailDto : Information
    {
        public List<OrderDetail> details { get; set; }
    }
    public class OrderDetail
    {
        public string Card_ICCID { get; set;}
        public string Card_ID { get; set; }
        public string CompanyName { get; set; }
        public DateTime Card_EndTime { get; set; }
    }

    ///<summary>
    ///采购订单接收类
    /// </summary>
    public class purchase
    {
        public int Id { get; set; }
        /// <summary>
        /// 采购订单编号
        /// </summary>
        public string PurchaseNo { get; set; }
        ///<summary>
        ///供应商名称
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// 当前登录的用户的CompanyID
        /// </summary>
        public string CompanyID { get; set; }
        /// <summary>
        /// 对接API的信息
        /// </summary>
        public string accountID { get; set; }
        /// <summary>
        ///产品型号
        /// </summary>
        public string ProductModel { get; set; }
        public decimal Price { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string Pic { get; set; }
        public string Remark { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// 卡类型id
        /// </summary>
        public string CardTypeID { get; set; }
        /// <summary>
        /// 卡形态ID
        /// </summary>
        public string CardXTID { get; set; }
        /// <summary>
        /// 卡类型名称
        /// </summary>
        public string CardTypeName { get; set; }
        /// <summary>
        /// 卡形态名称
        /// </summary>
        public string CardXTName { get; set; }
        public DateTime AddTime { get; set; }
    }

    ///<summary>
    ///采购单信息
    /// </summary>
    public class PurchaseDto : Information
    {
        public List<PurchaseInfos> purchases { get; set; }
    }

    ///<summary>
    ///采购订单信息列表
    /// </summary>
    public class PurchaseInfos : purchase
    {
        public string Card_ActivationDate { get; set; }
        public string Card_EndTime { get; set; }
        
    }

    ///<summary>
    ///采购到哪详情
    /// </summary>
    public class PurchaseDetailDto : Information
    {
        public List<PurchaseDetailInfo> DetailInfos { get; set; }
    }

    public class PurchaseDetailInfo
    {
        public string Card_ID { get; set; }
        public string Card_ICCID { get; set; }
    }
}