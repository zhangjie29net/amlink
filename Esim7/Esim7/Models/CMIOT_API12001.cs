using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{//在线信息实时查询

    public class CMIOT_API12001
    {
        /// <summary>
        /// 
        /// </summary>
        public string APN { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GPRSSTATUS { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RAT { get; set; }
    }
    /// <summary>
    /// CMIOT_API2001-在线信息实时查询 
    /// 集团客户根据所属物联卡的码号信息查询该卡的GPRS在线状态、IP地址、APN、RAT信息。
    /// </summary>
    public class Root_CMIOT_API12001
    {
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 正确
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<CMIOT_API12001> result { get; set; }
    }








    public class CMIOT_API2101
    {
        /// <summary>
        /// 
        /// </summary>
        public string gprstotalnum { get; set; }
    }
    /// <summary>
    /// CMIOT_API2101-集团GPRS在线物联卡数量查询 
    /// 集团客户通过API服务平台实时查询GPRS在线物联卡的数量。
    /// </summary>
    public class Root_CMIOT_API2101
    {
        /// <summary>
        /// 
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<CMIOT_API2101> result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
    }






    public class CMIOT_API2008
    {
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
    }
    /// <summary>
    /// CMIOT_API2008-开关机信息实时查询   根据物联卡码号信息实时查询终端开关机状态
    /// </summary>
    public class Root_CMIOT_API2008
    {
        /// <summary>
        /// 
        /// </summary>

        public string imsi { get; set; }
        /// <summary>
        /// 正确
        /// </summary>
        public string message { get; set; }

        public string status { get; set; }


        public string Card_ID { get; set; }
        public string Card_IMSI { get; set; }
        public string Card_IMEI { get; set; }
        public string Card_ICCID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<CMIOT_API2008> result { get; set; }
    }


    public class CMIOT_API2102
    {
        /// <summary>
        /// 
        /// </summary>
        public string issignsms { get; set; }
    }
    /// <summary>
    /// CMIOT_API2102-物联卡短信服务开通查询   集团客户可以通过卡号（MSISDN/ICCID/IMSI，单卡）信息查询此卡的短信服务开通状态。
    /// </summary>
    public class Root_CMIOT_API2102
    {
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 正确
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<CMIOT_API2102> result { get; set; }
    }










    public class GprsItem_CMIOT_API2020
    {
        /// <summary>
        /// 
        /// </summary>
        public string left { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string prodid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string prodinstid { get; set; }
        /// <summary>
        /// GPRS中小流量新10元套餐
        /// </summary>
        public string prodname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string  used { get; set; }
    }

    public class ResultItem_CMIOT_API2020
    {
        /// <summary>
        /// 
        /// </summary>
        public List<GprsItem_CMIOT_API2020> gprs { get; set; }
    }
    /// <summary>
    ///  CMIOT_API2020-套餐内GPRS流量使用情况实时查询 (集团客户)  集团客户可查询所属物联卡当月套餐内GPRS流量使用情况
    /// </summary>
    public class Root_CMIOT_API2020
    {
        /// <summary>
        /// 正确
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ResultItem_CMIOT_API2020> result { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
    }









    public class ResultItem_CMIOT_API2300
    {
        /// <summary>
        /// 
        /// </summary>
        public string gprs { get; set; }
    }

    /// <summary>
    ///  CMIOT_API2300-物联卡单日GPRS使用量查询   集团客户可以主动查询某张物联卡、某一天的GPRS使用量，单位KB（仅能查询昨天或昨天之前的最近7天的某一天的使用量）  单卡查询
    /// </summary>
    public class Root_CMIOT_API2300
    {
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 正确
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>


        public List<ResultItem_CMIOT_API2300> result { get; set; }
        public string IMSI { get; set; }
    }

    /// <summary>
    ///  CMIOT_API2300-物联卡单日GPRS使用量查询   集团客户可以主动查询某张物联卡、某一天的GPRS使用量，单位KB（仅能查询昨天或昨天之前的最近7天的某一天的使用量） 集合查询
    /// </summary>
    public class  Root_List_CMIOT_API2300{


      public Root_CMIOT_API2300 Root_CMIOT_API2300 { get; set; }
     //  public Card Card { get; set; }

      
        public string IMSI { get; set; }




    }








    public class ResultItem_CMIOT_API2002
    {
        /// <summary>
        /// 
        /// </summary>
        public string STATUS { get; set; }
    }
    /// <summary>
    ///  CMIOT_API2002-用户状态信息实时查询  集团客户可根据所属物联卡的码号信息实时查询该卡的状态信息。
    /// </summary>
    public class Root_CMIOT_API2002
    {
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 正确
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ResultItem_CMIOT_API2002> result { get; set; }
    }



    public class ResultItem_CMIOT_API2103
    {
        /// <summary>
        /// 
        /// </summary>
        public string issigngprs { get; set; }
    }
    /// <summary>
    ///  CMIOT_API2103-物联卡GPRS服务开通查询 集团客户可以通过卡号（MSISDN/ICCID/IMSI，单卡）信息查询此卡的GPRS服务开通状态
    /// </summary>
    public class Root_CMIOT_API2103
    {
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 正确
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ResultItem_CMIOT_API2103> result { get; set; }
    }







    public class ResultItem_CMIOT_API2105
    {
        /// <summary>
        /// 
        /// </summary>
        public string lifecycle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string opentime { get; set; }
    }
    /// <summary>
   /// CMIOT_API2105-物联卡生命周期查询
   /// 集团客户根据卡号（imsi、msisdn、iccid三个中任意一个），查询物联卡当前生命周期，生命周期包括：00:正式期，01:测试期，02:沉默期，03:其他。
    /// </summary>
    public class Root_CMIOT_API2105
    {
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 正确
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ResultItem_CMIOT_API2105> result { get; set; }
    }





    public class ResultItem_CMIOT_API2107
    {
        /// <summary>
        /// 
        /// </summary>
        public string msisdn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string issignCall { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string issignGprs { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string issignSms { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string issignApn { get; set; }
    }
    /// <summary>
   /// CMIOT_API2107-单个用户已开通服务查询
   /// 集团客户可以通过卡号（仅MSISDN）查询物联卡当前的服务开通状态。
    /// </summary>
    public class Root_CMIOT_API2107
    {
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 正确
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ResultItem_CMIOT_API2107> result { get; set; }
    }




    public class ResultItem_CMIOT_API2110
    {
        /// <summary>
        /// 
        /// </summary>
        public string iccid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string msisdn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string imsi { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string openTime { get; set; }
    }
    /// <summary>
    ///  CMIOT_API2110-物联卡开户日期查询
    ///集团客户可以通过API来实现对单个询物联卡基础信息的查询，包括ICCID、MSISDN、IMSI、入网日期（开户日期）
    /// </summary>
    public class Root_CMIOT_API2110
    {
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 正确
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ResultItem_CMIOT_API2110> result { get; set; }
    }





    public class ResultItem_CMIOT_API2005
    {
        /// <summary>
        /// 
        /// </summary>
        public string total_gprs { get; set; }
    }
    /// <summary>
    /// 
   /// CMIOT_API2005-用户当月GPRS查询
   /// 集团客户可查询所属物联卡当月截止到前一天24点为止的GPRS使用量（单位：KB）。
    /// </summary>
    public class Root_CMIOT_API2005
    {
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 正确
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ResultItem_CMIOT_API2005> result { get; set; }
    }







    public class ProdinfosItem_CMIOT_API2037
    {
        /// <summary>
        /// 
        /// </summary>
        public string prodid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string prodinstefftime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string prodinstexptime { get; set; }
        /// <summary>
        /// GPRS中小流量新3元套餐
        /// </summary>
        public string prodname { get; set; }
    }

    public class ResultItem_CMIOT_API2037
    {
        /// <summary>
        /// 
        /// </summary>
        public string iccid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string imsi { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string msisdn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ProdinfosItem_CMIOT_API2037> prodinfos { get; set; }
    }
    /// <summary>
    ///  CMIOT_API2037-物联卡资费套餐查询 集团客户可以根据物联卡码号信息查询该卡的套餐信息
    /// </summary>
    public class Root_CMIOT_API2037
    {
        /// <summary>
        /// 正确
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ResultItem_CMIOT_API2037> result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
    }







    public class ResultItem_CMIOT_API2011
    {
        /// <summary>
        /// 
        /// </summary>
        public string balance { get; set; }
    }

    public class Root_CMIOT_API2011
    {
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 正确
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ResultItem_CMIOT_API2011> result { get; set; }
    }
           /// <summary>
           ///      API2104
           /// </summary>
    public class APN {
        public class ResultItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string status { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string apnname { get; set; }
        }

        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public string status { get; set; }
            /// <summary>
            /// 正确
            /// </summary>
            public string message { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<ResultItem> result { get; set; }
        }
    }



   
}