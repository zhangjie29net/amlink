using Esim7.CT;
using Esim7.CT.CTDAL;
using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Esim7.Controllers
{

    public class CTController : ApiController
    {
        CTAPIDAL r = new CTAPIDAL();
        ///<summary>
        ///查询企业月使用量 电信 
        /// </summary>
        [Route("FlowInfo")]
        public Monthlyflow FlowInfo(string method, string user_id, string date, string type)
        {
            Monthlyflow t = new Monthlyflow();
            t = r.FlowInfo(method, user_id, date, type);
            return t;
        }

        ///<summary>
        ///流量查询（当天）接口
        /// </summary>
        [Route("xmltest")]
        public string xmltest()
        {
            string c = "";
            c = r.xmltest();
            return c;
        }

        ///<summary>
        ///获取电信卡的月使用流量
        /// </summary>
        [HttpGet]
        [Route("GetCuccCardMonthFlow")]
        public string GetCuccCardMonthFlow(string ICCID)
        {
            string c = "";
            c = r.GetCuccCardMonthFlow(ICCID);
            return c;
        }

        ///<summary>
        ///获取卡的工作状态(在线/离线)
        /// </summary>
        [HttpGet]
        [Route("GetCTCardWorkStatus")]
        public RootCTCardWorkStatus GetCTCardWorkStatus(string ICCID)
        {
            RootCTCardWorkStatus c = new RootCTCardWorkStatus();
            c = r.GetCTCardWorkStatus(ICCID);
            return c;
        }

        ///<summary>
        ///查询卡的状态 1：可激活 2：测试激活 3:测试去激活 4:在用 5:停机 6:运营商状态管理
        /// </summary>
        [HttpGet]
        [Route("GetCTCardStatus")]
        public RootCTStatus GetCTCardStatus(string ICCID)
        {
            RootCTStatus c = new RootCTStatus();
            c = r.GetCTCardStatus(ICCID);
            return c;
        }

        ///<summary>
        ///三码互查接口 根据电信卡iccid号码去物联网卡号
        /// </summary>
        [HttpGet]
        [Route("GetCTCard_ID")]
        public string GetCTCard_ID(string ICCID)
        {
            string c = "";
            c = r.GetCTCard_ID(ICCID);
            return c;
        }

        ///<summary>
        ///基站定位
        /// </summary>
        [HttpGet]
        [Route("Location1")]
        public string Location1(string access_number)
        {
            string c = "";
            c = r.Location1(access_number);
            return c;
        }

        ///<summary>
        ///基站定位 新
        /// </summary>
        [HttpGet]
        [Route("Location")]
        public LocationInfo Location(string access_number)
        {
            LocationInfo info = new LocationInfo();
            info = r.Location(access_number);
            return info;
        }

        ///<summary>
        ///电信修改状态停机保号 orderTypeId 19：停机保号  20：停机保号复机
        /// </summary>
        [HttpGet]
        [Route("UpdateCtStatus")]
        public Information UpdateCtStatus(string Card_ICCID, string orderTypeId)
        {
            Information info = new Information();
            info = r.UpdateCtStatus(Card_ICCID,orderTypeId);
            return info;
        }

        ///<summary>
        ///电信机卡重绑
        /// </summary>
        [HttpGet]
        [Route("SetJiKaChongBang")]
        public Rootjkcb SetJiKaChongBang(string ICCID)
        {
            Rootjkcb root = new Rootjkcb();
            root = r.SetJiKaChongBang(ICCID);
            return root;
        }


        ///<summary>
        ///奇迹电信卡基站定位
        /// </summary>
        [HttpGet]
        [Route("GetQiJiLocation")]
        public YiYuanLocationInfo GetYiYuanLocation(string access_number)
        {
            YiYuanLocationInfo info = new YiYuanLocationInfo();
            info = r.GetYiYuanLocation(access_number);
            return info;
        }

        ///<summary>
        ///奇迹电信卡月使用流量
        /// </summary>
        [HttpGet]
        [Route("GetQiJiCtFlow")]
        public YiYuanRootFlow GetYiYuanCtFlow(string month)
        {
            YiYuanRootFlow flow = new YiYuanRootFlow();
            flow = r.GetYiYuanCtFlow(month);
            return flow;
        }
        /// <summary>
        /// 奇迹电信卡工作状态
        /// </summary>
        /// <param name="Card_ICCID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetYiYuanWorkStatusInfo")]
        public YiYuanWorkStatusRoot GetYiYuanWorkStatusInfo(string Card_ICCID)
        {
            YiYuanWorkStatusRoot dto = new YiYuanWorkStatusRoot();
            dto = r.GetYiYuanWorkStatusInfo(Card_ICCID);
            return dto;
        }

        ///<summary>
        ///资产详细信息
        /// </summary>
        [HttpGet]
        [Route("YiYuanCardDetail")]
        public YiYuanCardDetailDto YiYuanCardDetail(string iccid)
        {
            YiYuanCardDetailDto dto = new YiYuanCardDetailDto();
            dto = r.YiYuanCardDetail(iccid);
            return dto;
        }

        ///<summary>
        ///电信NB基站定位支持奇迹和电信两种 access_number ICCID或者卡号
        /// </summary>
        [HttpGet]
        [Route("GetCTLocationInfo")]
        public LocationInfo GetCTLocationInfo(string access_number)
        {
            LocationInfo info = new LocationInfo();
            info = r.GetCTLocationInfo(access_number);
            return info;
        }
    }

}

