
using Dapper;
using Esim7.CMCC.CMCCDAL;
using Esim7.CMCC.CMCCModel;
using Esim7.Dto;
using Esim7.IOT.IOTAPI;
using Esim7.IOT.IOTModel;
using Esim7.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Linq;
using static Esim7.Action.Action_UpdateCard_RealTime_Message;
using static Esim7.Action.CardAction;

namespace Esim7.IOT.IOTDAL
{
    /// <summary>
    /// 海外漫游API
    /// </summary>
    public class IOTAPIDAL
    {
        string baseusl = "https://cmhk.dcp.ericsson.net/dcpapi/";
        /// <summary>
        /// 计算出月初至今日凌晨的累计用量 
        /// </summary>
        /// <returns></returns>
        public IOTMonthFlow RoveMonthFlow(string startdate, string enddate)
        {
            IOTMonthFlow t = new IOTMonthFlow();
            List<UsageData> datas = new List<UsageData>();
            StringBuilder soap = new StringBuilder();
            soap.Append("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:usag=\"http://api.dcp.ericsson.net/UsageDataDownload\">");
            soap.Append("<soapenv:Header>");
            soap.Append("<wsse:Security xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" wsse:mustUnderstand =\"1\">");
            soap.Append("<wsse:UsernameToken>");
            soap.Append("<wsse:Username>wendy_wang@amaziot.com</wsse:Username>");
            soap.Append("<wsse:Password>Gr7Su7Gn1Bb2</wsse:Password>");
            soap.Append("</wsse:UsernameToken>");
            soap.Append("</wsse:Security>");
            soap.Append("</soapenv:Header>");
            soap.Append("<soapenv:Body>");
            soap.Append("<usag:Query>");
            soap.Append("<customerno>67000137</customerno>");//客户编码
            soap.Append("<startdate>");
            soap.Append(startdate);
            soap.Append("</startdate>");
            soap.Append("<enddate>");
            soap.Append(enddate);
            soap.Append("</enddate>");
            soap.Append("</usag:Query>");
            soap.Append("</soapenv:Body>");
            soap.Append("</soapenv:Envelope>");
            string url = @"https://cmhk.dcp.ericsson.net/dcpapi/UsageDataDownload";
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
            string UserName = "wendy_wang@amaziot.com";
            string Password = "Gr7Su7Gn1Bb2";
            webRequest.PreAuthenticate = true;
            NetworkCredential networkCredential = new NetworkCredential(UserName, Password);
            webRequest.Credentials = networkCredential;
            byte[] bs = Encoding.UTF8.GetBytes(soap.ToString());
            webRequest.Method = "POST";
            webRequest.ContentType = "text/xml;charset=utf-8";
            webRequest.ContentLength = bs.Length;
            try
            {
                using (Stream reqStream = webRequest.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                }
                WebResponse myWebResponse = webRequest.GetResponse();
                string result;
                using (StreamReader sr = new StreamReader(myWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
                {
                    // 返回结果
                    result = sr.ReadToEnd();
                }
                XmlDocument doc = new XmlDocument();
                string xml = result.ToString();
                doc.LoadXml(xml);
                XmlElement node = doc.DocumentElement;
                XmlNodeList xmlNode = node.ChildNodes;
                xmlNode = xmlNode[1].ChildNodes;
                xmlNode = xmlNode[0].ChildNodes;
                for (int i = 0; i < xmlNode.Count; i++)
                {
                    UsageData usage = new UsageData();
                    XmlNode date = xmlNode[i].ChildNodes[0];
                    XmlNode size = xmlNode[i].ChildNodes[1];
                    XmlNode path = xmlNode[i].ChildNodes[2];
                    usage.date = date.InnerText;
                    usage.size = size.InnerText;
                    usage.path = path.InnerText;
                    datas.Add(usage);
                }
                t.usageData = datas;
                t.flg = "1";
                t.Msg = "成功!";
            }
            catch (Exception ex)
            {
                t.usageData = null;
                t.flg = "-1";
                t.Msg = "错误:" + ex;
            }
            return t;
        }
        ///<summary>
        ///获取当天实时流量
        /// </summary>
        public IOTDayFlow DayFlow(string id, string type)
        {
            IOTDayFlow t = new IOTDayFlow();
            List<DayGprs> dayGprs = new List<DayGprs>();
            StringBuilder soap = new StringBuilder();
            soap.Append("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:sub=\"http://api.dcp.ericsson.net/SubscriptionTraffic\">");
            soap.Append("<soapenv:Header>");
            soap.Append("<wsse:Security xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\"  wsse:mustUnderstand=\"1\">");
            soap.Append("<wsse:UsernameToken>");
            soap.Append("<wsse:Username>wendy_wang@amaziot.com</wsse:Username>");
            soap.Append("<wsse:Password>Gr7Su7Gn1Bb2</wsse:Password>");
            soap.Append("</wsse:UsernameToken>");
            soap.Append("</wsse:Security>");
            soap.Append("</soapenv:Header>");
            soap.Append("<soapenv:Body>");
            soap.Append("<sub:Query>");
            soap.Append("<resource>");
            soap.Append("<id>");
            soap.Append(id);
            soap.Append("</id>");
            soap.Append("<type>");
            soap.Append(type);
            soap.Append("</type>");
            soap.Append("</resource>");
            soap.Append("<range>1</range>");
            soap.Append("</sub:Query>");
            soap.Append("</soapenv:Body>");
            soap.Append("</soapenv:Envelope>");
            string url = @"https://cmhk.dcp.ericsson.net/dcpapi/SubscriptionTraffic";
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
            string UserName = "wendy_wang@amaziot.com";
            string Password = "Gr7Su7Gn1Bb2";
            webRequest.PreAuthenticate = true;
            NetworkCredential networkCredential = new NetworkCredential(UserName, Password);
            webRequest.Credentials = networkCredential;
            byte[] bs = Encoding.UTF8.GetBytes(soap.ToString());
            webRequest.Method = "POST";
            webRequest.ContentType = "text/xml;charset=utf-8";
            webRequest.ContentLength = bs.Length;
            using (Stream reqStream = webRequest.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }
            WebResponse myWebResponse = webRequest.GetResponse();
            string result;
            using (StreamReader sr = new StreamReader(myWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
            {
                // 返回结果
                result = sr.ReadToEnd();
            }
            XmlDocument doc = new XmlDocument();
            string xml = result;
            doc.LoadXml(xml);
            XmlElement node = doc.DocumentElement;
            XmlNodeList xmlNode = node.ChildNodes;
            xmlNode = xmlNode[1].ChildNodes;
            for (int i = 0; i < xmlNode.Count; i++)
            {
                DayGprs gprs = new DayGprs();
                XmlNode date = xmlNode[i].ChildNodes[0];
                XmlNode s = date.ChildNodes[3];
                XmlNode ss = s.ChildNodes[1];
                gprs.tx = ss.InnerText;
                gprs.rx = s.ChildNodes[2].InnerText;
                dayGprs.Add(gprs);
                t.flg = "1";
                t.Msg = "成功!";
            }
            t.DayGprs = dayGprs;
            string json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            //var result1 = JsonConvert.DeserializeObject<IOTMonthFlow>(json);
            return t;
        }
        /// <summary>
        ///获取短信的用量
        /// </summary>
        public string ShorMessageNum(string startdate, string enddate)
        {
            string ss = "< soapenv:Envelope xmlns:soapenv = \"http://schemas.xmlsoap.org/soap/envelope/\" xmlns: usag = \"http://api.dcp.ericsson.net/UsageDataDownload\">";
            string customerno = "67000137";
            string sss = "";
            StringBuilder soap = new StringBuilder();
            soap.Append("<soapenv:Envelope xmlns:soapenv = \"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:agg=\"http://api.dcp.ericsson.net/AggregatedTraffic\">");
            soap.Append("<soapenv:Header>");
            soap.Append("<wsse:Security xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" wsse:mustUnderstand =\"1\">");
            soap.Append("<wsse:UsernameToken>");
            soap.Append("<wsse:Username>wendy_wang@amaziot.com</wsse:Username>");
            soap.Append("<wsse:Password>Gr7Su7Gn1Bb2</wsse:Password>");
            soap.Append("</wsse:UsernameToken>");
            soap.Append("</wsse:Security>");
            soap.Append("</soapenv:Header>");
            soap.Append("<soapenv:Body>");
            soap.Append("<agg:QueryTrafficUsageHistory>");
            soap.Append("<customerNo>67000137</customerNo>");
            soap.Append("<startDate>2020-07-01</startDate>");
            soap.Append("<endDate>2020-07-20</endDate>");
            soap.Append("</agg:QueryTrafficUsageHistory>");
            soap.Append("</soapenv:Body>");
            soap.Append("</soapenv:Envelope>");
            string url = @"https://cmhk.dcp.ericsson.net/dcpapi/AggregatedTraffic";
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
            string UserName = "wendy_wang@amaziot.com";
            string Password = "Gr7Su7Gn1Bb2";
            webRequest.PreAuthenticate = true;
            NetworkCredential networkCredential = new NetworkCredential(UserName, Password);
            webRequest.Credentials = networkCredential;
            byte[] bs = Encoding.UTF8.GetBytes(soap.ToString());
            webRequest.Method = "POST";
            webRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
            webRequest.ContentType = "text/xml;charset=utf-8";
            webRequest.ContentLength = bs.Length;
            webRequest.Host = "cmhk.dcp.ericsson.net";
            webRequest.UserAgent = " HttpClient / 4.1.1(java 1.5)";
            webRequest.Accept = "gzip,deflate";
            webRequest.Connection = "Keep - Alive";
            using (Stream reqStream = webRequest.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }
            //reqStream.Close();

            WebResponse myWebResponse = webRequest.GetResponse();
            string result;
            using (StreamReader sr = new StreamReader(myWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
            {
                // 返回结果
                result = sr.ReadToEnd();
            }
            string sssss = " \"env: Envelope\":{\"@xmlns:env\":\"http://schemas.xmlsoap.org/soap/envelope/\",\"env:Header\":\"\",\"env:Body\":{\"ns2:QueryResponse\":{\"@xmlns:ns2\":\"http://api.dcp.ericsson.net/UsageDataDownload\",";
            XmlDocument doc = new XmlDocument();
            string xml = result.ToString();
            doc.LoadXml(xml);
            string json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            return json;
        }


        /// <summary>
        ///获取卡状态
        /// </summary>
        public string GetMyCardStatus(string Card_ID)
        {
            string ss = "< soapenv:Envelope xmlns:soapenv = \"http://schemas.xmlsoap.org/soap/envelope/\" xmlns: usag = \"http://api.dcp.ericsson.net/SubscriptionManagement\">";
            string customerno = "67000137";
            string sss = "";
            StringBuilder soap = new StringBuilder();
            //soap.Append("<soapenv:Envelope xmlns:soapenv = \"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:agg=\"http://api.dcp.ericsson.net/SubscriptionManagement\">");
            soap.Append("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:sub=\"http://api.dcp.ericsson.net/SubscriptionManagement\">");
            soap.Append("<soapenv:Header>");
            soap.Append("<wsse:Security xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" wsse:mustUnderstand =\"1\">");
            soap.Append("<wsse:UsernameToken>");
            soap.Append("<wsse:Username>wendy_wang@amaziot.com</wsse:Username>");
            soap.Append("<wsse:Password>Gr7Su7Gn1Bb2</wsse:Password>");
            soap.Append("</wsse:UsernameToken>");
            soap.Append("</wsse:Security>");
            soap.Append("</soapenv:Header>");
            soap.Append("<soapenv:Body>");
            soap.Append("<sub:QuerySimResource>");
            soap.Append("<resource>");
            soap.Append("<id>" + Card_ID + "</id>");
            soap.Append("<type>msisdn</type>");
            soap.Append("</resource>");
            soap.Append("</sub:QuerySimResource>");
            soap.Append("</soapenv:Body>");
            soap.Append("</soapenv:Envelope>");
            string url = @"https://cmhk.dcp.ericsson.net/dcpapi/SubscriptionManagement";
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
            string UserName = "wendy_wang@amaziot.com";
            string Password = "Gr7Su7Gn1Bb2";
            webRequest.PreAuthenticate = true;
            NetworkCredential networkCredential = new NetworkCredential(UserName, Password);
            webRequest.Credentials = networkCredential;
            byte[] bs = Encoding.UTF8.GetBytes(soap.ToString());
            webRequest.Method = "POST";
            webRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
            webRequest.ContentType = "text/xml;charset=utf-8";
            webRequest.ContentLength = bs.Length;
            webRequest.Host = "cmhk.dcp.ericsson.net";
            webRequest.UserAgent = " HttpClient / 4.1.1(java 1.5)";
            webRequest.Accept = "gzip,deflate";
            webRequest.Connection = "Keep - Alive";
            using (Stream reqStream = webRequest.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }
            //reqStream.Close();
            XmlDocument xml = new XmlDocument();
            WebResponse myWebResponse = webRequest.GetResponse();
            string result;
            using (StreamReader sr = new StreamReader(myWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
            {
                // 返回结果
                result = sr.ReadToEnd();
                xml.LoadXml(result);//加载数据
                XmlNodeList xxList = xml.GetElementsByTagName("SimResource"); //取得节点e799bee5baa6e58685e5aeb931333337396362名为row的XmlNode集
                foreach (XmlNode item in xxList)
                {
                    sss = item["simSubscriptionStatus"].InnerText;
                }
            }
            XmlDocument doc = new XmlDocument();
            return sss;
        }

        ///<summary>
        ///根据卡号或ICCID获取卡的状态、工作状态、月使用流量、卡IMEI等信息
        /// </summary>
        public IotCardInfoDto GetCardDtatilInfo(string Value)
        {
            IotCardInfoDto info = new IotCardInfoDto();
            List<ItoCardInfo> cardInfos = new List<ItoCardInfo>();
            ItoCardInfo infos = new ItoCardInfo();
            CMCCRootToken tokeninfo = new CMCCRootToken();
            try
            {
                string APPID = string.Empty;
                string TOKEN = string.Empty;
                string TRANSID = string.Empty;
                string cardaccountsql = "select * from card_copy1 where Card_ID='" + Value + "' or Card_ICCID='" + Value + "'";
                string cardaccountctsql = "select * from ct_cardcopy where Card_ID='" + Value + "' or Card_ICCID='" + Value + "'";
                string cardaccountcuccsql = "select * from cucc_cardcopy where Card_ID='" + Value + "' or Card_ICCID='" + Value + "'";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    #region 移动
                    string accountsid = conn.Query<Card>(cardaccountsql).Select(t => t.accountsID).FirstOrDefault();
                    var cardinfo = conn.Query<Card>(cardaccountsql).FirstOrDefault();
                    string sqlaccountinfo = "select * from accounts where accountID='" + accountsid + "'";
                    var accountinfo = conn.Query<accounts>(sqlaccountinfo).FirstOrDefault();
                    #endregion
                    #region 电信
                    string ctaccountsid = conn.Query<Card>(cardaccountctsql).Select(t => t.accountsID).FirstOrDefault();
                    var ctcardinfo = conn.Query<Card>(cardaccountctsql).FirstOrDefault();
                    string sqlctaccountinfo = "select * from accounts where accountID='" + ctaccountsid + "'";
                    var ctaccountinfo = conn.Query<accounts>(sqlctaccountinfo).FirstOrDefault();
                    #endregion
                    #region 联通
                    string cuccaccountsid = conn.Query<Card>(cardaccountcuccsql).Select(t => t.accountsID).FirstOrDefault();
                    var cucccardinfo = conn.Query<Card>(cardaccountcuccsql).FirstOrDefault();
                    string sqlcuccaccountinfo = "select * from accounts where accountID='" + cuccaccountsid + "'";
                    var cuccaccountinfo = conn.Query<accounts>(sqlcuccaccountinfo).FirstOrDefault();
                    #endregion
                    if (accountinfo != null)
                    {
                        if (accountinfo.Platform == 10)//移动老平台
                        {
                            APPID = accountinfo.APPID;
                            TOKEN = accountinfo.TOKEN;
                            TRANSID = accountinfo.TRANSID;
                            infos = IOTAPIS.CmccOldCardInfo(APPID, TOKEN, TRANSID, null, Value);
                            infos.iccid = cardinfo.Card_ICCID;
                            infos.access_number = cardinfo.Card_ID;
                            infos.imei = cardinfo.Card_IMEI;
                            cardInfos.Add(infos);
                            info.flg = "1";
                            info.Msg = "成功";
                        }
                        if (accountinfo.Platform == 11)//移动新平台
                        {
                            CMCCAPIDAL api = new CMCCAPIDAL();
                            tokeninfo = api.GetToken(Value);
                            TOKEN = tokeninfo.result[0].token;
                            APPID = accountinfo.APPID;
                            infos = IOTAPIS.CmccNewCardInfo(APPID, TOKEN, Value);
                            infos.iccid = cardinfo.Card_ICCID;
                            infos.access_number = cardinfo.Card_ID;
                            infos.imei = cardinfo.Card_IMEI;
                            cardInfos.Add(infos);
                            info.cardInfos = cardInfos;
                            info.flg = "1";
                            info.Msg = "成功";
                        }
                    }
                    if (ctaccountinfo != null)
                    {
                        if (ctaccountinfo.Platform == 21)//电信
                        {
                            string appid = ctaccountinfo.APPID;
                            string userid = ctaccountinfo.UserId;
                            string password = ctaccountinfo.PASSWORD;
                            string sqlctaccount = "select * from ct_cardcopy where Card_ID='" + Value + "' or Card_ICCID='" + Value + "'";//获取电信卡信息
                            var ctcardinfos = conn.Query<Card>(cardaccountctsql).FirstOrDefault();
                            if (Value.Length != 19)
                            {
                                Value = ctcardinfos.Card_ICCID;
                            }
                            infos = IOTAPIS.CtCardInfo(userid, appid, Value, password);
                            if (infos != null)
                            {
                                infos.iccid = ctcardinfos.Card_ICCID;
                                infos.imei = ctcardinfos.Card_IMEI;
                                infos.access_number = ctcardinfos.Card_ID;
                                cardInfos.Add(infos);
                                info.cardInfos = cardInfos;
                                info.flg = "1";
                                info.Msg = "成功";
                            }
                            if (infos == null)
                            {
                                info.flg = "-1";
                                info.Msg = "暂未查到相关信息!";
                            }
                        }
                    }
                    if (cuccaccountinfo != null)//联通
                    {
                        string apikey = cuccaccountinfo.APIkey;
                        string username = cuccaccountinfo.UserName;
                        string sqlctaccount = "select * from ct_cardcopy where Card_ID='" + Value + "' or Card_ICCID='" + Value + "'";//获取电信卡信息
                        var cucccardinfos = conn.Query<Card>(cardaccountcuccsql).FirstOrDefault();
                        if (Value.Length != 20)
                        {
                            Value = cucccardinfos.Card_ICCID;
                        }
                        infos = IOTAPIS.CuccCardInfo(username, apikey, Value);
                        if (infos != null)
                        {
                            infos.iccid = cucccardinfos.Card_ICCID;
                            infos.imei = cucccardinfos.Card_IMEI;
                            infos.access_number = cucccardinfos.Card_ID;
                            cardInfos.Add(infos);
                            info.cardInfos = cardInfos;
                            info.flg = "1";
                            info.Msg = "成功";
                        }
                        if (infos == null)
                        {
                            info.flg = "-1";
                            info.Msg = "未查询到会话信息、卡状态、流量信息";
                        }

                    }

                    //if (accountinfo.Platform == 51)//大流量卡
                    //{

                    //}

                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "失败:" + ex;
            }
            return info;

        }


        ///<summary>
        ///获取当天实时流量
        /// </summary>
        public string GetRoamDayFlow(string Card_ID)
        {
            string sss = "";
            string flow = string.Empty;
            StringBuilder soap = new StringBuilder();
            soap.Append("<soapenv:Envelope xmlns:soapenv =\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:sub=\"http://api.dcp.ericsson.net/SubscriptionTraffic\">");
            soap.Append("<soapenv:Header>");
            soap.Append("<wsse:Security xmlns:wsse =\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" wsse:mustUnderstand =\"1\" >");
            soap.Append("<wsse:UsernameToken>");
            soap.Append("<wsse:Username>wendy_wang@amaziot.com</wsse:Username>");
            soap.Append("<wsse:Password>Gr7Su7Gn1Bb2</wsse:Password>");
            soap.Append("</wsse:UsernameToken>");
            soap.Append(" </wsse:Security>");
            soap.Append("</soapenv:Header>");
            soap.Append("<soapenv:Body>");
            soap.Append("<sub:Query>");
            soap.Append("<resource>");
            soap.Append("<id>" + Card_ID + "</id>");
            soap.Append("<type>msisdn</type>");
            soap.Append("</resource>");
            soap.Append("<range>1</range>");
            soap.Append("</sub:Query>");
            soap.Append("</soapenv:Body>");
            soap.Append("</soapenv:Envelope>");
            string url = @"https://cmhk.dcp.ericsson.net/dcpapi/SubscriptionTraffic";
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
            string UserName = "wendy_wang@amaziot.com";
            string Password = "Gr7Su7Gn1Bb2";
            webRequest.PreAuthenticate = true;
            NetworkCredential networkCredential = new NetworkCredential(UserName, Password);
            webRequest.Credentials = networkCredential;
            byte[] bs = Encoding.UTF8.GetBytes(soap.ToString());
            webRequest.Method = "POST";
            webRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
            webRequest.ContentType = "text/xml;charset=utf-8";
            webRequest.ContentLength = bs.Length;
            webRequest.Host = "cmhk.dcp.ericsson.net";
            webRequest.UserAgent = " HttpClient / 4.1.1(java 1.5)";
            webRequest.Accept = "gzip,deflate";
            webRequest.Connection = "Keep - Alive";
            using (Stream reqStream = webRequest.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }
            //reqStream.Close();
            XmlDocument xml = new XmlDocument();
            WebResponse myWebResponse = webRequest.GetResponse();
            string result;
            using (StreamReader sr = new StreamReader(myWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8")))
            {
                // 返回结果
                result = sr.ReadToEnd();
                xml.LoadXml(result);//加载数据
                XmlNodeList xxList = xml.GetElementsByTagName("gprs"); //取得节点e799bee5baa6e58685e5aeb931333337396362名为row的XmlNode集
                foreach (XmlNode item in xxList)
                {
                    sss = item["tx"].InnerText;
                    string rx = item["rx"].InnerText;
                    //sss = sss + "," + rx;
                    //sss = item["tx"].InnerText;
                    decimal f = Convert.ToDecimal(sss) + Convert.ToDecimal(rx);
                    f = f / 1024;
                    flow = f.ToString();
                }
            }
            return flow;
        }

        ///<summary>
        ///更新漫游卡的状态和流量数据
        /// </summary>
        public Flow_M_Used GetUpdateRoamFlowStatus(string Value)
        {
            Flow_M_Used used = new Flow_M_Used();
            try
            {
                string flow = string.Empty;
                string Card_Monthlyusageflow = string.Empty;
                decimal flows = 0;
                decimal monthflow = 0;
                string status = string.Empty;
                flow = GetRoamDayFlow(Value);//当天使用流量
                status = GetMyCardStatus(Value);//卡状态
                if (!string.IsNullOrWhiteSpace(status))
                {
                    if (status == "Active")//已激活
                    {
                        status = "2";
                    }
                    if (status == "Deactivated")//去激活 待激活
                    {
                        status = "1";
                    }
                    if (status == "Pause")//停机
                    {
                        status = "4";
                    }
                    if (status == "Terminated")//已终止 注销用户
                    {
                        status = "8";
                    }
                }
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    if (!string.IsNullOrWhiteSpace(flow))
                    {
                        flows = Convert.ToDecimal(flow);
                        string sql = "select Card_Monthlyusageflow from roamcard where Card_ID='" + Value + "'";
                        Card_Monthlyusageflow = conn.Query<Card>(sql).Select(t => t.Card_Monthlyusageflow).FirstOrDefault();
                        if (!string.IsNullOrWhiteSpace(Card_Monthlyusageflow))
                        {
                            monthflow = Convert.ToDecimal(Card_Monthlyusageflow);
                            flows = flows + monthflow;
                        }
                    }
                    else
                    {
                        string sql = "select Card_Monthlyusageflow from roamcard where Card_ID='" + Value + "'";
                        Card_Monthlyusageflow = conn.Query<Card>(sql).Select(t => t.Card_Monthlyusageflow).FirstOrDefault();
                        if (!string.IsNullOrWhiteSpace(Card_Monthlyusageflow))
                        {
                            monthflow = Convert.ToDecimal(Card_Monthlyusageflow);
                            flows = flows + monthflow;
                        }
                    }
                }
                used.Card_Monthlyusageflow = flows.ToString();
                used.Card_State = status;
                used.Message = "接口调取成功";
                used.status = "0";
                used.Success = true;
            }
            catch (Exception ex)
            {
                used.Message = "失败:"+ex;
            }
            return used;
        }


        #region 对接讯众API
        ///<summary>
        ///单个号码状态查询
        /// </summary>
        public XunZhongCardStatusRoot GetDanGeCardStatus(string Card_ICCID)
        {
            XunZhongCardStatusRoot t = new XunZhongCardStatusRoot();
            //iccid = "89860481212090016400";
            string version = "2.0";//版本
            string sign = string.Empty;
            long timestampS = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;//;时间戳
            string timestamp = timestampS.ToString();
            string appId = "yy20210610115834908";
            string appKey = "01fb3189544f429a82c8396202f213b2";
            string secret = "071dd2d6aba04f798ce0dd982328533a";
            string URL = @"https://iot.ytx.net/openapi/openApi/card/getCardStatus";
            string str = "appId=yy20210610115834908&appKey=01fb3189544f429a82c8396202f213b2&iccid=89860481212090016400&timestamp=1623316261&version=2.0&secret=071dd2d6aba04f798ce0dd982328533a";
            sign = "dEdtbDR6ejRYU1B5dWpPVERrdU1Ld0w2QXlVKy96V3RJbmxaZkVvZWkvR1dyL0JYZVB4TjIvaXFQNTNWQktvT3dGL1lmM2piYkdDaA0KZG52UzlBTnVwd3kvekh2bFVIT1ViTzFtYTRLWGxOS3NEVXNuS29ZMi9hemwrc1AvNEY1Z3VQczlDTmR6NTZ6aHM4NEJaYWJ5RmY4dw0KTTUwRURwK0J0L3o4YThuYXR2OU8xUllFRVBLWVkzUXpEdU1OMUJ3Y3VHWTVISFZSanEvazQvdlk1N0tFYTc3VlFLaHAwOW5D";
            //sign = "dEdtbDR6ejRYU1B5dWpPVERrdU1Ld0w2QXlVKy96V3RJbmxaZkVvZWkvR1dyL0JYZVB4TjIvaXFQNTNWQktvT3dGL1lmM2piYkdDaA0KZG52UzlBTnVwd3kvekh2bFVIT1ViTzFtYTRLWGxOS3NEVXNuS29ZMi9hemwrc1AvNEY1Z3VQczlDTmR6NTZ5WlVEenR1eXBBUGk3ag0KSStrZWtwVjR0L3o4YThuYXR2OU8xUllFRVBLWVkzUXpEdU1OMUJ3Y3VHWTVISFZSanEvazQvdlk1N0tFYTc3VlFLaHAwOW5D";
            StringBuilder strs = new StringBuilder();
            strs.Append("{");
            strs.Append("\"sign\":\"" + sign + "\",");
            strs.Append("\"version\":\"" + version + "\",");
            strs.Append("\"iccid\":\"" + Card_ICCID + "\",");
            strs.Append("\"timestamp\":\"" + timestamp + "\",");
            strs.Append("\"appId\":\"" + appId + "\",");
            strs.Append("\"appKey\":\"" + appKey + "\"");
            strs.Append("}");
            string json = strs.ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = bytes.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            if (responseStream != null)
            {
                using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    t = JsonConvert.DeserializeObject<XunZhongCardStatusRoot>(reader.ReadToEnd());
                }
            }
            //result = reader.ReadToEnd();
            return t;
        }

        ///<summary>
        ///号码信息查询
        /// </summary>
        public XunZhongCardInfoRoot GetXunZhongCardInfo(string Card_ICCID)
        {
            string ssss = string.Empty;
            XunZhongCardInfoRoot root = new XunZhongCardInfoRoot();
            string version = "2.0";//版本
            string sign = string.Empty;
            long timestampS = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;//;时间戳
            string timestamp = timestampS.ToString();
            string appId = "yy20210610115834908";
            string appKey = "01fb3189544f429a82c8396202f213b2";
            string secret = "071dd2d6aba04f798ce0dd982328533a";
            string URL = @"https://iot.ytx.net/openapi/openApi/card/getCadDetail";
            string str = "appId=yy20210610115834908&appKey=01fb3189544f429a82c8396202f213b2&iccid=89860481212090016400&timestamp=1623316261&version=2.0&secret=071dd2d6aba04f798ce0dd982328533a";
            sign = "dEdtbDR6ejRYU1B5dWpPVERrdU1Ld0w2QXlVKy96V3RJbmxaZkVvZWkvR1dyL0JYZVB4TjIvaXFQNTNWQktvT3dGL1lmM2piYkdDaA0KZG52UzlBTnVwd3kvekh2bFVIT1ViTzFtYTRLWGxOS3NEVXNuS29ZMi9hemwrc1AvNEY1Z3VQczlDTmR6NTZ6aHM4NEJaYWJ5RmY4dw0KTTUwRURwK0J0L3o4YThuYXR2OU8xUllFRVBLWVkzUXpEdU1OMUJ3Y3VHWTVISFZSanEvazQvdlk1N0tFYTc3VlFLaHAwOW5D";
            //sign = "dEdtbDR6ejRYU1B5dWpPVERrdU1Ld0w2QXlVKy96V3RJbmxaZkVvZWkvR1dyL0JYZVB4TjIvaXFQNTNWQktvT3dGL1lmM2piYkdDaA0KZG52UzlBTnVwd3kvekh2bFVIT1ViTzFtYTRLWGxOS3NEVXNuS29ZMi9hemwrc1AvNEY1Z3VQczlDTmR6NTZ5WlVEenR1eXBBUGk3ag0KSStrZWtwVjR0L3o4YThuYXR2OU8xUllFRVBLWVkzUXpEdU1OMUJ3Y3VHWTVISFZSanEvazQvdlk1N0tFYTc3VlFLaHAwOW5D";
            StringBuilder strs = new StringBuilder();
            strs.Append("{");
            strs.Append("\"sign\":\"" + sign + "\",");
            strs.Append("\"version\":\"" + version + "\",");
            strs.Append("\"iccid\":\"" + Card_ICCID + "\",");
            strs.Append("\"timestamp\":\"" + timestamp + "\",");
            strs.Append("\"appId\":\"" + appId + "\",");
            strs.Append("\"appKey\":\"" + appKey + "\"");
            strs.Append("}");
            string json = strs.ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = bytes.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            if (responseStream != null)
            {
                using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    //ssss = reader.ReadToEnd();
                    root = JsonConvert.DeserializeObject<XunZhongCardInfoRoot>(reader.ReadToEnd());
                }
            }
            return root;
        }

        ///<summary>
        ///基站定位
        /// </summary>
        public ZhongXunLocationRoot GetZhongXunLocation(string Card_ICCID)
        {
            ZhongXunLocationRoot root = new ZhongXunLocationRoot();
            string version = "2.0";//版本
            string sign = string.Empty;
            long timestampS = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;//;时间戳
            string timestamp = timestampS.ToString();
            string appId = "yy20210610115834908";
            string appKey = "01fb3189544f429a82c8396202f213b2";
            string secret = "071dd2d6aba04f798ce0dd982328533a";
            string URL = @"https://iot.ytx.net/openapi/openApi/position/getPosition";
            string str = "appId=yy20210610115834908&appKey=01fb3189544f429a82c8396202f213b2&iccid=89860481212090016400&timestamp=1623316261&version=2.0&secret=071dd2d6aba04f798ce0dd982328533a";
            sign = "dEdtbDR6ejRYU1B5dWpPVERrdU1Ld0w2QXlVKy96V3RJbmxaZkVvZWkvR1dyL0JYZVB4TjIvaXFQNTNWQktvT3dGL1lmM2piYkdDaA0KZG52UzlBTnVwd3kvekh2bFVIT1ViTzFtYTRLWGxOS3NEVXNuS29ZMi9hemwrc1AvNEY1Z3VQczlDTmR6NTZ6aHM4NEJaYWJ5RmY4dw0KTTUwRURwK0J0L3o4YThuYXR2OU8xUllFRVBLWVkzUXpEdU1OMUJ3Y3VHWTVISFZSanEvazQvdlk1N0tFYTc3VlFLaHAwOW5D";
            //sign = "dEdtbDR6ejRYU1B5dWpPVERrdU1Ld0w2QXlVKy96V3RJbmxaZkVvZWkvR1dyL0JYZVB4TjIvaXFQNTNWQktvT3dGL1lmM2piYkdDaA0KZG52UzlBTnVwd3kvekh2bFVIT1ViTzFtYTRLWGxOS3NEVXNuS29ZMi9hemwrc1AvNEY1Z3VQczlDTmR6NTZ5WlVEenR1eXBBUGk3ag0KSStrZWtwVjR0L3o4YThuYXR2OU8xUllFRVBLWVkzUXpEdU1OMUJ3Y3VHWTVISFZSanEvazQvdlk1N0tFYTc3VlFLaHAwOW5D";
            StringBuilder strs = new StringBuilder();
            strs.Append("{");
            strs.Append("\"sign\":\"" + sign + "\",");
            strs.Append("\"version\":\"" + version + "\",");
            strs.Append("\"iccid\":\"" + Card_ICCID + "\",");
            strs.Append("\"timestamp\":\"" + timestamp + "\",");
            strs.Append("\"appId\":\"" + appId + "\",");
            strs.Append("\"appKey\":\"" + appKey + "\"");
            strs.Append("}");
            string json = strs.ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = bytes.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            if (responseStream != null)
            {
                using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    //ssss = reader.ReadToEnd();
                    root = JsonConvert.DeserializeObject<ZhongXunLocationRoot>(reader.ReadToEnd());
                }
            }
            return root;
        }
        #endregion


        ///<summary>
        ///获取酷宅订单数据   count获取数量  groupflg 分组标识
        /// </summary>
        public string GetHouzhaiData(string count,string groupflg)
        {
            //count = "jordan_zhao@amaziot.com";
            string s = string.Empty;
            KuZhaiRoot t = new KuZhaiRoot();
            List<GetKuZhaiDto> dtos = new List<GetKuZhaiDto>();
            string url = "http://127.0.0.3:44544/deviceid/";//deviceid
            //Encoding encoding = Encoding.UTF8;
            string sss = "{\"count\":\"" + count + "\",\"once\":\"2222\"}";
            byte[] bs = Encoding.UTF8.GetBytes(sss);
            Information info = new Information();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = bs.Length;
            request.Headers.Add("X-CK-Appid", "deiwu33");
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bs, 0, bs.Length);
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    s = reader.ReadToEnd();
                    s = s.Replace("\'{", "{");
                    s = s.Replace("}\'", "}");
                //    string sssc = "['{\"deviceid\":\"1001333496\",\"factory_apikey\":\"95a232c2-16de-4aa4-b77e-6e5fb2711e16\",\"sta_mac\":\"d0:27:02:66:66:7c\",\"sap_mac\":\"d0:27:02:66:66:7d\",\"device_model\":\"AM430E - SW1\"}'," +
                //        "'{\"deviceid\":\"1001333497\",\"factory_apikey\":\"a3c81d86-9184-42e3-a3f1-6cd2c5be9e85\",\"sta_mac\":\"d0:27:02:66:66:7e\",\"sap_mac\":\"d0:27:02:66:66:7f\",\"device_model\":\"AM430E - SW1\"}'," +
                //        "'{\"deviceid\":\"1001333498\",\"factory_apikey\":\"a5f8042b-b96b-4fd0-9961-fb38dffb4342\",\"sta_mac\":\"d0: 27:02:66:66:80\",\"sap_mac\":\"d0: 27:02:66:66:81\",\"device_model\":\"AM430E - SW1\"}']";
                //sssc = sssc.Replace("\'{", "{");
                //sssc = sssc.Replace("}\'", "}");
                dynamic objects1 = JsonConvert.DeserializeObject<dynamic>(s);
                dtos = JsonConvert.DeserializeObject<List<GetKuZhaiDto>>(objects1.ToString());
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    for (int i = 0; i < dtos.Count; i++)
                    {
                        string add = "insert into houzhaidata(deviceid,sta_mac,factory_apikey,sap_mac,device_model,groupflg,AddTime)values('" + dtos[i].deviceid + "','" + dtos[i].sta_mac + "','" + dtos[i].factory_apikey + "','" + dtos[i].sap_mac + "','" + dtos[i].device_model + "','"+groupflg+ "','" + DateTime.Now + "')";
                        conn.Execute(add);
                    }
                }
                    
                //KuZhaiOrderDto st = new KuZhaiOrderDto();
                //    st= JsonConvert.DeserializeObject<KuZhaiOrderDto>(reader.ReadToEnd());
                //    string sede = "{\"orderName\": \"QJ01 - 107_AM430E - SW1_200_20210408001\",\"totalLicenses\": 200, \"availableLicenses\": 0, \"productModel\": \"QJ01 - 107\"}";
                //    JObject jo = (JObject)JsonConvert.DeserializeObject(sede);
                //    st = JsonConvert.DeserializeObject<KuZhaiOrderDto>(jo.ToString());
                //    string s1 = "[{ \"orderName\": \"QJ01-107_AM430E-SW1_200_20210408001\", \"totalLicenses\": 200, \"availableLicenses\": 0, \"productModel\": \"QJ01-107\"}]";
                //    dynamic objects = JsonConvert.DeserializeObject<dynamic>(s);
                //    List<KuZhaiOrderDto> ss3 = new List<KuZhaiOrderDto>();
                //    ss3 = JsonConvert.DeserializeObject<List<KuZhaiOrderDto>>(objects.ToString());




                    //t = JsonConvert.DeserializeObject<KuZhaiRoot>(reader.ReadToEnd());
                    //t1 = JsonConvert.DeserializeObject<GetKuZhaiDto>(reader.ReadToEnd());
                    //string deviceid = t.dtos[0].deviceid;
                    //string device_model = t.dtos[0].device_model;
                    //string factory_apikey = t.dtos[0].factory_apikey;
                    //string sap_mac = t.dtos[0].sap_mac;
                    //string sta_mac = t.dtos[0].sta_mac;

                    //string deviceidt1 = t1.deviceid;
                    //string device_modelt1 = t1.device_model;
                    //string factory_apikeyt1 = t1.factory_apikey;
                    //string sap_mact1 = t1.sap_mac;
                    //string sta_mact1 = t1.sta_mac;
                    //t = JsonConvert.DeserializeObject<CuccCardFlow>(reader.ReadToEnd());
                    //info.Msg = t.iccid;
                    //info.flg = "1";
                }
            }
            catch (Exception ex)
            {
                //info.Msg = "出错:" + ex;
                //info.flg = "-1";
            }
            return s;
        }


        ///<summary>
        ///wifi定位
        /// </summary>
        public string wifidw()
        {
            string device_id= "719673367";
            string s = "";
            string url = "http://api.heclouds.com/devices/719673367/lbs/latestWifiLocation?";
            string para = "{\"$OneNET_LBS_WIFI\":{\"macs\":\"FC:D7:33:55:92:6A,-77|B8:F8:83:E6:24:DF,-60\",\"serverip\":\"10.2.166.4\",\"imsi\":\"352315052834187\", \"mmac\":\"FC:D7:33:55:92:6A,-80\",\"smac\":\"E0:DB:55:E4:C7:49\",\"idfa\":\"583D2BB0-B19C- 4A9A-A600-2A1EB2FB7E39\"}}";
            url = url + para;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
           
            Information info = new Information();
            request.Method = "get";
            request.ContentType = "application/json";
            request.Accept = "text/html, application/xhtml+xml, */*";
            //Stream requestStream = request.GetRequestStream();
            //requestStream.Write(bs, 0, bs.Length);
            request.Headers.Add("api-key", "tPDFc8UqYDv3BluF7v=21nEKLdo=");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                s = reader.ReadToEnd();
            }
            return s;
        }

        ///<summary>
        ///生成EXCEL
        /// </summary>
        public string GetExcel()
        {
            FilesPath filesPath = new FilesPath();
            string FileName = string.Empty;
            string FilePath = string.Empty;
            //创建工作簿对象
            IWorkbook workbook = new HSSFWorkbook();
            //创建工作表
            ISheet sheet = workbook.CreateSheet("onesheet");
            IRow row0 = sheet.CreateRow(0);
            row0.CreateCell(0).SetCellValue("DeviceName");
                    
            for (int r = 1; r < 2001; r++)
            {
                string DeviceName = "v1.0";
                //创建行row
                IRow row = sheet.CreateRow(r);
                row.CreateCell(0).SetCellValue(DeviceName);
                       
            }
            //创建流对象并设置存储Excel文件的路径  D:写入excel.xls
            //C:\Users\Administrator\Desktop\交接文档\Esim7\Esim7\Files\写入excel.xls
            //自动生成文件名称
            FileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".xls";
            FilePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Files/" + FileName);
            //FilePath=@"D:Files/" + FileName;
            using (FileStream url = File.OpenWrite(FilePath))
            {
                //导出Excel文件
                workbook.Write(url);
            };
            //ProcessRequest1(li);
            filesPath.Path = FilePath;
            filesPath.Flage = "1";
            filesPath.Message = "Success";
               
            
            return filesPath.Path;
        }


        ///<summary>
        ///酷宅烧录数据上传
        /// </summary>
        public Information updatekuzhaidata(UploadkzPara para)
        {
            Information info = new Information();
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    if (para.kzdatas.Count > 0)
                    {
                        DateTime time = DateTime.Now;
                        foreach (var item in para.kzdatas)
                        {
                            string adddata = "insert into houzhaidata (deviceid,factory_apikey,sta_mac,sap_mac,device_model,IsBurn,groupflg,AddTime) " +
                                "values('" + item.deviceid + "','" + item.factory_apikey + "','" + item.sta_mac + "','" + item.sap_mac + "','" + item.device_model + "','0','1','" + time + "')";
                            conn.Execute(adddata);
                        }
                        info.Msg = "成功!";
                        info.flg = "1";
                    }
                }
            }
            catch (Exception ex)
            {
                info.Msg = "失败!"+ex;
                info.flg = "-1";
            }
            
            return info;
        }
    }
}