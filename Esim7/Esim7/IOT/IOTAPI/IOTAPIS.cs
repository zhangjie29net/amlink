using Esim7.CT;
using Esim7.CUCC.CUCCModel;
using Esim7.IOT.IOTModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

namespace Esim7.IOT.IOTAPI
{
    public static class IOTAPIS
    {
        public static ItoCardInfo CmccOldCardInfo(string APPID,string TOKEN,string TRANSID,string EBID,string Value)
        {
            ItoCardInfo info = new ItoCardInfo();
            string online_status = string.Empty;
            string status = string.Empty;
            try
            {
                EBID = "0001000000008";
                OldRoot_CMIOT_API12001 t1 = new OldRoot_CMIOT_API12001();
                string url = string.Empty;
                string SS = string.Empty;
                url = "https://api.iot.10086.cn/v2";
                //https://api.iot.10086.cn/v2/gprsrealsingle?appid=xxx&transid=xxx&ebid=xxx&token=xxx&msisdn=xxx -以msisdn进行查询  老平台卡的工作状态
                if (Value.Length == 20)
                {
                    SS = url + "/gprsrealsingle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&iccid=" + Value;
                }
                if (Value.Length == 13)
                {
                    SS = url + "/gprsrealsingle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + Value;
                }
                Encoding encoding1 = Encoding.UTF8;
                HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(SS);
                request1.Method = "GET";
                request1.Accept = "text/html, application/xhtml+xml, */*";
                request1.ContentType = "application/json";
                HttpWebResponse response1 = (HttpWebResponse)request1.GetResponse();
                using (StreamReader reader = new StreamReader(response1.GetResponseStream(), Encoding.UTF8))
                {
                    t1 = JsonConvert.DeserializeObject<OldRoot_CMIOT_API12001>(reader.ReadToEnd());
                    if (t1.result.Count > 0)
                    {
                        info.online_status = t1.result[0].GPRSSTATUS;
                    }
                }

                //老平台卡的状态
                EBID = "0001000000009";
                OldRoot_CMIOT_API2002 t2 = new OldRoot_CMIOT_API2002();
                url = "https://api.iot.10086.cn/v2";
                SS = string.Empty;
                // https://api.iot.10086.cn/v2/userstatusrealsingle?appid=xxx&ebid=xxx&transid=xxx&token=xxx&msisdn=xxx –以msisdn进行查询
                if (Value.Length == 20)
                {
                    SS = url + "/userstatusrealsingle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&iccid=" + Value;
                }
                if (Value.Length == 13)
                {
                    SS = url + "/userstatusrealsingle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + Value;
                }
                Encoding encoding2 = Encoding.UTF8;
                HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(SS);
                request2.Method = "GET";
                request2.Accept = "text/html, application/xhtml+xml, */*";
                request2.ContentType = "application/json";
                HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();
                using (StreamReader reader = new StreamReader(response2.GetResponseStream(), Encoding.UTF8))
                {
                    t2 = JsonConvert.DeserializeObject<OldRoot_CMIOT_API2002>(reader.ReadToEnd());
                    if (t2.result.Count > 0)
                    {
                        string cardStatus = t2.result[0].STATUS;
                        info.status = cardStatus;
                    }
                }
                //老平台卡的月使用流量
                EBID = "0001000000012";
                DateTime time = DateTime.Now;
                OldRootCMIOT_API2005 t = new OldRootCMIOT_API2005();
                SS = string.Empty;
                url = "https://api.iot.10086.cn/v2/";
                // https://api.iot.10086.cn/v2/gprsusedinfosingle?appid=xxx&transid=xxx&ebid=xxx&token=xxx&msisdn=xxx –以msisdn进行查询  
                if (Value.Length == 13)
                {
                    SS = url + "gprsusedinfosingle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + Value;
                }
                if (Value.Length == 20)
                {
                    SS = url + "gprsusedinfosingle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&iccid=" + Value;
                }
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
                request.Method = "GET";
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.ContentType = "application/json";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    t = JsonConvert.DeserializeObject<OldRootCMIOT_API2005>(reader.ReadToEnd());
                    if (t.result.Count > 0)
                    {
                        string total_gprs = t.result[0].total_gprs;
                        info.month_flow =Convert.ToDecimal(total_gprs);
                    }
                }

                //老平台的卡套餐名称
                EBID = "0001000000264";
                CMIOT_API2037 t3 = new CMIOT_API2037();
                SS = string.Empty;
                url = "https://api.iot.10086.cn/v2/"; 
                // https://api.iot.10086.cn/v2/gprsusedinfosingle?appid=xxx&transid=xxx&ebid=xxx&token=xxx&msisdn=xxx –以msisdn进行查询  
                if (Value.Length == 13)
                {
                    SS = url + "querycardprodinfo?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + Value;
                }
                if (Value.Length == 20)
                {
                    SS = url + "querycardprodinfo?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&iccid=" + Value;
                }
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                Encoding encoding3 = Encoding.UTF8;
                HttpWebRequest request3 = (HttpWebRequest)WebRequest.Create(SS);
                request3.Method = "GET";
                request3.Accept = "text/html, application/xhtml+xml, */*";
                request3.ContentType = "application/json";
                HttpWebResponse response3 = (HttpWebResponse)request3.GetResponse();
                using (StreamReader reader = new StreamReader(response3.GetResponseStream(), Encoding.UTF8))
                {
                    t3 = JsonConvert.DeserializeObject<CMIOT_API2037>(reader.ReadToEnd());
                    if (t3.result.Count > 0)
                    {
                        string SetmealName = t3.result[0].prodinfos[0].prodname;
                        info.SetmealName = SetmealName;
                    }
                }

                //老平台卡余额
                EBID = "0001000000035";
                CMIOT_API2011 t4 = new CMIOT_API2011();
                SS = string.Empty;
                url = "https://api.iot.10086.cn/v2/";
                if (Value.Length == 13)
                {
                    SS = url + "balancerealsingle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + Value;
                }
                if (Value.Length == 20)
                {
                    SS = url + "balancerealsingle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&iccid=" + Value;
                }
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                Encoding encoding4 = Encoding.UTF8;
                HttpWebRequest request4 = (HttpWebRequest)WebRequest.Create(SS);
                request4.Method = "GET";
                request4.Accept = "text/html, application/xhtml+xml, */*";
                request4.ContentType = "application/json";
                HttpWebResponse response4 = (HttpWebResponse)request4.GetResponse();
                using (StreamReader reader = new StreamReader(response4.GetResponseStream(), Encoding.UTF8))
                {
                    t4 = JsonConvert.DeserializeObject<CMIOT_API2011>(reader.ReadToEnd());
                    if (t4.result.Count > 0)
                    {
                        string balance = t4.result[0].balance;
                        info.balance = balance;
                    }
                }


            }
            catch (Exception ex)
            {
                info = null;
            }
            return info;
        }

