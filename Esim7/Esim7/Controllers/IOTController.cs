using Esim7.IOT.IOTDAL;
using Esim7.IOT.IOTModel;
using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using static Esim7.Action.Action_UpdateCard_RealTime_Message;
using static Esim7.Action.CardAction;

namespace Esim7.Controllers
{
    public class IOTController : ApiController
    {
        IOTAPIDAL r = new IOTAPIDAL();
        ///<summary>
        ///漫游卡月初至今日凌晨的累计用量 
        /// </summary>
        /// <returns>开始时间yyyy-mm-dd</returns>
        /// <returns>结束时间yyyy-mm-dd</returns>
        [HttpGet]
        [Route("RoveMonthFlow")]
        public IOTMonthFlow RoveMonthFlow(string startdate, string enddate)
        {
            IOTMonthFlow c = new IOTMonthFlow();
            c = r.RoveMonthFlow(startdate, enddate);
            return c;
        }

        ///<summary>
        ///获取当天实时流量
        /// </summary>
        [HttpGet]
        [Route("DayFlow")]
        public IOTDayFlow DayFlow(string id, string type)
        {
            IOTDayFlow c = new IOTDayFlow();
            c = r.DayFlow(id, type);
            return c;
        }


        ///<summary>
        ///获取短信的用量
        /// </summary>
        [HttpGet]
        [Route("InvokeService")]
        public string ShorMessageNum(string startdate, string enddate)
        {
            string s = "";
            s = r.ShorMessageNum(startdate, enddate);
            return s;
        }


        ///<summary>
        ///获取卡的状态信息
        /// </summary>
        [HttpGet]
        [Route("GetMyCardStatus")]
        public string GetMyCardStatus(string Card_ICCD)
        {
            string s = "";
            s = r.GetMyCardStatus(Card_ICCD);
            return s;
        }
        ///<summary>
        ///根据物联网卡号或ICCID获取卡的流量状态等相关信息
        /// </summary>
        public IotCardInfoDto GetCardDtatilInfo(string Value)
        {
            IotCardInfoDto info = new IotCardInfoDto();
            info = r.GetCardDtatilInfo(Value);
            return info;
        }

        ///<summary>
        ///获取当天实时流量
        /// </summary>
        [HttpGet]
        [Route("GetRoamDayFlow")]
        public string GetRoamDayFlow(string Card_ID)
        {
            string s = "";
            s = r.GetRoamDayFlow(Card_ID);
            return s;
        }

        ///<summary>
        ///更新漫游卡的状态和流量数据  Value 卡号
        /// </summary>
        [Route("GetUpdateRoamFlowStatus")]
        [HttpGet]
        public Flow_M_Used GetUpdateRoamFlowStatus(string Value)
        {
            Flow_M_Used used = new Flow_M_Used();
            used = r.GetUpdateRoamFlowStatus(Value);
            return used;
        }

        #region 众讯API对接
        ///<summary>
        ///单个号码状态查询
        /// </summary>
        [HttpGet]
        [Route("GetDanGeCardStatus")]
        public XunZhongCardStatusRoot GetDanGeCardStatus(string Card_ICCID)
        {
            XunZhongCardStatusRoot ss=new  XunZhongCardStatusRoot();
            ss = r.GetDanGeCardStatus(Card_ICCID);
            return ss;
        }

        ///<summary>
        ///号码信息查询
        /// </summary>
        [HttpGet]
        [Route("GetXunZhongCardInfo")]
        public XunZhongCardInfoRoot GetXunZhongCardInfo(string Card_ICCID)
        {
            //string root = string.Empty;
            XunZhongCardInfoRoot root = new XunZhongCardInfoRoot();
            root = r.GetXunZhongCardInfo(Card_ICCID);
            return root;
        }

        ///<summary>
        ///基站定位
        /// </summary>
        [HttpGet]
        [Route("GetZhongXunLocation")]
        public ZhongXunLocationRoot GetZhongXunLocation(string Card_ICCID)
        {
            ZhongXunLocationRoot root = new ZhongXunLocationRoot();
            root = r.GetZhongXunLocation(Card_ICCID);
            return root;
        }
        #endregion

        ///<summary>
        ///获取酷宅订单数据 count获取数量  groupflg分组标识
        /// </summary>
        [HttpGet]
        [Route("GetHouzhaiData")]
        public string GetHouzhaiData(string count, string groupflg)
        {
            string ss = null;
            ss = r.GetHouzhaiData(count, groupflg);
            return ss;
        }

        ///<summary>
        ///wifi定位
        /// </summary>
        [HttpGet]
        [Route("wifidw")]
        public string wifidw()
        {
            string ss = null;
            ss = r.wifidw();
            return ss;
        }

        

        ///<summary>
        ///生成EXCEL
        /// </summary>
        [HttpGet]
        [Route("GetExcel")]
        public string GetExcel()
        {
            string path = "";
            path = r.GetExcel();
            return path;
        }


        ///<summary>
        ///酷宅烧录数据上传
        /// </summary>
        [HttpPost]
        [Route("updatekuzhaidata")]
        public  Information updatekuzhaidata(UploadkzPara para)
        {
            Information info = new Information();
            info = r.updatekuzhaidata(para);
            return info;
        }

    }
}
