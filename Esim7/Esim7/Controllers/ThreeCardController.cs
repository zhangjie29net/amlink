using Esim7.Action;
using Esim7.Dto;
using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Esim7.Controllers
{
    /// <summary>
    /// 三合一卡
    /// </summary>
    public class ThreeCardController : ApiController
    {
        ThreeCardAction r = new ThreeCardAction();
        ///<summary>
        ///入库上传上传 奇迹将卡上传至三合一表中
        /// </summary>
        [HttpPost]
        [Route("AddQjThreeCardInfo")]
        public Information AddQjThreeCardInfo(ThreeCardStockPara para)
        {
            Information info = new Information();
            info = r.AddQjThreeCardInfo(para);
            return info;
        }

        ///<summary>
        ///修改三合一卡套餐
        /// </summary>
        [HttpPost]
        [Route("UpdateThreeCardSetmalInfo")]
        public Information UpdateThreeCardSetmalInfo(UpdateThreeCardSetmeal para)
        {
            Information info = new Information();
            info = r.UpdateThreeCardSetmalInfo(para);
            return info;
        }

        ///<summary>
        ///给用户分配三合一卡
        /// </summary>
        [HttpPost]
        [Route("ThreeCardToCustom")]
        public Information ThreeCardToCustom(ThreeCardCopyPara para)
        {
            Information info = new Information();
            info = r.ThreeCardToCustom(para);
            return info;
        }

        ///<summary>
        ///查看三合一卡信息(用户和奇迹物联共用)
        /// </summary>
        [HttpPost]
        [Route("GetThreeCardInfo")]
        public ThreeCardInfoDto GetThreeCardInfo(GetThreeCardPara para)
        {
            ThreeCardInfoDto dto = new ThreeCardInfoDto();
            dto = r.GetThreeCardInfo(para);
            return dto;
        }

        ///<summary>
        ///查看三合一卡详细信息
        /// </summary>
        [HttpGet]
        [Route("GetThreeCardDetailInfo")]
        public ThreeDetail GetThreeCardDetailInfo(string SN)
        {
            ThreeDetail detail = new ThreeDetail();
            detail = r.GetThreeCardDetailInfo(SN);
            return detail;
        }

        /// <summary>
        /// 比对两个集合数据取不同
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //[Route("sss")]
        //public string sss(duibijihepara para)
        //{
        //    string s = "";
        //    s = r.sss(para);
        //    return s;
        //}

        ///<summary>
        ///返回数据
        /// </summary>
        [HttpGet]
        [Route("testlayui")]
        public TestLayuiModel testlayui(string Card_CompanyID, int PagNumber, int Num)
        {
            TestLayuiModel t = new TestLayuiModel();
            t = r.testlayui(Card_CompanyID, PagNumber, Num);
            return t;
        }

        ///<summary>
        ///导出三合一卡信息
        /// </summary>
        [Route("ThreeExportData")]
        [HttpGet]
        public FilesPath ThreeExportData(string Card_CompanyID)
        {
            FilesPath path = new FilesPath();
            path = r.ThreeExportData(Card_CompanyID);
            return path;
        }

    }
}