        public static ItoCardInfo CmccNewCardInfo(string APPID,string token,string Value)
        {
            ItoCardInfo info = new ItoCardInfo();
            try
            {
                //移动新平台卡的工作状态
                Root_CMIOT_API25M01 t2 = new Root_CMIOT_API25M01();
                string SS = string.Empty;
                string url = string.Empty;
                url = @"https://api.iot.10086.cn/v5/ec/query/sim-session?";
                string Transid = APPID + "2019071002415709643582";
                if (Value.Length == 13)
                {
                    SS = url + "transid=" + Transid + "&token=" + token + "&msisdn=" + Value;
                }
                if (Value.Length == 20)
                {
                    SS = url + "transid=" + Transid + "&token=" + token + "&iccid=" + Value;
                }
                Encoding encoding2 = Encoding.UTF8;
                HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(SS);
                request2.Method = "GET";
                request2.Accept = "text/html, application/xhtml+xml, */*";
                request2.ContentType = "application/json";
                HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();
                using (StreamReader reader = new StreamReader(response2.GetResponseStream(), Encoding.UTF8))
                {
                    t2 = JsonConvert.DeserializeObject<Root_CMIOT_API25M01>(reader.ReadToEnd());
                    if (t2.result.Count > 0)
                    {
                        string status = t2.result[0].simSessionList[0].status;
                        info.online_status = status;
                    }
                }
                //移动新平台卡的状态
                CMIOT_API25S04 t1 = new CMIOT_API25S04();
                DateTime time = DateTime.Now;
                Transid = string.Empty;
                Transid = APPID + "2019071002415709643582";
                if (Value.Length == 13)
                {
                    SS = url + "transid=" + Transid + "&token=" + token + "&msisdn=" + Value;
                }
                if (Value.Length == 20)
                {
                    SS = url + "transid=" + Transid + "&token=" + token + "&iccid=" + Value;
                }
                Encoding encoding1 = Encoding.UTF8;
                HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(SS);
                request1.Method = "GET";
                request1.Accept = "text/html, application/xhtml+xml, */*";
                request1.ContentType = "application/json";
                HttpWebResponse response1 = (HttpWebResponse)request1.GetResponse();
                using (StreamReader reader = new StreamReader(response1.GetResponseStream(), Encoding.UTF8))
                {
                    t1 = JsonConvert.DeserializeObject<CMIOT_API25S04>(reader.ReadToEnd());
                    if (t1.result.Count > 0)
                    {
                        string cardStatus = t1.result[0].cardStatus;
                        info.status = cardStatus;
                    }
                }

                //移动新平台卡月使用流量
                CMIOT_API25U04 t = new CMIOT_API25U04();
                 SS = string.Empty;
                 url = string.Empty;
                //https://api.iot.10086.cn/v5/ec/query/sim-data-usage?transid=xxxxxx&token=xxxxxx&msisdn=xxxxxx –以msisdn进行查询   
                url = @"https://api.iot.10086.cn/v5/ec/query/sim-data-usage?";
                Transid = APPID + "2019071002415709643582";
                if (Value.Length == 13)
                {
                    SS = url + "transid=" + Transid + "&token=" + token + "&msisdn=" + Value;
                }
                if (Value.Length == 20)
                {
                    SS = url + "transid=" + Transid + "&token=" + token + "&iccid=" + Value;
                }
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
                request.Method = "GET";
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.ContentType = "application/json";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    t = JsonConvert.DeserializeObject<CMIOT_API25U04>(reader.ReadToEnd());
                    if (t.result.Count > 0)
                    {
                        string total_gprs = t.result[0].dataAmount;
                        info.month_flow = Convert.ToDecimal(total_gprs);
                    }
                }

                //资费id
                //CMIOT_API23R00 t3 = new CMIOT_API23R00();
                //string offeringId = string.Empty;
                //SS = string.Empty;
                //url = string.Empty;
                ////https://api.iot.10086.cn/v5/ec/query/sim-data-usage?transid=xxxxxx&token=xxxxxx&msisdn=xxxxxx –以msisdn进行查询   
                //url = @"https://api.iot.10086.cn/v5/ec/query/ordered-offerings?";
                //Transid = APPID + "2019071002415709643582";
                //if (Value.Length == 13)
                //{
                //    SS = url + "transid=" + Transid + "&token=" + token + "&queryType=3&msisdn=" + Value;
                //}
                //if (Value.Length == 20)
                //{
                //    SS = url + "transid=" + Transid + "&token=" + token + "&queryType=3&iccid=" + Value;
                //}
                ////ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                //Encoding encoding3 = Encoding.UTF8;
                //HttpWebRequest request3 = (HttpWebRequest)WebRequest.Create(SS);
                //request3.Method = "GET";
                //request3.Accept = "text/html, application/xhtml+xml, */*";
                //request3.ContentType = "application/json";
                //HttpWebResponse response3 = (HttpWebResponse)request3.GetResponse();
                //using (StreamReader reader = new StreamReader(response3.GetResponseStream(), Encoding.UTF8))
                //{
                //    t3 = JsonConvert.DeserializeObject<CMIOT_API23R00>(reader.ReadToEnd());
                //    if (t3.result.Count > 0)
                //    {
                //        offeringId = t3.result[0].offeringInfoList[0].offeringId;
                //        info.SetmealName= t3.result[0].offeringInfoList[0].offeringName;
                //    }
                //}


                //激活时间
                CMIOT_API23S00 t4 = new CMIOT_API23S00();
                SS = string.Empty;
                url = string.Empty;
                //https://api.iot.10086.cn/v5/ec/query/sim-data-usage?transid=xxxxxx&token=xxxxxx&msisdn=xxxxxx –以msisdn进行查询   
                url = @"https://api.iot.10086.cn/v5/ec/query/sim-basic-info?";
                Transid = APPID + "2019071002415709643582";
                if (Value.Length == 13)
                {
                    SS = url + "transid=" + Transid + "&token=" + token + "&queryType=3&msisdn=" + Value;
                }
                if (Value.Length == 20)
                {
                    SS = url + "transid=" + Transid + "&token=" + token + "&queryType=3&iccid=" + Value;
                }
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                Encoding encoding4 = Encoding.UTF8;
                HttpWebRequest request4 = (HttpWebRequest)WebRequest.Create(SS);
                request4.Method = "GET";
                request4.Accept = "text/html, application/xhtml+xml, */*";
                request4.ContentType = "application/json";
                HttpWebResponse response4 = (HttpWebResponse)request4.GetResponse();
                using (StreamReader reader = new StreamReader(response4.GetResponseStream(), Encoding.UTF8))
                {
                    t4 = JsonConvert.DeserializeObject<CMIOT_API23S00>(reader.ReadToEnd());
                    if (t4.result.Count > 0)
                    {
                        info.activetime = t4.result[0].activeDate;
                    }
                }
                //卡余额
                CMIOT_API23B01 t3 = new CMIOT_API23B01();
                string amount = string.Empty;
                SS = string.Empty;
                url = string.Empty;
                //https://api.iot.10086.cn/v5/ec/query/sim-data-usage?transid=xxxxxx&token=xxxxxx&msisdn=xxxxxx –以msisdn进行查询   
                url = @"https://api.iot.10086.cn/v5/ec/query/balance-info?";
                Transid = APPID + "2019071002415709643582";
                if (Value.Length == 13)
                {
                    SS = url + "transid=" + Transid + "&token=" + token + "&queryType=3&msisdn=" + Value;
                }
                if (Value.Length == 20)
                {
                    SS = url + "transid=" + Transid + "&token=" + token + "&queryType=3&iccid=" + Value;
                }
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                Encoding encoding3 = Encoding.UTF8;
                HttpWebRequest request3 = (HttpWebRequest)WebRequest.Create(SS);
                request3.Method = "GET";
                request3.Accept = "text/html, application/xhtml+xml, */*";
                request3.ContentType = "application/json";
                HttpWebResponse response3 = (HttpWebResponse)request3.GetResponse();
                using (StreamReader reader = new StreamReader(response3.GetResponseStream(), Encoding.UTF8))
                {
                    t3 = JsonConvert.DeserializeObject<CMIOT_API23B01>(reader.ReadToEnd());
                    if (t3.result.Count > 0)
                    {
                        amount = t3.result[0].amount;
                        info.balance = amount;
                    }
                }
            }
            catch (Exception ex)
            {
                info = null;
            }
            return info;
        }

        ///<summary>
        ///电信返回卡状态 工作状态 月使用流量
        /// </summary>
        public static ItoCardInfo CtCardInfo(string user_id, string appid, string Value,string password)
        {
            ItoCardInfo info = new ItoCardInfo();
            DateTime time = DateTime.Now;
            RootCTCardWorkStatus t = new RootCTCardWorkStatus();
            string method = "getOnlineStatus";
            //string user_id = "367DSTQjk59fCbnL3t9M68QwSdDL0Kks";

            //string password = "40P4Xn2bkyyGvmrB";
            string key1 = appid.Substring(0, 3).ToString();// "682";
            string key2 = appid.Substring(3, 3).ToString(); //"NB5";
            string key3 = appid.Substring(6, 3).ToString(); //"ckb";
            string SS = string.Empty;
            //具体接口参数需参照自管理门户在线文档
            string access_number = "14914000000";  //物联网卡号(149或10649号段)
            string[] arr = { user_id, password, method, Value }; //加密数组，数组所需参数根据对应的接口文档
            DesUtils des = new DesUtils(); //加密工具类实例化
            string passwordEnc = des.strEnc(password, key1, key2, key3);  //密码加密 
            string sign = des.strEnc(des.naturalOrdering(arr), key1, key2, key3); //生成sign加密值
            string url = string.Empty;
            url = "http://api.ct10649.com:9001/m2m_ec/query.do?method=getOnlineStatus";
            SS = url + "&iccid=" + Value + "&user_id=" + user_id + "&passWord=" + passwordEnc + "&sign=" + sign;
            //ss = URL + "queryapnopenstatus?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + ICCID;
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    t = JsonConvert.DeserializeObject<RootCTCardWorkStatus>(reader.ReadToEnd());
                    if (t.resultCode == "0")
                    {
                        string workstate = t.description.result;
                        info.online_status = workstate;
                    }
                }
            }
            catch (Exception ex)
            {
                info = null;
            }
              
            //电信卡状态
            RootCTStatus t1 = new RootCTStatus();
            method = "getSIMList";
            string[] arr1 = { user_id, password, method }; //加密数组，数组所需参数根据对应的接口文档
            string passwordEnc1 = des.strEnc(password, key1, key2, key3);  //密码加密 
            string sign1 = des.strEnc(des.naturalOrdering(arr1), key1, key2, key3); //生成sign加密值
            string url1 = string.Empty;
            url1 = "http://api.ct10649.com:9001/m2m_ec/query.do?method=getSIMList";
            SS = url1 + "&method=" + method + "&pageIndex=1"  + "&accNumber=" + Value + "&user_id=" + user_id + "&passWord=" + passwordEnc1 + "&sign=" + sign1;
            //ss = URL + "queryapnopenstatus?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + ICCID;
            Encoding encodin1g = Encoding.UTF8;
            HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(SS);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            try
            {
                HttpWebResponse response1 = (HttpWebResponse)request1.GetResponse();
                using (StreamReader reader = new StreamReader(response1.GetResponseStream(), Encoding.UTF8))
                {
                    t1 = JsonConvert.DeserializeObject<RootCTStatus>(reader.ReadToEnd());
                    if (t1.resultCode == "0")
                    {
                        string cardstate = t1.description.simList[0].simStatus[0];
                        if (cardstate == "2")
                        {
                            cardstate = "10";
                        }
                        if (cardstate == "3")
                        {
                            cardstate = "11";
                        }
                        if (cardstate == "6")
                        {
                            cardstate = "12";
                        }
                        info.status = cardstate;
                        info.activetime = t1.description.simList[0].activationTime;//激活时间
                    }
                }
            }
            catch (Exception ex)
            {
                info = null;
            }
               
            //电信卡月使用流量
            XmlDocument xml = new XmlDocument();
            method = "queryTraffic";
            string MonthFlow = string.Empty;
            string TOTALCOUNT = string.Empty;
            string[] arr2 = { user_id, password, method, Value }; //加密数组，数组所需参数根据对应的接口文档
            string passwordEnc2 = des.strEnc(password, key1, key2, key3);  //密码加密 
            string sign2 = des.strEnc(des.naturalOrdering(arr2), key1, key2, key3); //生成sign加密值
            string url2 = string.Empty;
            url = "http://api.ct10649.com:9001/m2m_ec/query.do?";
            SS = url + "method=" + method + "&iccid=" + Value + "&needDtl=1&user_id=" + user_id + "&passWord=" + passwordEnc2 + "&sign=" + sign2;
            //ss = URL + "queryapnopenstatus?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + ICCID;
            Encoding encoding2 = Encoding.UTF8;
            HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(SS);
            request2.Method = "GET";
            request2.Accept = "text/html, application/xhtml+xml, */*";
            request2.ContentType = "application/json";
            try
            {
                HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();
                using (StreamReader reader = new StreamReader(response2.GetResponseStream(), Encoding.UTF8))
                {
                    string str = reader.ReadToEnd();
                    if (str.Length > 200)
                    {
                        xml.LoadXml(str);//加载数据
                        XmlNodeList xxList = xml.GetElementsByTagName("web:NEW_DATA_TICKET_QRsp"); //取得节点e799bee5baa6e58685e5aeb931333337396362名为row的XmlNode集
                        foreach (XmlNode item in xxList)
                        {
                            MonthFlow = item["TOTAL_BYTES_CNT"].InnerText;
                            TOTALCOUNT = item["TOTALCOUNT"].InnerText;//次数??
                            string result = MonthFlow.Substring(0, MonthFlow.IndexOf('M'));
                            MonthFlow = (Convert.ToDecimal(result) * 1024).ToString();
                        }
                        info.month_flow = Convert.ToDecimal(MonthFlow);
                    }
                }
            }
            catch (Exception ex)
            {
                info = null;
            }
            //卡余额
            XmlDocument xml1 = new XmlDocument();
            method = "queryBalance";
            string[] arr3 = { user_id, password, method, Value }; //加密数组，数组所需参数根据对应的接口文档
            string passwordEnc3 = des.strEnc(password, key1, key2, key3);  //密码加密 
            string sign3 = des.strEnc(des.naturalOrdering(arr3), key1, key2, key3); //生成sign加密值
            url = "http://api.ct10649.com:9001/m2m_ec/query.do?";
            SS = url + "method=" + method + "&iccid=" + Value + "&user_id=" + user_id + "&passWord=" + passwordEnc3 + "&sign=" + sign3;
            //ss = URL + "queryapnopenstatus?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + ICCID;
            Encoding encoding3 = Encoding.UTF8;
            HttpWebRequest request3 = (HttpWebRequest)WebRequest.Create(SS);
            request3.Method = "GET";
            request3.Accept = "text/html, application/xhtml+xml, */*";
            request3.ContentType = "application/json";
            try
            {
                HttpWebResponse response3 = (HttpWebResponse)request3.GetResponse();
                using (StreamReader reader = new StreamReader(response3.GetResponseStream(), Encoding.UTF8))
                {
                    string str = reader.ReadToEnd();
                    if (str.Length > 100)
                    {
                        xml1.LoadXml(str);//加载数据
                        XmlNodeList xxList = xml1.GetElementsByTagName("root"); //取得节点e799bee5baa6e58685e5aeb931333337396362名为row的XmlNode集
                        foreach (XmlNode item in xxList)
                        {
                            var itemsss = item.FirstChild.ChildNodes;
                            info.balance=itemsss[1].InnerText;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                info = null;
            }

            return info;
        }

        ///<summary>
        ///联通返回卡状态 卡工作状态 月使用流量
        /// </summary>
        public static ItoCardInfo CuccCardInfo(string username,string apikey,string Value)
        {
            ItoCardInfo info = new ItoCardInfo();
            //try
            //{
                //卡工作状态
                CuccCardWorkStatus t = new CuccCardWorkStatus();
                //string url = "https://api.10646.cn/rws/api/v1/devices/89860620160013379998/sessionInfo";
                string url = "https://api.10646.cn/rws/api/v1/devices/" + Value + "/sessionInfo";
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "get";
                //string code = "wangyaping977:47e164a1-2806-43d7-9aca-609e81535bb2";
                string code = username + ":" + apikey;
                System.Text.Encoding encode = System.Text.Encoding.ASCII;
                byte[] bytedata = encode.GetBytes(code);
                string strPath = Convert.ToBase64String(bytedata, 0, bytedata.Length);
                string ssss = strPath;
                string BaseApi = "Basic " + ssss;
                request.Headers.Add("Authorization", BaseApi);
                // request.Headers.Add("Authorization", "Basic d2FuZ3lhcGluZzc3OjRlMTUzMjYxLWUxMzUtNGM0YS1iYWM1LTNiYTE2NjZhZTc4OQ==");           
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        t = JsonConvert.DeserializeObject<CuccCardWorkStatus>(reader.ReadToEnd());
                        string cardworkstate = "00";//离线
                        if (string.IsNullOrWhiteSpace(t.dateSessionEnded))
                        {
                            cardworkstate = "01";//在线
                        }
                        info.online_status = cardworkstate;
                    }
                }
                catch (Exception ex)
                {
                    info = null;
                }
                
                //卡状态 激活时间
                CUCCCardStatus t1 = new CUCCCardStatus();
                url = "https://api.10646.cn/rws/api/v1/devices/" + Value;
                Encoding encoding1 = Encoding.UTF8;
                HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(url);
                request1.Method = "get";
                //string code = "wangyaping977:47e164a1-2806-43d7-9aca-609e81535bb2";
                System.Text.Encoding encode1 = System.Text.Encoding.ASCII;
                byte[] bytedata1 = encode.GetBytes(code);
                strPath = Convert.ToBase64String(bytedata1, 0, bytedata1.Length);
                ssss = strPath;
                BaseApi = "Basic " + ssss;
                request1.Headers.Add("Authorization", BaseApi);
                //request.Headers.Add("Authorization", "Basic d2FuZ3lhcGluZzk3Nzo0N2UxNjRhMS0yODA2LTQzZDctOWFjYS02MDllODE1MzViYjI=");
                try
                {
                    HttpWebResponse response1 = (HttpWebResponse)request1.GetResponse();
                    using (StreamReader reader = new StreamReader(response1.GetResponseStream(), Encoding.UTF8))
                    {
                        t1 = JsonConvert.DeserializeObject<CUCCCardStatus>(reader.ReadToEnd());
                        if (t1.status == "TEST_READY")
                        {
                            t1.status = "0";
                        }
                        if (t1.status == "ACTIVATED")
                        {
                            t1.status = "2";
                        }
                        if (t1.status == "ACTIVATION_READY")
                        {
                            t1.status = "07";
                        }
                        if (t1.status == "DEACTIVATED")
                        {
                            t1.status = "02";
                        }
                        if (t1.status == "INVENTORY")
                        {
                            t1.status = "7";
                        }
                        if (t1.status == "PURGED")
                        {
                            t1.status = "9";
                        }
                        if (t1.status == "RETIRED")
                        {
                            t1.status = "9";
                        }
                        info.status = t1.status;
                        info.activetime = t1.dateActivated;
                        info.SetmealName = t1.ratePlan;
                    }
                }
                catch (Exception ex)
                {
                    info = null;
                }
               
                //卡月使用流量
                CuccCardFlow t2 = new CuccCardFlow();
                //https://api.10646.cn/rws/api/v1/devices/89860620160013379998/ctdUsages
                url = "https://api.10646.cn/rws/api/v1/devices/" + Value + "/ctdUsages";
                Encoding encoding2 = Encoding.UTF8;
                HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(url);
                request2.Method = "get";
                System.Text.Encoding encode2 = System.Text.Encoding.ASCII;
                byte[] bytedata2 = encode.GetBytes(code);
                strPath = Convert.ToBase64String(bytedata2, 0, bytedata2.Length);
                ssss = strPath;
                BaseApi = "Basic " + ssss;
                request2.Headers.Add("Authorization", BaseApi);
                //request.Headers.Add("Authorization", "Basic d2FuZ3lhcGluZzk3Nzo0N2UxNjRhMS0yODA2LTQzZDctOWFjYS02MDllODE1MzViYjI=");
                try
                {
                    HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();
                    using (StreamReader reader = new StreamReader(response2.GetResponseStream(), Encoding.UTF8))
                    {
                        info.month_flow = t2.ctdDataUsage;
                    }
                }
                catch (Exception ex)
                {
                    info = null;
                }
            return info;
        }



    }
}