using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using Dapper;
using Esim7.Dto;
using Esim7.Models;
using Esim7.Models.UserStockModel;
using Esim7.parameter.PayManage;
using Esim7.parameter.User;
using Esim7.UNity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Esim7.Action
{
    /// <summary>
    /// 支付管理
    /// </summary>
    public class PayManageAction
    {
        ///<summary>
        ///添加支付信息
        /// </summary>
        public Information AddPayInfo(AddPayInfoPara para)
         {
            Information info = new Information();
            try
            {
                string sqladdpayinfo = string.Empty;
                string ordernum = string.Empty;
                DateTime time = DateTime.Now;
                string content = string.Empty;
                string PayName = string.Empty;
                if (string.IsNullOrWhiteSpace(para.ApiFlg)) 
                {
                    if (!string.IsNullOrWhiteSpace(para.OrderNum))
                    {
                        ordernum = para.OrderNum;
                    }
                    else
                    {
                        ordernum = "Pay" + Unit.GetTimeStamp(time);
                    }
                    int PayType = 0;
                    if (para.PayType == PayTypes.zfb)
                    {
                        PayType = 1;
                        PayName = "支付宝支付";
                    }
                    if (para.PayType == PayTypes.wx)
                    {
                        PayType = 2;
                        PayName = "微信支付";
                    }
                    if (para.PayType == PayTypes.yl)
                    {
                        PayType = 3;
                        PayName = "银行卡转账支付";
                    }
                    sqladdpayinfo = "insert into payinfo(PayUserName,PayCompany,Company_ID,Phone,PayType,PayMoney,WaterOrderNum,OrderNum,Remark,Addtime,fileName)" +
                        "values('" + para.PayUserName + "','" + para.PayCompany + "','" + para.Company_ID + "','" + para.Phone + "'," + PayType + ",'" + para.PayMoney + "','" + para.WaterOrderNum + "','" + ordernum + "','" + para.Remark + "','" + time + "','" + para.fileName + "')";
                    content = para.PayCompany + PayName + "付款" + para.PayMoney + "元" + "请及时核对！";
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        conn.Execute(sqladdpayinfo);
                        SendEmail.SendMail("zj13091019378@163.com", "zj13091019378@163.com", "zjnyq20200108", null, null, null, "用户支付信息", content);
                        info.flg = "1";
                        info.Msg = "成功!";
                    }
                }
                if (para.ApiFlg == "2")//支付编辑
                {
                    string sqlupdate = "update payinfo set PayUserName='" + para.PayUserName + "',Phone='" + para.Phone + "',PayType=" + 1 + ",PayMoney=" + para.PayMoney + "," +
                    "fileName='" + para.fileName + "',WaterOrderNum='" + para.WaterOrderNum + "',Remark='" + para.Remark + "',Status=" + 0 + ",Addtime='" + time + "' where OrderNum='" + para.OrderNum + "'";
                     content = para.PayCompany + "公司" + para.PayUserName + "支付" + para.PayMoney + "元,请及时核对!";
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        conn.Execute(sqlupdate);
                        info.flg = "1";
                        info.Msg = "成功!";
                        SendEmail.SendMail("zj13091019378@163.com", "zj13091019378@163.com", "zjnyq20200108", null, null, null, "用户支付信息", content);
                    }
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "添加失败:"+ex;
            }
            return info;
        }
        /// <summary>
        /// 查看支付信息 (废弃)
        /// </summary>
        /// <param name="Company_ID"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        //public GetPayInfoDto GetPayInfos1(string Company_ID,string Status )
        //{
        //    GetPayInfoDto dto = new GetPayInfoDto();
        //    string sqlpayinfo = string.Empty;
        //    try
        //    {
        //        if (Company_ID == "1556265186243")
        //        {
        //            sqlpayinfo = "select * from payinfo";
        //            using (IDbConnection conn = DapperService.MySqlConnection())
        //            {
        //                dto.payInfos = conn.Query<PayInfo>(sqlpayinfo).ToList().OrderByDescending(t => t.AddTime).ToList();
        //                if (!string.IsNullOrWhiteSpace(Status))
        //                {
        //                    if (Status == "0")
        //                    {
        //                        dto.payInfos = dto.payInfos.Where(t => t.Status == 0).ToList();
        //                    }
        //                    if (Status == "1")
        //                    {
        //                        dto.payInfos = dto.payInfos.Where(t => t.Status == 1).ToList();
        //                    }
        //                    if (Status == "2")
        //                    {
        //                        dto.payInfos = dto.payInfos.Where(t => t.Status == 2).ToList();
        //                    }
        //                }
        //                dto.flg = "1";
        //                dto.Msg = "查询成功";
        //            }
        //        }
        //        else
        //        {
        //            sqlpayinfo = "select * from payinfo where Company_ID='" +Company_ID + "'";
        //            using (IDbConnection conn = DapperService.MySqlConnection())
        //            {
        //                dto.payInfos = conn.Query<PayInfo>(sqlpayinfo).ToList().OrderByDescending(t => t.AddTime).ToList();
        //                if (!string.IsNullOrWhiteSpace(Status))
        //                {
        //                    if (Status == "0")
        //                    {
        //                        dto.payInfos = dto.payInfos.Where(t => t.Status == 0).ToList();
        //                    }
        //                    if (Status == "1")
        //                    {
        //                        dto.payInfos = dto.payInfos.Where(t => t.Status == 1).ToList();
        //                    }
        //                    if (Status == "1")
        //                    {
        //                        dto.payInfos = dto.payInfos.Where(t => t.Status == 2).ToList();
        //                    }
        //                }

        //                dto.flg = "1";
        //                dto.Msg = "查询成功";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        dto.flg = "-1";
        //        dto.Msg = "查询失败:" + ex;
        //    }
        //    return dto;
        //}

        ///<summary>
        ///查看支付信息  奇迹物联所有权限 Status 0:未审核  1:已审核  （废弃）
        /// </summary>
        //public GetPayInfoDto GetPayInfos2(PayInfoPara para)
        //{
        //    GetPayInfoDto dto = new GetPayInfoDto();
        //    List<PayInfo> payinfos =new List<PayInfo>();
        //    List<PayInfo> payinfos1 = new List<PayInfo>() ;
        //    string sqlpayinfo = string.Empty;
        //    try
        //    {
        //        if (para.Company_ID == "1556265186243")
        //        {
        //            sqlpayinfo = "select * from payinfo";
        //            using (IDbConnection conn = DapperService.MySqlConnection())
        //            {
        //                //dto.payInfos = conn.Query<PayInfo>(sqlpayinfo).ToList().OrderByDescending(t=>t.AddTime).ToList();
        //                //var listpayinfo=conn.Query<PayInfo>(sqlpayinfo).ToList().OrderByDescending(t => t.AddTime).ToList();
        //                payinfos= conn.Query<PayInfo>(sqlpayinfo).ToList().OrderByDescending(t => t.AddTime).ToList();
        //                if (!string.IsNullOrWhiteSpace(para.Status))
        //                {
        //                    if (para.Status == "0")
        //                    {
        //                        //dto.payInfos = dto.payInfos.Where(t => t.Status == 0).ToList();
        //                        payinfos = payinfos.Where(t => t.Status == 0).ToList();

        //                    }
        //                    if (para.Status == "1")
        //                    {
        //                        //dto.payInfos = dto.payInfos.Where(t => t.Status == 1).ToList();
        //                        payinfos = payinfos.Where(t => t.Status == 1).ToList();
        //                    }
        //                    if (para.Status == "2")
        //                    {
        //                        //dto.payInfos = dto.payInfos.Where(t => t.Status == 2).ToList();
        //                        payinfos = payinfos.Where(t => t.Status ==2).ToList();
        //                    }
        //                }
        //                foreach (var item in payinfos)
        //                {
        //                    PayInfo info = new PayInfo();
        //                    info.Company_ID = item.Company_ID;
        //                    info.AddTime = item.AddTime;
        //                    info.BankCardNum = item.BankCardNum;
        //                    info.BankName = item.BankName;
        //                    info.fileName = "GetUserPayFile?fileName=" + item.fileName;
        //                    info.Id = item.Id;
        //                    info.OrderNum = item.OrderNum;
        //                    info.PayCompany = item.PayCompany;
        //                    info.PayMoney = item.PayMoney;
        //                    info.PayType = item.PayType;
        //                    info.PayUserName = item.PayUserName;
        //                    info.Phone = item.Phone;
        //                    info.Remark = item.Remark;
        //                    info.Status = item.Status;
        //                    info.WaterOrderNum = item.WaterOrderNum;
        //                    info.ApiFlg = item.ApiFlg;
        //                    string addresql = "select Address from saleorder where OrderNum='" + item.OrderNum+"'";
        //                    info.Address = conn.Query<PayInfo>(addresql).Select(t => t.Address).FirstOrDefault();
        //                    payinfos1.Add(info);
        //                }

        //                #region 暂时未传
        //                if (!string.IsNullOrWhiteSpace(para.WaterOrderNum))
        //                {
        //                    dto.payInfos = dto.payInfos.Where(t => t.WaterOrderNum.Contains(para.WaterOrderNum)).ToList();
        //                }
        //                if (para.AddTime != null)
        //                {
        //                    string statrtime= para.AddTime[0];
        //                    string endtime = para.AddTime[1];
        //                    if (statrtime.Length > 10)
        //                    {
        //                        statrtime = statrtime.Substring(0, 10);
        //                    }
        //                    if (endtime.Length > 10)
        //                    {
        //                        endtime = endtime.Substring(0, 10);
        //                    }
        //                    DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        //                    long lTime = long.Parse(statrtime + "0000000");
        //                    TimeSpan toNow = new TimeSpan(lTime);
        //                    long ETime = long.Parse(endtime + "0000000");
        //                    TimeSpan etoNow = new TimeSpan(ETime);
        //                    DateTime StatrTime = dateTimeStart.Add(toNow);
        //                    DateTime EndTime = dateTimeStart.Add(etoNow);
        //                    dto.payInfos = dto.payInfos.Where(t => t.AddTime > StatrTime && t.AddTime <=EndTime.AddDays(1)).ToList();
        //                }
        //                #endregion
        //                dto.flg = "1";
        //                dto.Msg = "查询成功";
        //                dto.payInfos = payinfos1;
        //            }
        //        }
        //        else
        //        {
        //            sqlpayinfo = "select * from payinfo where Company_ID='" + para.Company_ID + "'";
        //            using (IDbConnection conn = DapperService.MySqlConnection())
        //            {
        //                dto.payInfos = conn.Query<PayInfo>(sqlpayinfo).ToList().OrderByDescending(t=>t.AddTime).ToList();
        //                if (!string.IsNullOrWhiteSpace(para.Status))
        //                {
        //                    if (para.Status == "0")
        //                    {
        //                        dto.payInfos = dto.payInfos.Where(t => t.Status == 0).ToList();
        //                    }
        //                    if (para.Status == "1")
        //                    {
        //                        dto.payInfos = dto.payInfos.Where(t => t.Status == 1).ToList();
        //                    }
        //                    if (para.Status == "2")
        //                    {
        //                        dto.payInfos = dto.payInfos.Where(t => t.Status == 2).ToList();
        //                    }
        //                }
        //                #region 暂时未传
        //                if (!string.IsNullOrWhiteSpace(para.WaterOrderNum))
        //                {
        //                    dto.payInfos = dto.payInfos.Where(t => t.WaterOrderNum.Contains(para.WaterOrderNum)).ToList();
        //                }
        //                if (para.AddTime != null)
        //                {
        //                    string statrtime = para.AddTime[0];
        //                    string endtime = para.AddTime[1];
        //                    if (statrtime.Length > 10)
        //                    {
        //                        statrtime = statrtime.Substring(0, 10);
        //                    }
        //                    if (endtime.Length > 10)
        //                    {
        //                        endtime = endtime.Substring(0, 10);
        //                    }
        //                    DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        //                    long lTime = long.Parse(statrtime + "0000000");
        //                    TimeSpan toNow = new TimeSpan(lTime);
        //                    long ETime = long.Parse(endtime + "0000000");
        //                    TimeSpan etoNow = new TimeSpan(ETime);
        //                    DateTime StatrTime = dateTimeStart.Add(toNow);
        //                    DateTime EndTime = dateTimeStart.Add(etoNow);
        //                    dto.payInfos = dto.payInfos.Where(t => t.AddTime > StatrTime && t.AddTime <= EndTime.AddDays(1)).ToList();
        //                }
        //                #endregion
        //                dto.flg = "1";
        //                dto.Msg = "查询成功";
        //            }
        //        } 
        //    }
        //    catch (Exception ex)
        //    {
        //        dto.flg = "-1";
        //        dto.Msg = "查询失败:"+ex;
        //    }
        //    return dto;
        //}

        ///<summary>
        ///查看支付信息
        /// </summary>
        public OrderPayInfoDto GetPayInfos(string Company_ID, string Status) 
        {
            OrderPayInfoDto dto = new OrderPayInfoDto();
            string sqlpay = string.Empty;
            string orderststus = string.Empty;
            string striccid = string.Empty;
            string strsn = string.Empty;
            string OperatorsFlg = string.Empty;
            string sql2 = string.Empty;
            string RenewType = string.Empty;//续费类型
            int RenewNum = 0;//续费时长
            string DT = string.Empty;
            string sqlupdateendtime = string.Empty;
            List<Model_Charing_Card> charing_Cards = new List<Model_Charing_Card>();
            try
            {
                if (Company_ID == "1556265186243")
                {
                    sqlpay = "select t1.OrderNum,t1.Address,t1.Addtime,t1.TotalPrice,t1.Company_ID,t1.Status,t2.CompanyName as Company_Name from saleorder t1 left join company t2 on t1.Company_ID=t2.CompanyID";
                }
                if (Company_ID != "1556265186243")
                {
                    sqlpay = "select t1.OrderNum,t1.Address,t1.Addtime,t1.TotalPrice,t1.Company_ID,t1.Status,t2.CompanyName as Company_Name from saleorder t1 left join company t2 on t1.CustomerCompany=t2.CompanyID where t1.CustomerCompany='" + Company_ID + "'";
                }
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    //获取当前用户的订单状态
                    string sqlorderlist = "select OrderNum,OrderType from saleorder where CustomerCompany='" + Company_ID + "' and Status is null or Status =''";
                    var listorder = conn.Query<OrderPay>(sqlorderlist).ToList();
                    foreach (var item in listorder)
                    {
                        string APP_PRIVATE_KEY = "MIIEpAIBAAKCAQEAni1vQDBddwdR7xfTMEs1x+0i7wJjlL/NN7eqa3yR8qHQrmDMXGhrYFenrhPwp31HF1FrWHrcSxPgEYbzokIFlhWwgNkW7gDBvBpZCXB5SwKiwmOMXZHxs3C+257/FX7onPeDTxSHbEerJfHbDgK3xXtKbqcUEGSRhR41LyZZ23GrDmDmGP4G7LSLaapOCqIJR7qxib67bqFKwpYMB7fKJvo2mXeT9H84v5WuB5maE5c3Fq9tnK7Cj0xH88/6GUuW7NkBfFJJ0kuDMvx4djBRDs3IRF4CA4qgxj43Fddi8R6O3jxKtDj4wrFKQSCJ99dhM+SCwrx9pu7VUrXvDLRpDQIDAQABAoIBAH26xaxzBUgApIr1GCRSFAy3nMX40yjAkKHSNv87RFNldhe1z5tAUOGCq0E+jlLDzMdnK3loJ0TyJnAoIe5+piwXT3YLmSNTrKsVEmLjaTZLgQ4czMzvnfyxCLRPnJj8iG+EenZYbhCOoycFKMbpOCQcDR0JZ3RkTBtQ2JuC03gOkSAUuIGY7HAUbxo3Na8q4XWrC4r5Tst9Up/5YXJkOKRIfRhHLj8eTVFr55yRQr25rfbtAywgUBDziQV/6TtjfW89ve5ZyIWJJ2IiZBCZaGiWFv/0E1VXwK3GMGhlKMxakW2ErnGdjFgLsgZe251gZFbB3aUqIzEVD0IXbQfRzQECgYEAzQwf6Um0ORMwjAwIToJ9hE0adXcCuqPRmknz0LhRlOIWLz0wLHD3A/d4SpummesnphwQzPv/32eGQSVa3C8mJVR67r+9tyhgehPSTbTknfBLi/EMlPuP21//CfY6BDw8D+Qp83pecIDpC9G1YuqY6kck+LgTIDU4t3EbXZRaQsECgYEAxXu7HBAil8NUBzw5VEljq31VayMhEkxEjjKQ1oOqEb8D21MeCcg4pYlWrdEXPzQTYDTktGgw51iQhl3RQtWOiB+kZ9eB1Jeeb+DJ7LPQQLMomJ2WW3Ifc4PTl37L+eHtZF2UnSAXzcWE8oEWBHqDXoKHn1rR0VOxlYYWKDW0lU0CgYBBiMiCNS5Gt51ihU36WbZoHISCWoEiyczp9QzZ/q8EWKYifvnwmkI4oFVv8wNyWjgX3Qx5l2kWK+460AeUK/WTJMcNm3a1HVCIc+FZOKGA4RYkKeyNiUFXKf1HX8z3IZFIuvG1gbzBVFInluTUMoqhBhAI9jwWpSv1ZU8Lv6iBgQKBgQC12nC9MOaKFmcEmqK4STStMKc+F4VW0kC3KT1TLL+pw9qLV4jrvSLc3RGi0k6z9wJ4r3yD3QZbo6TC9xXCk+HC3FCLB1sZJ93QbJHFlPgK/HA/ya4x22+28gghE7RGEZSHvd0iq/D/ngoFPnXF/gOHPnAhgIKCXq3DYUx6DqqSaQKBgQCg0UrQByafQdU01BY9TGjvnzSeJszkd87QW8uEOJUuyNMdqESnOZImjCBrunv7WGErPeUOlHKiv5eaztDqvWwMQSAIghOlrvw+QrpqCPq3J2C1CJpWAMpc9BX9ymwiu1dC8LsmPtFNNa9Aw8gLnbOIIw8ejVg+cUv4nxnUOvFfbQ==";
                        string ALIPAY_PUBLIC_KEY = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAqfnMoeipEFw3aZfh7OcRQ/nvN/R3udxILYIzA/SoQgtyso6OX5CgdUJZTTf0D39W1yE4At0iAiyv+2vhmXwqs00JgQY6LZ3Aq5FDCIP+8+X1XavfNAeG8bRyiaulhVYTlkyByFMHlwcEFQZDQ0yeIcQ42Q6BnXu8sCar7IETRzwdRvd2Y+w0W/jobWaQILPGjplAObzk4fnyGUlekkdPsxAt+xM6rNyUZy0/173toaLjZkv2a1uBePTzEhfGsTh145DEBJUqMj4PSYFgtr9muZEV9tuxmu+L8urVcnCnyLSusIImNmaAEC4e0rEwhJSbsec5mGkYhllRTDru3NHsOwIDAQAB";
                        IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", "2021001186603089", APP_PRIVATE_KEY, "json", "1.0", "RSA2", ALIPAY_PUBLIC_KEY, "GBK", false);
                        AlipayTradeQueryRequest request = new AlipayTradeQueryRequest();
                        request.BizContent = "{" +
                        "\"out_trade_no\":\"" + item.OrderNum + "\"" +
                        "  }";
                        AlipayTradeQueryResponse response = client.Execute(request);
                        if (response.Code == "10000")
                        {
                            orderststus = response.TradeStatus;
                            if (orderststus == "TRADE_SUCCESS")//支付成功
                            {
                                if (item.OrderType != "2")//不是在线续费
                                {
                                    //根据当前用户登录的公司编码找到父级公司编码
                                    string cfuserpid = "select User_Pid from cf_user where Company_ID='" + Company_ID + "'";
                                    string sqlorder = "select Commission from saleorder where OrderNum='" + item.OrderNum + "'";
                                    decimal Balance = 0;
                                    decimal Commission = 0;
                                    string pcompany_id = string.Empty;
                                    int id = 0;
                                    id = conn.Query<User>(cfuserpid).Select(t => t.User_Pid).FirstOrDefault();
                                    string cfuserpcompany = "select Company_ID from cf_user where id=" + id + "";
                                    pcompany_id = conn.Query<User>(cfuserpcompany).Select(t => t.Company_ID).FirstOrDefault();
                                    string sqlcfuser = "select AccountBalance from cf_user where Company_ID='" + pcompany_id + "'";
                                    Balance = conn.Query<User>(sqlcfuser).Select(t => t.AccountBalance).FirstOrDefault();
                                    Commission = conn.Query<OrderInfo>(sqlorder).Select(t => t.Commission).FirstOrDefault();
                                    Balance += Commission;
                                    string sqlupdatecfuser = "update  cf_user set AccountBalance=" + Balance + "  where Company_ID='" + pcompany_id + "'";
                                    conn.Execute(sqlupdatecfuser);
                                }
                                if (item.OrderType == "2")//是在线续费
                                {
                                    //根据订单编号找到续费的卡集合
                                    string sqlordercard = "select * from renewalrecord where OrderNum='"+item.OrderNum+"'";
                                    var listcard = conn.Query<renewalrecord>(sqlordercard).ToList();
                                    foreach (var itemiccid in listcard)
                                    {
                                        OperatorsFlg = itemiccid.OperatorsFlg;
                                        RenewType = itemiccid.RenewType;
                                        RenewNum = itemiccid.RenewNum;
                                        if (itemiccid.OperatorsFlg != "4")
                                        {
                                            striccid += "'" + itemiccid.Card_ICCID + "',";
                                        }
                                        else
                                        {
                                            strsn += "'" + itemiccid.SN + "',";
                                        }
                                    }
                                    if (!string.IsNullOrWhiteSpace(striccid))
                                    {
                                        striccid = striccid.Substring(0, striccid.Length - 1);
                                    }
                                    if (!string.IsNullOrWhiteSpace(strsn))
                                    {
                                        strsn = strsn.Substring(0, striccid.Length - 1);
                                    }
                                    if (OperatorsFlg == "1")//移动
                                    {
                                        //检查数据是否匹配
                                        sql2 = @"select Card_ICCID, DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime from card_copy1 where status=1 and Card_CompanyID='"+Company_ID+"' and Card_ICCID in (" + striccid + ")";
                                    }
                                    if (OperatorsFlg == "2")//电信
                                    {
                                        //检查数据是否匹配
                                        sql2 = @"select Card_ICCID,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime,from ct_cardcopy where status=1 and Card_CompanyID='"+Company_ID+"' and Card_ICCID in (" + striccid + ")";
                                    }
                                    if (OperatorsFlg == "3")//联通
                                    {
                                        //检查数据是否匹配
                                        sql2 = @"select Card_ICCID,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime from cucc_cardcopy where status=1 and Card_CompanyID='"+Company_ID +"' and Card_ICCID in (" + striccid + ")";
                                    }
                                    if (OperatorsFlg == "5")//漫游
                                    {
                                        //检查数据是否匹配
                                        sql2 = @"select Card_ICCID,DATE_FORMAT(Card_EndTime,'%Y-%m-%d %H:%i:%s') as Card_EndTime from roamcard_copy where status=1 and Card_CompanyID='"+Company_ID+"' and Card_ICCID in (" + striccid + ")";
                                    }
                                    charing_Cards = conn.Query<Model_Charing_Card>(sql2).ToList();
                                    if (RenewType == "1")//年
                                    {
                                        foreach (var itemdate in charing_Cards)
                                        {
                                            itemdate.Card_EndTime = itemdate.Card_EndTime.AddYears(RenewNum);
                                            DT = Unit.GetTimeStamp(itemdate.Card_EndTime);
                                            if (OperatorsFlg == "1")//移动
                                            {
                                                sqlupdateendtime = "update card_copy1 set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + itemdate.Card_ICCID + "'";
                                            }
                                            if (OperatorsFlg == "2")//电信
                                            {
                                                sqlupdateendtime = "update ct_cardcopy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + itemdate.Card_ICCID + "'";
                                            }
                                            if (OperatorsFlg == "3")//联通
                                            {
                                                sqlupdateendtime = "update cucc_cardcopy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + itemdate.Card_ICCID + "'";
                                            }
                                            if (OperatorsFlg == "5")//漫游
                                            {
                                                sqlupdateendtime = "update roamcard_copy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + itemdate.Card_ICCID + "'";
                                            }
                                            conn.Execute(sqlupdateendtime);
                                        }
                                    }
                                    if (RenewType == "2")//月
                                    {
                                        foreach (var itemdate in charing_Cards)
                                        {
                                            itemdate.Card_EndTime = itemdate.Card_EndTime.AddMonths(RenewNum);
                                            DT = Unit.GetTimeStamp(itemdate.Card_EndTime);
                                            if (OperatorsFlg == "1")//移动
                                            {
                                                sqlupdateendtime = "update card_copy1 set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + itemdate.Card_ICCID + "'";
                                            }
                                            if (OperatorsFlg == "2")//电信
                                            {
                                                sqlupdateendtime = "update ct_cardcopy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + itemdate.Card_ICCID + "'";
                                            }
                                            if (OperatorsFlg == "3")//联通
                                            {
                                                sqlupdateendtime = "update cucc_cardcopy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + itemdate.Card_ICCID + "'";
                                            }
                                            if (OperatorsFlg == "5")//漫游
                                            {
                                                sqlupdateendtime = "update roamcard_copy set  Card_EndTime =FROM_UNIXTIME(" + DT + "/1000)  ,RenewDate=" + DT + " where Card_ICCID='" + itemdate.Card_ICCID + "'";
                                            }
                                            conn.Execute(sqlupdateendtime);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            orderststus = "TRADE_CLOSED";
                        }
                        string sqlupdate = "update saleorder set Status='" + orderststus + "' where OrderNum='" + item.OrderNum + "'";
                        conn.Execute(sqlupdate);
                    }
                    var listpay = conn.Query<OrderPay>(sqlpay).ToList().OrderByDescending(t=>t.AddTime).ToList();
                    if (!string.IsNullOrWhiteSpace(Status))
                    {
                        listpay = listpay.Where(t => t.Status == Status).ToList();
                    }
                    dto.flg = "1";
                    dto.Msg = "成功!";
                    dto.orderPays = listpay;
                }
            }
            catch (Exception ex)
            {
                dto.flg = "-1";
                dto.Msg = "失败!";
                dto.orderPays = null;
            }
            return dto; 
        }

        ///<summary>
        ///奇迹审核到账操作  0:待审核  1:已审核
        /// </summary>
        public Information PayExamine(PayExaminePara para)
        {
            Information info = new Information();
            try
            {
                if (para.Company_ID == "1556265186243")
                {
                    string sqlupdatestatus = "update payinfo set Status="+para.Status+ " where Id="+para.id+"";
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        if (para.Status == 1)//通过审核
                        {
                            string sqlpaymoney = "select * from saleorder where OrderNum='"+para.OrderNum+"'";
                            decimal Commission = 0;
                            string company_Id =string.Empty;
                            decimal AccountBalance = 0;
                            decimal Extract = 0;//提现
                            decimal AccountBalance1 = 0;//提现后余额
                            var orderinfo = conn.Query<OrderInfo>(sqlpaymoney).FirstOrDefault();
                            if (orderinfo != null)
                            {
                                company_Id= orderinfo.Company_ID;
                                Commission = orderinfo.Commission;
                                Extract = orderinfo.Extract;
                                string sqlAccountBalance = "select AccountBalance as Commission from cf_user where Company_ID='" + company_Id + "'";

                                AccountBalance = conn.Query<OrderInfo>(sqlAccountBalance).Select(t => t.Commission).FirstOrDefault();
                                AccountBalance = AccountBalance + Commission;
                                AccountBalance1 = AccountBalance - Extract;
                                if (Commission != 0)
                                {
                                    string updatecfuser = "update cf_user set AccountBalance= " + AccountBalance + "  where Company_ID='" + company_Id + "'";
                                    conn.Execute(updatecfuser);
                                }
                                if (Extract != 0)
                                {
                                    string updatecfuser = "update cf_user set AccountBalance= " + AccountBalance1 + "  where Company_ID='" + company_Id + "'";
                                    conn.Execute(updatecfuser);
                                }
                            }
                        }
                        conn.Execute(sqlupdatestatus);
                        info.flg = "1";
                        info.Msg = "审核成功!";
                    }
                }
                else
                {
                    info.flg = "-1";
                    info.Msg = "您没有审核权限!";
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "出错:"+ex;
            }
            return info;
        }

        ///<summary>
        ///微信支付
        /// </summary>
        public string CreateWxPayOrder()
        {
            return null;
        }

        ///<summary>
        ///创建支付账单
        /// </summary>
        public Information AddPayOrder(CreateOrderPara para)
        {
            Information info = new Information();
            string ordernum = Unit.GetTimeStamp(DateTime.Now);//订单编号
            try
            {
                DateTime time = DateTime.Now;
                int CardNum = para.Cards.Count;
                string strcards = string.Empty;
                string SetmealName = string.Empty;
                decimal CardPrice = para.CardTotalPrice/CardNum;
                string CustomerCompanyName = string.Empty;
                StringBuilder sqlexcel = new StringBuilder("");
                if (para.Cards[0].SN != null)
                {
                     sqlexcel.Append("insert into saleorder_excel (SN,OrderNum,AddTime) VALUES(");
                    foreach (var item in para.Cards)
                    {
                        strcards += "'" + item.SN + "',"+"'"+ordernum+"',"+"'"+time+"'),(";
                    }
                    strcards = strcards.Substring(0, strcards.Length - 2);
                    sqlexcel.Append(strcards);
                }
                if (!string.IsNullOrWhiteSpace(para.Cards[0].ICCID) && string.IsNullOrWhiteSpace(para.Cards[0].Card_ID))
                {
                    sqlexcel.Append("insert into saleorder_excel(ICCID,OrderNum,AddTime)VALUES(");
                    foreach (var item in para.Cards)
                    {
                        strcards += "'" + item.ICCID + "'," + "'" + ordernum + "'," + "'" + time + "'),(";
                    }
                    strcards = strcards.Substring(0, strcards.Length - 2);
                    sqlexcel.Append(strcards);
                }
                if (string.IsNullOrWhiteSpace(para.Cards[0].ICCID) && !string.IsNullOrWhiteSpace(para.Cards[0].Card_ID))
                {
                    sqlexcel.Append("insert into saleorder_excel(Card_ID,OrderNum,AddTime)VALUES(");
                    foreach (var item in para.Cards)
                    {
                        strcards += "'" + item.Card_ID + "'," + "'" + ordernum + "'," + "'" + time + "'),(";
                    }
                    strcards = strcards.Substring(0, strcards.Length - 2);
                    sqlexcel.Append(strcards);
                }
                if (!string.IsNullOrWhiteSpace(para.Cards[0].ICCID) && !string.IsNullOrWhiteSpace(para.Cards[0].Card_ID))
                {
                    sqlexcel.Append("insert into saleorder_excel(Card_ID,ICCID,OrderNum,AddTime) VALUES(");
                    foreach (var item in para.Cards)
                    {
                        strcards += "'" + item.Card_ID + "',"+"'"+item.ICCID+"'," + "'" + ordernum + "'," + "'" + time + "'),(";
                    }
                    strcards = strcards.Substring(0, strcards.Length - 2);
                    sqlexcel.Append(strcards);
                }
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    int status = -1;
                    string sqlsetmeal = "select * from  setmeal where SetmealID='"+para.SetmealID+"'";
                    SetmealName = conn.Query<setmeal>(sqlsetmeal).Select(t => t.PackageDescribe).FirstOrDefault();
                    string addordersql = "insert into saleorder(Company_ID,CardPrice,CardNum,MonthNum,TotalPrice,OrderNum,CustomerCompany,PurchaseDate,SetmealName)" +
                    " values('" + para.Company_ID + "'," + CardPrice + "," + CardNum + "," + para.MonthNum + "," + para.CardTotalPrice + ",'" + ordernum + "','" + para.CustomerCompanyId + "','" + time + "','"+ SetmealName + "')";
                    string sqlcoustom = "select * from company where CompanyID='"+para.CustomerCompanyId+"'";
                    CustomerCompanyName = conn.Query<Company>(sqlcoustom).Select(t => t.CompanyName).FirstOrDefault();
                    string addpayinfo = "insert into payinfo (PayCompany,Company_ID,PayMoney,Status,OrderNum,ApiFlg) values('" + CustomerCompanyName+"','"+para.CustomerCompanyId+"',"+para.CardTotalPrice+","+status+",'"+ordernum+"','2')";
                    conn.Execute(addordersql);//添加到账单
                    conn.Execute(addpayinfo);//添加到支付信息
                    conn.Execute(sqlexcel.ToString());//添加excel
                    info.flg = "1";
                    info.Msg = "成功!";
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "失败:"+ex;
                string deleordersql = "delete from saleorder where OrderNum='"+ ordernum + "'";
                string delpayinfo = "delete from payinfo where OrderNum='" + ordernum + "'";
                string delsqlexcel = "delete from saleorder_excel where OrderNum='" + ordernum + "'";
                using (IDbConnection conns = DapperService.MySqlConnection())
                {
                    conns.Execute(deleordersql);//删除账单
                    conns.Execute(delpayinfo);//删除支付信息
                    conns.Execute(delsqlexcel);//删除excel
                }
            }
            return info;
        }

        #region 当面付
        ///<summary>
        ///统一收单线下交易预创建 （扫码支付）
        /// </summary>
        public AlipayTradePrecreateResponse PayRequest()
        {
            //Information info = new Information();
            string APP_PRIVATE_KEY = "MIIEpAIBAAKCAQEAni1vQDBddwdR7xfTMEs1x+0i7wJjlL/NN7eqa3yR8qHQrmDMXGhrYFenrhPwp31HF1FrWHrcSxPgEYbzokIFlhWwgNkW7gDBvBpZCXB5SwKiwmOMXZHxs3C+257/FX7onPeDTxSHbEerJfHbDgK3xXtKbqcUEGSRhR41LyZZ23GrDmDmGP4G7LSLaapOCqIJR7qxib67bqFKwpYMB7fKJvo2mXeT9H84v5WuB5maE5c3Fq9tnK7Cj0xH88/6GUuW7NkBfFJJ0kuDMvx4djBRDs3IRF4CA4qgxj43Fddi8R6O3jxKtDj4wrFKQSCJ99dhM+SCwrx9pu7VUrXvDLRpDQIDAQABAoIBAH26xaxzBUgApIr1GCRSFAy3nMX40yjAkKHSNv87RFNldhe1z5tAUOGCq0E+jlLDzMdnK3loJ0TyJnAoIe5+piwXT3YLmSNTrKsVEmLjaTZLgQ4czMzvnfyxCLRPnJj8iG+EenZYbhCOoycFKMbpOCQcDR0JZ3RkTBtQ2JuC03gOkSAUuIGY7HAUbxo3Na8q4XWrC4r5Tst9Up/5YXJkOKRIfRhHLj8eTVFr55yRQr25rfbtAywgUBDziQV/6TtjfW89ve5ZyIWJJ2IiZBCZaGiWFv/0E1VXwK3GMGhlKMxakW2ErnGdjFgLsgZe251gZFbB3aUqIzEVD0IXbQfRzQECgYEAzQwf6Um0ORMwjAwIToJ9hE0adXcCuqPRmknz0LhRlOIWLz0wLHD3A/d4SpummesnphwQzPv/32eGQSVa3C8mJVR67r+9tyhgehPSTbTknfBLi/EMlPuP21//CfY6BDw8D+Qp83pecIDpC9G1YuqY6kck+LgTIDU4t3EbXZRaQsECgYEAxXu7HBAil8NUBzw5VEljq31VayMhEkxEjjKQ1oOqEb8D21MeCcg4pYlWrdEXPzQTYDTktGgw51iQhl3RQtWOiB+kZ9eB1Jeeb+DJ7LPQQLMomJ2WW3Ifc4PTl37L+eHtZF2UnSAXzcWE8oEWBHqDXoKHn1rR0VOxlYYWKDW0lU0CgYBBiMiCNS5Gt51ihU36WbZoHISCWoEiyczp9QzZ/q8EWKYifvnwmkI4oFVv8wNyWjgX3Qx5l2kWK+460AeUK/WTJMcNm3a1HVCIc+FZOKGA4RYkKeyNiUFXKf1HX8z3IZFIuvG1gbzBVFInluTUMoqhBhAI9jwWpSv1ZU8Lv6iBgQKBgQC12nC9MOaKFmcEmqK4STStMKc+F4VW0kC3KT1TLL+pw9qLV4jrvSLc3RGi0k6z9wJ4r3yD3QZbo6TC9xXCk+HC3FCLB1sZJ93QbJHFlPgK/HA/ya4x22+28gghE7RGEZSHvd0iq/D/ngoFPnXF/gOHPnAhgIKCXq3DYUx6DqqSaQKBgQCg0UrQByafQdU01BY9TGjvnzSeJszkd87QW8uEOJUuyNMdqESnOZImjCBrunv7WGErPeUOlHKiv5eaztDqvWwMQSAIghOlrvw+QrpqCPq3J2C1CJpWAMpc9BX9ymwiu1dC8LsmPtFNNa9Aw8gLnbOIIw8ejVg+cUv4nxnUOvFfbQ==";
            string ALIPAY_PUBLIC_KEY = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAqfnMoeipEFw3aZfh7OcRQ/nvN/R3udxILYIzA/SoQgtyso6OX5CgdUJZTTf0D39W1yE4At0iAiyv+2vhmXwqs00JgQY6LZ3Aq5FDCIP+8+X1XavfNAeG8bRyiaulhVYTlkyByFMHlwcEFQZDQ0yeIcQ42Q6BnXu8sCar7IETRzwdRvd2Y+w0W/jobWaQILPGjplAObzk4fnyGUlekkdPsxAt+xM6rNyUZy0/173toaLjZkv2a1uBePTzEhfGsTh145DEBJUqMj4PSYFgtr9muZEV9tuxmu+L8urVcnCnyLSusIImNmaAEC4e0rEwhJSbsec5mGkYhllRTDru3NHsOwIDAQAB";
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", "2021001186603089", APP_PRIVATE_KEY, "json", "1.0", "RSA2", ALIPAY_PUBLIC_KEY, "GBK", false);
            AlipayTradePrecreateRequest request = new AlipayTradePrecreateRequest();
            request.BizContent = "{" +
            "\"out_trade_no\":\"20150320010101006\"," +
            "\"total_amount\":0.01," +
            "\"subject\":\"Iphone616G\"" +
            "  }";
            AlipayTradePrecreateResponse response = client.Execute(request);
            return response;
        }

        ///<summary>
        ///统一收单线下交易查询 (扫码支付 支持电脑网站支付后的订单查询)
        /// </summary>
        public AlipayTradeQueryResponse TradePayResponse()
        {
            //Information info = new Information();
            string APP_PRIVATE_KEY = "MIIEpAIBAAKCAQEAni1vQDBddwdR7xfTMEs1x+0i7wJjlL/NN7eqa3yR8qHQrmDMXGhrYFenrhPwp31HF1FrWHrcSxPgEYbzokIFlhWwgNkW7gDBvBpZCXB5SwKiwmOMXZHxs3C+257/FX7onPeDTxSHbEerJfHbDgK3xXtKbqcUEGSRhR41LyZZ23GrDmDmGP4G7LSLaapOCqIJR7qxib67bqFKwpYMB7fKJvo2mXeT9H84v5WuB5maE5c3Fq9tnK7Cj0xH88/6GUuW7NkBfFJJ0kuDMvx4djBRDs3IRF4CA4qgxj43Fddi8R6O3jxKtDj4wrFKQSCJ99dhM+SCwrx9pu7VUrXvDLRpDQIDAQABAoIBAH26xaxzBUgApIr1GCRSFAy3nMX40yjAkKHSNv87RFNldhe1z5tAUOGCq0E+jlLDzMdnK3loJ0TyJnAoIe5+piwXT3YLmSNTrKsVEmLjaTZLgQ4czMzvnfyxCLRPnJj8iG+EenZYbhCOoycFKMbpOCQcDR0JZ3RkTBtQ2JuC03gOkSAUuIGY7HAUbxo3Na8q4XWrC4r5Tst9Up/5YXJkOKRIfRhHLj8eTVFr55yRQr25rfbtAywgUBDziQV/6TtjfW89ve5ZyIWJJ2IiZBCZaGiWFv/0E1VXwK3GMGhlKMxakW2ErnGdjFgLsgZe251gZFbB3aUqIzEVD0IXbQfRzQECgYEAzQwf6Um0ORMwjAwIToJ9hE0adXcCuqPRmknz0LhRlOIWLz0wLHD3A/d4SpummesnphwQzPv/32eGQSVa3C8mJVR67r+9tyhgehPSTbTknfBLi/EMlPuP21//CfY6BDw8D+Qp83pecIDpC9G1YuqY6kck+LgTIDU4t3EbXZRaQsECgYEAxXu7HBAil8NUBzw5VEljq31VayMhEkxEjjKQ1oOqEb8D21MeCcg4pYlWrdEXPzQTYDTktGgw51iQhl3RQtWOiB+kZ9eB1Jeeb+DJ7LPQQLMomJ2WW3Ifc4PTl37L+eHtZF2UnSAXzcWE8oEWBHqDXoKHn1rR0VOxlYYWKDW0lU0CgYBBiMiCNS5Gt51ihU36WbZoHISCWoEiyczp9QzZ/q8EWKYifvnwmkI4oFVv8wNyWjgX3Qx5l2kWK+460AeUK/WTJMcNm3a1HVCIc+FZOKGA4RYkKeyNiUFXKf1HX8z3IZFIuvG1gbzBVFInluTUMoqhBhAI9jwWpSv1ZU8Lv6iBgQKBgQC12nC9MOaKFmcEmqK4STStMKc+F4VW0kC3KT1TLL+pw9qLV4jrvSLc3RGi0k6z9wJ4r3yD3QZbo6TC9xXCk+HC3FCLB1sZJ93QbJHFlPgK/HA/ya4x22+28gghE7RGEZSHvd0iq/D/ngoFPnXF/gOHPnAhgIKCXq3DYUx6DqqSaQKBgQCg0UrQByafQdU01BY9TGjvnzSeJszkd87QW8uEOJUuyNMdqESnOZImjCBrunv7WGErPeUOlHKiv5eaztDqvWwMQSAIghOlrvw+QrpqCPq3J2C1CJpWAMpc9BX9ymwiu1dC8LsmPtFNNa9Aw8gLnbOIIw8ejVg+cUv4nxnUOvFfbQ==";
            string ALIPAY_PUBLIC_KEY = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAqfnMoeipEFw3aZfh7OcRQ/nvN/R3udxILYIzA/SoQgtyso6OX5CgdUJZTTf0D39W1yE4At0iAiyv+2vhmXwqs00JgQY6LZ3Aq5FDCIP+8+X1XavfNAeG8bRyiaulhVYTlkyByFMHlwcEFQZDQ0yeIcQ42Q6BnXu8sCar7IETRzwdRvd2Y+w0W/jobWaQILPGjplAObzk4fnyGUlekkdPsxAt+xM6rNyUZy0/173toaLjZkv2a1uBePTzEhfGsTh145DEBJUqMj4PSYFgtr9muZEV9tuxmu+L8urVcnCnyLSusIImNmaAEC4e0rEwhJSbsec5mGkYhllRTDru3NHsOwIDAQAB";
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", "2021001186603089", APP_PRIVATE_KEY, "json", "1.0", "RSA2", ALIPAY_PUBLIC_KEY, "GBK", false);
            AlipayTradeQueryRequest request = new AlipayTradeQueryRequest();
            request.BizContent = "{" +
            "\"out_trade_no\":\"1609315523435\"" +
            //"\"total_amount\":0.01," +
            //"\"subject\":\"Iphone616G\"" +
            "  }";
            AlipayTradeQueryResponse response = client.Execute(request);
            return response;
        }
        #endregion

        #region  电脑网站支付
        ///<summary>
        ///统一收单下单并支付页面接口
        /// </summary>
        public AlipayTradePagePayResponse TradePagePayResponse()
        {
            string APP_PRIVATE_KEY = "MIIEogIBAAKCAQEA05jrPUPy0yG1dps1+71F/N/4FjqFC7gJBBtRWIykm5WR5XwbNmWb0dhm29i4+bQINbilKa/qFuT65TgQuJ1nCfhtqCySSJ3MsdBallZLn3H+eXb+krHVELpAlbotAcCQ2ZFqzNuavL9bhlvGLeSCH1SJMC8zgZXp9ZW/0aFQZd+riZHXLhO8msWoQcRqN7f/YR9C2oha/vYpY2qym7ZslJsNHVyET2ZB8XLlvBDJy1G6GMXQLfHwUl7CWYzXwJG/5pDxoEbwDmnXxLqQkUX5xUKnPzGrNQb9F3tq2R8/C5yMtCUJssKmE46hYan4jr+B0qP8S63LuATjmqwqe7/wMQIDAQABAoIBAG0Cc/Z9IgU5cYYoEhid+wd6zxGMlmxiJGr0M+U9l7P7y00BsfdFQ5BJPzx1m14xLKWYeaZPVXb0AnnCd4LUvHe7f6rLQ5Wbjg/xOioHTTBYhvRGpIIokY7rlUhNwNANR9J+gxoE7OPeZaWDdEbCWXMQlxi2yH8zH3QA8PBrfcLtF0i7mfLA+/oo4hw4HpsqebUjNYGh5KR6O63yf7vJS99wFZP5Dmk5zoO8AAxeuRkEAoiGFtsa8S4xQw4GRp/3nAhUK2Ib7y0qZSlB1oUd4j6QR7IwbNDA7/8uJv4lU6OQ7QZMm02JX4zieQmkv3XtTDQ2V6TJ8EQ3NvtpyzYa9AECgYEA70SrOl0J7TygrJWVMm1hiW/uO58cZ+d+Ab/2DGOQs/pTL4I2aT0fW95Hc/IoTTRoi09VeQUAvfmhpmOwwkfZSUToB6ojV6Mg8BCntmyY7ASXWSUK95Lgfcj1Vr964PlAMBsSoM8jjgk0zvgeb8P1a6qyU/IPV1i4IAh9PbVoyEECgYEA4mTkKnkq/+iHiQsLXsyJNbNX90PbktbwCW3sa9GNLwZEhZFkq/N/tvB9+qVRYE1S/TXBHIhKaj34T0ZlYOu78ZQQM6LgM7nRStoLrlVSG/srgQlHm9LgOgynzZkXBUD+uU347cJ5aM4Ew8FAMEd9OWEeIPHrZDK4VvFXqOBYq/ECgYBn0CCG4yVSdJK2LvSb+49tRU5VOhTmFC+87KACAhUfscX0AAhBow5/GrNf4DqSPOH7R8GrD3uh8bSsb+aadPgW7TnLUYuiE5pP7roF0ZqMFPXh7MuUXXrfuJiSOeRDxoGOHcD4Wsdvchkij88M6TYLr/VNrOHxIQJKi8RjSNmcwQKBgANKxkqb0nVAM2BZycOKI+ClB/1vfizndTwd3hc/R9dMNwjeMWGSu+O0IZDYgJNu7GsEMhexH6vl1MuKUYUUSHpd1dJ6Zto5tIJrI0pYsUX45AwPT3xDl8EgV/xUYpJP/KRDLwB+GHferxENqVpKX9bKw75k5jBh0G5rOgQZpxBxAoGATTXDJ02LLE+cjJEF814V7AnMaH6/xMA9DJ/fNKaajyIviDTbB2+0ZM7bWoLdCuQ/k4K+tlMpT7foY6GE+sZHHKcdUe3llWVKR4kpGKrreP8/BZPTL3NjBMngvn68T5cq453/f7IaVDcXJyAJV/sM5wDqWfaY5KuUL1YTTallxzk=";
            string ALIPAY_PUBLIC_KEY = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA05jrPUPy0yG1dps1+71F/N/4FjqFC7gJBBtRWIykm5WR5XwbNmWb0dhm29i4+bQINbilKa/qFuT65TgQuJ1nCfhtqCySSJ3MsdBallZLn3H+eXb+krHVELpAlbotAcCQ2ZFqzNuavL9bhlvGLeSCH1SJMC8zgZXp9ZW/0aFQZd+riZHXLhO8msWoQcRqN7f/YR9C2oha/vYpY2qym7ZslJsNHVyET2ZB8XLlvBDJy1G6GMXQLfHwUl7CWYzXwJG/5pDxoEbwDmnXxLqQkUX5xUKnPzGrNQb9F3tq2R8/C5yMtCUJssKmE46hYan4jr+B0qP8S63LuATjmqwqe7/wMQIDAQAB";
            IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", "2021002118612482", APP_PRIVATE_KEY, "json", "1.0", "RSA2", ALIPAY_PUBLIC_KEY, "UTF-8", false);
            AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();
            request.BizContent = "{" +
          "\"out_trade_no\":\"20150320010101010\"," +
          "\"total_amount\":\"0.01\"," +
          "\"product_code\":\"FAST_INSTANT_TRADE_PAY\"," +
          "\"subject\":\"simcard\"," +
          "\"body\":\"订单描述\"" +
          "  }";
            AlipayTradePagePayResponse response = client.pageExecute(request, null, "post");
            
            return response;
        }
        #endregion

        #region 在线续费
        ///<summary>
        ///查看客户的套餐列表
        /// </summary>
        //public SetMealDto GetCustomSetMealDto1(string Company_ID)
        //{
        //    SetMealDto dto = new SetMealDto();
        //    try
        //    {
        //        List<SetMealDtos> Setmealdto = new List<SetMealDtos>();
        //        string sql = "select t2.SetMealID, t2.PackageDescribe as SetmealName  from card_copy1 t1 left join setmeal t2 on t1.SetMealID2 = t2.SetmealID where Card_CompanyID = '"+Company_ID+"' GROUP BY t1.SetMealID2";
        //        using (IDbConnection conn = DapperService.MySqlConnection())
        //        {
        //            dto.Setmealdto = conn.Query<SetMealDtos>(sql).ToList();
        //            dto.flg = "1";
        //            dto.Msg = "成功!";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        dto.Setmealdto = null;
        //        dto.flg = "-1";
        //        dto.Msg = "出错:"+ex;
        //    }
        //    return dto;
        //}

        ///<summary>
        ///查看客户的套餐列表（新）
        /// </summary>
        public CustomerSetMealDto GetCustomSetMealDto(string Company_ID)
        {
            CustomerSetMealDto dto = new CustomerSetMealDto();
            List<CustomOperatorSetMeal> setmealinfo = new List<CustomOperatorSetMeal>();
            try
            {
                //移动
                string cmccsql = "select t2.SetMealID, t2.PackageDescribe as SetmealName  from card_copy1 t1 left join setmeal t2 on t1.SetMealID2 = t2.SetmealID where Card_CompanyID = '"+Company_ID+"' GROUP BY t1.SetMealID2";
                //电信
                string ctsql = "select t2.SetMealID, t2.PackageDescribe as SetmealName  from ct_cardcopy t1 left join setmeal t2 on t1.SetMealID2 = t2.SetmealID where Card_CompanyID = '" + Company_ID + "' GROUP BY t1.SetMealID2";
                //联通
                string cuccsql = "select t2.SetMealID, t2.PackageDescribe as SetmealName  from cucc_cardcopy t1 left join setmeal t2 on t1.SetMealID2 = t2.SetmealID where Card_CompanyID = '" + Company_ID + "' GROUP BY t1.SetMealID2";
                //全网通
                string threesql = "select t2.SetMealID, t2.PackageDescribe as SetmealName  from three_cardcopy t1 left join setmeal t2 on t1.SetMealID2 = t2.SetmealID where Card_CompanyID = '" + Company_ID + "' GROUP BY t1.SetMealID2";
                //漫游
                string roamsql = "select t2.SetMealID, t2.PackageDescribe as SetmealName  from roamcard_copy t1 left join setmeal t2 on t1.SetMealID2 = t2.SetmealID where Card_CompanyID = '" + Company_ID + "' GROUP BY t1.SetMealID2";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    var listcmcc = conn.Query<SetMealDtos>(cmccsql).ToList();
                    var listct = conn.Query<SetMealDtos>(ctsql).ToList();
                    var listcucc = conn.Query<SetMealDtos>(cuccsql).ToList();
                    var listthree = conn.Query<SetMealDtos>(threesql).ToList();
                    var listroam = conn.Query<SetMealDtos>(roamsql).ToList();
                    if (listcmcc.Count>0)
                    {
                        CustomOperatorSetMeal setMeal = new CustomOperatorSetMeal();
                        List<CustomSetMeal> children = new List<CustomSetMeal>();
                        setMeal.value = "1";
                        setMeal.label = "中国移动";
                        foreach (var item in listcmcc)
                        {
                            CustomSetMeal customsetmeal = new CustomSetMeal();
                            customsetmeal.value = item.SetMealID;
                            customsetmeal.label = item.SetMealName;
                            children.Add(customsetmeal);
                            setMeal.children = children;
                        }
                        setmealinfo.Add(setMeal);
                    }
                    if (listct.Count>0)
                    {
                        CustomOperatorSetMeal setMeal = new CustomOperatorSetMeal();
                        List<CustomSetMeal> children = new List<CustomSetMeal>();
                        setMeal.value = "2";
                        setMeal.label = "中国电信";
                        foreach (var item in listct)
                        {
                            CustomSetMeal customsetmeal = new CustomSetMeal();
                            customsetmeal.value = item.SetMealID;
                            customsetmeal.label = item.SetMealName;
                            children.Add(customsetmeal);
                            setMeal.children = children;
                        }
                        setmealinfo.Add(setMeal);
                    }
                    if (listcucc.Count>0)
                    {
                        CustomOperatorSetMeal setMeal = new CustomOperatorSetMeal();
                        List<CustomSetMeal> children = new List<CustomSetMeal>();
                        setMeal.value = "3";
                        setMeal.label = "中国联通";
                        foreach (var item in listcucc)
                        {
                            CustomSetMeal customsetmeal = new CustomSetMeal();
                            customsetmeal.value = item.SetMealID;
                            customsetmeal.label = item.SetMealName;
                            children.Add(customsetmeal);
                            setMeal.children = children;
                        }
                        setmealinfo.Add(setMeal);
                    }
                    if (listthree.Count>0)
                    {
                        CustomOperatorSetMeal setMeal = new CustomOperatorSetMeal();
                        List<CustomSetMeal> children = new List<CustomSetMeal>();
                        setMeal.value = "4";
                        setMeal.label = "全网通";
                        foreach (var item in listthree)
                        {
                            CustomSetMeal customsetmeal = new CustomSetMeal();
                            customsetmeal.value = item.SetMealID;
                            customsetmeal.label = item.SetMealName;
                            children.Add(customsetmeal);
                            setMeal.children = children;
                        }
                        setmealinfo.Add(setMeal);
                    }
                    if (listroam.Count>0)
                    {
                        CustomOperatorSetMeal setMeal = new CustomOperatorSetMeal();
                        List<CustomSetMeal> children = new List<CustomSetMeal>();
                        setMeal.value = "5";
                        setMeal.label = "漫游";
                        foreach (var item in listroam)
                        {
                            CustomSetMeal customsetmeal = new CustomSetMeal();
                            customsetmeal.value = item.SetMealID;
                            customsetmeal.label = item.SetMealName;
                            children.Add(customsetmeal);
                            setMeal.children = children;
                        }
                        setmealinfo.Add(setMeal);
                    }
                    dto.setmealinfo = setmealinfo;
                    dto.flg = "1";
                    dto.Msg = "成功!";
                }
            }
            catch (Exception ex)
            {
                dto.flg = "-1";
                dto.Msg = "出错:"+ex;
            }
            return dto;
        }


        ///<summary>
        ///设置套餐价格和用户
        /// </summary>
        public Information SetUpapackageInfo(SetUpapackagePara para)
         {
            Information info = new Information();
            try
            {
                if (para.setflg == "1")//添加
                {
                    string addinfo = "insert into setupapackage(SetmealID,Price,Company_ID,CustomerCompanyID,AddTime) values('" + para.SetmealID + "'," + para.Price + ",'" + para.Company_ID + "','" + para.CustomerCompanyID + "','" + DateTime.Now + "')";
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        conn.Execute(addinfo);
                        info.flg = "1";
                        info.Msg = "添加成功";
                    }
                }
                if (para.setflg == "2")//编辑
                {
                    string updateinfo = "update  setupapackage set Price="+para.Price+" where id="+para.id+"";
                    using (IDbConnection conn = DapperService.MySqlConnection())
                    {
                        conn.Execute(updateinfo);
                        info.flg = "1";
                        info.Msg = "编辑成功";
                    }
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "失败:"+ex;
            }
            return info;
        }

        ///<summary>
        ///查看用户的套餐信息列表
        /// </summary>
        public GetSetUpapackageDto GetSetUpapackageInfo(string CompanyID)
        {
            GetSetUpapackageDto dto = new GetSetUpapackageDto();
            try
            {
                string sql = "SELECT t1.id,t1.SetmealID,t1.Price,t2.PackageDescribe as SetmealName,t2.OperatorID FROM `setupapackage` t1 left join setmeal t2 on t1.SetmealID=t2.SetmealID where CustomerCompanyID='" + CompanyID+"'";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    List<SetUpapackageDto> dtos = new List<SetUpapackageDto>();
                    var lists = conn.Query<SetUpapackageDto>(sql).ToList();
                    foreach (var item in lists)
                    {
                        SetUpapackageDto set = new SetUpapackageDto();
                        set.id = item.id;
                        set.Price = item.Price;
                        set.SetmealID = item.SetmealID;
                        set.SetmealName = item.SetmealName;
                        if (item.OperatorID == "1573631210918")
                        {
                            item.OperatorID = "1";
                        }
                        if (item.OperatorID == "1573631225967")
                        {
                            item.OperatorID = "2";
                        }
                        if (item.OperatorID == "1573631234734")
                        {
                            item.OperatorID = "3";
                        }
                        if (item.OperatorID == "1594176287219")
                        {
                            item.OperatorID = "4";
                        }
                        if (item.OperatorID == "1594176308883")
                        {
                            item.OperatorID = "5";
                        }
                        set.OperatorID = item.OperatorID;
                        dtos.Add(set);
                    }
                    dto.Dtos = dtos;//conn.Query<SetUpapackageDto>(sql).ToList();
                    dto.flg = "1";
                    dto.Msg = "成功";
                }
            }
            catch (Exception ex)
            {
                dto.Dtos = null;
                dto.flg = "-1";
                dto.Msg = "失败:"+ex;
            }
            return dto;
        }
        #endregion
    }
}