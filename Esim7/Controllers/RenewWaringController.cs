using Esim7.Action;
using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Esim7.Controllers
{
    public class RenewWaringController : ApiController
    {
        /// <summary>
        /// 客户续费预警  客户登录时显示
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        [HttpGet,Route("Waring_ForCustom")]
        public IHttpActionResult Get_renew_card(string CompanyID)
        {
            return Json<object>(Action.Action_RenewWaring.Waring_ForCustom(CompanyID));
        }
        /// <summary>
        ///     客户续费预警  客户登录时显示 点击张数时显示
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <returns></returns>
        [HttpGet,Route("Card_ForCustom_CardsMessage")]
        public IHttpActionResult Card_ForWarings(string CompanyID)
        {
            return Json<object>(Action.Action_RenewWaring.Card_ForWarings(CompanyID));
        }
        /// <summary>
        /// 客户续费预警 
        /// </summary>
        /// <param name="Card_CompanyID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Card_ForCustom_CardsMessage2")]
        public IHttpActionResult Card_ForWarings2(string Card_CompanyID)
        {
            return Json<object>(Action.Action_RenewWaring.Waring_total(Card_CompanyID));
        }
        ///<summary>
        ///客户续费预警查看详细信息
        /// </summary>
        [HttpGet]
        [Route("Waring_totalDetail")]
        public IHttpActionResult Waring_totalDetail(string Card_CompanyID)
        {
            return Json<object>(Action.Action_RenewWaring.Waring_totalDetail(Card_CompanyID));
        }
        /// <summary>
        /// 流量预警（之前的暂时废弃）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Flow_Warming1()
        {
           return  Json(Action.Action_RenewWaring.Flow_Waring());
        }

        ///<summary>
        ///流量预警新增参数
        /// </summary>
        [HttpGet]
        //[Route("Flow_Waring")]
        public IHttpActionResult Flow_Warming(string CompanyID)
        {
            return Json(Action.Action_RenewWaring.Flow_Waring1(CompanyID));
        }

        ///<summary>
        ///设置流量资费提醒规则
        /// </summary>
        [HttpPost]
        [Route("SetRules")]
        public Information SetRules(SetRulesPara para)
        {
            Information info = new Information();
            info = Action.Action_RenewWaring.SetRules(para);
            return info;
        }

        /// <summary>
        /// 查看设置提示信息
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetSetRulesInfos")]
        public SetRulesInfoDto GetSetRulesInfos(SetRulesInfoPara para)
        {
            SetRulesInfoDto info = new SetRulesInfoDto();
            info= Action.Action_RenewWaring.GetSetRulesInfos(para);
            return info;
        }

        ///<summary>
        ///查看设置了提示规则信息的详情(卡列表)
        ///</summary>
        [HttpGet]
        [Route("GerSetRulesDetail")]
        public RulesDetailInfos GerSetRulesDetail(string RelationCode)
        {
            RulesDetailInfos info = new RulesDetailInfos();
            info = Action.Action_RenewWaring.GerSetRulesDetail(RelationCode);
            return info;
        }

        ///<summary>
        ///删除预警规则
        ///</summary>
        [HttpGet]
        [Route("DeleteSetRules")]
        public Information DeleteSetRules(string RelationCode)
        {
            Information info = new Information();
            info = Action.Action_RenewWaring.DeleteSetRules(RelationCode);
            return info;
        }

        ///<summary>
        ///编辑预警规则
        /// </summary>
        [HttpPost]
        [Route("UpdateSetRules")]
        public Information UpdateSetRules(UpdateRulesPara para)
        {
            Information info = new Information();
            info = Action.Action_RenewWaring.UpdateSetRules(para);
            return info;
        }

        ///<summary>
        ///查看机卡分离数量
        /// </summary>
        [HttpGet]
        [Route("GetFenLiInfo")]
        public GetFenLiNum GetFenLiInfo(string CompanyID)
        {
            GetFenLiNum info = new GetFenLiNum();
            info = Action.Action_RenewWaring.GetFenLiInfo(CompanyID);
            return info;
        }
    }
}
