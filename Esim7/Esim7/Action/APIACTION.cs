using Dapper;
using Esim7.Models;
using Esim7.UNity;
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
using static Esim7.Models.CMIOT_API12001;

namespace Esim7.Action
{
    public class APIACTION
    {

       static  string APPID = ConfigurationManager.AppSettings["APPID"];
       static  string PASSWORD = ConfigurationManager.AppSettings["PASSWORD"];
       static  string TOKEN = ConfigurationManager.AppSettings["TOKEN"];
       static  string TRANSID = ConfigurationManager.AppSettings["TRANSID"];

       static  string URL = "https://api.iot.10086.cn/v2/";

      static   string ss;
        /// <summary>
        ///在线信息实时查询  返回实体对象单个
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        public static Root_CMIOT_API12001 GetCMIOT_API12001( string imsi)
        {


            

            List<Acount> acc = new List<Acount>();
            foreach (Acount item in Action.Action_GetAccountsMessage.acount(imsi))
            {

                APPID = item.APPID;
                TRANSID = item.TRANSID;
                TOKEN = item.TOKEN;


            }





            string EBID = "0001000000008";


            if (imsi.Length==15)
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

              
              return   JsonConvert.DeserializeObject<Root_CMIOT_API12001>(reader.ReadToEnd());
            }

           
        }
        /// <summary>
        ///  返回集团GPRS在线物联卡数量查询
        /// </summary>
        /// <returns></returns>
        public static Root_CMIOT_API2101 GetCMIOT_API2101() {
            string EBID = "0001000000428";
            https://api.iot.10086.cn/v2/querygprsonlinecardcount?appid=xxx&transid=xxx&ebid=xxx&token=xxx


            ss = URL + "querygprsonlinecardcount?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN ;


            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ss);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {


                return JsonConvert.DeserializeObject<Root_CMIOT_API2101>(reader.ReadToEnd());
            }






        }
        /// <summary>
        /// 开关机信息实时查询 
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        public static Root_CMIOT_API2008 GetCMIOT_API2008(string imsi) {
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

                if (imsi.Length==15)
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
               
         
            string Card_ID="";
            string Card_IMSI="";
            string Card_IMEI="";
            string Card_ICCID="";
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ss);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {               
                Root_CMIOT_API2008 s 

                = JsonConvert.DeserializeObject<Root_CMIOT_API2008>(reader.ReadToEnd());
                using (IDbConnection
 conn = DapperService.MySqlConnection())

                {
                    string sql2 = "select  Card_ID ,Card_IMSI, Card_IMEI,Card_ICCID from card where  Card_IMSI='" + imsi+"'";

                    Card card = new Card();


                    List<Card> li=     conn.Query<Card>(sql2).ToList();


                   

                    foreach (Card item in li)
                    {
                        Card_ID = item.Card_ID;
                        Card_IMSI = item.Card_IMSI;
                        Card_IMEI = item.Card_IMEI;
                        Card_ICCID = item.Card_ICCID;

                    }




                }               
                s.imsi = imsi;
                s.Card_ICCID = Card_ICCID;
                s.Card_ID = Card_ID;
                s.Card_IMEI = Card_IMEI;
                s.Card_IMSI = Card_IMSI;
                if (s.message!="正确")
                {
                    s.status = "1";
                }
                else
                {
                    s.status = "0";
                }

                return s;
            }


            }
            catch (Exception)
            {

                throw;
            }




        }
        /// <summary>
        ///  CMIOT_API2020-套餐内GPRS流量使用情况实时查询 (集团客户)  集团客户可查询所属物联卡当月套餐内GPRS流量使用情况
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        public static Root_CMIOT_API2020 GetGetCMIOT_API2020(string imsi) {
            List<Acount> acc = new List<Acount>();
            foreach (Acount item in Action.Action_GetAccountsMessage.acount(imsi))
            {

                APPID = item.APPID;
                TRANSID = item.TRANSID;
                TOKEN = item.TOKEN;


            }


            string EBID = "0001000000083";

            //https://api.iot.10086.cn/v2/gprsrealtimeinfo?appid=xxx&ebid=xxx&transid=xxx&token=xxx&imsi=xxx -以imsi进行查询 





            if (imsi.Length==15)
            {
                ss = URL + "gprsrealtimeinfo?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&imsi=" + imsi;

            }

            else if (imsi.Length == 20)
            {
                ss = URL + "gprsrealtimeinfo?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&iccid=" + imsi;
            }
            else if (imsi.Length == 13)
            {
                ss = URL + "gprsrealtimeinfo?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + imsi;
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


                return JsonConvert.DeserializeObject<Root_CMIOT_API2020>(reader.ReadToEnd());
            }





        }
        /// <summary>
        /// 物联卡单日GPRS使用量查询  CMIOT_API2300  单卡查询
        /// </summary>
        /// <param name="imsi"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static Root_CMIOT_API2300 GetCMIOT_API2300(string  imsi,string date) {


            List<Acount> acc = new List<Acount>();
            foreach (Acount item in Action.Action_GetAccountsMessage.acount(imsi))
            {

                APPID = item.APPID;
                TRANSID = item.TRANSID;
                TOKEN = item.TOKEN;


            }

            string EBID = "0001000000407";




            //https://api.iot.10086.cn/v2/gprsusedinfosinglebydate?appid=xxx&ebid=xxx&transid=xxx&token=xxx&imsi=xxx&queryDate=xxx -以imsi进行查询 


            if (imsi.Length==15)
            {
                ss = URL + "gprsusedinfosinglebydate?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&imsi=" + imsi + "&queryDate=" + date;
            }
            else if (imsi.Length==20)
            {
                ss = URL + "gprsusedinfosinglebydate?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&iccid=" + imsi + "&queryDate=" + date;
            }
            else if (imsi.Length == 13)
            {
                ss = URL + "gprsusedinfosinglebydate?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + imsi + "&queryDate=" + date;
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


                return JsonConvert.DeserializeObject<Root_CMIOT_API2300>(reader.ReadToEnd());
            }





        }
        /// <summary>
        /// 物联卡单日GPRS使用量查询  CMIOT_API2300  按公司查询
        /// </summary>
        /// <param name="Card_CompanyID"></param>
        /// <param name="num"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static List<Root_List_CMIOT_API2300> GetCMIOT_API2300s(string Card_CompanyID, string num,string date)
        {
            List<Root_List_CMIOT_API2300> list2 =new List<Root_List_CMIOT_API2300> ();



            string s = "and Card_CompanyID=@Card_CompanyID  limit " + num + ",20";

       



            using (IDbConnection
        conn = DapperService.MySqlConnection())

            {
                string sql2 = "select Card_ID,Card_IMSI,Card_Type,Card_IMEI,Card_State,Card_WorkState,  date_format(Card_OpenDate, '%Y-%m-%d %H:%i:%s') as  Card_OpenDate  ,         date_format( Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,Card_Remarks, status, Card_CompanyID,  Card_ICCID   from card where status=1   " + s;



                List<Card> list = conn.Query<Card>(sql2, new { Card_CompanyID = Card_CompanyID }).ToList();


              

                foreach (Card item in list)
                {
                    Root_List_CMIOT_API2300 root2300 = new Root_List_CMIOT_API2300();

                    if (item.status.ToString() == "1")
                    {
                        root2300.Root_CMIOT_API2300 = GetCMIOT_API2300(item.Card_IMSI, date);

                        root2300.IMSI = item.Card_IMSI;
                        //if (root2300.Root_CMIOT_API2300.status=="0")
                        //{
                        list2.Add(root2300);
                        //}
                    }



                }

              


            }



            return list2;





        }
        public static List<Root_CMIOT_API2300> GetCMIOT_API2300ss(string Card_CompanyID, string num, string date) {




            List<Root_CMIOT_API2300> list2 = new List<Root_CMIOT_API2300>();



            string s = "and Card_CompanyID=@Card_CompanyID  limit " + num + ",20";





            using (IDbConnection
        conn = DapperService.MySqlConnection())

            {
                string sql2 = "select Card_ID,Card_IMSI,Card_Type,Card_IMEI,Card_State,Card_WorkState,  date_format(Card_OpenDate, '%Y-%m-%d %H:%i:%s') as  Card_OpenDate  ,         date_format( Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,Card_Remarks, status, Card_CompanyID,  Card_ICCID   from card where status=1   " + s;



                List<Card> list = conn.Query<Card>(sql2, new { Card_CompanyID = Card_CompanyID }).ToList();




                foreach (Card item in list)
                {
                    Root_List_CMIOT_API2300 root2300 = new Root_List_CMIOT_API2300();
                    Root_CMIOT_API2300 Root = new Root_CMIOT_API2300();
                    if (item.status.ToString() == "1")
                    {




                   Root= GetCMIOT_API2300(item.Card_IMSI, date);

                        Root.IMSI = item.Card_IMSI;
                        //if (root2300.Root_CMIOT_API2300.status=="0")
                        //{
                        list2.Add(Root);
                        //}
                    }



                }




            }



            return list2;








        }
        /// <summary>
        ///  CMIOT_API2002-用户状态信息实时查询  集团客户可根据所属物联卡的码号信息实时查询该卡的状态信息。
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        public static Root_CMIOT_API2002 GetCMIOT_API2002(string imsi) {
            List<Acount> acc = new List<Acount>();
            foreach (Acount item in Action.Action_GetAccountsMessage.acount(imsi))
            {

                APPID = item.APPID;
                TRANSID = item.TRANSID;
                TOKEN = item.TOKEN;


            }


            string EBID = "0001000000009";

            //  https://api.iot.10086.cn/v2/userstatusrealsingle?appid=xxx&ebid=xxx&transid=xxx&token=xxx&imsi=xxx –以imsi进行查询


            if (imsi.Length==15)
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
        /// <summary>
        ///  CMIOT_API2103-物联卡GPRS服务开通查询 集团客户可以通过卡号（MSISDN/ICCID/IMSI，单卡）信息查询此卡的GPRS服务开通状态
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        public static Root_CMIOT_API2103 GetCMIOT_API2103(string imsi) {

            List<Acount> acc = new List<Acount>();
            foreach (Acount item in Action.Action_GetAccountsMessage.acount(imsi))
            {

                APPID = item.APPID;
                TRANSID = item.TRANSID;
                TOKEN = item.TOKEN;


            }

            string EBID = "0001000000430";

            // https://api.iot.10086.cn/v2/querygprsopenstatus?appid=xxx&transid=xxx&ebid=xxx&token=xxx&imsi=xxx –以imsi进行查询

            if (imsi.Length==15)
            {
                ss = URL + "querygprsopenstatus?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&imsi=" + imsi;

            }
            else if (imsi.Length==20)
            {
                ss = URL + "querygprsopenstatus?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&iccid=" + imsi;
            }
            else if (imsi.Length == 13)
            {
                ss = URL + "querygprsopenstatus?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + imsi;
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


                return JsonConvert.DeserializeObject<Root_CMIOT_API2103>(reader.ReadToEnd());
            }


        }
        /// <summary> 
       /// CMIOT_API2105-物联卡生命周期查询
       /// 集团客户根据卡号（imsi、msisdn、iccid三个中任意一个），查询物联卡当前生命周期，生命周期包括：00:正式期，01:测试期，02:沉默期，03:其他。
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        public static Root_CMIOT_API2105 GetCMIOT_API2105(string imsi) {

            List<Acount> acc = new List<Acount>();
            foreach (Acount item in Action.Action_GetAccountsMessage.acount(imsi))
            {

                APPID = item.APPID;
                TRANSID = item.TRANSID;
                TOKEN = item.TOKEN;


            }

            // https://api.iot.10086.cn/v2/querycardlifecycle?appid=xxx&transid=xxx&ebid=xxx&token=xxx&imsi=xxx –以imsi进行查询

            string EBID = "0001000000432";

            if (imsi.Length==15)
            {
                ss = URL + "querycardlifecycle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&imsi=" + imsi;

            }
            else if (imsi.Length == 20)
            {
                ss = URL + "querycardlifecycle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&iccid=" + imsi;
            }
            else if (imsi.Length == 13)
            {
                ss = URL + "querycardlifecycle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + imsi;
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


                return JsonConvert.DeserializeObject<Root_CMIOT_API2105>(reader.ReadToEnd());
            }

        }
        /// <summary>
        ///  CMIOT_API2107-单个用户已开通服务查询
       /// 集团客户可以通过卡号（仅MSISDN）查询物联卡当前的服务开通状态
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        public static Root_CMIOT_API2107 GetCMIOT_API2107(string imsi) {

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


                if (imsi.Length==15)
                {
                    sql2 = "select Card_ID from card where Card_IMSI=@Card_IMSI ";
                }
                else if (imsi.Length==20)
                {
                    sql2 = "select Card_ID from card where Card_ICCID=@Card_IMSI ";
                }
                else if (imsi.Length==13)
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
        /// <summary>
       /// CMIOT_API2110-物联卡开户日期查询
        ///集团客户可以通过API来实现对单个询物联卡基础信息的查询，包括ICCID、MSISDN、IMSI、入网日期（开户日期）。
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        public static Root_CMIOT_API2110 GetCMIOT_API2110(string imsi) {
            List<Acount> acc = new List<Acount>();
            foreach (Acount item in Action.Action_GetAccountsMessage.acount(imsi))
            {

                APPID = item.APPID;
                TRANSID = item.TRANSID;
                TOKEN = item.TOKEN;


            }


            // https://api.iot.10086.cn/v2/querycardopentime?appid=xxx&transid=xxx&ebid=xxx&token=xxx&imsi=xxx –以imsi进行查询

            string EBID = "0001000000901";


            if (imsi.Length==15)
            {
                ss = URL + "querycardopentime?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&imsi=" + imsi;
            }
            else if (imsi.Length==20)
            {
                ss = URL + "querycardopentime?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&iccid=" + imsi;
            }
            else if (imsi.Length == 13)
            {
                ss = URL + "querycardopentime?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + imsi;
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


                return JsonConvert.DeserializeObject<Root_CMIOT_API2110>(reader.ReadToEnd());
            }


        }
        /// <summary>
       /// CMIOT_API2005-用户当月GPRS查询
       /// 集团客户可查询所属物联卡当月截止到前一天24点为止的GPRS使用量（单位：KB）。
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        public static Root_CMIOT_API2005 Get_CMIOT_API2005(string imsi) {

            List<Acount> acc = new List<Acount>();
            foreach (Acount item in Action.Action_GetAccountsMessage.acount(imsi))
            {

                APPID = item.APPID;
                TRANSID = item.TRANSID;
                TOKEN = item.TOKEN;


            }


            // https://api.iot.10086.cn/v2/gprsusedinfosingle?appid=xxx&transid=xxx&ebid=xxx&token=xxx&imsi=xxx -以imsi进行查询 

            string EBID = "0001000000012";

            if (imsi.Length==15)
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
        /// 在线数量查询 离线数量 总数查询
        /// </summary>
        /// <param name="Card_CompanyID"></param>
        /// <returns></returns>
        public static  QuerryOnlineAndOff GetOnlineandOFFNumber(string Card_CompanyID) {

            int on = 0;
            int off = 0;
            int tol = 0;


            QuerryOnlineAndOff Q = new QuerryOnlineAndOff ();
            using (IDbConnection conn = DapperService.MySqlConnection())

            {
                string sql2 = "select Card_IMSI,Card_IMEI,status  from card where status=1  and Card_CompanyID=@Card_CompanyID";



                List<Card> list = conn.Query<Card>(sql2, new { Card_CompanyID = Card_CompanyID }).ToList();


                try
                {
                    foreach (Card item in list)
                    {


                       
                            //Root_CMIOT_API12001 r = new Root_CMIOT_API12001();
                            //r = GetCMIOT_API12001(item.Card_IMSI);
                            //List<CMIOT_API12001> result = new List<CMIOT_API12001>();
                            //result = r.result;
                            foreach (CMIOT_API12001 items in GetCMIOT_API12001(item.Card_IMSI).result)
                            {



                                if (items.GPRSSTATUS == "00")
                                {
                                    off++;
                                }
                                else if (items.GPRSSTATUS == "01")
                                {
                                    on++;
                                }

                            }


                     

                        tol++;

                    }
                    Q.stauts = "0";
                    Q.Message = "接口调用成功";
                }
                catch (Exception)
                {
                    Q.stauts = "1";
                    Q.Message = "接口调用失败";
                }

              




            }
            Q.OffLine = off;
            Q.Online = on;
            Q.Tol = tol;
            return Q;

        }
        /// <summary>
        ///  CMIOT_API2037-物联卡资费套餐查询集团 客户可以根据物联卡码号信息查询该卡的套餐信息。
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        public static Root_CMIOT_API2037 Get_CMIOT_API2037(string imsi) {
            List<Acount> acc = new List<Acount>();
            foreach (Acount item in Action.Action_GetAccountsMessage.acount(imsi))
            {

                APPID = item.APPID;
                TRANSID = item.TRANSID;
                TOKEN = item.TOKEN;


            }


            // https://api.iot.10086.cn/v2/querycardprodinfo?appid=xxx&transid=xxx&ebid=xxx&token=xxx&imsi=xxx –以imsi进行查询 

            string EBID = "0001000000264";
            if (imsi.Length==15)
            {
                ss = URL + "querycardprodinfo?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&imsi=" + imsi;
            }
            else if (imsi.Length==20)
            {
                ss = URL + "querycardprodinfo?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&iccid=" + imsi;
            }
            else if (imsi.Length==13)
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
        ///   CMIOT_API2011-用户余额信息实时查询         集团客户可以查询所属物联卡的实时余额情况，每次查询一张指定物联卡的实时余额。
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        public static Root_CMIOT_API2011 Get_CMIOT_API2011(string imsi) {
            //https://api.iot.10086.cn/v2/balancerealsingle?appid=xxx&ebid=xxx&transid=xxx&token=xxx&imsi=xxx –以imsi进行查询
            List<Acount> acc = new List<Acount>();
            foreach (Acount item in Action.Action_GetAccountsMessage.acount(imsi))
            {

                APPID = item.APPID;
                TRANSID = item.TRANSID;
                TOKEN = item.TOKEN;


            }



            string EBID = "0001000000035";


            if (imsi.Length==15)
            {
                ss = URL + "balancerealsingle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&imsi=" + imsi;
            }

            else if (imsi.Length == 20)
            {
                ss = URL + "balancerealsingle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&iccid=" + imsi;
            }
            else if (imsi.Length == 13)
            {
                ss = URL + "balancerealsingle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + imsi;
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


                return JsonConvert.DeserializeObject<Root_CMIOT_API2011>(reader.ReadToEnd());
            }


        }
        /// <summary>
        /// 关于库存的ICCID 查询AcountID 用于库存管理
        /// </summary>
        /// <param name="ICCIDEnd"></param>
        /// <returns></returns>
        public static List<Acount> GetICCID_ACount(string ICCIDEnd)
        {

            Acount a = new Acount();
            using (IDbConnection conn = DapperService.MySqlConnection())

            {





                string sql2 = @"SELECT t1.APPID,t1.`PASSWORD`,t1.ECID ,t1.TOKEN,t1.TRANSID,t1.accountID,t1.URL from  accounts t1

LEFT JOIN product  t2 on t1.accountID = t2.accountID
WHERE t2.ICCIDEnd=@ICCIDEnd";







                List<Acount> li = new List<Acount>();

                return conn.Query<Acount>(sql2, new { ICCIDEnd = ICCIDEnd }).ToList();




            }




        }
        /// <summary> 
        /// CMIOT_API2105-物联卡生命周期查询
        /// 集团客户根据卡号（imsi、msisdn、iccid三个中任意一个），查询物联卡当前生命周期，生命周期包括：00:正式期，01:测试期，02:沉默期，03:其他。
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        public static Root_CMIOT_API2105 JudgeGetCMIOT_API2105(string ICCID)
        {



            Root_CMIOT_API2105 root_CMIOT_API2105 = new Root_CMIOT_API2105();
            List<Acount> acc = new List<Acount>();
            foreach (Acount item in GetICCID_ACount(ICCID))
            {

                APPID = item.APPID;
                TRANSID = item.TRANSID;
                TOKEN = item.TOKEN;
                URL = item.URL;

            }
            //https://api.iot.10086.cn/v2/querycardlifecycle?appid=xxx&transid=xxx&ebid=xxx&token=xxx&iccid=xxx –以iccid进行查询
            // https://api.iot.10086.cn/v2/querycardlifecycle?appid=xxx&transid=xxx&ebid=xxx&token=xxx&imsi=xxx –以imsi进行查询

            string EBID = "0001000000432";

            if (ICCID.Length==20)
            {
                ss = URL + "querycardlifecycle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&iccid=" + ICCID;
            }
            else if (ICCID.Length==15)
            {
                ss = URL + "querycardlifecycle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&imsi=" + ICCID;
            }
            else if (ICCID.Length == 13)
            {
                ss = URL + "querycardlifecycle?appid=" + APPID + "&transid=" + TRANSID + "&ebid=" + EBID + "&token=" + TOKEN + "&msisdn=" + ICCID;
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


                return JsonConvert.DeserializeObject<Root_CMIOT_API2105>(reader.ReadToEnd());
            }









        }





          /// <summary>
          /// 获取APN
          /// </summary>
          /// <param name="ICCID"></param>
          /// <returns></returns>
        public static APN.Root GetAPN(string ICCID) {



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


                return JsonConvert.DeserializeObject<APN.Root>(reader.ReadToEnd());
            }






        }
    }
}