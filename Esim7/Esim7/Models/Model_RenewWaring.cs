using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    public class Model_RenewWaring
    {


       

        /// <summary>
        /// 客户登陆时需要显示  续费预警     Type=2
        /// </summary>
        public class Message_ForCustom
        {
           
            public string CompanyName { get; set; }
            public string CompanyID { get; set; }
            public string Number { get; set; }
            public string Phone { get; set; }
            public string Total { get; set; }
            // public string CompanyPhone { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<Card_ForWaring_Custom3> List_card_Custom { get; set; }

        }


        public class Message_ForHoutai_return {

            public string CompanyName { get; set; }
            public string CompanyID { get; set; }
            public string Number { get; set; }
            public string Phone { get; set; }
            public string Total { get; set; }


            public List<Message_ForHoutai> List_card_Custom { get; set; }

        }



        public class Message_ForCustom2
        {
                  
            public string CompanyName { get; set; }
            public string CompanyID { get; set; }
            public string Number { get; set; }
            public string Phone { get; set; }
            public string Total { get; set; }
            public List<Card_ForWaring_Custom> List_card_Custom { get; set; }

        }

        public class Waring_Total_Custom {

           
            public List<Message_ForCustom> Custom { get; set; }
          //  public List<Card_ForWaring_Custom> List_card_Custom { get; set; }
           


            public string Type { get; set; }

        }


        public class waringTotal_Houtai {

           public string Type { get; set; }

            public List<Message_ForHoutai_return> Message_ForHoutai { get; set; }

           
                
            public List<Message_ForCustom2> Message_Custom { get; set; }
            /// <summary>
            /// 后台登录时 获取客户数据
            /// </summary>
           


        }







        public class Waring {


            public List<Message_ForHoutai> Gonghai { get; set; }
            public List<Message_ForHoutai> Custom { get; set; }
         
    

        }
        /// <summary>
        ///  客户登陆时需要显示  点击张数时显示
        /// </summary>
        public class Card_ForWaring_Custom { 


            public string ICCID { get; set; }
            public string Card_ID { get; set; }
            public string CompanyID { get; set; }
            public string CustomName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Custom_EndTime { get; set; }



            //public string CustomName { get; set; }
            //public string Custom_EndTime { get; set; }

        }

   


        public class Card_ForWaring_Custom3 {

            public string ICCID { get; set; }
            public string Card_ID { get; set; }
            public string CompanyID { get; set; }
            public string CustomName { get; set; }
            public string Custom_EndTime { get; set; }

        }
        /// <summary>
        ///     后台登录时需要显示 点击张数 TYpe=1
        /// </summary>
        public class Message_ForHoutai
        {





            public string ICCID { get; set; }
            public string Card_ID { get; set; }
            public string Card_IMSI { get; set; }
            public string RealEndTime { get; set; }
            public string CompanyName { get; set; }
            public string CustomName { get; set; }
            public string Custom_EndTime { get; set; }
            public string CardTypeName { get; set; }
            public string operatorsName { get; set; }
            public string ProductID { get; set; }
            public string SetmealName { get; set; }
            public string ProductDate { get; set; }
            public string CardXingtai { get; set; }
          
            

             
           

           
           
           
           

                  

               
           




        }
        public class List_Card_ForWaring
        {


            public List<Card_ForWaring_Custom> List_card_Custom { get; set; }
            public List<Message_ForHoutai> List_Card_Houtai { get; set; }
            public string Type { get; set; }

        }




    }
}