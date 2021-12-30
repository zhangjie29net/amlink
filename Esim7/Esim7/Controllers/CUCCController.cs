using Esim7.CUCC.CUCCDAL;
using Esim7.CUCC.CUCCModel;
using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Esim7.Controllers
{
    public class CUCCController : ApiController
    {
        CUCCAPIDAL r = new CUCCAPIDAL();
        ///<summary>
        ///联通卡会话信息 dateSessionEnded为空时在线 否则离线
        /// </summary>
        [HttpGet]
        [Route("CuccCardWorkstatus")]
        public CuccCardWorkStatus CuccCardWorkstatus(string ICCID)
        {
            CuccCardWorkStatus t = new CuccCardWorkStatus();
            t = r.GetCuccCardWorkStatus(ICCID);
            return t;
        }
        ///<summary>
        ///查看卡的状态(测试期、正常使用、停机等状态)
        /// </summary>
        [HttpGet]
        [Route("CuccCardStatus")]
        public CUCCCardStatus CuccCardStatus(string ICCID)
        {
            CUCCCardStatus t = new CUCCCardStatus();
            t = r.CuccCardStatus(ICCID);
            return t;
        }
        ///<summary>
        ///获取联通卡的使用流量 KB
        /// </summary>
        [HttpGet]
        [Route("GetCuccCardFlow")]
        public CuccCardFlow GetCuccCardFlow(string ICCID)
        {
            CuccCardFlow t = new CuccCardFlow();
            t = r.GetCuccCardFlow(ICCID);
            return t;
        }

        ///<summary> 
        ///修改联通卡状态 Status：已激活：Activated   已停用：Deactivated
        /// </summary>
        [HttpGet]
        [Route("UpdateCuccStatus")]
        public Information UpdateCuccStatus(string ICCID, string Status)
        {
            Information info = new Information();
            info = r.UpdateCuccStatus(ICCID,Status);
            return info;
        }
    }
}
