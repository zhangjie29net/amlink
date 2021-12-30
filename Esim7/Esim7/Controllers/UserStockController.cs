using Esim7.Action;
using Esim7.Models;
using Esim7.Models.UserStockModel;
using Esim7.parameter.UserStockManage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace Esim7.Controllers
{
    /// <summary>
    /// 用户库存管理模块
    /// </summary>
    public class UserStockController : ApiController
    {
        UserStockAction r = new UserStockAction();
        ///<summary>
        ///用户一键导入信息返回数据
        /// </summary>
        [HttpGet]
        [Route("ImportInfoList")]
        public Infos ImportInfoList(string Company_ID)
        {
            Infos c = new Infos();
            c = r.ImportInfoList(Company_ID);
            return c;
        }


        ///<summary>
        ///用户导入SIM卡到库存
        /// </summary>
        [HttpPost]
        [Route("ImportCardInfo")]
        public Information ImportCardInfo(ExeclEnterStockPara para)
        {
            Information c = new Information();
            c = r.ImportCardInfo(para);
            return c;
        }
        ///<summary>
        ///查看入库信息
        /// </summary>
        [HttpPost]
        [Route("GetUserStockInfo")]
        public GetUserStockDto GetUserStockInfo(GetUserStockPara para)
        {
            GetUserStockDto c = new GetUserStockDto();
            c = r.GetUserStockInfo(para);
            return c;
        }

        ///<summary>
        ///修改入库信息
        /// </summary>
        [HttpPost]
        [Route("UpdateEnterStockInfo")]
        public Information UpdateEnterStockInfo(UpdateEnterStockPara para)
        {
            Information c = new Information();
            c = r.UpdateEnterStockInfo(para);
            return c;
        }
        ///<summary>
        ///出库操作  输入导出数量或者导入EXCEL方式导出数据
        /// </summary>
        [HttpPost]
        [Route("OutStock")]
        public Information OutStock(OutStockPara para)
        {
            Information c = new Information();
            c = r.OutStock(para);
            return c;
        }

        ///<summary>
        ///查看出库信息  成功出库后的信息
        /// </summary>
        [HttpPost]
        [Route("GetOutStockInfo")]
        public OutStockDto GetOutStockInfo(GetUserStockPara para)
        {
            OutStockDto c = new OutStockDto();
            c = r.GetOutStockInfo(para);
            return c;
        }

        ///<summary>
        ///撤销出库信息
        /// </summary>
        [HttpGet]
        [Route("CancelOutStock")]
        public Information CancelOutStock(string Company_ID, string OutCode)
        {
            Information c = new Information();
            c = r.CancelOutStock(Company_ID, OutCode);
            return c;
        }

        ///<summary>
        ///查看入库出库卡的详细信息（库存中每一张卡的信息）
        /// </summary>
        [HttpGet]
        [Route("GetOutEnterStockDetail")]
        public GetUserStockDetailDto GetOutEnterStockDetail(string OutCode, string EnterCode)
        {
            GetUserStockDetailDto c = new GetUserStockDetailDto();
            c = r.GetOutEnterStockDetail(OutCode, EnterCode);
            return c;
        }

        ///<summary>
        ///导出入库信息导出出库信息导出目前库存信息
        /// </summary>
        [HttpGet]
        [Route("ExportUserStock")]
        public GetUserStockDetailDto ExportUserStock(int ExportType, string Code)
        {
            GetUserStockDetailDto c = new GetUserStockDetailDto();
            c = r.ExportUserStock(ExportType, Code);
            return c;
        }

        #region 销售订单
        ///<summary>
        ///创建订单/合同
        /// </summary>
        [HttpPost]
        [Route("AddContractorderInfo")]
        public Information AddContractorderInfo(contractorder order)
        {
            Information info = new Information();
            info = r.AddContractorderInfo(order);
            return info;
        }

        ///<summary>
        ///修改订单/合同
        /// </summary>
        [HttpPost]
        [Route("UpdateContractorderInfo")]
        public Information UpdateContractorderInfo(contractorder order)
        {
            Information info = new Information();
            info = r.UpdateContractorderInfo(order);
            return info;
        }

        ///<summary>
        ///删除订单/合同
        /// </summary>
        [HttpGet]
        [Route("DelContractorderInfo")]
        public Information DelContractorderInfo(int Id)
        {
            Information info = new Information();
            info = r.DelContractorderInfo(Id);
            return info;
        }

        ///<summary>
        ///查看订单
        ///Company_ID  当前登录客户的companyid
        ///CompanyName 查找的用户的名称
        /// </summary>
        [HttpGet]
        [Route("GetContractorderInfo")]
        public GetContractorderDto GetContractorderInfo(string Company_ID, string CompanyName)
        {
            GetContractorderDto dto = new GetContractorderDto();
            dto = r.GetContractorderInfo(Company_ID,CompanyName);
            return dto;
        }

        ///<summary>
        ///查看销售订单详细信息
        /// </summary>
        [HttpGet]
        [Route("GetContractorderDetail")]
        public ContractorderDetailDto GetContractorderDetail(string ContractNo, string Value)
        {
            ContractorderDetailDto dto = new ContractorderDetailDto();
            dto = r.GetContractorderDetail(ContractNo,Value);
            return dto;

        }

        ///<summary>
        ///查看账单信息
        /// </summary>
        [HttpGet]
        [Route("GetOrderManageInfo")]
        public OrderManageDto GetOrderManageInfo(string Company_ID, string CompanyName)
        {
            OrderManageDto dto = new OrderManageDto();
            dto = r.GetOrderManageInfo(Company_ID,CompanyName);
            return dto;
        }

        ///<summary>
        ///查看账单详细信息
        ///Value 卡号或ICCID
        /// </summary>
        [HttpGet]
        [Route("GetOrderDetailInfo")]
        public OrderDetailDto GetOrderDetailInfo(string ContractNo, string Value)
        {
            OrderDetailDto dto = new OrderDetailDto();
            dto = r.GetOrderDetailInfo(ContractNo,Value);
            return dto;
        }
        #endregion

        #region 采购订单相关接口
        ///<summary>
        ///创建运营商接口
        /// </summary>
        [HttpPost]
        [Route("AddAccountInfo")]
        public Information AddAccountInfo(accounts para)
        {
            Information info = new Information();
            info = r.AddAccountInfo(para);
            return info;
        }

        ///<summary>
        ///添加API信息
        /// </summary>
        [HttpPost]
        [Route("AddOperatorApi")]
        public Information AddOperatorApi(accounts para)
        {
            Information info = new Information();
            info = r.AddOperatorApi(para);
            return info;
        }

        /// <summary>
        /// 查看奇迹的API信息
        /// </summary>
        /// <param name="Company_ID"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAccountInfos")]
        public GetAccountDtoInfos GetAccountInfo(AccountPara para)
        {
            GetAccountDtoInfos infos = new GetAccountDtoInfos();
            infos = r.GetAccountInfo(para);
            return infos;
        }

        ///<summary>
        ///查看奇迹对接的运营商信息
        /// </summary>
        [HttpGet]
        [Route("GetQijiAccountInfo")]
        public qijiaccountinfo GetQijiAccountInfo(string CompanyID)
        {
            qijiaccountinfo info = new qijiaccountinfo();
            info = r.GetQijiAccountInfo(CompanyID);
            return info;
        }

        ///<summary>
        ///编辑供应商信息
        /// </summary>
        [HttpPost]
        [Route("UpdateAccountInfo")]
        public Information UpdateAccountInfo(accounts para)
        {
            Information info = new Information();
            info = r.UpdateAccountInfo(para);
            return info;
        }

        ///<summary>
        ///删除供应商信息
        /// </summary>
        [HttpGet]
        [Route("DeleteAccountInfo")]
        public Information DeleteAccountInfo(int Id)
        {
            Information info = new Information();
            info = r.DeleteAccountInfo(Id);
            return info;
        }

        ///<summary>
        ///创建采购订单
        /// </summary>
        [HttpPost]
        [Route("AddPurchaseInfo")]
        public Information AddPurchaseInfo(purchase para)
        {
            Information info = new Information();
            info = r.AddPurchaseInfo(para);
            return info;
        }

        ///<summary>
        ///修改采购单
        /// </summary>
        [Route("UpdatePurchaseInfo")]
        [HttpPost]
        public Information UpdatePurchaseInfo(purchase para)
        {
            Information info = new Information();
            info = r.UpdatePurchaseInfo(para);
            return info;
        }

        ///<summary>
        ///删除采购单
        /// </summary>
        [HttpGet]
        [Route("DeletePurchaseInfo")]
        public Information DeletePurchaseInfo(int Id)
        {
            Information info = new Information();
            info = r.DeletePurchaseInfo(Id);
            return info;
        }

        ///<summary>
        ///查看采购单信息
        /// </summary>
        [HttpGet]
        [Route("GetPurchaseInfo")]
        public PurchaseDto GetPurchaseInfo(string PurchaseNo, string CompanyID)
        {
            PurchaseDto dto = new PurchaseDto();
            dto = r.GetPurchaseInfo(PurchaseNo, CompanyID);
            return dto;
        }

        ///<summary>
        ///查看采购单详情
        /// </summary>
        [HttpGet]
        [Route("GetPurchaseDetaliInfo")]
        public PurchaseDetailDto GetPurchaseDetaliInfo(string PurchaseNo)
        {
            PurchaseDetailDto dto = new PurchaseDetailDto();
            dto = r.GetPurchaseDetaliInfo(PurchaseNo);
            return dto;
        }
        #endregion
    }
}
