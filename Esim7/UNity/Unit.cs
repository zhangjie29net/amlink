using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Esim7.UNity
{
    public class Unit
    {


        /// <summary>
        /// MD5+反转字符串  加密方法
        /// </summary>
        /// <param name="password">密码</param>
        /// <returns></returns>
       public static string MD5_64(string password)
       {

            string cl = password;
            //string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
                                    // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(cl));
         string str=   Convert.ToBase64String(s);
                if (String.IsNullOrEmpty(str))
                    throw new ArgumentException("字符串为空！");
                StringBuilder sb = new StringBuilder(str.Length);
                for (int i = str.Length - 1; i >= 0; i--)
                {
                    sb.Append(str[i]);
                }
                return sb.ToString();
        }

        public static string GetMD5_32(string myString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            //byte[] fromData = System.Text.Encoding.Unicode.GetBytes(myString);
            byte[] fromData = System.Text.Encoding.UTF8.GetBytes(myString);//
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x");
            }
            return byte2String;
        }



        // <summary>
        /// 获取时间戳
        ///</summary>
        /// <returns></returns>
        public static string GetTimeStamp(System.DateTime time)
        {
            long ts = ConvertDateTimeToInt(time);
            return ts.ToString();
        }
        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }
        /// <summary>        
        /// 时间戳转为C#格式时间        
        /// </summary>        
        /// <param name="timeStamp"></param>        
        /// <returns></returns>        
        private DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

       /// <summary>
       /// Get 方法
       /// </summary>
       /// <param name="url"></param>
       /// <returns></returns>
        public dynamic  HttpGet(string url)
        {
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {

                var s = reader.ReadToEnd();
                return reader.ReadToEnd();
            }
        }




        public static string Receive(Socket socket)
        {
            byte[] bytes = new byte[1024];
            //从客户端接收消息
            int len = socket.Receive(bytes, bytes.Length, 0);
            //将消息转为字符串
            string recvStr = Encoding.ASCII.GetString(bytes, 0, len);
            Console.WriteLine("接收的客户端消息 ： {0}", recvStr);
            return recvStr;
        }

        public static void Send(Socket socket, string sendStr)
        {

            Console.WriteLine("发送给客户端消息 ： {0}", sendStr);
            // 将字符串消息转为数组
            byte[] bytes = Encoding.ASCII.GetBytes(sendStr);
            //发送消息给客户端
            socket.Send(bytes, bytes.Length, 0);
        }

        ///<summary>
        ///正则去掉字符串中的特殊字符只留字母数字和中文
        /// </summary>
        public static string RegexStr(string str)
        {
            string getstr = string.Empty;
            getstr = Regex.Replace(getstr, @"[^a-zA-Z0-9\u4e00-\u9fa5]", "");
            return getstr;
        }
    }
}