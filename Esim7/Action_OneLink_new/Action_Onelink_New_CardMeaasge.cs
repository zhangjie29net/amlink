using Dapper;
using Esim7.CMCC.CMCCDAL;
using Esim7.Model_New_Onelink_API;
using Esim7.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using static Esim7.Action.Sim_Action;
using static Esim7.Model_New_Onelink_API.FunctionalOpenQuery;
using static Esim7.Model_New_Onelink_API.Model_New_OneLink_CombinationClass;


namespace Esim7.Action_OneLink_new
{               /// <summary>
/// 新版本SimLink 平台 的Sim卡详细信息   包括旧平台 新平台的集合接口 
/// </summary>
    public class Action_Onelink_New_CardMeaasge
    {
        #region 新的Onelink平台接口对接        sim卡详细信息  界面
        /// <summary>
        /// 会话信息  对应新平台单张卡查询的会话信息   直接调取的未经处理的 字符
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static ConversationMessage GetConversationMessage(string Value)
        {
            ConversationMessage conversationMessage = new ConversationMessage();
            var serializer = new JavaScriptSerializer();

            var json = serializer.Serialize(Esim7.Action.Sim_Action.Get_NewOneLink_One(Value, "API23S04"));
            conversationMessage.GETIMEI = serializer.Deserialize<GetIMEI.Root>(serializer.Deserialize<returnMessage>(json).API);
            json = serializer.Serialize(Esim7.Action.Sim_Action.Get_NewOneLink_One(Value, "API25M00"));
            conversationMessage.On_OFF = serializer.Deserialize<On_OFF.Root>(serializer.Deserialize<returnMessage>(json).API);
            json = serializer.Serialize(Esim7.Action.Sim_Action.Get_NewOneLink_One(Value, "API25M01"));
            conversationMessage.GetOnLine = serializer.Deserialize<OnLine.Root>(serializer.Deserialize<returnMessage>(json).API);
            return conversationMessage;
        }
        /// <summary>
        ///  功能开通查询    通信功能服务： 01 基础语音通信服务 08 短信基础服务 10 国际漫游服务 11 数据通信服务    API23M08
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static FunctionalOpenQuery.Root GetFunctionalOpenQuery(string Value)
        {
            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(Esim7.Action.Sim_Action.Get_NewOneLink_One(Value, "API23M08"));
            return serializer.Deserialize<FunctionalOpenQuery.Root>(serializer.Deserialize<returnMessage>(json).API);
        }
        /// <summary>
        /// 资费订购实时查询 根据用户类型（企业、群组、sim卡）查询已订购的所有资费列表。   API23R00   
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static SubscribedFee.Root GetFunctionalOpenQuery2(string Value)
        {
            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(Esim7.Action.Sim_Action.Get_NewOneLink_Two(Value, "API23R00", "queryType", "3"));
            return serializer.Deserialize<SubscribedFee.Root>(serializer.Deserialize<returnMessage>(json).API);
        }
        #endregion
        #region  新旧平台结合 移动  单张卡查询    SIM卡详细信息

        #region  1 通讯服务

