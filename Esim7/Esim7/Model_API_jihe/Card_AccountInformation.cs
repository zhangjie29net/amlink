using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Model_API_jihe
{/// <summary>
/// 物联网卡信息管理界面下的 综合信息查询 接口集合类
/// </summary>
    public class Card_AccountInformation
    {


        // Root_CMIOT_API2002       GetCMIOT_API2002  获取物联网卡状态
        // Root_CMIOT_API2011       Get_CMIOT_API2011  获取余额
        // Root_CMIOT_API2110       GetCMIOT_API2110  获取开户日期
        // Root_CMIOT_API2020       GetGetCMIOT_API2020    获取套餐  

            /// <summary>
            /// 卡账户据信息  全
            /// </summary>
        public class AccountInformation_Full  {


            public Root_CMIOT_API2002 root_CMIOT_API2002 { get; set; }

            public Root_CMIOT_API2011 root_CMIOT_API2011 { get; set; }

            public Root_CMIOT_API2110 root_CMIOT_API2110 { get; set; }

            public Root_CMIOT_API2020 root_CMIOT_API2020 { get; set; }


        }

        /// <summary>
        /// 卡账户信息  拼接
        /// </summary>

        public class AccountInformation_main {



            public List<GprsItem_CMIOT_API2020> root_CMIOT_API2020 { get; set; }
            public string Root_CMIOT_API2020_status { get; set; }
            /// <summary>
            /// 开户时间
            /// </summary>
            public string openTime { get; set; }
            
            public string OpenTime_status { get; set; }

            public string iccid { get; set; }
            public string msisdn { get; set; }
            public string imsi { get; set; }
            /// <summary>
            /// 余额
            /// </summary>
            public string balance { get; set; }
            public string Balance_status { get; set; }

           public string STATUS { get; set; }

            public string STATUS_status { get; set; }


        }

        /// <summary>
        /// 通信状态
        /// </summary>
        public class CommunicateStatus {


            public string OnOrOff { get; set; }
            public string OnOrOff_status { get; set; }

            public string GGSN_status { get; set; }
            public string GGSN_APN { get; set; }
            public string GGSN_GPRSSTATUS { get; set; }
            public string GGSN_IP { get; set; }
            public string GGSN_RAT { get; set; }






        }





    }
}