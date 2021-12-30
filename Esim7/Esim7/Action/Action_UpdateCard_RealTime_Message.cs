using Dapper;
using Esim7.Action_OneLink_new;
using Esim7.CMCC.CMCCDAL;
using Esim7.CMCC.CMCCModel;
using Esim7.IOT.IOTDAL;
using Esim7.IOT.IOTModel;
using Esim7.LargeFlow.LargeFlowDAL;
using Esim7.LargeFlow.LargeFlowModel;
using Esim7.Model_New_Onelink_API;
using Esim7.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using static Esim7.Action.Sim_Action;
using static Esim7.SimModel.API;

namespace Esim7.Action
{    /// <summary>
/// 获取实时信息 用量信息 
/// </summary>
    public class Action_UpdateCard_RealTime_Message
    {

        public class Message
        {
            public string Card_ID { get; set; }

            public string Platform { get; set; }

        }
     
        #region     使用到的API接口

        static string APPID = ConfigurationManager.AppSettings["APPID"];
        static string PASSWORD = ConfigurationManager.AppSettings["PASSWORD"];
        static string TOKEN = ConfigurationManager.AppSettings["TOKEN"];
        static string TRANSID = ConfigurationManager.AppSettings["TRANSID"];

        static string URL = "https://api.iot.10086.cn/v2/";

        static string ss;

