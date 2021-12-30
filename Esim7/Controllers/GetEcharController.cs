using Esim7.Dto;
using Esim7.Models;
using Esim7.ReturnMessage;
using System;
using System.Collections.Generic;
using System.Web.Http;
using static Esim7.Action.Action_GetEchars;

namespace Esim7.Controllers
{
    [RoutePrefix("Chars")]
    public class GetEcharController : ApiController
    {
        /// <summary>
        /// 获取卡总数 (废弃)
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        [Route("GetNumForCardCountnumber1")]
        [HttpGet]
        public IHttpActionResult GetNum(string CompanyID)
        {
            Return_Chars_CardNum r = new Return_Chars_CardNum();
            r.Cards = Action.Action_GetEchars.GetCardNUM(CompanyID);
            if (r.Cards.Count == 0)
            {
                r.Message = "未能找到相应的卡数据";
                r.status = "1";
            }
            else
            {
                r.Message = "Success";
                r.status = "0";
            }
            return Json<Return_Chars_CardNum>(r);
        }


        /// <summary>
        ///获取卡数量 新的
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        [Route("GetNumForCardCountnumber")]
        [HttpGet]
        public Return_Chars_CardNum GetCardNUM1(string CompanyID)
        {
            Return_Chars_CardNum c = new Return_Chars_CardNum();
            c = Action.Action_GetEchars.GetCardNUM1(CompanyID);
            return c;
        }

        /// <summary>
        ///获取通讯卡数量
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        [Route("GetCommunicationstate")]
        [HttpGet]

        public IHttpActionResult GetCommunicationstate(string CompanyID)
        {
            Return_Chars_Card_CommunicationState r = new Return_Chars_Card_CommunicationState();
            try
            {
                r.card_CommunicationStates = Action.Action_GetEchars.GetCommunicationStates(CompanyID);
                r.Message = "ok";
                r.status = "0";
            }
            catch (Exception ex)
            {
                r.Message = "error:" + ex;
                r.status = "1";
            }
            return Json<Return_Chars_Card_CommunicationState>(r);
        }
        /// <summary>
        /// 获取 物联卡账户状态 正常停机 其他
        /// 
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        [Route("GetAccountStates")]
        [HttpGet]
        public IHttpActionResult GetAccountStates(string CompanyID)
        {
            Return_Chars_Card_GetAccountState r = new Return_Chars_Card_GetAccountState();
            try
            {
                r.card_CommunicationStates = Action.Action_GetEchars.getAccountStates(CompanyID);
                r.Message = "ok";
                r.status = "0";
            }
            catch (Exception ex)
            {
                r.Message = "error:" + ex;
                r.status = "1";
            }
            return Json<Return_Chars_Card_GetAccountState>(r);
        }

        /// <summary>
        /// 获取物联卡计费状态
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        [Route("GetCharingNum")]
        [HttpGet]

        public IHttpActionResult GetCharingNum(string CompanyID)
        {
            Return_Chars_Charing_CardNum r = new Return_Chars_Charing_CardNum();
            try
            {
                r.Chars_Charing_CardNum = Action.Action_GetEchars.Get_Charing_CardNums(CompanyID);
                r.Message = "ok";
                r.status = "0";
            }
            catch (Exception ex)
            {
                r.Message = "error:" + ex;
                r.status = "1";
            }
            return Json<Return_Chars_Charing_CardNum>(r);
        }
        /// <summary>
        /// 获取  活跃度
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        [HttpGet, Route("GetActive")]
        public IHttpActionResult GetHuoyue(string CompanyID)
        {
            return Json(Action.Action_GetEchars.GetHuoYueDu(CompanyID));
        }

        /// <summary>
        ///获取使用流量
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        [HttpGet, Route("GetFlowUsed")]
        public IHttpActionResult GetFlowUsed(string CompanyID)
        {
            return Json(Action.Action_GetEchars.GetFlowUsed(CompanyID));
        }
        /// <summary>
        /// 获取每个公司每天的流量使用情况
        /// </summary>
        /// <param name="CompanyID">ID</param>               
        /// <returns></returns>      
        [HttpGet, Route("GetFlowDay")]
        public IHttpActionResult GetFlowDay(string CompanyID)
        {
            return Json(Action.Action_GetEchars.GetCompanyFlow_Day2(CompanyID));
        }


        #region 新写的统计面板处接口
        ///<summary>
        ///获取联通电信卡数量 卡NB数量
        /// </summary>
        [HttpGet]
        [Route("GetCardNumTypeInfo")]
        public CardTotalNumberTypeDto GetCardNumTypeInfo(string CompanyID)
        {
            CardTotalNumberTypeDto dto = new CardTotalNumberTypeDto();
            dto = Action.Action_GetEchars.GetCardNumTypeInfo1(CompanyID);
            return dto;
        }

        ///<summary>
        ///获取移动电信联通月流量使用数
        /// </summary>
        [HttpGet]
        [Route("GetCardMonthFlowUsed")]
        public CardMonthFlowDto GetCardMonthFlowUsed(string CompanyID)
        {
            CardMonthFlowDto dto = new CardMonthFlowDto();
            dto = Action.Action_GetEchars.GetCardMonthFlowUsed(CompanyID);
            return dto;
        }

