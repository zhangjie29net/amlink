using Aop.Api.Response;
using Esim7.Action;
using Esim7.Dto;
using Esim7.Models;
using Esim7.parameter.PayManage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Esim7.Controllers
{
    /// <summary>
    /// 支付管理
    /// </summary>
    public class PayManageController : ApiController
    {
        PayManageAction r = new PayManageAction();
        ///<summary>
        ///添加支付信息
        /// </summary>
        [HttpPost]
        [Route("AddPayInfo")]
        public Information AddPayInfo(AddPayInfoPara para)
        {
            Information info = new Information();
            info = r.AddPayInfo(para);
            return info;
        }

        ///<summary>
        ///查看支付信息  奇迹物联所有权限 Status -1:待支付 0:已支付待审核  1:到账通过审核 2:未到账未通过审核 
        /// </summary>
        //[HttpGet]
        //[Route("GetPayInfos1")]
        //public GetPayInfoDto GetPayInfos1(string Company_ID, string Status)
        //{
        //    GetPayInfoDto dto = new GetPayInfoDto();
        //    dto = r.GetPayInfos1(Company_ID,Status);
        //    return dto;
        //}

        ///<summary>
        ///查看支付信息  奇迹物联所有权限 Status 0:未审核  1:已审核 2:未通过审核 （废弃）
        /// </summary>
        //[HttpPost]
        //[Route("GetPayInfos1")]
        //public GetPayInfoDto GetPayInfos1(PayInfoPara para)
        //{
        //    GetPayInfoDto dto = new GetPayInfoDto();
        //    dto = r.GetPayInfos2(para);
        //    return dto;
        //}

        ///<summary>
        ///查看支付信息 
        /// </summary>
        [HttpGet]
        [Route("GetPayInfos")]
        public OrderPayInfoDto GetPayInfos(string Company_ID, string Status)
        {
            OrderPayInfoDto dto = new OrderPayInfoDto();
            dto = r.GetPayInfos(Company_ID, Status);
            return dto;
        }

        ///<summary>
        ///奇迹审核到账操作  0:待审核  1:已审核  2:未通过审核
        /// </summary>
        [HttpPost]
        [Route("PayExamine")]
        public Information PayExamine(PayExaminePara para)
        {
            Information info = new Information();
            info = r.PayExamine(para);
            return info;
        }

        /// <summary>
        /// 创建账单
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddPayOrder")]
        public Information AddPayOrder(CreateOrderPara para)
        {
            Information info = new Information();
            info = r.AddPayOrder(para);
            return info;
        }

        #region 当面付
        ///<summary>
        ///统一收单线下交易预创建（扫码支付）
        /// </summary>
        [Route("PayRequest")]
        public AlipayTradePrecreateResponse PayRequest()
        {
            AlipayTradePrecreateResponse alipay = new AlipayTradePrecreateResponse();
            alipay = r.PayRequest();
            return alipay;
        }

        ///<summary>
        ///统一收单线下交易查询 (扫码支付)
        /// </summary>
        [Route("TradePayResponse")]
        public AlipayTradeQueryResponse TradePayResponse()
        {
            AlipayTradeQueryResponse alipayTrade = new AlipayTradeQueryResponse();
            alipayTrade = r.TradePayResponse();
            return alipayTrade;
        }
        #endregion

        #region  电脑网站支付
        ///<summary>
        ///统一收单下单并支付页面接口
        /// </summary>
        [Route("TradePagePayResponse")]
        public AlipayTradePagePayResponse TradePagePayResponse()
        {
            AlipayTradePagePayResponse response = new AlipayTradePagePayResponse();
            response = r.TradePagePayResponse();
            return response;
        }

        #endregion


        #region 在线续费
        ///<summary>
        ///查看客户的套餐列表
        /// </summary>
        //[HttpGet]
        //[Route("GetCustomSetMealDto1")]
        //public SetMealDto GetCustomSetMealDto1(string Company_ID)
        //{
        //    SetMealDto dto = new SetMealDto();
        //    dto = r.GetCustomSetMealDto1(Company_ID);
        //    return dto;
        //}

        ///<summary>
        ///查看客户的套餐列表
        /// </summary>
        [HttpGet]
        [Route("GetCustomSetMealDto")]
        public CustomerSetMealDto GetCustomSetMealDto(string Company_ID)
        {
            CustomerSetMealDto dto = new CustomerSetMealDto();
            dto = r.GetCustomSetMealDto(Company_ID);
            return dto;
        }

        ///<summary>
        ///设置套餐价格和用户  setflg 1:添加 2:编辑
        /// </summary>
        [HttpPost]
        [Route("SetUpapackageInfo")]
        public Information SetUpapackageInfo(SetUpapackagePara para)
        {
            Information info = new Information();
            info = r.SetUpapackageInfo(para);
            return info;
        }

        ///<summary>
        ///查看用户的套餐信息列表
        /// </summary>
        [HttpGet]
        [Route("GetSetUpapackageInfo")]
        public GetSetUpapackageDto GetSetUpapackageInfo(string CompanyID)
        {
            GetSetUpapackageDto dto = new GetSetUpapackageDto();
            dto = r.GetSetUpapackageInfo(CompanyID);
            return dto;
        }
        #endregion

        /// <summary>
        /// 用户上传支付图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UserPayImgFile")]
        public Task<HttpResponseMessage> UserPayImgFile()
        {
            // Check if the request contains multipart/form-data.  
            // 检查该请求是否含有multipart/form-data  
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            string root = this.PubFileRootPath;
            var provider = new MultipartFormDataStreamProvider(root);
            // 最大文件大小
            const int maxSize = 10000000;
            DateTime dat = DateTime.Now;
            string Tpath = string.Format("/{0}/{1}/{2}", dat.Year, dat.Month, dat.Day);
            string FilePath = root + "\\";//+ Tpath + "\\";
            DirectoryInfo di = new DirectoryInfo(FilePath);
            if (!di.Exists) { di.Create(); }
            // 读取表单数据，并返回一个async任务  
            var task = Request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<HttpResponseMessage>(t =>
                {
                    if (t.IsFaulted || t.IsCanceled)
                    {
                        Request.CreateErrorResponse(HttpStatusCode.InternalServerError, t.Exception);
                    }
                    // 以下描述了如何获取文件名  
                    string fileName = string.Empty;
                    List<PayFileName> files = new List<PayFileName>();
                    foreach (MultipartFileData fileitem in provider.FileData)
                    {
                        PayFileName res = new PayFileName();
                        var fileGuid = Guid.NewGuid();
                        string FileName = fileGuid.ToString("N");
                        res.fileName = FileName;
                        fileName = FileName;
                        string fileFullName = string.Empty;
                        fileFullName = string.Format("{0}{1}", FilePath, FileName);
                        //保存文件到路径 
                        //var file = fileitem;// provider.FileData[0];
                        var file = provider.FileData[0];
                        var fileinfo = new System.IO.FileInfo(file.LocalFileName);
                        fileinfo.MoveTo(fileFullName);
                        files.Add(res);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { success = true, fileName });
                });
            return task;
        }

        ///<summary>
        /// 公共文件目录
        /// </summary>
        public string PubFileRootPath
        {
            get
            {
                var mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/");
                return mappedPath + "UploadUserPayFiles";
            }
        }
        public class PayFileName
        {
            public string fileName { get; set; }
        }

        #region 微信支付

        #endregion
    }
}
