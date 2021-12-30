using Esim7.Action_OneLink_new;
using Esim7.CMCC.CMCCDAL;
using Esim7.CMCC.CMCCModel;
using Esim7.CUCC.CUCCModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Esim7.Controllers
{
    /// <summary>
    /// onelink新平台接口
    /// </summary>
    public class Onelink_NewController : ApiController
    {   /// <summary>
        /// 单卡基本信息查询
        ///查询物联卡码号信息、开卡时间、首次激活时间。
        /// </summary>
        /// <param name="status">查询方式</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetCMIOT_API23S00(string status, string value)
        {
            return Json(Action.Sim_Action.Get_CMIOT_API23S00(status, value));
        }
        /// <summary>
        /// 单卡基本信息查询
        ///查询物联卡码号信息、开卡时间、首次激活时间。
        /// </summary>
        /// <param name="status">查询方式</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetCMIOT_API23S004(string status, string value)
        {
            return Json(Action.Sim_Action.Get_CMIOT_API23S04(status, value));
        }
        /// <summary>
        /// 单卡状态查询  通过卡号查询物联卡的状态信息。
        /// </summary>
        /// <param name="status">查询方式</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetCMIOT_API25S004(string status, string value)
        {
            return Json(Action.Sim_Action.Get_CMIOT_API25S04(status, value));
        }
        /// <summary>
        /// 查询通用接口
        /// </summary>
        /// <param name="QueryType">查询类型 ICCID, IMSI, MSISDN</param>
        /// <param name="Value">类型值</param>
        /// <param name="URL">地址</param>
        /// <param name="APIName">api名称</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public IHttpActionResult currency(string QueryType, string Value, string APIName)
        {
            return Json(Action.Sim_Action.GetAPI_User_Class(QueryType, Value, APIName));
        }
        /// <summary>
        /// 测试接口
        /// </summary>
        /// <param name="Value">值</param>
        /// <param name="APINAME">API名称</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Test(string Value, string APINAME)
        {
            return Json(Action.Sim_Action.Get_NewOneLink_One(Value, APINAME));
        }
        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string getToken(string Valve)
        {
            return Action.Sim_Action.Token(Valve);
        }
        /// <summary>
        /// 新平台 单张卡   信息查询集合接口之 通信服务查询
        /// </summary>
        /// <param name="value"> 返回参数 ：   {"serverTypeLists":[{"ServiceType":"11","APNStatus":[{"ServiceStatus":"1","ApnName":"CMIOT"}]}],"status":"0","Mesage":"Success"} ServiceType：01 基础语音通信服务 08 短信基础服务 10 国际漫游服务 11 数据通信服务 ，ServiceStatus：0暂停 1 正常恢复开通等  </param>
        /// <returns>{"serverTypeLists":[{"ServiceType":"11","APNStatus":[{"ServiceStatus":"1","ApnName":"CMIOT"}]}],"status":"0","Mesage":"Success"} ServiceType：01 基础语音通信服务 08 短信基础服务 10 国际漫游服务 11 数据通信服务 ，ServiceStatus：0暂停 1 正常恢复开通等</returns>   
        [HttpGet]
        public IHttpActionResult Card_Information_FunctionalOpenQuery(string value)
        {
            return Json(Action_Onelink_New_CardMeaasge.GetFunctionalOpen(value));
        }
        /// <summary>
        /// 新平台 单张卡信息查询集合接口之 会话信息
        /// </summary>
        /// <param name="value">  {"ON_OFF":"1","APN":"CMIOT","IP":"10.111.252.133","OnlineState":"01","createDate":"2019-09-11 00:09:50","rat":"6","status":"0","Message":"Success"} 返回参数 ： {"ON_OFF":"1","APN":"CMIOT","IP":"10.111.252.133","OnlineState":"01","createDate":"2019-09-11 00:09:50","rat":"6","status":"0","Message":"Success"}    返回参数说明  ON_OFF：开关机状态 0 关机  1 开机 ，APN：APN名称， OnlineState：在线状态 00 离线 01 在线，createDate 会话创建时间 ，rat： 接入方式 1:3G、2:2G、6:4G、8:NB，status 接口返回状态 0 成功 1 失败 ，Message：  接口返回信息   </param>
        /// <returns>{"serverTypeLists":[{"ServiceType":"11","APNStatus":[{"ServiceStatus":"1","ApnName":"CMIOT"}]}],"status":"0","Mesage":"Success"} ServiceType：01 基础语音通信服务 08 短信基础服务 10 国际漫游服务 11 数据通信服务 ，ServiceStatus：0暂停 1 正常恢复开通等</returns>   

        [HttpGet]
        [Route("Card_information_Conversation")]
        public IHttpActionResult Card_information_Conversation(string value)
        {
            return Json(Action_Onelink_New_CardMeaasge.GetConversation(value));
        }
        /// <summary>
        /// 获取资费信息       返回参数      offeringId: 资费ID, offeringName:资费名称,effectiveDate: 生效时间,expiriedDate:失效时间,apnName:APN 名称     
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public IHttpActionResult Card_GetAccmMarginList(string value)
        {
            return Json(Action_Onelink_New_CardMeaasge.GetAccmMarginList(value));
        }


        ///<summary>
        ///查询移动新平台卡停机原因
        /// </summary>
        [HttpGet]
        [Route("GetCardStopInfo")]
        public IHttpActionResult GetCardStopInfo(string Card_ICCID)
        {
            return Json(Action_Onelink_New_CardMeaasge.GetCardStopInfo(Card_ICCID));
        }

        ///<summary>
        ///机卡分离状态查询
        /// </summary>
        [HttpGet]
        [Route("GetCMIOT_API23A04")]
        public string GetCMIOT_API23A04(string Card_ICCID)
        {
            string res = string.Empty;
            res = Action_Onelink_New_CardMeaasge.GetCMIOT_API23A04(Card_ICCID);
            return res;
        }
        ///<summary>
        ///位置定位类 获取移动NB卡的基站经纬度 参数value:卡号或ICCID或IMSI   返回值 lat:纬度 lon:经度
        /// </summary>
        [HttpGet]
        [Route("GetCmccNbAction")]
        public CMIOT_API25L00Root GetCmccNbAction(string Company_ID, string value)
        {
            CMIOT_API25L00Root root = new CMIOT_API25L00Root();
            CMCCAPIDAL dal = new CMCCAPIDAL();
            root = dal.GetCmccNbAction(Company_ID, value);
            return root;
        }

        [HttpGet]
        [Route("GetLocationAnalysis")]
        public baidunjxRoot GetLocationAnalysis(string lon, string lat)
        {
            baidunjxRoot t =new baidunjxRoot();
            t = CMCCAPIDAL.GetLocationAnalysis(lon,lat);
            return t;
        }

        [HttpGet]
        [Route("LonlatConversion")]
        public DTRoot LonlatConversion(string lon, string lat)
        {
            DTRoot t = new DTRoot();
            t = CMCCAPIDAL.LonlatConversion(lon,lat);
            return t;
        }

        ///<summary>
        ///CMIOT实名认证 物联卡实名登记请求基础版 value：输入卡号或ICCID或imsi
        ///返回值 busiSeq:活体实名登记业务流水号
        ///返回值 实名认证网页地址 用户在H5页面中提交身份信息，进行实名登记。实名登记结果将通过“CMIOT物联卡实名登记结果推送基础版推送”配置推送地址后获取，推送地址请联系客户经理申请维护。
        /// </summary>
        [HttpGet]
        [Route("GetCMIOT_API23A12")]
        public CMIOT_API23A12 GetCMIOT_API23A12(string value)
        {
            CMIOT_API23A12 cmiot = new CMIOT_API23A12();
            CMCCAPIDAL dal = new CMCCAPIDAL();
            cmiot = dal.GetCMIOT_API23A12(value);
            return cmiot;
        }

        ///<summary>
        ///CMIOT_API23A10-物联卡实名登记状态查询
        ///返回值 realNameStatus:实名状态： 1：已实名； 0：未实名；
        ///返回值 reason 实名原因： 01：登记到责任单位人和责任人信息 02：11位号码实名（除148号段） 05：订购语音或短信
        /// </summary>
        [HttpGet]
        [Route("GetCMIOT_API23A10")]
        public CMIOT_API23A10 GetCMIOT_API23A10(string value)
        {
            CMIOT_API23A10 t = new CMIOT_API23A10();
            CMCCAPIDAL dal = new CMCCAPIDAL();
            t = dal.GetCMIOT_API23A10(value);
            return t;
        }

        ///<summary>
        ///移动单卡状态变更 operType 0:申请停机(已激活转已停机) 1:申请复机(已停机转已激活) 2:库存转已激活 3:可测试转库存 4:可测试转待激活 5:可测试转已激活 6:待激活转已激活
        /// </summary>
        [HttpGet]
        [Route("UpdateCardStatusInfo")]
        public CMIOT_API23S03Root UpdateCardStatusInfo(string ICCID, string operType)
        {
            CMIOT_API23S03Root t = new CMIOT_API23S03Root();
            CMCCAPIDAL dal = new CMCCAPIDAL();
            t = dal.UpdateCardStatusInfo(ICCID,operType);
            return t;
        }

        ///<summary>
        ///物联卡区域限制状态查询
        /// </summary>
        [HttpGet]
        [Route("GetGuankongStatus")]
        public CMIOT_API23A11 GetGuankongStatus(string Company_ID, string Value)
        {
            CMIOT_API23A11 t = new CMIOT_API23A11();
            CMCCAPIDAL dal = new CMCCAPIDAL();
            t = dal.GetGuankongStatus(Company_ID,Value);
            return t;
        }
    }
}