        #region 1.1  通讯服务 分步
        /// <summary>
        /// 获取APN
        /// </summary>
        /// <param name="ICCID"></param>
        /// <returns></returns>
        public static Models.APN.Root GetAPN(string ICCID)
        {


            string APPID = "", TRANSID = "", TOKEN = "", ss = "", URL = "https://api.iot.10086.cn/v2/";

            List<Acount> acc = new List<Acount>();
            foreach (Acount item in Action.Action_GetAccountsMessage.acount(ICCID))
            {

                APPID = item.APPID;
                TRANSID = item.TRANSID;
                TOKEN = item.TOKEN;


            }
            //https://api.iot.10086.cn/v2/querycardlifecycle?appid=xxx&transid=xxx&ebid=xxx&token=xxx&iccid=xxx –以iccid进行查询
            // https://api.iot.10086.cn/v2/querycardlifecycle?appid=xxx&transid=xxx&ebid=xxx&token=xxx&imsi=xxx –以imsi进行查询

            string EBID = "0001000000431";

            if (ICCID.Length == 20)
            {
                ss = URL + "queryapnopenstatus?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&iccid=" + ICCID;
            }
            else if (ICCID.Length == 15)
            {
                ss = URL + "queryapnopenstatus?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&imsi=" + ICCID;
            }
            else if (ICCID.Length == 13)
            {
                ss = URL + "queryapnopenstatus?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + ICCID;
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


                return JsonConvert.DeserializeObject<Models.APN.Root>(reader.ReadToEnd());
            }






        }
        /// <summary>
        ///  CMIOT_API2107-单个用户已开通服务查询
        /// 集团客户可以通过卡号（仅MSISDN）查询物联卡当前的服务开通状态
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        public static Root_CMIOT_API2107 GetCMIOT_API2107(string imsi)
        {
            string APPID = "", TRANSID = "", TOKEN = "", ss = "", URL = "https://api.iot.10086.cn/v2/";
            List<Acount> acc = new List<Acount>();
            foreach (Acount item in Action.Action_GetAccountsMessage.acount(imsi))
            {

                APPID = item.APPID;
                TRANSID = item.TRANSID;
                TOKEN = item.TOKEN;
            }
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                string sql2 = "";
                if (imsi.Length == 15)
                {
                    sql2 = "select Card_ID from card where Card_IMSI=@Card_IMSI ";
                }
                else if (imsi.Length == 20)
                {
                    sql2 = "select Card_ID from card where Card_ICCID=@Card_IMSI ";
                }
                else if (imsi.Length == 13)
                {
                    sql2 = "select Card_ID from card where  Card_ID=@Card_IMSI ";
                }
                List<Card> list = conn.Query<Card>(sql2, new { Card_IMSI = imsi }).ToList();
                foreach (Card item in list)
                {
                    imsi = item.Card_ID;
                }
            }
            // https://api.iot.10086.cn/v2/useropenservice?appid=xxx&transid=xxx&ebid=xxx&token=xxx&msisdn=xxx –以msisdn进行查询
            string EBID = "0001000000447";
            ss = URL + "useropenservice?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + imsi;
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ss);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return JsonConvert.DeserializeObject<Root_CMIOT_API2107>(reader.ReadToEnd());
            }
        }

        #endregion
        #region   1.2 通讯服务结合

        public class Message
        {
            public string Card_ID { get; set; }

            public string Platform { get; set; }

        }
        /// <summary>
        ///    通讯服务结合
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static FunctionalOpen GetFunctionalOpen(string Value)
        {
            List<ServerTypeList> list = new List<ServerTypeList>();
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
                if (li.Count == 1)
                {
                    foreach (Message item in li)
                    {
                        Platform = item.Platform;
                        Card_ID = item.Card_ID;
                    }
                }                 
            }
            try
            {
                if (Platform == "10")
                {
                    FunctionalOpen f = new FunctionalOpen();
                    f.serverTypeLists = new List<ServerTypeList>();
                    Models.APN.Root r1 = new Models.APN.Root();
                    Root_CMIOT_API2107 r2 = new Root_CMIOT_API2107();
                    r2 = GetCMIOT_API2107(Value);
                    r1 = GetAPN(Value);
                    //List<ResultItem_CMIOT_API2107> r2_result = new List<ResultItem_CMIOT_API2107>();
                    //r2_result = r2.result; 
                    foreach (ResultItem_CMIOT_API2107 item in r2.result)
                    {
                        if (int.Parse(item.issignCall) > 0)
                        {
                            ServerTypeList ss = new ServerTypeList();
                            ss.ServiceType = "01";// 语音服务 
                            foreach (Models.APN.ResultItem items in r1.result)
                            {
                                ServerAPN apn = new ServerAPN();
                                apn.ApnName = items.apnname;
                                switch (items.status)
                                {
                                    case "01":
                                        items.status = "1";
                                        break;
                                    default:
                                        items.status = "0";
                                        break;
                                }
                                apn.ServiceStatus = items.status;
                                ss.APNStatus = new List<ServerAPN>();
                                ss.APNStatus.Add(apn);
                            }
                            f.serverTypeLists = new List<ServerTypeList>();
                            f.serverTypeLists.Add(ss);
                        }
                        if (int.Parse(item.issignGprs) > 0)
                        {
                            ServerTypeList ss = new ServerTypeList();
                            ss.ServiceType = "11";// 语音服务 
                            foreach (Models.APN.ResultItem items in r1.result)
                            {
                                ServerAPN apn = new ServerAPN();
                                apn.ApnName = items.apnname;
                                switch (items.status)
                                {
                                    case "01":
                                        items.status = "1";
                                        break;
                                    default:
                                        items.status = "0";
                                        break;
                                }
                                apn.ServiceStatus = items.status;
                                ss.APNStatus = new List<ServerAPN>();
                                ss.APNStatus.Add(apn);
                            }
                            f.serverTypeLists.Add(ss);
                        }
                        if (int.Parse(item.issignSms) > 0)
                        {
                            ServerTypeList ss = new ServerTypeList();
                            ss.ServiceType = "08";// 语音服务 
                            foreach (Models.APN.ResultItem items in r1.result)
                            {
                                ServerAPN apn = new ServerAPN();
                                apn.ApnName = items.apnname;
                                switch (items.status)
                                {
                                    case "01":
                                        items.status = "1";
                                        break;
                                    default:
                                        items.status = "0";
                                        break;
                                }
                                apn.ServiceStatus = items.status;
                                ss.APNStatus = new List<ServerAPN>();
                                ss.APNStatus.Add(apn);
                            }
                            f.serverTypeLists.Add(ss);
                        }
                    }
                    f.Mesage = "Ok";
                    f.status = "0";
                    return f;
                }
                else             
                {
                    FunctionalOpen f = new FunctionalOpen();
                    f.serverTypeLists = new List<ServerTypeList>();
                    FunctionalOpenQuery.Root r = new FunctionalOpenQuery.Root();
                    List<ServiceTypeListItem> serviceTypeList = new List<ServiceTypeListItem>();
                    foreach (ResultItem item in GetFunctionalOpenQuery(Value).result)
                    {
                        foreach (ServiceTypeListItem items in item.serviceTypeList)
                        {
                            ServerTypeList ss = new ServerTypeList();
                            List<ServerAPN> APNStatus = new List<ServerAPN>();
                            ServerAPN sss = new ServerAPN();
                            sss.ApnName = items.apnName;
                            sss.ServiceStatus = items.serviceStatus;
                            ss.ServiceType = items.serviceType;
                            ss.APNStatus = new List<ServerAPN>();
                            ss.APNStatus.Add(sss);
                            f.serverTypeLists.Add(ss);
                        }
                    }
                    f.status = "0";
                    f.Mesage = "Success";
                    return f;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }             
        }
        #endregion



        #endregion

        #region 2.会话信息  查询


        #region  分步查询
        /// <summary>
        /// 在线信息实时查询
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        public static Root_CMIOT_API12001 GetCMIOT_API12001(string imsi)
        {
            string APPID = "", TRANSID = "", TOKEN = "", ss = "", URL = "https://api.iot.10086.cn/v2/";
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


        public static Root_CMIOT_API2008 GetCMIOT_API2008(string imsi)
        {
            string APPID = "", TRANSID = "", TOKEN = "", ss = "", URL = "https://api.iot.10086.cn/v2/";
            try
            {
                List<Acount> acc = new List<Acount>();
                foreach (Acount item in Action.Action_GetAccountsMessage.acount(imsi))
                {
                    APPID = item.APPID;
                    TRANSID = item.TRANSID;
                    TOKEN = item.TOKEN;
                }
                string EBID = "0001000000025";
                //https://api.iot.10086.cn/v2/onandoffrealsingle?appid=xxx&transid=xxx&ebid=xxx&token=xxx&imsi=xxx -以imsi进行查询
                if (imsi.Length == 15)
                {
                    ss = URL + "onandoffrealsingle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&imsi=" + imsi;
                }
                else if (imsi.Length == 20)
                {
                    ss = URL + "onandoffrealsingle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&iccid=" + imsi;
                }
                else if (imsi.Length == 13)
                {
                    ss = URL + "onandoffrealsingle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + imsi;
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
                    return JsonConvert.DeserializeObject<Root_CMIOT_API2008>(reader.ReadToEnd());
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        #endregion
        #region   新旧结合
        /// <summary>
        ///  会话信息  接口 新旧集合
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static Conversation GetConversation(string Value)
        {
            Conversation conversation = new Conversation();
            List<ServerTypeList> list = new List<ServerTypeList>();
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
                if (li.Count == 1)
                {
                    foreach (Message item in li)
                    {
                        Platform = item.Platform;
                        Card_ID = item.Card_ID;
                    }
                }
            }
            //移动旧平台
            if (Platform == "10")
            {
                try
                {
                    Root_CMIOT_API12001 root_CMIOT_API12001 = new Root_CMIOT_API12001();
                    root_CMIOT_API12001.result = new List<CMIOT_API12001>();
                    root_CMIOT_API12001 = GetCMIOT_API12001(Value);
                    foreach (CMIOT_API12001 item in root_CMIOT_API12001.result)
                    {
                        conversation.APN = item.APN;
                        conversation.IP = item.IP;
                        conversation.OnlineState = item.GPRSSTATUS;
                        conversation.rat = item.RAT;
                    }
                    Root_CMIOT_API2008 root_CMIOT_API2008 = new Root_CMIOT_API2008();
                    root_CMIOT_API2008.result = new List<CMIOT_API2008>();
                    root_CMIOT_API2008 = GetCMIOT_API2008(Value);
                    foreach (CMIOT_API2008 item in root_CMIOT_API2008.result)
                    {
                        conversation.ON_OFF = item.status;
                    }
                    conversation.status = "0";
                    conversation.Message = "Success";
                }
                catch (Exception ex)
                {
                    conversation.status = "1";
                    conversation.Message = "出现错误" + ex.ToString();
                }
            }
            //移动新平台
            else if (Platform == "11")
            {
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(Esim7.Action.Sim_Action.Get_NewOneLink_One(Value, "API25M00"));
                foreach (On_OFF.ResultItem item in serializer.Deserialize<On_OFF.Root>(serializer.Deserialize<returnMessage>(json).API).result)
                {
                    conversation.ON_OFF = item.status;
                }
                json = serializer.Serialize(Esim7.Action.Sim_Action.Get_NewOneLink_One(Value, "API25M01"));
                OnLine.Root Onlne = new OnLine.Root();
                Onlne.result = new List<OnLine.ResultItem>();
                Onlne = serializer.Deserialize<OnLine.Root>(serializer.Deserialize<returnMessage>(json).API);
                foreach (OnLine.ResultItem item in Onlne.result)
                {
                    List<OnLine.SimSessionListItem> simSessionList = item.simSessionList;
                    foreach (OnLine.SimSessionListItem items in item.simSessionList)
                    {
                        conversation.APN = items.apnId;
                        conversation.createDate = items.createDate;
                        conversation.IP = items.ip;
                        conversation.OnlineState = items.status;
                        conversation.rat = items.rat;
                    }
                }
                conversation.status = "0";
                conversation.Message = "Success";
            }
            return conversation;
        }
        #endregion

        #endregion

        #region  3 已订购资费
        /// <summary>
        /// 旧平台 移动 获取资费
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        public static Root_CMIOT_API2037 Get_CMIOT_API2037(string imsi)
        {
            string APPID = "", TRANSID = "", TOKEN = "", ss = "", URL = "https://api.iot.10086.cn/v2/";
            List<Acount> acc = new List<Acount>();
            foreach (Acount item in Action.Action_GetAccountsMessage.acount(imsi))
            {
                APPID = item.APPID;
                TRANSID = item.TRANSID;
                TOKEN = item.TOKEN;
            }
            // https://api.iot.10086.cn/v2/querycardprodinfo?appid=xxx&transid=xxx&ebid=xxx&token=xxx&imsi=xxx –以imsi进行查询 
            string EBID = "0001000000264";
            if (imsi.Length == 15)
            {
                ss = URL + "querycardprodinfo?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&imsi=" + imsi;
            }
            else if (imsi.Length == 20)
            {
                ss = URL + "querycardprodinfo?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&iccid=" + imsi;
            }
            else if (imsi.Length == 13)
            {
                ss = URL + "querycardprodinfo?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + imsi;
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
                return JsonConvert.DeserializeObject<Root_CMIOT_API2037>(reader.ReadToEnd());
            }
        }


        /// <summary>
        /// 获取资费信息  结合
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static SubscribedFee_Jihe GetAccmMarginList(string Value)
        {
            SubscribedFee_Jihe subscribedFee_Jihe = new SubscribedFee_Jihe();
            subscribedFee_Jihe.accmMarginList = new List<accmMarginList>();
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
                if (li.Count == 1)
                {
                    foreach (Message item in li)
                    {
                        Platform = item.Platform;
                        Card_ID = item.Card_ID;
                    }
                } 
            }
             //旧平台  移动
            if (Platform=="10")
            {
                Root_CMIOT_API2037 root_CMIOT_API2037 = new Root_CMIOT_API2037();
                root_CMIOT_API2037.result = new List<ResultItem_CMIOT_API2037>();               
                root_CMIOT_API2037 = Get_CMIOT_API2037(Value);
                foreach (ResultItem_CMIOT_API2037 item in root_CMIOT_API2037.result)
                {
                    foreach (ProdinfosItem_CMIOT_API2037 items in item.prodinfos)
                    {
                        accmMarginList accmMarginList = new accmMarginList();
                        accmMarginList.offeringId = items.prodid;
                        accmMarginList.offeringName = items.prodname;
                        accmMarginList.effectiveDate = items.prodinstefftime;
                        accmMarginList.expiriedDate = items.prodinstexptime;
                        accmMarginList.apnName = "--";
                        subscribedFee_Jihe.accmMarginList.Add(accmMarginList);
                        subscribedFee_Jihe.status = "0";
                        subscribedFee_Jihe.Message = "Success";
                    }
                }
            }
            else if (Platform=="11")
            {
                SubscribedFee.Root  Sub= new SubscribedFee.Root();
                Sub.result = new List<SubscribedFee.ResultItem>();
                Sub = GetFunctionalOpenQuery2(Value);
                foreach (SubscribedFee.ResultItem item in Sub.result)
                {
                    foreach (SubscribedFee.OfferingInfoListItem items in item.offeringInfoList)
                    {
                        accmMarginList accmMarginList = new accmMarginList();
                        accmMarginList.apnName = items.apnName;
                        accmMarginList.effectiveDate = items.effectiveDate;
                        accmMarginList.expiriedDate = items.expiriedDate;
                        accmMarginList.offeringId = items.offeringId;
                        accmMarginList.offeringName = items.offeringName;
                        subscribedFee_Jihe.accmMarginList.Add(accmMarginList);
                        subscribedFee_Jihe.status = "0";
                        subscribedFee_Jihe.Message = "Success";
                    }
                }
            }
            return subscribedFee_Jihe;
        }


        ///<summary>
        ///停机原因查询
        /// </summary>
        public static string GetCardStopInfo(string Card_ICCID)
        {
            string info = "";
            string token = string.Empty;
            CMCC.CMCCModel.CMCCRootToken crt = new CMCC.CMCCModel.CMCCRootToken();//移动token接收
            CMCCAPIDAL cmcc = new CMCCAPIDAL();
            crt = cmcc.GetToken(Card_ICCID);//新平台移动卡的状态
            if (crt.status == "0")
            {
                token = crt.result[0].token;
            }
            Models.APIModel.CmccApiModel.CMIOT_API25S02 t = new Models.APIModel.CmccApiModel.CMIOT_API25S02();
            string URL = "https://api.iot.10086.cn/v5/ec/query/sim-stop-reason?";
            // 单卡停机原因查询
            //https://api.iot.10086.cn/v5/ec/query/sim-stop-reason?transid=xxx&token=xxx&iccid = xxx - 以iccid进行查询–以iccid进行查询

            //string ss = "https://api.iot.10086.cn/v5/ec/get/token?appid=200216378200200000&password=RXpURUbID&transid=2002163782002000002019071002415709643582";
            string ss = "";
            ss = URL + "transid=2002163782002000002019071002415709643582&token=" + token + "&iccid=" + Card_ICCID;
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ss);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                t = JsonConvert.DeserializeObject<Models.APIModel.CmccApiModel.CMIOT_API25S02>(reader.ReadToEnd());
                if (t.status == "0")
                {
                    info = t.result[0].stopReason;
                }
            }
            return info;
        }

        ///<summary>
        ///机卡分离查询
        /// </summary>
        public static string GetCMIOT_API23A04(string Card_ICCID)
        {
            string Res = string.Empty;
            string token = string.Empty;
            CMCC.CMCCModel.CMCCRootToken crt = new CMCC.CMCCModel.CMCCRootToken();//移动token接收
            CMCCAPIDAL cmcc = new CMCCAPIDAL();
            crt = cmcc.GetToken(Card_ICCID);//新平台移动卡的状态
            if (crt.status == "0")
            {
                token = crt.result[0].token;
            }
            Models.APIModel.CmccApiModel.CMIOT_API25S02 t = new Models.APIModel.CmccApiModel.CMIOT_API25S02();
            string URL = "https://api.iot.10086.cn/v5/ec/query/card-bind-status?";
            // 机卡分离查询
            //https://api.iot.10086.cn/v5/ec/query/card-bind-status?transid=xxxxxx& token=xxxxxx&msisdn=xxxx&testType=0

            //string ss = "https://api.iot.10086.cn/v5/ec/get/token?appid=200216378200200000&password=RXpURUbID&transid=2002163782002000002019071002415709643582";
            string ss = "";
            ss = URL + "transid=2002163782002000002019071002415709643582&token=" + token + "&msisdn=" + Card_ICCID+ "&testType=0";
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ss);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                //t = JsonConvert.DeserializeObject<Models.APIModel.CmccApiModel.CMIOT_API25S02>(reader.ReadToEnd());
                Res = reader.ReadToEnd();
            }
            return Res;
        }
        #endregion


        #endregion
        #region    用到的类 集合

        #region    1.通讯服务类
        /// <summary>
        ///  通讯服务
        /// </summary>
        public class FunctionalOpen
        {    /// <summary>
             /// 集合
             /// </summary>
            public List<ServerTypeList> serverTypeLists { get; set; }
            public string status { get; set; }

            public string Mesage { get; set; }
        }
        /// <summary>
        /// apn
        /// </summary>
        public class ServerTypeList
        {
            public string ServiceType { get; set; }
            /// <summary>
            /// 通讯功能状态    0 暂停 1 恢复
            /// </summary>           
            public List<ServerAPN> APNStatus { get; set; }

        }
        public class ServerAPN
        {
            public string ServiceStatus { get; set; }
            /// <summary>
            /// APN名称
            /// </summary>
            /// 
            public string ApnName { get; set; }
        }

        #endregion

        #region  2.会话信息类


        /// <summary>
        /// 会话信息 查询
        /// </summary>
        public class Conversation
        {
            /// <summary>
            /// 开关机状态
            /// </summary>
            public string ON_OFF { get; set; }
            /// <summary>
            /// APN名称
            /// </summary>
            public string APN { get; set; }
            /// <summary>
            /// IP
            /// </summary>
            public string IP { get; set; }
            /// <summary>
            /// 在线状态
            /// </summary>
            public string OnlineState { get; set; }
            /// <summary>
            /// 会话创建时间
            /// </summary>
            public string createDate { get; set; }
            /// <summary>
            /// 接入方式
            /// </summary>
            public string rat { get; set; }
            /// <summary>
            /// 接口成功标志
            /// </summary>
            public string status { get; set; }
            /// <summary>
            ///接口返回信息
            /// </summary>
            public string Message { get; set; }
        }
        #endregion

        #region   资费套餐

        /// <summary>
        ///资费套餐
        /// </summary>
        public class accmMarginList
        {
            /// <summary>
            /// 套餐 ID
            /// </summary>
            public string offeringId { get; set; }
            /// <summary>
            /// 套餐名称
            /// </summary>
            public string offeringName { get; set; }
            /// <summary>
            /// apn名称
            /// </summary>
            public string apnName { get; set; }
            /// <summary>
            /// 生效日期
            /// </summary>
            public string effectiveDate { get; set; }
            /// <summary>
            /// 失效日期
            /// </summary>
            public string expiriedDate { get; set; }
        }
        /// <summary>
        ///    资费套餐接口集合
        /// </summary>
        public class SubscribedFee_Jihe
        {
            /// <summary>
            /// 套餐
            /// </summary>
            public List<accmMarginList> accmMarginList { get; set; }
            /// <summary>
            ///   接口标志位
            /// </summary>
            public string status { get; set; }
            /// <summary>
            /// 接口返回信息
            /// </summary>
            public string Message { get; set; }
        }

        #endregion
        #endregion
    }
}