        ///<summary>
        ///获取移动电信联通卡的数量和其他卡的数量
        /// </summary>
        public CardNumberDto GetCardNumberInfo(string CompanyID)
        {
            CardNumberDto dto = new CardNumberDto();
            dto = Action.Action_GetEchars.GetCardNumberInfo(CompanyID);
            return dto;
        }
        #endregion


        ///<summary>
        ///统计卡总数 NB卡总数
        /// </summary>
        public string countcard()
        {
            string s = Action.Action_GetEchars.countcard();
            return s;
        }

        ///<summary>
        ///获取移动卡近七天的流量数据
        /// </summary>
        [HttpGet]
        [Route("GetcountcardFlow")]
        public CardFlowDays GetcountcardFlow(string CompanyID)
        {
            CardFlowDays info = new CardFlowDays();
            info = Action.Action_GetEchars.GetcountcardFlow(CompanyID);
            return info;
        }

        ///<summary>
        ///获取移动电信联通卡总数量、NB总数量 OperatorsFlg 1:移动 2:电信 3:联通
        /// </summary>
        //[Route("GetNumForCardCountnumber2")]
        //[HttpGet]
        //public Return_Chars_CardNum GetCardNUM2(string CompanyID, string OperatorsFlg)
        //{
        //    Return_Chars_CardNum cardNum = new Return_Chars_CardNum();
        //    cardNum = Action.Action_GetEchars.GetCardNUM2(CompanyID, OperatorsFlg);
        //    return cardNum;
        //}

        /// <summary>
        /// 获取物联卡账户状态 正常 停机 其他 OperatorsFlg 1:移动 2:电信 3:联通
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        //[Route("getAccountStates1")]
        //[HttpGet]
        //public List<Chars_Card_GetAccountState> getAccountStates1(string CompanyID, string OperatorsFlg)
        //{
        //    List<Chars_Card_GetAccountState> li = new List<Chars_Card_GetAccountState>();
        //    li = Action.Action_GetEchars.getAccountStates1(CompanyID, OperatorsFlg);
        //    return li;
        //}

        /// <summary>
        /// 流量使用情况 OperatorsFlg 1:移动 2:电信 3:联通
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        //[Route("GetFlowUsed1")]
        //[HttpGet]
        //public List<MoonUsed> GetFlowUsed1(string CompanyID, string OperatorsFlg)
        //{
        //    List<MoonUsed> useds = new List<MoonUsed>();
        //    useds = Action.Action_GetEchars.GetFlowUsed1(CompanyID, OperatorsFlg);
        //    return useds;
        //}

        /// <summary>
        ///   获取活跃度 OperatorsFlg 1:移动 2:电信 3:联通
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        //[Route("GetHuoYueDu1")]
        //[HttpGet]
        //public List<Huoyue> GetHuoYueDu1(string CompanyID, string OperatorsFlg)
        //{
        //    List<Huoyue> huoyues = new List<Huoyue>();
        //    huoyues = Action.Action_GetEchars.GetHuoYueDu1(CompanyID, OperatorsFlg);
        //    return huoyues;
        //}


        ///<summary>
        ///主界面数据大集合  OperatorsFlg 1:移动 2:电信 3:联通
        /// </summary>
        [Route("GetIndexInfo")]
        [HttpGet]
        public IndexDto GetIndexInfo(string CompanyID, string OperatorsFlg)
        {
            IndexDto dto = new IndexDto();
            dto = Action.Action_GetEchars.GetIndexInfo(CompanyID,OperatorsFlg);
            return dto;
        }

        ///<summary>
        ///主界面数据返回用户总卡使用的近七天流量 按运营商 OperatorsFlg 1:移动 2:电信 3:联通
        /// </summary>
        [Route("GetIndexFlow")]
        [HttpGet]
        public CardFlowDays GetIndexFlow(string CompanyID, string OperatorsFlg)
        {
            CardFlowDays dto = new CardFlowDays();
            dto = Action.Action_GetEchars.GetIndexFlow(CompanyID,OperatorsFlg);
            return dto;
        }

        ///<summary>
        ///返回十张本月流量使用最多的卡号和流量值 OperatorsFlg 1:移动 2:电信 3:联通 4:全网通
        /// </summary>
        [HttpGet]
        [Route("GetMonthMaxFlow")]
        public  MaxFlowDto GetMonthMaxFlow(string CompanyID, string OperatorsFlg)
        {
            MaxFlowDto dto = new MaxFlowDto();
            dto = Action.Action_GetEchars.GetMonthMaxFlow(CompanyID,OperatorsFlg);
            return dto;
        }

        ///<summary>
        ///在线问题反馈
        /// </summary>
        [Route("SubmitFeedback")]
        [HttpPost]
        public  Information SubmitFeedback(feedback para)
        {
            Information info = new Information();
            info = Action.Action_GetEchars.SubmitFeedback(para);
            return info;
        }

        ///<summary>
        ///查看问题反馈信息
        /// </summary>
        [Route("GetFeedBackInfo")]
        [HttpGet]
        public  FeedBackDto GetFeedBackInfo(string Company_ID)
        {
            FeedBackDto dto = new FeedBackDto();
            dto = Action.Action_GetEchars.GetFeedBackInfo(Company_ID);
            return dto;
        }
    }
}
