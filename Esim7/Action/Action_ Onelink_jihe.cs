using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Esim7.Model_API_jihe.Card_AccountInformation;

namespace Esim7.Action
{
    public class Action__Onelink_jihe
    {




        #region  物联网卡信息管理界面下的 综合信息查询 接口集合类


        #region 卡账户信息
        // Root_CMIOT_API2002       GetCMIOT_API2002  获取物联网卡状态
        // Root_CMIOT_API2011       Get_CMIOT_API2011  获取余额
        // Root_CMIOT_API2110       GetCMIOT_API2110  获取开户日期
        // Root_CMIOT_API2020       GetGetCMIOT_API2020    获取套餐  



        /// <summary>
        /// 卡账户信息 全
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        public static AccountInformation_Full accountInformation_Full(string imsi) {


            AccountInformation_Full account = new AccountInformation_Full();


            account.root_CMIOT_API2002 = Action.APIACTION.GetCMIOT_API2002(imsi);

            account.root_CMIOT_API2011 = Action.APIACTION.Get_CMIOT_API2011(imsi);

            account.root_CMIOT_API2020 = APIACTION.GetGetCMIOT_API2020(imsi);
            account.root_CMIOT_API2110 = APIACTION.GetCMIOT_API2110(imsi);

            return account;





        }
        /// <summary>
        /// 卡账户信息  拼接
        /// </summary>
        /// <param name="imsi"></param>
        /// <returns></returns>
        public   static  AccountInformation_main information_Main(string imsi) {
            AccountInformation_main account = new AccountInformation_main();
            #region 获取余额
            List<ResultItem_CMIOT_API2011> list2011 = new List<ResultItem_CMIOT_API2011>();
             Root_CMIOT_API2011  root_CMIOT_API2011 = new Root_CMIOT_API2011();

            root_CMIOT_API2011 = APIACTION.Get_CMIOT_API2011(imsi);

            list2011 = root_CMIOT_API2011.result;


            foreach (ResultItem_CMIOT_API2011 item in list2011)
            {



                account.balance = item.balance;
            }
            account.Balance_status = root_CMIOT_API2011.status;



            #endregion
            #region 获取物联网卡状态
            List<ResultItem_CMIOT_API2002> list2002 = new List<ResultItem_CMIOT_API2002>();
            Root_CMIOT_API2002 root_CMIOT_API2002 = new Root_CMIOT_API2002();
            root_CMIOT_API2002 = APIACTION.GetCMIOT_API2002(imsi);
            list2002 = root_CMIOT_API2002.result;
            foreach (ResultItem_CMIOT_API2002 item in list2002)
            {
                account.STATUS = item.STATUS;
            }
            account.STATUS_status = root_CMIOT_API2002.status;
            #endregion
            #region 获取开户日期
            Root_CMIOT_API2110 root_CMIOT_API2110 = new Root_CMIOT_API2110();
            List<ResultItem_CMIOT_API2110> list2110 = new List<ResultItem_CMIOT_API2110>();
            root_CMIOT_API2110 =  APIACTION.GetCMIOT_API2110(imsi);


            list2110 = root_CMIOT_API2110.result;

            foreach (ResultItem_CMIOT_API2110 item in list2110)
            {
                account.openTime = item.openTime;
                account.iccid = item.iccid;
                account.msisdn = item.msisdn;
                account.imsi = item.imsi;
            }
            account.OpenTime_status = root_CMIOT_API2110.status;


            #endregion
            #region 获取套餐信息
            Root_CMIOT_API2020 root_CMIOT_API2020 = new Root_CMIOT_API2020();

            List<ResultItem_CMIOT_API2020> list2020 = new List<ResultItem_CMIOT_API2020>();
            root_CMIOT_API2020 = APIACTION.GetGetCMIOT_API2020(imsi);

            list2020 = root_CMIOT_API2020.result;


            foreach (ResultItem_CMIOT_API2020 item in list2020)
            {



                foreach (GprsItem_CMIOT_API2020 items in item.gprs)
                {


                    double left = double.Parse(items.left)/1024;
                    items.left = left.ToString();

                    double used = double.Parse(items.used)/1024;

                    items.used = used.ToString();




                }
                account.root_CMIOT_API2020=item.gprs;

                #region 流量数据转化
               







                #endregion











            }

            account.Root_CMIOT_API2020_status = root_CMIOT_API2020.status;


            #endregion

            return account;

        }


        #endregion


        #region 通信状态
        public static CommunicateStatus communicateStatus(string imsi) {


            CommunicateStatus communicate = new CommunicateStatus();

            #region  获取开关机状态
            List<CMIOT_API2008> List2008 = new List<CMIOT_API2008>();


            Root_CMIOT_API2008 root_CMIOT_API2008 = new Root_CMIOT_API2008();
            root_CMIOT_API2008 = APIACTION.GetCMIOT_API2008(imsi);

            List2008 = root_CMIOT_API2008.result;
            foreach (CMIOT_API2008 item in List2008 )
            {

                communicate.OnOrOff = item.status;

            }
            communicate.OnOrOff_status = root_CMIOT_API2008.status;

            #endregion


            #region 获取 GGSN




            Root_CMIOT_API12001 root_CMIOT_API12001 = new Root_CMIOT_API12001();
            List<CMIOT_API12001> List2001 = new List<CMIOT_API12001>();

            root_CMIOT_API12001 = APIACTION.GetCMIOT_API12001(imsi);
            List2001 = root_CMIOT_API12001.result;
            foreach (CMIOT_API12001 item in List2001)
            {


                communicate.GGSN_APN = item.APN;
                communicate.GGSN_GPRSSTATUS = item.GPRSSTATUS;
                communicate.GGSN_IP = item.IP;
                communicate.GGSN_RAT = item.RAT;
            }
            communicate.GGSN_status = root_CMIOT_API12001.status;

            #endregion





            return communicate;
        }




        #endregion
        #endregion













    }
}