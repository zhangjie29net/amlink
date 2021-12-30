using Dapper;
using Esim7.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Esim7.Action
{
    public class CardTypeAction
    {


        /// <summary>
        /// 添加类型
        /// </summary>
        /// <param name="com">[{"CompanyID":"123","CardType_Name":"123"}]</param>
        /// <returns></returns>
        public static string Add(string Json)
        {
            string s = "";

            List<CardType> com = JsonConvert.DeserializeObject<List<CardType>>(Json);
            if (com.Count != 0)
            {
                foreach (CardType item in com)
                {
                    using (IDbConnection conn = DapperService.MySqlConnection())

                    {



                        if (Judged(item.CompanyID, item.CardType_Name))
                        {
                            s = "此类型已经存在请检查";
                        }
                        else
                        {
                            try
                            {
                                string sql = "insert card_type(CompanyID,CardType_Name,status)values(@CompanyID,@CardType_Name,1)";
                                conn.Execute(sql, com);

                                s = "添加成功";

                            }
                            catch (Exception ex)
                            {

                                s = ex.ToString();
                            }
                        }
                    }






























                }






            }
            else
            {
                s = "添加失败或数据错误";
            }
            return s;
        }
        /// <summary>
        /// 删除类型
        /// </summary>
        /// <param name="Json">[{"CompanyID":"123","CardType_Name":"123"}]</param>
        /// <returns></returns>
        public static string Dele(string Json)
        {


            List<CardType> com = JsonConvert.DeserializeObject<List<CardType>>(Json);
            if (com.Count != 0)
            {
                foreach (CardType item in com)
                {


                    using (IDbConnection
         conn = DapperService.MySqlConnection())

                    {

                        try
                        {
                            string sql = "update card_type set status=0 where CompanyID = " + item.CompanyID + "and CardType_Name=" + item.CardType_Name;
                            conn.Execute(sql);



                        }
                        catch (Exception ex)
                        {

                            return ex.ToString();
                        }

                    }



                }
                return "删除成功";

            }
            else
            {
                return "删除失败";
            }


        }

        public static bool Judged(string CompanyID, string cardtypeName)
        {


            bool f = false;
            using (IDbConnection
        conn = DapperService.MySqlConnection())

            {
                CardType CardTypes = new CardType();
                List<CardType> li = new List<CardType>();
                try

                {
                    //有 true
                    string query = "SELECT CardType_ID,CompanyID,CardType_Name FROM card_type WHERE CompanyID = '" + CompanyID + "' and status=1 and CardType_Name= '" + cardtypeName+"'";
                    CardTypes = conn.Query<CardType>(query).SingleOrDefault();
                    li.Add(CardTypes);
                    Console.WriteLine(CardTypes.CardType_Name);
                    f = true;

                }
                catch (Exception ex)
                {
                    //没有 false
                    f = true;
                }

            }





            return f;







        }


    }
}