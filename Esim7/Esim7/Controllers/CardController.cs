using Esim7.Action;
using Esim7.Dto;
using Esim7.IOT.IOTModel;
using Esim7.Models;
using Esim7.Models.Para;
using Esim7.ReturnMessage;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using static Esim7.Action.Action_UpdateCard_RealTime_Message;
using static Esim7.Action.CardAction;

namespace Esim7.Controllers
{       /// <summary>
/// 卡信息查看和操作
/// </summary>
    public class CardController : ApiController
    {
        /// <summary>
        /// 获取 卡片信息 单张卡
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("GetMessage")]
        [HttpGet]
        public IHttpActionResult Getmessage(string value, string Card_CompanyID)
        {
            Return_CardMessage r = new Return_CardMessage();
            try
            {
                r.Cards = CardAction.GetCards(value, Card_CompanyID);
                int count = r.Cards.Count;
                if (count < 1)
                {
                    r.flg = "1";
                    r.Msg = "卡的位数为20位请重新输入！";
                }
                else
                {
                    r.flg = "1";
                    r.Msg = "Success";
                }
            }
            catch (Exception ex)
            {
                r.flg = "0";
                r.Msg = "error" + ex;
            }
            return Json<Return_CardMessage>(r);
        }
        /// <summary>
        /// ch
        /// </summary>
        /// <param name="Card_CompanyID"></param>
        /// <param name="PagNumber"></param>
        /// <param name="Num"></param>
        /// <returns></returns>
        [Route("GetMessageForCompany")]
        [HttpGet]
        public IHttpActionResult GetmessageForCompany(string Card_CompanyID, int PagNumber, int Num)
        {
            Return_CardMessage3 r = new Return_CardMessage3();
            try
            {
                r.Cards = CardAction.GetCardsForCompany(Card_CompanyID, PagNumber, Num);
                r.flg = "1";
                r.Msg = "Success";
            }
            catch (Exception ex)
            {
                r.flg = "0";
                r.Msg = "error" + ex;
            }
            return Json(r);
        }

        ///<summary>
        ///异步调用
        /// </summary>
        /// <param name="Card_CompanyID">公司ID</param>
        /// <param name="cards"></param>
        /// <returns></returns>

