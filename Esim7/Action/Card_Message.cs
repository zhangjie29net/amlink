using Dapper;
using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Esim7.Action
{
    public class Card_Message
    {

        /// <summary>
        /// 按照公司ID查询
        /// </summary>
        /// <param name="Card_CompanyID"></param>
        /// <returns></returns>
        public static List<Card> GetCardsForCompanyID(string Card_CompanyID)
        {

            using (IDbConnection
     conn = DapperService.MySqlConnection())

            {





                string sql2 = "select Card_ID,Card_IMSI,Card_Type,Card_IMEI,Card_State,Card_WorkState,  date_format(Card_OpenDate, '%Y-%m-%d %H:%i:%s') as  Card_OpenDate  ,         date_format( Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,Card_Remarks, status, Card_CompanyID,  Card_ICCID ,Card_totalflow,Card_userdflow, Card_Residualflow from card  where Card_CompanyID=@Card_CompanyID ";

                Card card = new Card();





                List<Card> li = new List<Card>();

                return conn.Query<Card>(sql2,new { Card_CompanyID = Card_CompanyID }).ToList();




            }

        }

        /// <summary>
        /// 以IMsi查询单张卡信息
        /// </summary>
        /// <param name="IMSI"></param>
        /// <returns></returns>
        public static List<Card> GetCardsForIMSI(string IMSI) {


            using (IDbConnection conn = DapperService.MySqlConnection())

            {





                string sql2 = "select Card_ID,Card_IMSI,Card_Type,Card_IMEI,Card_State,Card_WorkState,  date_format(Card_OpenDate, '%Y-%m-%d %H:%i:%s') as  Card_OpenDate  ,         date_format( Card_ActivationDate, '%Y-%m-%d %H:%i:%s') as Card_ActivationDate ,Card_Remarks, status, Card_CompanyID,  Card_ICCID ,Card_totalflow,Card_userdflow, Card_Residualflow from card  where Card_IMSI=@IMSI ";

                Card card = new Card();





                List<Card> li = new List<Card>();

                return conn.Query<Card>(sql2, new { IMSI = IMSI }).ToList();




            }



        }



       


    }
}