using Dapper;
using Esim7.Action;
using Esim7.Action_OneLink_new;
using Esim7.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Esim7.Action_JiHe_CardMess
{
    public class Action_NewSimlink_Card_Message
    {

        public class Message
        {
            public string Card_ID { get; set; }

            public string Platform { get; set; }

        }

        /// <summary>
        ///  查询服务开通情况   短信 流量等
        /// </summary>
        /// <param name="Value">ICCID IMSI ID</param>
        /// <returns></returns>
        public static  object GetFunctionalOpenQuery(string Value)
        {
            string Platform = "";
            string Card_ID = "";
            string sql2;
            string s = "";
            if (Value.Length == 20)
            {
                s = "Card_ICCID=@Value";

            }
            else if (Value.Length == 15)
            {
                s = "Card_IMSI=@Value";
            }
            else if(Value.Length==13)
            {
                s = "Card_ID=@Value";
            }                            
            using (IDbConnection conn = DapperService.MySqlConnection())
            {

                List<Message> li = new List<Message>();
                sql2 = @"select Card_ID ,Platform from card where   " + s;
                li= conn.Query<Message>(sql2,new { Value=Value }).ToList();

              

                if (li.Count==1)
                {
                    foreach (Message item in li)
                    {

                        Platform = item.Platform;
                        Card_ID = item.Card_ID; 
                    }




                }
                else
                {
                    return "error  List.count!=1";
                }


               














            }
            //移动旧平台
            if (Platform == "10")
            {
                return APIACTION.GetCMIOT_API2107(Card_ID);
            }
            else
            {
                return Action_Onelink_New_CardMeaasge.GetFunctionalOpenQuery(Card_ID);
            }



        }

    }
















}