        #region 流量使用情况
        //1. 流量使用情况    :旧平台 CMIOT_API2005-用户当月GPRS查询       新平台          CMIOT_API25U04     单卡本月流量累计使用量查询
        /// <summary>
        /// CMIOT_API2005-用户当月GPRS查询
        /// 集团客户可查询所属物联卡当月截止到前一天24点为止的GPRS使用量（单位：KB）。
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        public static Root_CMIOT_API2005 Get_CMIOT_API2005(string imsi)
        {

            List<Acount> acc = new List<Acount>();
            foreach (Acount item in Action.Action_GetAccountsMessage.acount(imsi))
            {
                APPID = item.APPID;
                TRANSID = item.TRANSID;
                TOKEN = item.TOKEN;
            }
            // https://api.iot.10086.cn/v2/gprsusedinfosingle?appid=xxx&transid=xxx&ebid=xxx&token=xxx&imsi=xxx -以imsi进行查询 
            string EBID = "0001000000012";
            if (imsi.Length == 15)
            {
                ss = URL + "gprsusedinfosingle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&imsi=" + imsi;
            }
            else if (imsi.Length == 20)
            {
                ss = URL + "gprsusedinfosingle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&iccid=" + imsi;
            }
            else if (imsi.Length == 13)
            {
                ss = URL + "gprsusedinfosingle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + imsi;
            }
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ss);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return JsonConvert.DeserializeObject<Root_CMIOT_API2005>(reader.ReadToEnd());
            }
        }
        /// <summary>
        /// 单月使用流量
        /// </summary>
        /// <param name="Value">ICCID 或IMSI等</param>
        /// <returns></returns>
        public static Flow_Mom_One.Root GetFlow_Mom_One(string Value)
        {
            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(Esim7.Action.Sim_Action.Get_NewOneLink_One(Value, "API25U04"));
            return serializer.Deserialize<Flow_Mom_One.Root>(serializer.Deserialize<returnMessage>(json).API);
        }
        ///<summary>
        ///卡在线离线状态   旧平台
        /// </summary>        
        public static Root_CMIOT_API12001 GetCMIOT_API12001(string imsi)
        {
            List<Acount> acc = new List<Acount>();
            foreach (Acount item in Action.Action_GetAccountsMessage.acount(imsi))
            {
                APPID = item.APPID;
                TRANSID = item.TRANSID;
                TOKEN = item.TOKEN;
            }
            string EBID = "0001000000008";
            if (imsi.Length == 15)
            {
                ss = URL + "gprsrealsingle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&imsi=" + imsi;
            }
            else if (imsi.Length == 20)
            {
                ss = URL + "gprsrealsingle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&iccid=" + imsi;
            }
            else if (imsi.Length == 13)
            {
                ss = URL + "gprsrealsingle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + imsi;
            }
            //https://api.iot.10086.cn/v2/gprsrealsingle?appid=xxx&transid=xxx&ebid=xxx&token=xxx&msisdn=xxx -以msisdn进行查询
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ss);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return JsonConvert.DeserializeObject<Root_CMIOT_API12001>(reader.ReadToEnd());
            }
        }
        /// <summary>
        /// 卡在线离线状态 新平台
        /// </summary>
        /// <param name="Value">ICCID 或IMSI等</param>
        /// <returns></returns>
        public static OnLine.Root GetCard_WorkStatus(string Value)
        {
            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(Esim7.Action.Sim_Action.Get_NewOneLink_One(Value, "API25M01"));
            return serializer.Deserialize<OnLine.Root>(serializer.Deserialize<returnMessage>(json).API);
        }

        /// <summary>
        ///  CMIOT_API2002-用户状态信息实时查询  集团客户可根据所属物联卡的码号信息实时查询该卡的状态信息。
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        public static Root_CMIOT_API2002 GetCMIOT_API2002(string imsi)
        {
            List<Acount> acc = new List<Acount>();
            foreach (Acount item in Action.Action_GetAccountsMessage.acount(imsi))
            {
                APPID = item.APPID;
                TRANSID = item.TRANSID;
                TOKEN = item.TOKEN;
            }
            string EBID = "0001000000009";
            //  https://api.iot.10086.cn/v2/userstatusrealsingle?appid=xxx&ebid=xxx&transid=xxx&token=xxx&imsi=xxx –以imsi进行查询
            if (imsi.Length == 15)
            {
                ss = URL + "userstatusrealsingle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&imsi=" + imsi;
            }
            else if (imsi.Length == 20)
            {
                ss = URL + "userstatusrealsingle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&iccid=" + imsi;
            }
            else if (imsi.Length == 13)
            {
                ss = URL + "userstatusrealsingle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + imsi;
            }
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ss);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return JsonConvert.DeserializeObject<Root_CMIOT_API2002>(reader.ReadToEnd());
            }
        }

        ///<summary>
        ///卡状态查询 新平台
        /// </summary>
        public static CMIOT_API25S041.Root GetCard_Status(string Value)
        {
            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(Esim7.Action.Sim_Action.Get_NewOneLink_One(Value, "API25S04"));
            return serializer.Deserialize<CMIOT_API25S041.Root>(serializer.Deserialize<returnMessage>(json).API); 
        }

        /// <summary>
        ///流量集合  接口(太慢废弃)
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static Flow_M_Used GetGetFlow_Mom_One_Used1(string Value)
        {
            Flow_M_Used flow_M_Used = new Flow_M_Used();
            LargeFlowApiDAL lfdal = new LargeFlowApiDAL();
            LargeFlowDetailDto lfdto = new LargeFlowDetailDto();
            string Flow = "";
            string Card_WorkState = "";
            string Card_State = "";
            string Platform = "";
            string Card_ID = "";
            string sql2;
            string s = "";
            if (Value.Length == 20)
            {
                s = "Card_ICCID=@Value";

            }
            else if (Value.Length == 15)
            {
                s = "Card_IMSI=@Value";
            }
            else if (Value.Length == 13)
            {
                s = "Card_ID=@Value";
            }
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                List<Message> li = new List<Message>();
                sql2 = @"select Card_ID ,Platform from card where   " + s;
                li = conn.Query<Message>(sql2, new { Value = Value }).ToList();
                try
                {
                    if (li.Count == 1)
                    {
                        foreach (Message item in li)
                        {
                            Platform = item.Platform;
                            Card_ID = item.Card_ID;
                        }
                        //移动旧平台
                        if (Platform == "10")
                        {
                            foreach (ResultItem_CMIOT_API2005 item in Get_CMIOT_API2005(Card_ID).result)
                            {
                                Flow = item.total_gprs;
                            }
                            foreach (CMIOT_API12001 items in GetCMIOT_API12001(Card_ID).result)
                            {
                                Card_WorkState = items.GPRSSTATUS;//卡的工作状态
                            }
                            foreach (var itemss in GetCMIOT_API2002(Card_ID).result)
                            {
                                Card_State = itemss.STATUS;
                            }
                        }
                        else if (Platform == "11")
                        {

                            foreach (Flow_Mom_One.ResultItem item in GetFlow_Mom_One(Card_ID).result)
                            {
                                Flow = item.dataAmount;
                            }
                            foreach (OnLine.ResultItem items in GetCard_WorkStatus(Card_ID).result)
                            {
                                Card_WorkState = items.simSessionList[0].status;//卡的工作状态
                            }
                            foreach (var itemss in GetCard_Status(Card_ID).result)
                            {
                                Card_State = itemss.cardStatus;//卡的状态
                            }
                        }
                        else if (Platform == "51")//移动大流量卡
                        {
                            string Card_ICCID = string.Empty;
                            string sqlcardiccid = "select Card_ICCID from card where Card_ID='"+Card_ID+"'";
                            using (IDbConnection conns = DapperService.MySqlConnection())
                            {
                                Card_ICCID = conns.Query<Card>(sqlcardiccid).Select(t => t.Card_ICCID).FirstOrDefault();
                                if (!string.IsNullOrWhiteSpace(Card_ICCID))
                                {
                                    lfdto = lfdal.LargeFLowCardDetail(Card_ICCID);
                                    if (lfdto.code == "0")
                                    {
                                        if (lfdto.data.status == "in_stock")//在库
                                        {
                                            Card_State = "7";
                                        }
                                        if (lfdto.data.status == "test_ready")//可测试
                                        {
                                            Card_State = "6";
                                        }
                                        if (lfdto.data.status == "tested")//测试结束
                                        {
                                            Card_State = "06";
                                        }
                                        if (lfdto.data.status == "wait_activated")//待激活
                                        {
                                            Card_State = "1";
                                        }
                                        if (lfdto.data.status == "activated")//已激活
                                        {
                                            Card_State = "2";
                                        }
                                        if (lfdto.data.status == "stopped")//停卡
                                        {
                                            Card_State = "4";
                                        }
                                        if (lfdto.data.status == "disconnected")//断网
                                        {
                                            Card_State = "06";
                                        }
                                        if (lfdto.data.status == "canceled")//销卡
                                        {
                                            Card_State = "9";
                                        }
                                        Card_WorkState = lfdto.data.online_status;//卡工作状态
                                        Flow = lfdto.data.traffic_use.ToString();
                                    }
                                }
                            }
                        }
                        flow_M_Used.Card_Monthlyusageflow = Flow; ;
                        flow_M_Used.Card_WorkState = Card_WorkState;
                        flow_M_Used.Card_State = Card_State;
                        flow_M_Used.Message = "接口调取成功";
                        flow_M_Used.status = "0";
                        flow_M_Used.Success = true;
                    }
                    else
                    {
                        flow_M_Used.Card_Monthlyusageflow = "NULL";
                        flow_M_Used.Message = "接口调取成功，未能查找到数据";
                        flow_M_Used.status = "0";
                        flow_M_Used.Success = false;
                    }
                }
                catch (Exception  ex )
                {
                    flow_M_Used.Card_Monthlyusageflow = "NULL";
                    flow_M_Used.Message = "服务器内部错误";
                    flow_M_Used.status = "500";
                    flow_M_Used.Success = false;

                }
            }
            return flow_M_Used;
        }
        #endregion


        #region 移动获取流量卡状态卡工作状态
        public static Flow_M_Used GetGetFlow_Mom_One_Used(string Value)
        {
            Flow_M_Used flow_M_Used = new Flow_M_Used();
            CMCCAPIDAL cmcc = new CMCCAPIDAL();
            CMCCRootToken crt = new CMCCRootToken();//移动token接收
            NewCMCCCardStatus ncs = new NewCMCCCardStatus();//移动新平台卡状态接收
            NewCMCCCardWorkStatus nws = new NewCMCCCardWorkStatus();//移动新平台卡工作状态接收
            NewCMCCCardMonthFlow nwf = new NewCMCCCardMonthFlow();//移动卡新平台卡月使用流量
            OldCmccCardStatus ocs = new OldCmccCardStatus();//移动老平台卡状态接收
            OldCmccCardWorkStatus ocw = new OldCmccCardWorkStatus();//移动老平台卡工作状态
            OldCmccCardMonthFlow ocf = new OldCmccCardMonthFlow();//移动老平台卡月使用流量
            LargeFlowDetailDto lfdto = new LargeFlowDetailDto();//大流量卡
            LargeFlowApiDAL lfdal = new LargeFlowApiDAL();
            IOTAPIDAL iotapi = new IOTAPIDAL();//讯众卡
            XunZhongCardInfoRoot zxroot = new XunZhongCardInfoRoot();//众讯卡状态使用流量接收类
            string APPID = string.Empty;
            string TRANSID = string.Empty;
            string EBID = string.Empty;
            string TOKEN = string.Empty;
            string Card_ICCID = string.Empty;
            string YearFlow = string.Empty;//年包卡的累计流量数据
            string PackageDescribe = string.Empty;//套餐名称
            int YearFlg = 0;
            double Flow = 0;
            try
            {
                //获取卡ICCID和是否大流量
                string sqlcardinfo = "select * from card where Card_ID='"+Value+"'";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    //判断是否是年包的卡
                    string sqlyear = "select t1.SetmealID2,t2.PackageDescribe,t1.YearFlow from card t1 left join setmeal t2 on t1.SetmealID2=t2.SetmealID where t1.Card_ID='" + Value+"'";
                    var yearinfo = conn.Query<Card>(sqlyear).FirstOrDefault();
                    if (yearinfo!=null)
                    {
                        if (yearinfo.PackageDescribe.Contains("年"))
                        {
                            YearFlg = 1;
                            if (string.IsNullOrWhiteSpace(yearinfo.YearFlow))
                            {
                                yearinfo.YearFlow = "0";
                            }
                            else
                            {
                                YearFlow = yearinfo.YearFlow;
                                Flow = Convert.ToDouble(YearFlow);
                            }
                        }
                    }
                    var cardinfo = conn.Query<Card>(sqlcardinfo).FirstOrDefault();
                    if (cardinfo != null)
                    {
                        Card_ICCID = cardinfo.Card_ICCID;
                        string sqlaccount = "select * from accounts where accountID='"+cardinfo.accountsID+"'";
                        var accountinfo = conn.Query<accounts>(sqlaccount).FirstOrDefault();
                        if (accountinfo != null)
                        {
                            APPID = accountinfo.APPID;
                            TRANSID = accountinfo.TRANSID;
                            TOKEN = accountinfo.TOKEN;
                        }
                        #region 老平台
                        if (cardinfo.Platform == "10")
                        {
                            EBID = "0001000000009";//卡的状态  00-正常；01-单向停机；02-停机；03-预销号；04-销号；05-过户；06-休眠；07-待激活；99-号码不存在
                            ocs = cmcc.GetOldCmccCardStatus(APPID, TRANSID, EBID, TOKEN, Card_ICCID);
                            if (ocs.status == "0")
                            {
                                flow_M_Used.Card_State = ocs.result[0].STATUS;
                            }
                            EBID = "0001000000008";//卡的工作状态
                            ocw = cmcc.GetOldCmccCardWorkStatus(APPID, TRANSID, EBID, TOKEN, Card_ICCID);
                            if (ocw.status == "0")
                            {
                                flow_M_Used.Card_WorkState = ocw.result[0].GPRSSTATUS;
                            }
                            EBID = "0001000000012";//卡的月流量
                            ocf = cmcc.GetOldCmccCardMonthFlow(APPID, TRANSID, EBID, TOKEN, Card_ICCID);
                            if (YearFlg == 1)//年包卡
                            {
                                if (ocf.status == "0")
                                {
                                    if (string.IsNullOrWhiteSpace(ocf.result[0].total_gprs))
                                    {
                                        ocf.result[0].total_gprs = "0";
                                        flow_M_Used.Card_Monthlyusageflow = Flow.ToString();
                                    }
                                    else
                                    {
                                        Flow = Flow + Convert.ToDouble(ocf.result[0].total_gprs);
                                        flow_M_Used.Card_Monthlyusageflow = Flow.ToString();
                                    }
                                } 
                            }
                            if(YearFlg==0)
                            {
                                if (ocf.status == "0")
                                {
                                    flow_M_Used.Card_Monthlyusageflow = ocf.result[0].total_gprs;
                                }
                            }                     
                        }
                        #endregion

                        #region 新平台
                        if (cardinfo.Platform == "11")
                        {
                            crt = cmcc.GetToken(Card_ICCID);//新平台TOKEN
                            if (crt.status == "0")
                            {
                                TOKEN = crt.result[0].token;
                            }
                            ncs = cmcc.GetNewCmccCardStatus(TOKEN, APPID, Card_ICCID);//卡状态
                            if (ncs.status == "0")//移动新平台卡状态
                            {
                                flow_M_Used.Card_State = ncs.result[0].cardStatus;
                            }
                            nws = cmcc.GetNewCmccCardWrokStatus(TOKEN, APPID, Card_ICCID);//卡工作状态
                            if (nws.status == "0")
                            {
                                flow_M_Used.Card_WorkState = nws.result[0].simSessionList[0].status;
                            }
                            nwf = cmcc.GetNewCmccCardMonthFlow(TOKEN, APPID, Card_ICCID);//卡的月使用流量
                            if (YearFlg == 1)//年包卡
                            {
                                if (nwf.status == "0")
                                {
                                    if (string.IsNullOrWhiteSpace(nwf.result[0].dataAmount))
                                    {
                                        nwf.result[0].dataAmount = "0";
                                        flow_M_Used.Card_Monthlyusageflow = Flow.ToString();
                                    }
                                    else
                                    {
                                        Flow = Flow + Convert.ToDouble(nwf.result[0].dataAmount);
                                        flow_M_Used.Card_Monthlyusageflow = Flow.ToString();
                                    }
                                }   
                            }
                            if(YearFlg==0)
                            {
                                if (nwf.status == "0")
                                {
                                    flow_M_Used.Card_Monthlyusageflow = nwf.result[0].dataAmount;
                                }
                            }
                        }
                        #endregion
                        #region 众讯移动卡
                        if (cardinfo.Platform == "61")
                        {
                            zxroot = iotapi.GetXunZhongCardInfo(Card_ICCID);//卡信息
                            if (zxroot.errorCode == "0")
                            {
                                decimal flow = zxroot.data.flowsUsed * 1024;
                                flow_M_Used.Card_Monthlyusageflow = flow.ToString();
                                //1.待激活 2.启用  4.停机 5.销号 7.测试期 8.库存 
                                if (zxroot.data.status == 1)
                                {
                                    flow_M_Used.Card_State = "1";
                                }
                                if (zxroot.data.status == 2)
                                {
                                    flow_M_Used.Card_State = "2";
                                }
                                if (zxroot.data.status == 4)
                                {
                                    flow_M_Used.Card_State = "4";
                                }
                                if (zxroot.data.status == 5)
                                {
                                    flow_M_Used.Card_State = "8";
                                }
                                if (zxroot.data.status == 7)
                                {
                                    flow_M_Used.Card_State = "6";
                                }
                                if (zxroot.data.status == 8)
                                {
                                    flow_M_Used.Card_State = "7";
                                }
                            }
                        }
                        #endregion

                        #region 移动大流量卡
                        lfdto = lfdal.LargeFLowCardDetail(Card_ICCID);
                        if (lfdto.code == "0")
                        {
                            if (lfdto.data.status == "in_stock")//在库
                            {
                                flow_M_Used.Card_State = "7";
                            }
                            if (lfdto.data.status == "test_ready")//可测试
                            {
                                flow_M_Used.Card_State = "6";
                            }
                            if (lfdto.data.status == "tested")//测试结束
                            {
                                flow_M_Used.Card_State = "14";
                            }
                            if (lfdto.data.status == "wait_activated")//待激活
                            {
                                flow_M_Used.Card_State = "1";
                            }
                            if (lfdto.data.status == "activated")//已激活
                            {
                                flow_M_Used.Card_State = "2";
                            }
                            if (lfdto.data.status == "stopped")//停卡
                            {
                                flow_M_Used.Card_State = "4";
                            }
                            if (lfdto.data.status == "disconnected")//断网
                            {
                                flow_M_Used.Card_State = "15";
                            }
                            if (lfdto.data.status == "canceled")//销卡
                            {
                                flow_M_Used.Card_State = "9";
                            }
                            flow_M_Used.Card_WorkState = lfdto.data.online_status;//卡工作状态
                            flow_M_Used.Card_Monthlyusageflow = lfdto.data.traffic_use.ToString();
                        }
                        #endregion
                        flow_M_Used.Message = "接口调取成功";
                        flow_M_Used.status = "0";
                        flow_M_Used.Success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                flow_M_Used.Card_State = null;
                flow_M_Used.Card_WorkState = null;
                flow_M_Used.Card_Monthlyusageflow = null;
                flow_M_Used.Success = false;
                flow_M_Used.status = "500";
                flow_M_Used.Message = "接口调取失败"+ex;
            }
            return flow_M_Used;
        }
        #endregion





        #endregion



        #region  集合接口

        #endregion

        #region   用到的类
        /// <summary>
        /// 使用的流量 单张卡  每个月
        /// </summary>
        public class Flow_M_Used
        {
            /// <summary>
            /// 使用的流量
            /// </summary>
            public string Card_Monthlyusageflow { get; set; }     
            ///<summary>
            ///卡的工作状态
            /// </summary>
            public string Card_WorkState { get; set; }
            ///<summary>
            ///卡的状态
            /// </summary>
            public string Card_State { get; set; }
            /// <summary>
            /// 信息
            /// </summary>
            public string Message { get; set; }
            /// <summary>
            /// 接口调用成功标志
            /// </summary>
            public string status { get; set; }
            /// <summary>
            /// 获取数据正确判定
            /// </summary>
            public bool Success { get; set; }
        }
        #endregion
    }
}