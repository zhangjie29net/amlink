using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Esim7.Action
{
    public class TokenAction
    {

        //sha256加密方法
        //str=APPID＋PASSWORD＋TRANSID,如：APPID=A0B40MY，PASSWORD=1234，TRANSID=A0B40MY2016101711070753214709；则参数str=A0B40MY1234A0B40MY2016101711070753214709


        
        public static string SHA256Encrypt()
        {

            string APPID = ConfigurationManager.AppSettings["APPID"];
            string PASSWORD = ConfigurationManager.AppSettings["PASSWORD"];


            string TRANSID = APPID + DateTime.Now.ToString("yyyyMMddHHmmss") + "00000001";
            string str = TRANSID;
                    //string strIN = getstrIN(strIN);
                    byte[] tmpByte;
            SHA256 sha256 = new SHA256Managed();
            tmpByte = sha256.ComputeHash(GetKeyByteArray(str));
            sha256.Clear();
            return GetHexString(tmpByte);
        }

        private static string GetHexString(byte[] Byte)
        {
            StringBuilder stringBuilder = new StringBuilder();
            //逐字节变为16进制字符
            for (int i = 0; i < Byte.Length; i++)
            {
                stringBuilder.Append(Convert.ToString(Byte[i], 16));
            }
            return stringBuilder.ToString();
        }

        private static byte[] GetKeyByteArray(string strKey)
        {
            ASCIIEncoding Asc = new ASCIIEncoding();
            int tmpStrLen = strKey.Length;
            byte[] tmpByte = new byte[tmpStrLen - 1];
            tmpByte = Asc.GetBytes(strKey);
            return tmpByte;

        }







    }
}