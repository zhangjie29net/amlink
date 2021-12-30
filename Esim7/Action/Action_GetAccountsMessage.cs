using Dapper;
using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Esim7.Action
{
    public class Action_GetAccountsMessage
    {
        /// <summary>
        /// 获取 Account 版本旧的中国移动 onelink
        /// </summary>
        /// <param name="imsi">imsi号码</param>
        /// <returns></returns>
        public static  List<Acount> acount(string imsi)
        {
            Acount a = new Acount();
            using (IDbConnection conn = DapperService.MySqlConnection())
            {
                string sql2 = "";
                if (imsi.Length==15)
                {
                    sql2 = @"SELECT t1.APPID,t1.`PASSWORD`,t1.ECID ,t1.TOKEN,t1.TRANSID,t1.accountID from  accounts t1 LEFT JOIN card t2 on t1.accountID = t2.accountsID
                            WHERE t2.Card_IMSI =@IMSI or t2.Card_IMEI=@IMSI";
                }
                else  if(imsi.Length==20)
                {
                    sql2 = @"SELECT t1.APPID,t1.`PASSWORD`,t1.ECID ,t1.TOKEN,t1.TRANSID,t1.accountID from  accounts t1 LEFT JOIN card t2 on t1.accountID = t2.accountsID
                            WHERE t2.Card_ICCID =@IMSI";
                }
                else if (imsi.Length == 13)
                {
                    sql2 = @"SELECT t1.APPID,t1.`PASSWORD`,t1.ECID ,t1.TOKEN,t1.TRANSID,t1.accountID from  accounts t1 LEFT JOIN card t2 on t1.accountID = t2.accountsID
                            WHERE t2.Card_ID =@IMSI";
                }
                Card card = new Card();
                List<Acount> li = new List<Acount>();
                return  conn.Query<Acount>(sql2, new { IMSI = imsi }).ToList();
            }
        }
    }
}