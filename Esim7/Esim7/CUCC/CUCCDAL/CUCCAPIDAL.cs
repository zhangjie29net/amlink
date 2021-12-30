using Esim7.CUCC.CUCCModel;
using Esim7.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Web;

namespace Esim7.CUCC.CUCCDAL
{
    public class CUCCAPIDAL
    {
        ///<summary>
        ///获取数据 返回指定设备的会话信息。dateSessionEnded为空时在线 否则离线
        /// </summary>
        public CuccCardWorkStatus GetCuccCardWorkStatus(string ICCID)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            CuccCardWorkStatus t = new CuccCardWorkStatus();
            //string url = "https://api.10646.cn/rws/api/v1/devices/89860620160013379998/sessionInfo";
            string url = "https://api.10646.cn/rws/api/v1/devices/"+ICCID+ "/sessionInfo";
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "get";
            string code = "wangyaping977:47e164a1-2806-43d7-9aca-609e81535bb2";
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
                }
            }
            catch(Exception ex)
            {
                t = null;
            }
            //string code = "wangyaping77:4e153261-e135-4c4a-bac5-3ba1666ae789";
            //System.Text.Encoding encode = System.Text.Encoding.ASCII;
            //byte[] bytedata = encode.GetBytes(code);
            //string strPath = Convert.ToBase64String(bytedata, 0, bytedata.Length);
            //string ssss = strPath;

            //t.dateSessionEnded 不为空时离线  为空时在线
            return t;
        }


        ///<summary>
        ///查看卡的状态(测试期、正常使用、停机等状态)
        /// </summary>
        public CUCCCardStatus CuccCardStatus(string ICCID)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            CUCCCardStatus t = new CUCCCardStatus();
            string url = "https://api.10646.cn/rws/api/v1/devices/"+ICCID;
            //https://api.10646.cn/rws/api/v1/devices/89860620160013350395
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "get";
            string code = "wangyaping977:47e164a1-2806-43d7-9aca-609e81535bb2";
            System.Text.Encoding encode = System.Text.Encoding.ASCII;
            byte[] bytedata = encode.GetBytes(code);
            string strPath = Convert.ToBase64String(bytedata, 0, bytedata.Length);
            string ssss = strPath;
            string BaseApi = "Basic " + ssss;
            request.Headers.Add("Authorization", BaseApi);
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    t = JsonConvert.DeserializeObject<CUCCCardStatus>(reader.ReadToEnd());
                }
            }
            catch
            {
                t = null;
            }
           
            return t;
        }

        ///<summary>
        ///获取联通卡的使用流量 KB
        /// </summary>
        public CuccCardFlow GetCuccCardFlow(string ICCID)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            CuccCardFlow t = new CuccCardFlow();
            //https://api.10646.cn/rws/api/v1/devices/89860620160013379998/ctdUsages
            string url = "https://api.10646.cn/rws/api/v1/devices/" + ICCID + "/ctdUsages";
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "get";
            string code = "wangyaping977:47e164a1-2806-43d7-9aca-609e81535bb2";
            System.Text.Encoding encode = System.Text.Encoding.ASCII;
            byte[] bytedata = encode.GetBytes(code);
            string strPath = Convert.ToBase64String(bytedata, 0, bytedata.Length);
            string ssss = strPath;
            string BaseApi = "Basic " + ssss;
            request.Headers.Add("Authorization", BaseApi);
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    t = JsonConvert.DeserializeObject<CuccCardFlow>(reader.ReadToEnd());
                }
            }
            catch(Exception ex)
            {
                t = null;
            }
            return t;
        }

        ///<summary> 
        ///修改联通卡状态 Status：已激活：Activated   已停用：Deactivated
        /// </summary>
        public Information UpdateCuccStatus(string ICCID,string Status)
        {
            Information info = new Information();
            CuccCardFlow t = new CuccCardFlow();
            //http://restapi1.jasper.com/rws/api/v1/devices/9088217871987000005"
            string url = "http://restapi1.jasper.com/rws/api/v1/devices/" + ICCID + "/ctdUsages";
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "PUT";
            string code = "wangyaping977:47e164a1-2806-43d7-9aca-609e81535bb2";
            StringBuilder para = new StringBuilder();
            para.Append("{");
            para.Append("status:\"" + Status + "\"");
            para.Append("}");
            code = code + para.ToString();
            System.Text.Encoding encode = System.Text.Encoding.ASCII;
            byte[] bytedata = encode.GetBytes(code);
            string strPath = Convert.ToBase64String(bytedata, 0, bytedata.Length);
            string ssss = strPath;
            string BaseApi = "Basic " + ssss;
            request.Headers.Add("Authorization", BaseApi);
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    t = JsonConvert.DeserializeObject<CuccCardFlow>(reader.ReadToEnd());
                    info.Msg = t.iccid;
                    info.flg = "1";
                }
            }
            catch (Exception ex)
            {
                info.Msg = "出错:"+ex;
                info.flg = "-1";
            }
            return info;
        }
    }
}