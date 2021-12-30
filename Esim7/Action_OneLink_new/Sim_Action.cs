using Esim7.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using static Esim7.Action.ChangeURL;
using static Esim7.SimModel.API;

namespace Esim7.Action
{    
    /// <summary>
    /// 新的Onelink 平台 接口
    /// </summary>
    public class Sim_Action
    {
        public class returnMessage
        {
            public string API { get; set; }
            public string status { get; set; }
            public string Message { get; set; }
        }
        /// <summary>
        /// 模板   参数为一个时 如IMSI或ICCID 或MSISDN    查询
        /// </summary>
        /// <param name="Status">ICCID/IMSI 等</param>
        /// <param name="Value">值</param>
        /// <param name="APIName"> API名称</param>
        /// <returns></returns>
        public static returnMessage Get_NewOneLink_One(string Value, string APIName)
        {
            returnMessage Mess = new returnMessage();
            GetURL r = new GetURL();
            r = ChangeURL.URL2(APIName);
            string URL = r.URL;
            string s = "#";
            if (r.Type.ToLower().Contains(s.ToLower()))
            {
                //  string[] sArray = r.Type.Split('#');
                Mess.Message = "调用方法错误";
                Mess.status = "1";
                return Mess;
            }
            else if (r.Type != "query/")
            {
                Mess.Message = "非查询接口";
                Mess.status = "1";
                return Mess;
            }
            //string URL = "https://api.iot.10086.cn/v5/ec/query/sim-status?";
            //            单卡状态查询

            // 通过卡号查询物联卡的状态信息。

            //接口调用请求说明

            //http请求方式： GET / POST

            //API请求URL示例：
            //https://api.iot.10086.cn/v5/ec/query/sim-status?transid=xxxxxx&token=xxxxxx&msisdn=xxxxxx -以msisdn进行查询
            //        https://api.iot.10086.cn/v5/ec/query/sim-status?transid=xxxxxx&token=xxxxxx&imsi=xxxxxx –以imsi进行查询
            //        https://api.iot.10086.cn/v5/ec/query/sim-status?transid=xxxxxx&token=xxxxxx&iccid=xxxxxx –以iccid进行查询
            string ss = "https://api.iot.10086.cn/v5/ec/get/token?appid=200216378200200000&password=RXpURUbID&transid=2002163782002000002019071002415709643582";
            if (Value.Length == 20)
            {
                ss = URL + "transid=2002163782002000002019071002415709643582&token=" + Action.Sim_Action.Token(Value) + "&iccid=" + Value;
            }
            else if (Value.Length == 15)
            {
                ss = URL + "transid=2002163782002000002019071002415709643582&token=" + Action.Sim_Action.Token(Value) + "&imsi=" + Value;
            }
            else if (Value.Length == 13)
            {
                // ss = URL + "transid=2002163782002000002019071002415709643582&token=" + Action.Sim_Action.Token(Value) + "&msisdn=" + Value;
                ss = URL + "transid=2902107181001000002019071002415709643582&token=" + Action.Sim_Action.Token(Value) + "&msisdn=" + Value; 
            }
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ss);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                Mess.API = JsonConvert.DeserializeObject<object>(reader.ReadToEnd()).ToString();
            }
            Mess.Message = "Success";
            Mess.status = "0";
            return Mess;
        }
        /// <summary>
        /// 模板   参数为多个  查询
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="APIName"></param>
        /// <param name="type3"></param>
        /// <param name="Type3Value"></param>
        /// <returns></returns>
        public static returnMessage Get_NewOneLink_Two(string Value, string APIName, string type3,string Type3Value)
        {
            returnMessage Mess = new returnMessage();
            GetURL r = new GetURL();
            r = ChangeURL.URL2(APIName);
            string URL = r.URL;
            string s = "#";
            if (r.Type.ToLower().Contains(s.ToLower()))
            {
                string[] sArray = r.Type.Split('#');
                Mess.Message = "ok";
                Mess.status = "1";
                string type1 = "&" + sArray[0].ToString();
                string type2 = "&" + sArray[1].ToString();
                type3 = "&" + sArray[2].ToString()+"=";
                string ss = "https://api.iot.10086.cn/v5/ec/get/token?appid=200216378200200000&password=RXpURUbID&transid=2002163782002000002019071002415709643582";
                if (Value.Length == 20)
                {
                    ss = URL + "transid=2002163782002000002019071002415709643582&token=" + Action.Sim_Action.Token(Value) + "&iccid=" + Value + type3+Type3Value;
                }
                else if (Value.Length == 15)
                {
                    ss = URL + "transid=2002163782002000002019071002415709643582&token=" + Action.Sim_Action.Token(Value) + "&imsi=" + Value + type3 + Type3Value;

                }
                else if (Value.Length == 13)
                {
                    ss = URL + "transid=2002163782002000002019071002415709643582&token=" + Action.Sim_Action.Token(Value) + "&msisdn=" + Value + type3 + Type3Value;
                }
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ss);
                request.Method = "GET";
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.ContentType = "application/json";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    Mess.API = JsonConvert.DeserializeObject<object>(reader.ReadToEnd()).ToString();
                }
                Mess.Message = "Success";
                Mess.status = "0";
            }
            return Mess;
        }
        /// <summary>
        /// 模板
        /// </summary>
        /// <param name="QueryType">查询类型  ICCID IMSI MSISDN</param>
        /// <param name="Value"> 类型值</param>
        /// <param name="URL"> URL地址</param>
        /// <param name="APINAME">API接口名称</param>
        /// <returns></returns>
        public static object GetAPI_User_Class(string QueryType, string Value, string APINAME)
        {
            if (APINAME == "API23S00")
            {
                return Get_CMIOT_API23S00(QueryType, Value);
            }
            else if (APINAME == "API23S04")
            {
                return Get_CMIOT_API23S04(QueryType, Value);
            }
            else if (APINAME == "API25S04")
            {
                return Get_CMIOT_API25S04(QueryType, Value);
            }
            else
            {
                SimModel.API.Root r = new Root();
                r.message = "Fail";
                r.status = "1";
                return r;
            }
        }
        #region  用户类 单个查询



        /// <summary>
        /// 单卡基本信息查询
        ///查询物联卡码号信息、开卡时间、首次激活时间。
        /// </summary>
        /// <param name="Status">查询方式</param>
        /// <param name="Value">值</param>
        /// <returns></returns>
        public static CMIOT_API23S00.Root Get_CMIOT_API23S00(string Status, string Value)
        {
            /// <summary>
            /// 单卡基本信息查询
            ///查询物联卡码号信息、开卡时间、首次激活时间。
            ///接口调用请求说明
            ///http请求方式： GET/POS
            ///API请求URL示例：
            ///https://api.iot.10086.cn/v5/ec/query/sim-basic-info?transid=xxxxxx&token=xxxxxx&msisdn=xxxxxx -以msisdn进行查询
            ///https://api.iot.10086.cn/v5/ec/query/sim-basic-info?transid=xxxxxx&token=xxxxxx&imsi=xxxxxx –以imsi进行查询
            ///https://api.iot.10086.cn/v5/ec/query/sim-basic-info?transid=xxxxxx&token=xxxxxx&iccid=xxxxxx –以iccid进行查询
            /// </summary>
            string ss = "https://api.iot.10086.cn/v5/ec/get/token?appid=200216378200200000&password=RXpURUbID&transid=2002163782002000002019071002415709643582";
            if (Status == "ICCID")
            {
                ss = "https://api.iot.10086.cn/v5/ec/query/sim-basic-info?transid=2002163782002000002019071002415709643582&token=" + Action.Sim_Action.Token(Value) + "&iccid=" + Value;
            }
            else if (Status == "IMSI")
            {
                ss = "https://api.iot.10086.cn/v5/ec/query/sim-basic-info?transid=2002163782002000002019071002415709643582&token=" + Action.Sim_Action.Token(Value) + "&imsi=" + Value;
            }
            else if (Status == "MSISDN")
            {
                ss = "https://api.iot.10086.cn/v5/ec/query/sim-basic-info?transid=2002163782002000002019071002415709643582&token=" + Action.Sim_Action.Token(Value) + "&msisdn=" + Value;
            }
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ss);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return JsonConvert.DeserializeObject<CMIOT_API23S00.Root>(reader.ReadToEnd());
            }
        }

        /// <summary>
        /// 单卡绑定IMEI实时查询    通过卡号查询物联卡绑定的IMEI信息。
        /// </summary>
        /// <param name="Status">查询方式</param>
        /// <param name="Value">值</param>
        /// <returns></returns>
        public static CMIOT_API23S04.Root Get_CMIOT_API23S04(string Status, string Value)
        {
            string URL = "https://api.iot.10086.cn/v5/ec/query/sim-imei?";
            //            单卡绑定IMEI实时查询

            //  通过卡号查询物联卡绑定的IMEI信息。

            //接口调用请求说明

            //http请求方式： GET / POST

            //API请求URL示例：
            //https://api.iot.10086.cn/v5/ec/query/sim-imei?transid=xxxxxx&token=xxxxxx&msisdn=xxxxxx -以msisdn进行查询
            //        https://api.iot.10086.cn/v5/ec/query/sim-imei?transid=xxxxxx&token=xxxxxx&imsi=xxxxxx–以imsi进行查询
            //        https://api.iot.10086.cn/v5/ec/query/sim-imei?transid=xxxxxx&token=xxxxxx&iccid=xxxxxx –以iccid进行查询

            string ss = "https://api.iot.10086.cn/v5/ec/get/token?appid=200216378200200000&password=RXpURUbID&transid=2002163782002000002019071002415709643582";
            if (Status == "ICCID")
            {
                ss = URL + "transid=2002163782002000002019071002415709643582&token=" + Action.Sim_Action.Token(Value) + "&iccid=" + Value;

            }
            else if (Status == "IMSI")
            {
                ss = URL + "transid=2002163782002000002019071002415709643582&token=" + Action.Sim_Action.Token(Value) + "&imsi=" + Value;

            }
            else if (Status == "MSISDN")
            {
                ss = URL + "transid=2002163782002000002019071002415709643582&token=" + Action.Sim_Action.Token(Value) + "&msisdn=" + Value;
            }
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ss);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return JsonConvert.DeserializeObject<CMIOT_API23S04.Root>(reader.ReadToEnd());
            }
        }

        //单卡状态查询

        //通过卡号查询物联卡的状态信息。

        //接口调用请求说明

        //http请求方式： GET/POST

        //API请求URL示例：
        //https://api.iot.10086.cn/v5/ec/query/sim-status?transid=xxxxxx&token=xxxxxx&msisdn=xxxxxx -以msisdn进行查询
        //https://api.iot.10086.cn/v5/ec/query/sim-status?transid=xxxxxx&token=xxxxxx&imsi=xxxxxx –以imsi进行查询
        //https://api.iot.10086.cn/v5/ec/query/sim-status?transid=xxxxxx&token=xxxxxx&iccid=xxxxxx –以iccid进行查询

        /// <summary>
        ///     单卡状态查询
        /// </summary>
        /// <param name="Status">ICCID IMSI  MSISDN</param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static CMIOT_API25S041.Root Get_CMIOT_API25S04(string Status, string Value)
        {
            string URL = "https://api.iot.10086.cn/v5/ec/query/sim-status?";
            //            单卡状态查询

            // 通过卡号查询物联卡的状态信息。
            //接口调用请求说明
            //http请求方式： GET / POST
            //API请求URL示例：
            //https://api.iot.10086.cn/v5/ec/query/sim-status?transid=xxxxxx&token=xxxxxx&msisdn=xxxxxx -以msisdn进行查询
            //        https://api.iot.10086.cn/v5/ec/query/sim-status?transid=xxxxxx&token=xxxxxx&imsi=xxxxxx –以imsi进行查询
            //        https://api.iot.10086.cn/v5/ec/query/sim-status?transid=xxxxxx&token=xxxxxx&iccid=xxxxxx –以iccid进行查询

            string ss = "https://api.iot.10086.cn/v5/ec/get/token?appid=200216378200200000&password=RXpURUbID&transid=2002163782002000002019071002415709643582";


            if (Status == "ICCID")
            {
                ss = URL + "transid=2002163782002000002019071002415709643582&token=" + Action.Sim_Action.Token(Value) + "&iccid=" + Value;

            }
            else if (Status == "IMSI")
            {
                ss = URL + "transid=2002163782002000002019071002415709643582&token=" + Action.Sim_Action.Token(Value) + "&imsi=" + Value;

            }
            else if (Status == "MSISDN")
            {
                ss = URL + "transid=2002163782002000002019071002415709643582&token=" + Action.Sim_Action.Token(Value) + "&msisdn=" + Value;
            }      
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ss);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return JsonConvert.DeserializeObject<CMIOT_API25S041.Root>(reader.ReadToEnd());
            }
        }
        #endregion


        #region 用量类
        //查询近七天的流量使用情况












        #endregion


        public static string Token(string imsi)
        {

            string APPID="";
            string TRANSID="";
            string TOKEN="";

            string passeord = "";
            List<Acount> acc = new List<Acount>();
            foreach (Acount item in Action.Action_GetAccountsMessage.acount(imsi))
            {

                APPID = item.APPID;
                TRANSID = item.TRANSID;
                TOKEN = item.TOKEN;
                passeord = item.PASSWORD;

            }

            string ss = "https://api.iot.10086.cn/v5/ec/get/token?appid="+APPID+"&password="+passeord+"&transid=2002163782002000002019071002415709643582";
            string token = "";
            SimModel.Token.Root t = new SimModel.Token.Root();
            List<SimModel.Token.ResultItem> Li = new List<SimModel.Token.ResultItem>();
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ss);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                t = JsonConvert.DeserializeObject<SimModel.Token.Root>(reader.ReadToEnd());
            }
            Li = t.result;
            foreach (SimModel.Token.ResultItem item in Li)
            {
                token = item.token;
            }
            return token;
        }
    }
}