        //[Route("GetExportData")]
        //[HttpGet]
        //public async Task<FilesPath> GetExportData(string Card_CompanyID, string OperatorsFlg)
        //{
        //    FilesPath r = new FilesPath();
        //    // r.Path = "eoeeo";
        //    //string r = string.Empty;
        //    try
        //    {
        //        r = CardAction.ExportData(Card_CompanyID, OperatorsFlg);
        //        r.Flage = "1";
        //        r.Message = "Success";
        //    }
        //    catch (Exception ex)
        //    {
        //        r.Flage = "0";
        //        r.Message = "error" + ex;
        //    }
        //    return r;
        //}
        /// <summary>
        /// 更新每月使用的流量
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        /// 
        [Route("GetFlow_Mon_Used")]
        [HttpGet]
        public IHttpActionResult GetFlow_Mon_Used(string Value)
        {
            return Json<Flow_M_Used>(GetGetFlow_Mom_One_Used(Value));
        }
        /// <summary>
        /// 批量信息查询   参数json {"CompanyID":"1556265186243","List_Cards":[{"card":"1440177235225"}]}
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("Query_Piliang")]
        public IHttpActionResult Renew_realDate()
        {
            return Json<object>(CardAction.Query_Piliang());
        }
        /// <summary>
        ///  更新  备注  
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("Update_Remarks")]
        public IHttpActionResult Update_Remarks()
        {
            return Json<object>(CardAction.Update_Remarks());
        }
        /// <summary>
        /// 更新 IMEI
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("Update_IMEI")]
        public IHttpActionResult Update_IMEI()
        {
            return Json<object>(CardAction.Update_IMEI());
        }
        /// <summary>
        ///    查询
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// 
        [HttpGet, Route("GetCard_Message_ForWeiChat")]
        public IHttpActionResult GetCard_Message_ForWeiChat(string value)
        {
            Return_CardMessage r = new Return_CardMessage();
            try
            {
                r.Cards = CardAction.GetCards(value, "1556265186243");
                r.flg = "1";
                r.Msg = "Success";
            }
            catch (Exception ex)
            {
                r.flg = "0";
                r.Msg = "error" + ex;
            }
            return Json<Return_CardMessage>(r);
        }
        /// <summary>
        /// 条件查询   IMSI  IMEI 备注  ICCID  卡号 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        [HttpPost, Route("Query_requirement")]
        public IHttpActionResult Query_requirement()
        {
            return Json(CardAction.Query_requirement());
        }

        ///<summary>
        ///导出物联网卡数据
        /// </summary>
        [HttpPost]
        [Route("GetExportData")]
        public FilesPath GetExportData(GetCardPara para)
        {
            FilesPath path = new FilesPath();
            path = CardAction.ExportData(para);
            return path;
        }
        ///<summary>
        ///根据ICCID电信卡的流量、卡状态、卡工作状态信息  Card_OperatorsFlg 2:电信  3:联通
        /// </summary>
        [HttpGet]
        [Route("GetCardStstusFlowInfo")]
        public CardStatusFlowInfoDto GetCardStstusFlowInfo(string Card_ICCID, string Card_OperatorsFlg)
        {
            CardStatusFlowInfoDto dto = new CardStatusFlowInfoDto();
            dto = CardAction.GetCardStstusFlowInfo(Card_ICCID, Card_OperatorsFlg);
            return dto;
        }
        ///<summary>
        ///移动新平台APN关停  备注:操作类接口请慎重使用
        /// </summary>
        [HttpPost]
        [Route("APNoffon")]
        public Information APNoffon(CardOffOnPara para)
        {
            Information info = new Information();
            info = CardAction.APNoffon(para);
            return info;
        }

        ///<summary>
        ///删除卡数据
        /// </summary>
        [HttpPost]
        [Route("DeleteCardInfo")]
        public Information DeleteCardInfo(CardDelPara para)
        {
            Information info = new Information();
            info = CardAction.DeleteCardInfo(para);
            return info;
        }

        ///<summary>
        ///根据卡号获取卡的IMEI和卡的近七天流量使用情况卡的近六个月流量使用情况 Card_OperatorsFlg 1：移动 2:电信
        /// </summary>
        [HttpGet]
        [Route("GetCardFlowImeiInfo")]
        public CardFlowImeiInfo GetCardFlowImeiInfo(string Card_ID, string Card_OperatorsFlg)
        {
            CardFlowImeiInfo info = new CardFlowImeiInfo();
            info = CardAction.GetCardFlowImeiInfo(Card_ID, Card_OperatorsFlg);
            return info;
        }

        ///<summary>
        ///查看漫游卡信息
        /// </summary>
        [HttpPost]
        [Route("GetRoamCardInfo")]
        public Query_RequirementMessage GetRoamCardInfo(GetRoamCardPara para)
        {
            Query_RequirementMessage info = new Query_RequirementMessage();

            info = CardAction.GetRoamCardInfo(para);
            return info;
        }

        ///<summary>
        ///月使用流量添加
        /// </summary>
        [HttpGet]
        [Route("sumnum")]
        public string sumnum()
        {
            string str = string.Empty;
            str = CardAction.sumnum();
            return str;
        }
        ///<summary>
        ///查看单卡的近六个月流量使用信息
        /// </summary>
        [HttpPost]
        [Route("GetCmccCardMonthFlowInfo")]
        public  CmccYearFlows GetCmccCardMonthFlowInfo(GetYearFlowPara para)
        {
            CmccYearFlows flows = new CmccYearFlows();
            flows = CardAction.GetCmccCardMonthFlowInfo(para);
            return flows;
        }

        ///<summary>
        ///物联网卡月使用量流量数据批量导出
        /// </summary>
        [HttpPost]
        [Route("MonthFlowExcel")]
        public string MonthFlowExcel(GetYearFlowPara para)
        {
            string path = string.Empty;
            path = CardAction.MonthFlowExcel(para);
            return path;
        }

        ///<summary>
        ///查看僵尸卡数据信息
        /// </summary>
        [HttpGet]
        [Route("GetDieCardInfo")]
        public DieCardDto GetDieCardInfo(string Company_ID, string Card_ICCID)
        {
            DieCardDto dto = new DieCardDto();
            dto = CardAction.GetDieCardInfo(Company_ID, Card_ICCID);
            return dto;
        }

        /// <summary>
        /// 用户登录弹出是否有僵尸卡信息
        /// </summary>
        [HttpGet]
        [Route("GetDieCardCountInfo")]
        public  DieCardCountInfo GetDieCardCountInfo(string Company_ID)
        {
            DieCardCountInfo info = new DieCardCountInfo();
            info = CardAction.GetDieCardCountInfo(Company_ID);
            return info;
        }

        ///<summary>
        ///生产抛料对比
        /// </summary>
        [HttpPost]
        [Route("GetPaoLiaoInfo")]
        public PaoLiaoDto GetPaoLiaoInfo(ICCIDSS para)
        {
            PaoLiaoDto info = new PaoLiaoDto();
            info = CardAction.GetPaoLiaoInfo(para);
            return info;
        }

        ///<summary>
        ///获取运营商
        /// </summary>
        [HttpGet]
        [Route("GetAccountInfo")]
        public  AccountDto GetAccountInfo(string OperatorID)
        {
            AccountDto dto = new AccountDto();
            dto = CardAction.GetAccountInfo(OperatorID);
            return dto;
        }

        ///<summary>
        ///区域管控查询
        /// </summary>
        [HttpGet]
        [Route("GetRegionInfo")]
        public Regions GetRegionInfo(string Company_ID, string Value)
        {
            Regions info = new Regions();
            info = CardAction.GetRegionInfo(Company_ID,Value);
            return info;
        }

        ///<summary>
        ///批量修改标签
        /// </summary>
        [HttpPost]
        [Route("UpdateRegionLable")]
        public Information UpdateRegionLable(UpdateRegionLablePara para)
        {
            Information info = new Information();
            info = CardAction.UpdateRegionLable(para);
            return info;
        }

        /// <summary>
        /// 查看单卡的会话信息列表近七次
        /// </summary>
        /// <param name="Card_ICCID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCardHuiHuaInfo")]
        public  GetCardHuihuaDto GetCardHuiHuaInfo(string Card_ICCID)
        {
            GetCardHuihuaDto dto = new GetCardHuihuaDto();
            dto = CardAction.GetCardHuiHuaInfo(Card_ICCID);
            return dto;
        }

        ///<summary>
        ///修改使用场景
        /// </summary>
        [HttpPost]
        [Route("UpdateCardScene")]
        public Information UpdateCardScene(UpdateRegionLablePara para)
        {
            Information info = new Information();
            info = CardAction.UpdateCardScene(para);
            return info;
        }
    }
}
