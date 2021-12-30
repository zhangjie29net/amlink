using Dapper;
using Esim7.CMCC.CMCCModel;
using Esim7.CUCC.CUCCModel;
using Esim7.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using static Esim7.SimModel.API;

namespace Esim7.CMCC.CMCCDAL
{
    public class CMCCAPIDAL
    {
        ///<summary>
        ///获取TOKEN
        /// </summary>
        public CMCCRootToken GetToken(string ICCID)
        {
            CMCCRootToken t = new CMCCRootToken();
            string APPID = "";
            string TRANSID = "";
            string TOKEN = "";
            string passeord = "";
            List<Acount> acc = new List<Acount>();
            foreach (Acount item in Action.Action_GetAccountsMessage.acount(ICCID))
            {
                APPID = item.APPID;
                TRANSID = item.TRANSID;
                TOKEN = item.TOKEN;
                passeord = item.PASSWORD;
            }
            //string ss = "https://api.iot.10086.cn/v5/ec/get/token?appid=" + APPID + "&password=" + passeord + "&transid=2002163782002000002019071002415709643582";
            TRANSID =APPID + "2019071002415709643582";
            string ss = "https://api.iot.10086.cn/v5/ec/get/token?appid=" + APPID + "&password=" + passeord + "&transid="+TRANSID;
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ss);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                t = JsonConvert.DeserializeObject<CMCCRootToken>(reader.ReadToEnd());
            }
            return  t;
        }

        #region 新平台api
        ///<summary>
        ///查看新平台卡的状态
        /// </summary>
        public NewCMCCCardStatus GetNewCmccCardStatus(string TOKEN, string appid, string ICCID)
        {
            string url = @"https://api.iot.10086.cn/v5/ec/query/sim-status?";
            NewCMCCCardStatus t = new NewCMCCCardStatus();
            string Transid = appid + "2019071002415709643582";
            //string Transid = "2002163782002000002019071002415709643582";
            string SS = url + "transid=" + Transid + "&token=" + TOKEN + "&iccid=" + ICCID;
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                t = JsonConvert.DeserializeObject<NewCMCCCardStatus>(reader.ReadToEnd());
            }
            return t;
        }


        ///<summary>
        ///查看新平台卡的工作状态
        /// </summary>
        public NewCMCCCardWorkStatus GetNewCmccCardWrokStatus(string TOKEN, string appid, string ICCID)
        {
            NewCMCCCardWorkStatus t = new NewCMCCCardWorkStatus();
            string url = @"https://api.iot.10086.cn/v5/ec/query/sim-session?";
            string Transid = appid + "2019071002415709643582";
            string SS = url + "transid=" + Transid + "&token=" + TOKEN + "&iccid=" + ICCID;
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                t = JsonConvert.DeserializeObject<NewCMCCCardWorkStatus>(reader.ReadToEnd());
            }
            return t;
        }

        ///<summary>
        ///查看新平台卡的月使用流量
        /// </summary>
        public NewCMCCCardMonthFlow GetNewCmccCardMonthFlow(string TOKEN, string appid, string ICCID)
        {
            NewCMCCCardMonthFlow t = new NewCMCCCardMonthFlow();
            string url = @"https://api.iot.10086.cn/v5/ec/query/sim-data-usage?";
            string Transid = appid + "2019071002415709643582";
            //string MSISDN = "1440233542043";
            string SS = url + "transid=" + Transid + "&token=" + TOKEN + "&iccid=" + ICCID;
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                t = JsonConvert.DeserializeObject<NewCMCCCardMonthFlow>(reader.ReadToEnd());
            }
            return t;
        }

        ///<summary>
        ///查看apn名称以及单卡开通apn信息
        /// </summary>
        public CMIOT_API23M03 GetApnInfo(string token,string appid,string ICCID)
        {
            CMIOT_API23M03 t = new CMIOT_API23M03();
            string url = @"https://api.iot.10086.cn/v5/ec/query/apn-info?";
            string Transid = appid + "2019071002415709643582";
            //string MSISDN = "1440233542043";
            string SS = url + "transid=" + Transid + "&token=" + token + "&iccid=" + ICCID;
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                t = JsonConvert.DeserializeObject<CMIOT_API23M03>(reader.ReadToEnd());
            }
            return t;
        }

        ///<summary>
        ///移动新平台的卡位置定位B类获取NB卡的定位信息
        /// </summary>
        public CMIOT_API25L00Root GetCmccNbAction(string Company_ID,string value)
        {
            CMCCAPIDAL ss = new CMCCAPIDAL();
            CMCCRootToken tokens = new CMCCRootToken();
            CMIOT_API25L00Root t = new CMIOT_API25L00Root();
            int id = 0;
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                if (value.Length != 13 && value.Length != 15 && value.Length != 20)
                {
                    t.status = "301";
                    t.message = "输入的卡号或iccid或imsi号码不正确";
                }
                else
                {
                    string sqlnum = "select * from cardlocation where CustomerComapnyID='" + Company_ID + "' and UseNum<TotalNum and EendTime >'"+DateTime.Now+"' ORDER BY EendTime";//是否在可调用次数内
                    var listsql = conn.Query<cmcclocation>(sqlnum).FirstOrDefault();
                    if (listsql != null)
                    {
                        if (listsql.UseNum < listsql.TotalNum && DateTime.Now < listsql.EendTime)//使用次数小于总次数且小于结束时间
                        {
                            id = listsql.id;
                            string token = string.Empty;
                            string SS = string.Empty;
                            string lat = string.Empty;
                            string lon = string.Empty;
                            tokens = ss.GetToken(value);
                            if (tokens.status == "0")
                            {
                                token = tokens.result[0].token;
                            }
                            string url = @"https://api.iot.10086.cn/v5/ec/query/position-location-message?";
                            string appid = "290210718100100000";
                            string Transid = appid + "2019071002415709643582";
                            if (value.Length == 20)
                            {
                                SS = url + "transid=" + Transid + "&token=" + token + "&iccid=" + value;
                            }
                            if (value.Length == 13)
                            {
                                SS = url + "transid=" + Transid + "&token=" + token + "&msisdn=" + value;
                            }
                            if (value.Length == 15)
                            {
                                //https://api.iot.10086.cn/v5/ec/query/position-location-message?transid=xxxxxx&token=xxxxxx&imsi=xxxxxx –以imsi进行查询
                                SS = url + "transid=" + Transid + "&token=" + token + "&imsi=" + value;
                            }
                            Encoding encoding = Encoding.UTF8;
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
                            request.Method = "GET";
                            request.Accept = "text/html, application/xhtml+xml, */*";
                            request.ContentType = "application/json";
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                            {
                                t = JsonConvert.DeserializeObject<CMIOT_API25L00Root>(reader.ReadToEnd());
                                if (t.status == "0")//调用成功
                                {

                                    string sqlUseNum = "select UseNum as Total from cardlocation where CustomerComapnyID='" + Company_ID + "' and id="+id+"";
                                    int usenum = conn.Query<Card>(sqlUseNum).Select(s => s.Total).FirstOrDefault();
                                    usenum = usenum + 1;
                                    string updates = "update cardlocation set  UseNum=" + usenum + " where CustomerComapnyID='" + Company_ID + "' and id="+id+"";
                                    conn.Execute(updates);
                                }
                            }
                        }
                        else
                        {
                            t.status = "-1";
                            t.message = "您的接口次数已用完或已到期!";
                        }
                    }
                    else
                    {
                        t.status = "-1";
                        t.message = "您没有订购改接口,请先订购后在使用!";
                    }
                }
               
            }
            return t;
        }


        /// <summary>
        /// 百度地图api坐标转换
        /// </summary>
        /// <returns></returns>
        public static DTRoot LonlatConversion(string lon,string lat)
        {
            DTRoot t = new DTRoot();
            //string url = "https://api.map.baidu.com/geoconv/v1/?coords=114.21892734521,29.575429778924&from=1&to=5&ak=GimHB4i4TdT0a8NDk1Qwv1axSw1zwcwu";
            string urlzhjwd = string.Empty;
            string urlzh = "https://api.map.baidu.com/geoconv/v1/?";
            urlzhjwd = urlzh + "coords=" + lon + "," + lat + "&from=1&to=5&ak=GimHB4i4TdT0a8NDk1Qwv1axSw1zwcwu";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlzhjwd);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response1 = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response1.GetResponseStream(), Encoding.UTF8))
            {
                t = JsonConvert.DeserializeObject<DTRoot>(reader.ReadToEnd());//转换后的   
            }
            return t;
        }

        /// <summary>
        /// 百度地图api地址逆解析
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <returns></returns>
        public static baidunjxRoot GetLocationAnalysis(string lon, string lat)
        {
            baidunjxRoot t = new baidunjxRoot();
            string lo = string.Empty;
            lo = lat + "," + lon;
            string urlnjx = string.Empty;
            string ak = "GimHB4i4TdT0a8NDk1Qwv1axSw1zwcwu";
            //string url = "https://api.map.baidu.com/reverse_geocoding/v3/?ak=GimHB4i4TdT0a8NDk1Qwv1axSw1zwcwu&output=json&coordtype=bd09ll&location=22.839487,113.521838";
            urlnjx = "https://api.map.baidu.com/reverse_geocoding/v3/?ak=GimHB4i4TdT0a8NDk1Qwv1axSw1zwcwu&output=json&coordtype=bd09ll&location=" + lo;
            //string ssss = urlnjx;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlnjx);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                t = JsonConvert.DeserializeObject<baidunjxRoot>(reader.ReadToEnd());
            }
            return t;
        }

        ///<summary>
        ///CMIOT实名认证 物联卡实名登记请求基础版
        /// </summary>
        public CMIOT_API23A12 GetCMIOT_API23A12(string value)
        {
            CMIOT_API23A12 t = new CMIOT_API23A12();
            CMCCAPIDAL ss = new CMCCAPIDAL();
            CMCCRootToken tokens = new CMCCRootToken();
            string token = string.Empty;
            string SS = string.Empty;
            string lat = string.Empty;
            string lon = string.Empty;
            tokens = ss.GetToken(value);
            if (tokens.status == "0")
            {
                token = tokens.result[0].token;
            }
            string url = @"https://api.iot.10086.cn/v5/ec/secure/sim-real-name-reg?";
            string appid = "290210718100100000";
            string Transid = appid + "2019071002415709643582";
            if (value.Length == 20)
            {
                SS = url + "transid=" + Transid + "&token=" + token + "&iccid=" + value;
            }
            if (value.Length == 13)
            {
                SS = url + "transid=" + Transid + "&token=" + token + "&msisdn=" + value;
            }
            if (value.Length == 15)
            {
                SS = url + "transid=" + Transid + "&token=" + token + "&imsi=" + value;
            }
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                t = JsonConvert.DeserializeObject<CMIOT_API23A12>(reader.ReadToEnd());
            }
            return t;
        }


        ///<summary>
        ///CMIOT_API23A10-物联卡实名登记状态查询
        /// </summary>
        public CMIOT_API23A10 GetCMIOT_API23A10(string value)
        {
            CMIOT_API23A10 t = new CMIOT_API23A10();
            CMCCAPIDAL ss = new CMCCAPIDAL();
            CMCCRootToken tokens = new CMCCRootToken();
            string token = string.Empty;
            string SS = string.Empty;
            string lat = string.Empty;
            string lon = string.Empty;
            tokens = ss.GetToken(value);
            if (tokens.status == "0")
            {
                token = tokens.result[0].token;
            }
            string url = @"https://api.iot.10086.cn/v5/ec/query/sim-real-name-status?";
            string appid = "290210718100100000";
            string Transid = appid + "2019071002415709643582";
            if (value.Length == 20)
            {
                SS = url + "transid=" + Transid + "&token=" + token + "&iccid=" + value;
            }
            if (value.Length == 13)
            {
                SS = url + "transid=" + Transid + "&token=" + token + "&msisdn=" + value;
            }
            if (value.Length == 15)
            {
                SS = url + "transid=" + Transid + "&token=" + token + "&imsi=" + value;
            }
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                t = JsonConvert.DeserializeObject<CMIOT_API23A10>(reader.ReadToEnd());
            }
            return t;
        }

        ///<summary>
        ///设置APN关停   operType：0：开 1：停
        /// </summary>
        public CMIOT_API23M07 Operateapn (string token, string appid, string ICCID,string apnName,string operType)
        {
            CMIOT_API23M07 t = new CMIOT_API23M07();
            string url = @"https://api.iot.10086.cn/v5/ec/operate/sim-apn-function?";
        //https://api.iot.10086.cn/v5/ec/operate/sim-apn-function?transid=xxx&token=xxx&iccid=xxx&operType=xxx&apnName=xxx
            string Transid = appid + "2019071002415709643582";
            //string MSISDN = "1440233542043";
            string SS = url + "transid=" + Transid + "&token=" + token + "&iccid=" + ICCID+ "&operType="+operType+ "&apnName="+apnName;
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                t = JsonConvert.DeserializeObject<CMIOT_API23M07>(reader.ReadToEnd());
            }
            return t;

        }

        ///<summary>
        ///移动修改单卡状态API
        /// </summary>
        public CMIOT_API23S03Root UpdateCardStatusInfo(string ICCID, string operType)
        {
            CMCCRootToken tok = new CMCCRootToken();
            string token = string.Empty;
            string appid = string.Empty;
            string accountsID = string.Empty;
            tok= GetToken(ICCID);
            if (tok.status == "0")
            {
                token = tok.result[0].token;
            }
            string sqlcard = "select accountsID from card where Card_ICCID='" + ICCID+"'";
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                accountsID = conn.Query<Card>(sqlcard).Select(s => s.accountsID).FirstOrDefault();
                string sqlaccount = "select APPID from accounts where accountID='"+accountsID+"'";
                appid = conn.Query<accounts>(sqlaccount).Select(r => r.accountID).FirstOrDefault();
            }
            CMIOT_API23S03Root t = new CMIOT_API23S03Root();
            string url = @"https://api.iot.10086.cn/v5/ec/change/sim-status?";
            //https://api.iot.10086.cn/v5/ec/change/sim-status?transid=xxx&token=xxx&iccid=xxx&operType=xxx  以iccid进行变更
            string Transid = appid + "2019071002415709643582";
            //string MSISDN = "1440233542043";
            string SS = url + "transid=" + Transid + "&token=" + token + "&iccid=" + ICCID + "&operType=" + operType ;
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                t = JsonConvert.DeserializeObject<CMIOT_API23S03Root>(reader.ReadToEnd());
            }
            return t;
        }

        ///<summary>
        ///物联卡区域限制状态查询
        /// </summary>
        public CMIOT_API23A11 GetGuankongStatus(string Company_ID, string value)
        {
            //https://api.iot.10086.cn/v5/ec/query/region-limit-status?transid=xxx&token=xxx&msisdn=xxx –以msisdn进行查询 
            string SS = string.Empty;
            CMCCRootToken tok = new CMCCRootToken();
            CMIOT_API23A11 t = new CMIOT_API23A11();
            string token = string.Empty;
            string appid = string.Empty;
            string accountsID = string.Empty;
            string Card_CompanyID = string.Empty;
            tok = GetToken(value);
            if (tok.status == "0")
            {
                token = tok.result[0].token;
            }
            string sqlcard = string.Empty;
            if (Company_ID == "1556265186243")
            {
                sqlcard = "select accountsID,Card_CompanyID from card where Card_ICCID='" + value + "' or Card_ID='"+value+"' or Card_IMSI='"+value+"'";
            }
            else
            {
                sqlcard = "select accountsID,Card_CompanyID from card_copy1 where  Card_ICCID='" + value + "' or Card_ID='" + value + "' or Card_IMSI='" + value + "' and Card_CompanyID='" + Company_ID + "'";
            }
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                Card_CompanyID = conn.Query<Card>(sqlcard).Select(r => r.Card_CompanyID).FirstOrDefault();
                accountsID = conn.Query<Card>(sqlcard).Select(s => s.accountsID).FirstOrDefault();
                if (accountsID != null && Card_CompanyID==Company_ID)
                {
                    string sqlaccount = "select APPID from accounts where accountID='" + accountsID + "'";
                    appid = conn.Query<accounts>(sqlaccount).Select(r => r.accountID).FirstOrDefault();
                    string url = @"https://api.iot.10086.cn/v5/ec/query/region-limit-status?";
                    string Transid = appid + "2019071002415709643582";
                    //string MSISDN = "1440233542043";
                    if (value.Length == 13)
                    {
                        SS = url + "transid=" + Transid + "&token=" + token + "&msisdn=" + value;
                    }
                    if (value.Length == 15)
                    {
                        SS = url + "transid=" + Transid + "&token=" + token + "&imsi=" + value;
                    }
                    if (value.Length == 20)
                    {
                        SS = url + "transid=" + Transid + "&token=" + token + "&iccid=" + value;
                    }
                    Encoding encoding = Encoding.UTF8;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
                    request.Method = "GET";
                    request.Accept = "text/html, application/xhtml+xml, */*";
                    request.ContentType = "application/json";
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        t = JsonConvert.DeserializeObject<CMIOT_API23A11>(reader.ReadToEnd());
                    }
                }
                else
                {
                    t.status = "101";
                    t.message = "此卡不是您名下的请确认后重新输入!";
                }
            }
            return t;
        }
        #endregion

        #region  老平台接口

        ///<summary>
        ///查看老平台卡的状态
        /// </summary>
        public OldCmccCardStatus GetOldCmccCardStatus(string APPID, string TRANSID, string EBID, string TOKEN, string iccid)
        {
            OldCmccCardStatus t = new OldCmccCardStatus();
            string url = "https://api.iot.10086.cn/v2";
            string SS = string.Empty;
            // https://api.iot.10086.cn/v2/userstatusrealsingle?appid=xxx&ebid=xxx&transid=xxx&token=xxx&msisdn=xxx –以msisdn进行查询
            SS = url + "/userstatusrealsingle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&iccid=" + iccid;
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                t = JsonConvert.DeserializeObject<OldCmccCardStatus>(reader.ReadToEnd());
            }
            return t;
        }

        ///<summary>
        ///查看老平台卡的工作状态
        /// </summary>
        public OldCmccCardWorkStatus GetOldCmccCardWorkStatus(string APPID, string TRANSID, string EBID, string TOKEN, string iccid)
        {
            OldCmccCardWorkStatus t = new OldCmccCardWorkStatus();
            string url = "https://api.iot.10086.cn/v2";
            string SS = string.Empty;
            //https://api.iot.10086.cn/v2/gprsrealsingle?appid=xxx&transid=xxx&ebid=xxx&token=xxx&msisdn=xxx -以msisdn进行查询
            SS = url + "/gprsrealsingle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&iccid=" + iccid;
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                t = JsonConvert.DeserializeObject<OldCmccCardWorkStatus>(reader.ReadToEnd());
            }
            return t;
        }

        ///<summary>
        ///查看老平台卡的流量
        /// </summary>
        public OldCmccCardMonthFlow GetOldCmccCardMonthFlow(string APPID, string TRANSID, string EBID, string TOKEN, string iccid)
        {
            OldCmccCardMonthFlow t = new OldCmccCardMonthFlow();
            string SS = string.Empty;
            string url = "https://api.iot.10086.cn/v2/";
            // https://api.iot.10086.cn/v2/gprsusedinfosingle?appid=xxx&transid=xxx&ebid=xxx&token=xxx&msisdn=xxx –以msisdn进行查询            
            SS = url + "gprsusedinfosingle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&iccid=" + iccid;
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                t = JsonConvert.DeserializeObject<OldCmccCardMonthFlow>(reader.ReadToEnd());
            }
            return t;
        }
        #endregion
    }
}