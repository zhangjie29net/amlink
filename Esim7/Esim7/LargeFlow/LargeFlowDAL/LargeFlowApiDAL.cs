using Esim7.LargeFlow.LargeFlowModel;
using Esim7.UNity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Esim7.LargeFlow.LargeFlowDAL
{
    /// <summary>
    /// 大流量卡API对接
    /// </summary>
    public class LargeFlowApiDAL
    {
        string url = @"https://api.simsky.cn/api/v3/";

        public  string sha256(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hash = SHA256Managed.Create().ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("X2"));
            }
            return builder.ToString();
        }

        /// <summary>
        /// 获取单卡详细信息
        /// </summary>
        /// <returns></returns>
        public LargeFlowDetailDto LargeFLowCardDetail(string iccid)
        {
            LargeFlowDetailDto t = new LargeFlowDetailDto();
            string URL = url + "sim_cards/get_sim_card_detail";
            DateTime dt = DateTime.Now;
            DateTime dateStart = new DateTime(1970, 1, 1, 8, 0, 0);
            int timeStamp = Convert.ToInt32((dt - dateStart).TotalSeconds);
            string timestamp = timeStamp.ToString();
            string app_secret = "155adda9da9dc82176b369eabe86d354";
            string app_id = "38635745241776";
            string sign = string.Empty;
            string raw_sign = "app_id=" + app_id + "&iccid=" + iccid + "&timestamp=" + timestamp + app_secret;//拼接参数
            string a = string.Empty;
            byte[] b = System.Text.Encoding.Default.GetBytes(raw_sign);
            //转成 Base64 形式的 System.String  
            a = Convert.ToBase64String(b);
            var Asc = new ASCIIEncoding();
            var tmpByte = Asc.GetBytes(a);
            var EncryptBytes = sha256(a);
            string data = EncryptBytes.ToString().ToLower();
            string param = "app_id=38635745241776&iccid="+iccid+"&sign="+data+ "&timestamp="+timestamp;
            byte[] bytes = Encoding.UTF8.GetBytes(param);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            request.ContentLength = bytes.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            string ssssss = string.Empty ;
            try
            {
                if (responseStream != null)
                {
                    using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        t = JsonConvert.DeserializeObject<LargeFlowDetailDto>(reader.ReadToEnd());
                        //ssssss = reader.ReadToEnd();
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                t.message = t.message + ":"+ex;
            }
        
            return t;
        }

        /// <summary>
        /// 批量卡状态查询 多个卡用,隔开最多支持100张卡
        /// </summary>
        /// <returns></returns>
        public LargerFlowCardstatusDto query_status(string iccid)
        {
            LargerFlowCardstatusDto t = new LargerFlowCardstatusDto();
            string URL = url + "sim_cards/query_status";
            DateTime dt = DateTime.Now;
            DateTime dateStart = new DateTime(1970, 1, 1, 8, 0, 0);
            int timeStamp = Convert.ToInt32((dt - dateStart).TotalSeconds);
            string timestamp = timeStamp.ToString();
            string app_secret = "155adda9da9dc82176b369eabe86d354";
            string app_id = "38635745241776";
            string sign = string.Empty;
            string raw_sign = "app_id=" + app_id + "&iccids=" + iccid + "&timestamp=" + timestamp + app_secret;//拼接参数
            string a = string.Empty;
            byte[] b = System.Text.Encoding.Default.GetBytes(raw_sign);
            //转成 Base64 形式的 System.String  
            a = Convert.ToBase64String(b);
            var Asc = new ASCIIEncoding();
            var tmpByte = Asc.GetBytes(a);
            var EncryptBytes = sha256(a);
            string data = EncryptBytes.ToString().ToLower();
            string param = "app_id=38635745241776&iccids=" + iccid + "&sign=" + data + "&timestamp=" + timestamp;
            byte[] bytes = Encoding.UTF8.GetBytes(param);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            request.ContentLength = bytes.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            string ssssss = string.Empty;
            if (responseStream != null)
            {
                using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
                {
                     t = JsonConvert.DeserializeObject<LargerFlowCardstatusDto>(reader.ReadToEnd());
                    reader.Close();
                }
            }
            return t;
        }

        ///<summary>
        ///批量当前周期流量查询
        /// </summary>
        public string current_cycle_traffic_use(string iccid)
        {
            string result = string.Empty;
            LargerFlowCardstatusDto t = new LargerFlowCardstatusDto();
            string URL = url + "sim_cards/current_cycle_traffic_use";
            DateTime dt = DateTime.Now;
            DateTime dateStart = new DateTime(1970, 1, 1, 8, 0, 0);
            int timeStamp = Convert.ToInt32((dt - dateStart).TotalSeconds);
            string timestamp = timeStamp.ToString();
            string app_secret = "155adda9da9dc82176b369eabe86d354";
            string app_id = "38635745241776";
            string sign = string.Empty;
            string raw_sign = "app_id=" + app_id + "&iccids=" + iccid + "&timestamp=" + timestamp + app_secret;//拼接参数
            string a = string.Empty;
            byte[] b = System.Text.Encoding.Default.GetBytes(raw_sign);
            //转成 Base64 形式的 System.String  
            a = Convert.ToBase64String(b);
            var Asc = new ASCIIEncoding();
            var tmpByte = Asc.GetBytes(a);
            var EncryptBytes = sha256(a);
            string data = EncryptBytes.ToString().ToLower();
            string param = "app_id=38635745241776&iccids=" + iccid + "&sign=" + data + "&timestamp=" + timestamp;
            byte[] bytes = Encoding.UTF8.GetBytes(param);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            request.ContentLength = bytes.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            string ssssss = string.Empty;
            if (responseStream != null)
            {
                using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                    //t = JsonConvert.DeserializeObject<LargerFlowCardstatusDto>(reader.ReadToEnd());
                    reader.Close();
                }
            }
            return result;
        }

        public string ss()
        {
            string result = "";
            string sss = "YONGBAO_C";
            result = Unit.MD5_64(sss);
            byte[] b = System.Text.Encoding.Default.GetBytes(result);
            //转成 Base64 形式的 System.String  
            result = Convert.ToBase64String(b);
            return result;
        }
    }
}