using Esim7.LargeFlow.LargeFlowDAL;
using Esim7.LargeFlow.LargeFlowModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Esim7.Controllers
{
    public class LargeFlowController : ApiController
    {
        LargeFlowApiDAL r = new LargeFlowApiDAL();
        /// <summary>
        /// 获取单卡详细信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("LargeFLowCardDetail")]
        public LargeFlowDetailDto LargeFLowCardDetail(string iccid)
        {
            LargeFlowDetailDto t = new LargeFlowDetailDto();
            //string t = string.Empty;
            t = r.LargeFLowCardDetail(iccid);
            return t;
        }

        /// <summary>
        /// 批量卡状态查询 多个卡用,隔开最多支持100张卡
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("query_status")]
        public LargerFlowCardstatusDto query_status(string iccid)
        {
            LargerFlowCardstatusDto t = new LargerFlowCardstatusDto();
            t = r.query_status(iccid);
            return t;
        }

        ///<summary>
        ///批量当前周期流量查询 多个 ICCID，按逗号分割，最多支持 100 个
        /// </summary>
        [HttpGet]
        [Route("current_cycle_traffic_use")]
        public string current_cycle_traffic_use(string iccid)
        {
            string result = "";
            result = r.current_cycle_traffic_use(iccid);
            return result;
        }

        ///<summary>
        ///转码
        /// </summary>
        [HttpGet]
        [Route("sssssss")]
        public string ss()
        {
            string sss = r.ss();
            return sss;
        }
    }
}
