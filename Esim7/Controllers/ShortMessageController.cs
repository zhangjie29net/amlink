using CMPP;
using Esim7.Action;
using Esim7.Models;
using Esim7.Models.ShortMessageModel;
using Esim7.parameter.ShortMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Esim7.Controllers
{
    public class ShortMessageController : ApiController
    {
        ShortMessageAction r = new ShortMessageAction();
        ///<summary>
        ///添加模板
        /// </summary>
        [HttpPost]
        [Route("CreateTemplate")]
        public Information CreateTemplate(CreateTemplatePara para)
        {
            Information info = new Information();
            info = r.CreateTemplate(para);
            return info;
        }

        ///<summary>
        ///修改模板信息
        /// </summary>
        [HttpPost]
        [Route("EditTemplate")]
        public Information EditTemplate(EditTemplatePara para)
        {
            Information info = new Information();
            info = r.EditTemplate(para);
            return info;
        }

        ///<summary>
        ///查看模板信息
        /// </summary>
        [HttpGet]
        [Route("GetTemplateInfo")]
        public ShortMessageDto GetTemplateInfo(string Company_ID)
        {
            ShortMessageDto dto = new ShortMessageDto();
            dto = r.GetTemplateInfo(Company_ID);
            return dto;
        }

        ///<summary>
        ///删除模板信息
        /// </summary>
        [HttpGet]
        [Route("DeleteTemplateInfo")]
        public Information DeleteTemplateInfo(int Id)
        {
            Information info = new Information();
            info = r.DeleteTemplateInfo(Id);
            return info;
        }

        ///<summary>
        ///发送短信
        /// </summary>
        [HttpPost]
        [Route("SendoutMessage")]
        public Information SendoutMessage(SendoutMessagePara para)
        {
            Information info = new Information();
            info = r.SendoutMessage(para);
            return info;
        }

        ///<summary>
        ///查看下行短信发送记录
        /// </summary>
        //[HttpGet]
        //[Route("GetShotMessage")]
        //public CMIOT_API25C02 GetShotMessage(string Value)
        //{
        //    CMIOT_API25C02 t = new CMIOT_API25C02();
        //    t = r.GetShotMessage(Value);
        //    return t;
        //}
        ///<summary>
        ///查看物联网卡上行短信发送记录（卡回复内容）
        /// </summary>


        /// <summary>
        /// 获取用户短信发送日志
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("GetShortMessageLog")]
        //public ShortMessageLogDto GetShortMessageLog(GetShortMessagePara para)
        //{
        //    ShortMessageLogDto t = new ShortMessageLogDto();
        //    t = r.GetShortMessageLog(para);
        //    return t;
        //}

        ///<summary>
        ///查询上行短信记录
        /// </summary>
        //[HttpGet]
        //[Route("upshortmessageinfo")]
        //public CMIOT_API25C03 upshortmessageinfo(string Value, string startDate, string endDate)
        //{
        //    CMIOT_API25C03 t = new CMIOT_API25C03();
        //    t = r.upshortmessageinfo(Value, startDate, endDate);
        //    return t;
        //}

        ///<summary>
        ///发送网关短信
        /// </summary>
        //[HttpGet]
        //[Route("SendShortMessage")]
        //public string SendShortMessage(string CardNum,string Content)
        //{
        //    string t = "";
        //    t = r.SendShortMessge(CardNum,Content);
        //    return t;
        //}

        ///<summary>
        ///获取回复短信信息
        /// </summary>
        //[HttpPost]
        //[Route("GetShortMessageInfo")]
        //public GetShortMessageDto GetShortMessageInfo(ShortMessagePara para)
        //{
        //    GetShortMessageDto dto = new GetShortMessageDto();
        //    dto = r.GetShortMessageInfo(para);
        //    return dto;
        //}

        ///<summary>
        ///获取短信详细信息
        /// </summary>
        [HttpPost]
        [Route("GetShortMeaages")]
        public getShortMessageInfos GetShortMeaages(GetShotrPara para)
        {
            getShortMessageInfos dto = new getShortMessageInfos();
            dto = r.GetShortMeaages(para);
            return dto;
        }


        //[HttpGet]
        //public GetShortMessageDetailDto GetShortMeaages(string Company_ID, string CardID)
        //{
        //    GetShortMessageDetailDto dto = new GetShortMessageDetailDto();
        //    dto = r.GetShortMeaages(Company_ID, CardID);
        //    return dto;
        //}
    }
}
