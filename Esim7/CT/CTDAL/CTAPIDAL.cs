using Dapper;
using Esim7.Models;
using Esim7.UNity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;

namespace Esim7.CT.CTDAL
{
    public class CTAPIDAL
    {
        /// <summary>
        /// 查询电信卡月使用流量信息
        /// </summary>
        public Monthlyflow FlowInfo(string method,string user_id,string date,string type)
        {           
            Monthlyflow t = new Monthlyflow();
            method = "monthlyUsage";
            user_id = "367DSTQjk59fCbnL3t9M68QwSdDL0Kks";
            date = "202001";
            type = "1";
            string password = "40P4Xn2bkyyGvmrB";
            string key1 = "682";
            string key2 = "NB5";
            string key3 = "ckb";
            string SS = string.Empty;
            //具体接口参数需参照自管理门户在线文档
            string[] arr = {user_id, password, method,date,type }; //加密数组，数组所需参数根据对应的接口文档
            DesUtils des = new DesUtils(); //加密工具类实例化
            string passwordEnc = des.strEnc(password, key1, key2, key3);  //密码加密 
            string sign = des.strEnc(des.naturalOrdering(arr), key1, key2, key3); //生成sign加密值
            string url = string.Empty;
            url = "http://api.ct10649.com:9001/m2m_ec/query.do?method=monthlyUsage";
            SS = url + "&date=" + date + "&type=" + type + "&user_id=" + user_id + "&passWord=" + passwordEnc + "&sign=" + sign;
            //ss = URL + "queryapnopenstatus?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + ICCID;
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                t = JsonConvert.DeserializeObject<Monthlyflow>(reader.ReadToEnd());
            }
            return t;
        }

