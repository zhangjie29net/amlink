using System.Collections.Generic;

namespace Esim7.SimModel
{
    public class API
    {

        /// <summary>
        /// 单卡基本信息查询
        ///查询物联卡码号信息、开卡时间、首次激活时间。
        ///接口调用请求说明
        ///http请求方式： GET/POS
        ///API请求URL示例：
        ///https://api.iot.10086.cn/v5/ec/query/sim-basic-info?transid=xxxxxx&token=xxxxxx&msisdn=xxxxxx -以msisdn进行查询
        ///https://api.iot.10086.cn/v5/ec/query/sim-basic-info?transid=xxxxxx&token=xxxxxx&imsi=xxxxxx –以imsi进行查询
        ///https://api.iot.10086.cn/v5/ec/query/sim-basic-info?transid=xxxxxx&token=xxxxxx&iccid=xxxxxx –以iccid进行查询
        /// </summary>
        public class CMIOT_API23S00
        {


            public class ResultItem
            {
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
                public string iccid { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string activeDate { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string openDate { get; set; }
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





        //        单卡绑定IMEI实时查询

        //通过卡号查询物联卡绑定的IMEI信息。

        //接口调用请求说明

        //http请求方式： GET/POST

        //API请求URL示例：
        //https://api.iot.10086.cn/v5/ec/query/sim-imei?transid=xxxxxx&token=xxxxxx&msisdn=xxxxxx -以msisdn进行查询
        //https://api.iot.10086.cn/v5/ec/query/sim-imei?transid=xxxxxx&token=xxxxxx&imsi=xxxxxx–以imsi进行查询
        //https://api.iot.10086.cn/v5/ec/query/sim-imei?transid=xxxxxx&token=xxxxxx&iccid=xxxxxx –以iccid进行查询
        public class CMIOT_API23S04
        {
            public class ResultItem
            {
                /// <summary>
                /// 
                /// </summary>
                public string imei { get; set; }
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


        //        单卡状态查询

        //通过卡号查询物联卡的状态信息。

        //接口调用请求说明

        //http请求方式： GET/POST

        //API请求URL示例：
        //https://api.iot.10086.cn/v5/ec/query/sim-status?transid=xxxxxx&token=xxxxxx&msisdn=xxxxxx -以msisdn进行查询
        //https://api.iot.10086.cn/v5/ec/query/sim-status?transid=xxxxxx&token=xxxxxx&imsi=xxxxxx –以imsi进行查询
        //https://api.iot.10086.cn/v5/ec/query/sim-status?transid=xxxxxx&token=xxxxxx&iccid=xxxxxx –以iccid进行查询
        public class CMIOT_API25S041
        {

            public class ResultItem
            {
                /// <summary>
                /// 
                /// </summary>
                public string cardStatus { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string lastChangeDate { get; set; }
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
           
        }
        /// <summary>
        /// 单卡本月语音累计使用量实时查询      实时查询物联卡本月语音累计使用量。   
        /// </summary>
        public class CMIOT_API23U01
        {
            public class ResultItem
            {
                /// <summary>
                /// 
                /// </summary>
                public string voiceAmount { get; set; }
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
}