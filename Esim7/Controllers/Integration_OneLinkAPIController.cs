using Esim7.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Esim7.Controllers
{            /// <summary>
///新的平台 单张卡信息集合查询
/// </summary>
    public class Integration_OneLinkAPIController : ApiController
    {

        #region  物联网卡信息管理 ——信息
        [Route("GetAccountInformation_Full")]
        [HttpGet]
        public IHttpActionResult GetAccountInformation_Full(string imsi)
        {



            return Json<object>(Action__Onelink_jihe.accountInformation_Full(imsi));



        }
        /// <summary>
        /// 卡账户信息
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        [Route("GetAccountInformation_Main")]
        [HttpGet]
        public IHttpActionResult GetAccountInformation_Main(string imsi)
        {



            return Json<object>(Action__Onelink_jihe.information_Main(imsi));



        }
        /// <summary>
        /// 通信状态
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        [Route("GetCommunicateStatus")]
        [HttpGet]
        public IHttpActionResult GetCommunicateStatus(string imsi) {

            return Json<object>(Action__Onelink_jihe.communicateStatus(imsi));



        }
        /// <summary>
        /// 资费信息
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        [Route("GetChargesinformation")]
        [HttpGet]
        public IHttpActionResult Get2037(string imsi) {


            return Json<object>(APIACTION.Get_CMIOT_API2037(imsi));


        }

        #endregion











    }
}