        ///<summary>
        ///流量查询（当天）接口  解析XML格式
        /// </summary>
        public string xmltest()
        {
            string str = "";
            XmlDocument xml = new XmlDocument();
            string method = "queryTrafficOfToday";
            string user_id = "367DSTQjk59fCbnL3t9M68QwSdDL0Kks";
            string password = "40P4Xn2bkyyGvmrB";
            string key1 = "682";
            string key2 = "NB5";
            string key3 = "ckb";
            string SS = string.Empty;
            string iccid = "8986111925804813264";           
            //具体接口参数需参照自管理门户在线文档
            string[] arr = { user_id, password, method, iccid}; //加密数组，数组所需参数根据对应的接口文档
            DesUtils des = new DesUtils(); //加密工具类实例化
            string passwordEnc = des.strEnc(password, key1, key2, key3);  //密码加密 
            string sign = des.strEnc(des.naturalOrdering(arr), key1, key2, key3); //生成sign加密值
            string url = string.Empty;           
            url = "http://api.ct10649.com:9001/m2m_ec/query.do?";
            SS = url+ "method="+method+ "&iccid=" + iccid + "&needDtl=1&user_id=" + user_id + "&passWord=" + passwordEnc + "&sign=" + sign;
            //ss = URL + "queryapnopenstatus?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + ICCID;
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                 str = reader.ReadToEnd();
                xml.LoadXml(str);//加载数据
                XmlNodeList xxList = xml.GetElementsByTagName("web:NEW_DATA_TICKET_QRsp"); //取得节点e799bee5baa6e58685e5aeb931333337396362名为row的XmlNode集
                foreach (XmlNode item in xxList)
                {
                    str = item["IRESULT"].InnerText;
                }
            }
            return str;
        }
        ///<summary>
        ///获取电信卡的月使用流量
        /// </summary>
        public string GetCuccCardMonthFlow(string ICCID)
        {
            string MonthFlow = "";
            try
            {
                if (ICCID.Length == 20)
                {
                    ICCID = ICCID.Substring(0, ICCID.Length - 1);
                }
                //http://api.ct10649.com:9001/m2m_ec/query.do?method=queryTraffic&access_number=1491000000&user_id= test & passWord = 443 * *****EC6E & sign = 7E7D6 * **edDtl = 0
                XmlDocument xml = new XmlDocument();
                string method = "queryTraffic";
                string user_id = "367DSTQjk59fCbnL3t9M68QwSdDL0Kks";
                string password = "40P4Xn2bkyyGvmrB";
                string key1 = "682";
                string key2 = "NB5";
                string key3 = "ckb";
                string SS = string.Empty;
                //string iccid = "8986111925804813264";
                //具体接口参数需参照自管理门户在线文档
                string[] arr = { user_id, password, method, ICCID }; //加密数组，数组所需参数根据对应的接口文档
                DesUtils des = new DesUtils(); //加密工具类实例化
                string passwordEnc = des.strEnc(password, key1, key2, key3);  //密码加密 
                string sign = des.strEnc(des.naturalOrdering(arr), key1, key2, key3); //生成sign加密值
                string url = string.Empty;
                url = "http://api.ct10649.com:9001/m2m_ec/query.do?";
                SS = url + "method=" + method + "&iccid=" + ICCID + "&needDtl=1&user_id=" + user_id + "&passWord=" + passwordEnc + "&sign=" + sign;
                //ss = URL + "queryapnopenstatus?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + ICCID;
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
                request.Method = "GET";
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.ContentType = "application/json";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    try
                    {
                        string str = reader.ReadToEnd();
                        if (str.Length > 30)
                        {
                            xml.LoadXml(str);//加载数据
                            XmlNodeList xxList = xml.GetElementsByTagName("web:NEW_DATA_TICKET_QRsp"); //取得节点e799bee5baa6e58685e5aeb931333337396362名为row的XmlNode集
                            foreach (XmlNode item in xxList)
                            {
                                MonthFlow = item["TOTAL_BYTES_CNT"].InnerText;
                            }
                        }
                        else
                        {
                            MonthFlow = "暂未查询到月使用流量信息";
                        }
                    }
                    catch
                    {
                        MonthFlow = "暂未查询到月使用流量信息";
                    }
                }
            }
            catch
            {
                MonthFlow = "暂未查询到月使用流量信息";
            }
            
            return MonthFlow;
        }

        ///<summary>
        ///获取卡的工作状态(在线/离线)
        /// </summary>
        public RootCTCardWorkStatus GetCTCardWorkStatus(string ICCID)
        {
            if (ICCID.Length == 20)
            {
                ICCID = ICCID.Substring(0, ICCID.Length - 1);
            }
            RootCTCardWorkStatus t = new RootCTCardWorkStatus();
            //ICCID = "8986111925804813264";
            string method = "getOnlineStatus";
            string user_id = "367DSTQjk59fCbnL3t9M68QwSdDL0Kks";
            string password = "40P4Xn2bkyyGvmrB";
            string key1 = "682";
            string key2 = "NB5";
            string key3 = "ckb";
            string SS = string.Empty;
            //具体接口参数需参照自管理门户在线文档
            string access_number = "14914000000";  //物联网卡号(149或10649号段)
            string[] arr = { user_id, password, method,ICCID}; //加密数组，数组所需参数根据对应的接口文档
            DesUtils des = new DesUtils(); //加密工具类实例化
            string passwordEnc = des.strEnc(password, key1, key2, key3);  //密码加密 
            string sign = des.strEnc(des.naturalOrdering(arr), key1, key2, key3); //生成sign加密值
            string url = string.Empty;
            url = "http://api.ct10649.com:9001/m2m_ec/query.do?method=getOnlineStatus";
            SS = url+ "&iccid="+ICCID+ "&user_id=" + user_id + "&passWord=" + passwordEnc + "&sign=" + sign;
            //ss = URL + "queryapnopenstatus?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + ICCID;
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                //string ss = reader.ReadToEnd();
                t = JsonConvert.DeserializeObject<RootCTCardWorkStatus>(reader.ReadToEnd());
            }
            return t;
        }

        ///<summary>
        ///查询卡的状态  1：可激活 2：测试激活 3:测试去激活 4:在用 5:停机 6:运营商状态管理(状态不准确)
        /// </summary>
        public RootCTStatus GetCTCardStatus(string ICCID)
        {
            RootCTStatus t = new RootCTStatus();
            //http://api.ct10649.com:9001/m2m_ec/query.do
            string method = "getSIMList";
            string user_id = "367DSTQjk59fCbnL3t9M68QwSdDL0Kks";
            string date = "202001";
            string pageIndex = "1";
            string password = "40P4Xn2bkyyGvmrB";
            string key1 = "682";
            string key2 = "NB5";
            string key3 = "ckb";
            string SS = string.Empty;
            //具体接口参数需参照自管理门户在线文档
            string access_number = "14914000000";  //物联网卡号(149或10649号段)
            string[] arr = { user_id, password, method}; //加密数组，数组所需参数根据对应的接口文档
            DesUtils des = new DesUtils(); //加密工具类实例化
            string passwordEnc = des.strEnc(password, key1, key2, key3);  //密码加密 
            string sign = des.strEnc(des.naturalOrdering(arr), key1, key2, key3); //生成sign加密值
            string url = string.Empty;
            url = "http://api.ct10649.com:9001/m2m_ec/query.do?method=getSIMList";
            SS = url + "&method=" + method + "&pageIndex=" + pageIndex + "&accNumber="+ICCID+"&user_id=" + user_id + "&passWord=" + passwordEnc + "&sign=" + sign;
            //ss = URL + "queryapnopenstatus?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + ICCID;
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                t = JsonConvert.DeserializeObject<RootCTStatus>(reader.ReadToEnd());
                //if (t.resultCode == "0")
                //{
                //    t.resultCode = t.resultCode;
                //    t.resultMsg = t.resultMsg;
                //    t.groupTransactionId = t.groupTransactionId;
                //    t.description.simList[0].simStatus = t.description.simList[0].simStatus;
                //}
            }
            return t;
        }

        ///<summary>
        ///电信卡状态 
        /// </summary>
        public string GetStatusName(string ICCID)
        {
           
            XmlDocument xml = new XmlDocument();
            //ICCID = "1410028804391";
            string StatusName = string.Empty;
            string method = "prodInstQuery";
            string user_id = "367DSTQjk59fCbnL3t9M68QwSdDL0Kks";
            string password = "40P4Xn2bkyyGvmrB";
            string key1 = "682";
            string key2 = "NB5";
            string key3 = "ckb";
            string SS = string.Empty;
            //具体接口参数需参照自管理门户在线文档
            string[] arr = { ICCID,user_id,password,method}; //加密数组，数组所需参数根据对应的接口文档
            DesUtils des = new DesUtils(); //加密工具类实例化
            string passwordEnc = des.strEnc(password, key1, key2, key3);  //密码加密 
            string sign = des.strEnc(des.naturalOrdering(arr), key1, key2, key3); //生成sign加密值
            string url = string.Empty;
            url = "http://api.ct10649.com:9001/m2m_ec/query.do?";
            SS = url + "method=" + method + "&access_number=" + ICCID + "&user_id=" + user_id + "&passWord=" + passwordEnc + "&sign=" + sign;
            //ss = URL + "queryapnopenstatus?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + ICCID;
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                string str = reader.ReadToEnd();
                if (str.Length > 30)
                {
                    xml.LoadXml(str);//加载数据
                    XmlNodeList xxList = xml.GetElementsByTagName("prodInfos"); //取得节点e799bee5baa6e58685e5aeb931333337396362名为row的XmlNode集
                    foreach (XmlNode item in xxList)
                    {
                        StatusName = item["prodStatusName"].InnerText;
                    }
                }
                else
                {
                    StatusName = "";
                }
            }
            return StatusName;
        }

        ///<summary>
        ///三码互查接口 根据电信卡iccid号码去物联网卡号
        /// </summary>
        public string GetCTCard_ID(string ICCID)
        {
            if (ICCID.Length == 20)
            {
                ICCID = ICCID.Substring(0,ICCID.Length-1);
            }
            string s = null;
            string method = "getTelephonePlus";
            string user_id = "367DSTQjk59fCbnL3t9M68QwSdDL0Kks";
            string password = "40P4Xn2bkyyGvmrB";
            string key1 = "682";
            string key2 = "NB5";
            string key3 = "ckb";
            string SS = string.Empty;
            Root t = new Root();
            //具体接口参数需参照自管理门户在线文档
            string access_number = "14914000000";  //物联网卡号(149或10649号段)
            string[] arr = { user_id, password, method, ICCID}; //加密数组，数组所需参数根据对应的接口文档
            DesUtils des = new DesUtils(); //加密工具类实例化
            string passwordEnc = des.strEnc(password, key1, key2, key3);  //密码加密 
            string sign = des.strEnc(des.naturalOrdering(arr), key1, key2, key3); //生成sign加密值
            string url = string.Empty;
           //http://api.ct10649.com:9001m2m_ec/query.do?method=getTelephonePlus&iccid=365601975911101505&user_id= test & passWord = 32C4 * ***7F2000 & sign = C503 * ***36FD
            url = @"http://api.ct10649.com:9001/m2m_ec/query.do?method=getTelephonePlus";
            SS = url + "&iccid=" + ICCID +"&user_id=" + user_id + "&passWord=" + passwordEnc + "&sign=" + sign;
            //ss = URL + "queryapnopenstatus?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + ICCID;
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                try
                {
                    t = JsonConvert.DeserializeObject<Root>(reader.ReadToEnd());
                    s = t.info.access_number;
                    if (string.IsNullOrWhiteSpace(s))
                    {
                        s = "暂未查询到卡号";
                    }
                }
                catch
                {
                    s = "暂未查询到卡号";
                }
            }
            return s;
        }

        ///<summary>
        ///基站粗定位
        /// </summary>
        public string Location1(string access_number)
        {
            access_number = access_number.Substring(0, access_number.Length - 1);
            access_number = GetCTCard_ID(access_number);
            string method = "getLocationByPhone";
            string user_id = "367DSTQjk59fCbnL3t9M68QwSdDL0Kks";
            string password = "40P4Xn2bkyyGvmrB";
            string key1 = "682";
            string key2 = "NB5";
            string key3 = "ckb";
            string SS = string.Empty;
            //access_number = access_number;//物联网卡接入号码
            string str = string.Empty;
            //具体接口参数需参照自管理门户在线文档
            string[] arr = { user_id, password, method,access_number}; //加密数组，数组所需参数根据对应的接口文档
            DesUtils des = new DesUtils(); //加密工具类实例化
            string passwordEnc = des.strEnc(password, key1, key2, key3);  //密码加密 
            string sign = des.strEnc(des.naturalOrdering(arr), key1, key2, key3); //生成sign加密值
            string url = string.Empty;
            url = "http://api.ct10649.com:9001/m2m_ec/app/location.do?method=getLocationByPhone";
            SS = url + "&access_number=" + access_number + "&user_id=" + user_id + "&passWord=" + passwordEnc + "&sign=" + sign;
            //ss = URL + "queryapnopenstatus?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + ICCID;
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                str = reader.ReadToEnd();
            }
            //解析响应密文
            var data =str; //获取的定位信息加密字符串
            var position = des.strDec(data, key1, key2, key3);//定位数据解密，调用DES解密函数
            return position;
        }

        ///<summary>
        ///基站粗定位新
        /// </summary>
        public LocationInfo Location(string access_number)
        {
            LocationInfo info = new LocationInfo();
            if (access_number.Length == 20)
            {
                access_number = access_number.Substring(0, access_number.Length - 1);
                access_number = GetCTCard_ID(access_number);
            }
            if (access_number.Length == 19)
            {
                access_number = GetCTCard_ID(access_number);
            }
            string method = "getLocationByPhone";
            string user_id = "367DSTQjk59fCbnL3t9M68QwSdDL0Kks";
            string password = "40P4Xn2bkyyGvmrB";
            string key1 = "682";
            string key2 = "NB5";
            string key3 = "ckb";
            string SS = string.Empty;
            //access_number = access_number;//物联网卡接入号码
            string str = string.Empty;
            //具体接口参数需参照自管理门户在线文档
            string[] arr = { user_id, password, method, access_number }; //加密数组，数组所需参数根据对应的接口文档
            DesUtils des = new DesUtils(); //加密工具类实例化
            string passwordEnc = des.strEnc(password, key1, key2, key3);  //密码加密 
            string sign = des.strEnc(des.naturalOrdering(arr), key1, key2, key3); //生成sign加密值
            string url = string.Empty;
            url = "http://api.ct10649.com:9001/m2m_ec/app/location.do?method=getLocationByPhone";
            SS = url + "&access_number=" + access_number + "&user_id=" + user_id + "&passWord=" + passwordEnc + "&sign=" + sign;
            //ss = URL + "queryapnopenstatus?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + ICCID;
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                str = reader.ReadToEnd();
            }
            //解析响应密文
            var data = str; //获取的定位信息加密字符串
            var position = des.strDec(data, key1, key2, key3);//定位数据解密，调用DES解密函数
            info = JsonConvert.DeserializeObject<LocationInfo>(position);
            return info;
        }



        ///<summary>
        ///电信修改状态停机保号 orderTypeId 19：停机保号  20：停机保号复机
        /// </summary>
        public Information UpdateCtStatus(string Card_ICCID, string orderTypeId)
        {
            //停机保号请求地址  disabledNumber
            //http://api.ct10649.com:9001/m2m_ec/query.do?method=disabledNumber&user_id=test&access_number=149****00 & acctCd = &passWord = 03A**** CE24&sign = 03A3 * ***0E9BE & orderTypeId = 19
            Information info = new Information();
            XmlDocument xml = new XmlDocument();
            string method = "disabledNumber";
            string user_id = "367DSTQjk59fCbnL3t9M68QwSdDL0Kks";
            string password = "40P4Xn2bkyyGvmrB";
            string key1 = "682";
            string key2 = "NB5";
            string key3 = "ckb";
            string SS = string.Empty;
            string acctCd =string.Empty;
            //Card_ICCID = "1410404400343";
            //具体接口参数需参照自管理门户在线文档
            string[] arr = { Card_ICCID,user_id, password, method, acctCd,orderTypeId }; //加密数组，数组所需参数根据对应的接口文档
            DesUtils des = new DesUtils(); //加密工具类实例化
            string passwordEnc = des.strEnc(password, key1, key2, key3);  //密码加密 
            string sign = des.strEnc(des.naturalOrdering(arr), key1, key2, key3); //生成sign加密值
            string url = string.Empty;
            url = "http://api.ct10649.com:9001/m2m_ec/query.do?";
            SS = url + "method=" + method + "&access_number=" + Card_ICCID + "&user_id=" + user_id + "&passWord=" + passwordEnc + "&acctCd="+acctCd + "&sign=" + sign+ "&orderTypeId="+orderTypeId;
            //ss = URL + "queryapnopenstatus?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + ICCID;
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                string str = reader.ReadToEnd();
                //xml.LoadXml(str);//加载数据
                info.Msg = str;
                info.flg = "1";
            }
            return info;
        }

        ///<summary>
        ///电信机卡重绑
        /// </summary>
        public Rootjkcb SetJiKaChongBang(string ICCID)
        {
            Rootjkcb root = new Rootjkcb();
            try
            {
                //机卡重绑地址
                //http://api.ct10649.com :9001/m 2m _ec/app/serviceAccept.do?m ethod=IM EIReRecord& action=AD D & iccid=8986031642100394660 & user_id = test & passWord = 32C40A3FC633213EF9EF670D 337F2000 & sign =**
                Information info = new Information();
                XmlDocument xml = new XmlDocument();
                string method = "M EIReRecord";
                string user_id = "367DSTQjk59fCbnL3t9M68QwSdDL0Kks";
                string password = "40P4Xn2bkyyGvmrB";
                string key1 = "682";
                string key2 = "NB5";
                string key3 = "ckb";
                string SS = string.Empty;
                string action = "AD D";
                //Card_ICCID = "1410404400343";
                //具体接口参数需参照自管理门户在线文档
                string[] arr = { ICCID, user_id,action, password, method }; //加密数组，数组所需参数根据对应的接口文档
                DesUtils des = new DesUtils(); //加密工具类实例化
                string passwordEnc = des.strEnc(password, key1, key2, key3);  //密码加密 
                string sign = des.strEnc(des.naturalOrdering(arr), key1, key2, key3); //生成sign加密值
                string url = string.Empty;
                url = "http://api.ct10649.com:9001/m2m_ec/app/serviceAccept.do?";
                SS = url + "method=" + method + "&iccid=" + ICCID + "&user_id=" + user_id + "&passWord=" + passwordEnc +"&sign=" + sign;
                //ss = URL + "queryapnopenstatus?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + ICCID;
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SS);
                request.Method = "GET";
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.ContentType = "application/json";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    root = JsonConvert.DeserializeObject<Rootjkcb>(reader.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                
                
            }
            return root;
        }




        ///<summary>
        ///移远电信卡基站定位
        /// </summary>
        public YiYuanLocationInfo GetYiYuanLocation(string access_number)
        {
            YiYuanLocationInfo info = new YiYuanLocationInfo();
            string URL = @"http://api.quectel.com/openapi/router?"; 
            string appKey="29V5HOM2852524G9";
            string secret ="6TaZ26rC";          
            long t =(DateTime.Now.ToUniversalTime().Ticks-621355968000000000)/10000000;//;时间戳
            string method ="fc.function.card.location";
            string s = secret + "appKey" + appKey + "iccid" + access_number + "method" + method + "t" + t+secret;
            //string singstr ="abcdefappKey000001contenttestMessagemethodfc.function.sms.sendmsisdns1069868950247t1493893971abcdef";
            string sign = GetSing(s);
            sign = sign.ToUpper();//转大写
            URL  = URL+"appKey="+appKey+ "&iccid="+access_number+ "&method="+method+ "&sign="+sign+"&t="+t;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/json";
            Stream requestStream = request.GetRequestStream();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                info = JsonConvert.DeserializeObject<YiYuanLocationInfo>(reader.ReadToEnd());
            }
            return info;
        }

        ///<summary>
        ///移远单卡流量查询
        /// </summary>
         public YiYuanRootFlow GetYiYuanCtFlow(string month)
         {
            YiYuanRootFlow info = new YiYuanRootFlow();
            string URL = @"http://api.quectel.com/openapi/router?";
            string appKey = "29V5HOM2852524G9";
            string secret = "6TaZ26rC";
            long t = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;//;时间戳
            string method = "fc.function.monthflow.list";
            string s = secret + "appKey" + appKey + "month" + month + "method" + method + "t" + t + secret;
            string sign = GetSing(s);
            sign = sign.ToUpper();//转大写
            URL = URL + "appKey=" + appKey + "&month=" + month + "&method=" + method + "&sign=" + sign + "&t=" + t;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/json";
            Stream requestStream = request.GetRequestStream();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                info = JsonConvert.DeserializeObject<YiYuanRootFlow>(reader.ReadToEnd());
            }
            return info;
         }
        ///<summary>
        ///移远电信卡实时工作状态
        /// </summary>
        public YiYuanWorkStatusRoot GetYiYuanWorkStatusInfo(string Card_ICCID)
        {
            YiYuanWorkStatusRoot info = new YiYuanWorkStatusRoot();
            string URL = @"http://api.quectel.com/openapi/router?";
            string appKey = "29V5HOM2852524G9";
            string secret = "6TaZ26rC";
            long t = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;//;时间戳
            string method = "fc.function.card.realtimestatus";
            string s = secret + "appKey" + appKey + "iccid" + Card_ICCID + "method" + method + "t" + t + secret;
            string sign = GetSing(s);
            sign = sign.ToUpper();//转大写
            URL += "appKey=" + appKey + "&iccid=" + Card_ICCID + "&method=" + method + "&sign=" + sign + "&t=" + t + "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/json";
            Stream requestStream = request.GetRequestStream();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                info = JsonConvert.DeserializeObject<YiYuanWorkStatusRoot>(reader.ReadToEnd());
            }
            return info;
        }

        ///<summary>
        ///资产详细信息
        /// </summary>
        public YiYuanCardDetailDto YiYuanCardDetail(string iccid)
        {
            YiYuanCardDetailDto dto =new YiYuanCardDetailDto();
            string URL = @"http://api.quectel.com/openapi/router?";
            string appKey = "29V5HOM2852524G9";
            string secret = "6TaZ26rC";
            long t = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;//;时间戳
            string method = "fc.function.card.info";
            string s = secret + "appKey" + appKey + "iccid" + iccid + "method" + method + "t" + t + secret;
            string sign = GetSing(s);
            sign = sign.ToUpper();//转大写
            URL += "appKey="+appKey+ "&iccid="+iccid+ "&method="+method+"&sign="+sign+"&t="+t+"";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/json";
            Stream requestStream = request.GetRequestStream();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                dto = JsonConvert.DeserializeObject<YiYuanCardDetailDto>(reader.ReadToEnd());  
            }
            return dto;
        }

        ///<summary>
        ///电信NB基站定位支持移远和电信两种 access_number ICCID或者卡号
        /// </summary>
        public LocationInfo GetCTLocationInfo(string access_number)
        {
            LocationInfo info = new LocationInfo();
            try
            {
                string sqlct = string.Empty;
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    if (access_number.Length == 20 || access_number.Length==19)
                    {
                        if (access_number.Length == 20)
                        {
                            access_number = access_number.Substring(0, access_number.Length - 1);
                            sqlct = "select Platform,Card_ICCID,Card_ID from ct_card where Card_ICCID='" + access_number + "'";
                        }
                        if (access_number.Length == 19)
                        {
                            sqlct = "select Platform,Card_ICCID,Card_ID from ct_card where Card_ICCID='" + access_number + "'";
                        }
                        var listct= conn.Query<Card>(sqlct).FirstOrDefault();
                        if (listct != null)
                        {
                            if (listct.Platform == "21")//运营商
                            {
                                info = Location(listct.Card_ICCID);
                            }
                            if (listct.Platform == "20")//移远
                            {
                                info = GetYiYuanLocation(access_number);
                            }
                        }   
                    }
                    if (access_number.Length == 13)
                    {
                        sqlct = "select Platform,Card_ICCID,Card_ID from ct_card where Card_ID='" + access_number + "'";
                        var listct = conn.Query<Card>(sqlct).FirstOrDefault();
                        if (listct != null)
                        {
                            if (listct.Platform == "21")//运营商
                            {
                                info = Location(access_number);
                            }
                            if (listct.Platform == "20")//移远
                            {
                                info = GetYiYuanLocation(listct.Card_ICCID);
                            }
                        }
                    } 
                }
            }
            catch (Exception ex)
            {
                info.POSITIONRESULT = -1;
            }
            return info;
        }

        
        /// <summary>
        /// 签名(sign)算法  SHA1加密字符串
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public string GetSing(string singstr)
        {
            string sing = string.Empty;
            var strRes = Encoding.Default.GetBytes(singstr);
            HashAlgorithm iSha = new SHA1CryptoServiceProvider();
            strRes = iSha.ComputeHash(strRes);
            var enText = new StringBuilder();
            foreach (byte iByte in strRes)
            {
                enText.AppendFormat("{0:x2}", iByte);
            }
            sing = enText.ToString();
            return sing;
        }


    }
}