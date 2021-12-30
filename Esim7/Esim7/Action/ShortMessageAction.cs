using CMPP;
using Dapper;
using Esim7.Models;
using Esim7.Models.ShortMessageModel;
using Esim7.parameter.ShortMessage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;

namespace Esim7.Action
{
    /// <summary>
    /// 短信管理
    /// </summary>
    public class ShortMessageAction
    {
        ///<summary>
        ///添加模板
        /// </summary>
        public Information CreateTemplate(CreateTemplatePara para)
        {
            int i = 0;
            string sqlnum = "select count(*) as id from shortmsgtemplate where Company_ID='"+para.Company_ID+"'";
            using (IDbConnection CONN = DapperService.MySqlConnection())
            {
                i = CONN.Query<shortmsgtemplate>(sqlnum).Select(t => t.Id).FirstOrDefault();
                i += 1;
            }
            string TemplateNum = i.ToString("d3");
            Information info = new Information();
            DateTime time = DateTime.Now;
            string sql = "insert into shortmsgtemplate(TemplateName,TemplateContent,Remark,AddTime,Company_ID,TemplateNum)" +
                         "values('"+para.TemplateName+"','"+para.TemplateContent+"','"+para.Remark+"','"+time+"','"+para.Company_ID+"','"+TemplateNum+ "')";
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    conn.Execute(sql);
                    info.flg = "1";
                    info.Msg = "创建模板成功!";
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "创建模板失败:"+ex;
            }
            return info;
        }

        ///<summary>
        ///修改模板信息
        /// </summary>
        public Information EditTemplate(EditTemplatePara para)
        {
            Information info = new Information();
            string sql = "update shortmsgtemplate set TemplateName='"+para.TemplateName+ "',TemplateContent='"+para.TemplateContent+ "'," +
                         "Remark='"+para.Remark+"' where Id="+para.Id+"";
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    conn.Execute(sql);
                    info.flg = "1";
                    info.Msg = "修改模板信息成功!";
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "修改模板信息失败:"+ex;
            }
            return info;
        }

        ///<summary>
        ///查看模板信息
        /// </summary>
        public ShortMessageDto GetTemplateInfo(string Company_ID)
        {
            
            ShortMessageDto info = new ShortMessageDto();
            try
            {
                string sql = "select * from shortmsgtemplate where Company_ID='"+Company_ID+ "' and IsDelete=1";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    info.ShortMessage = conn.Query<shortmsgtemplate>(sql).OrderBy(t=>t.Id).ToList();
                    info.flg = "1";
                    info.Msg = "查询成功!";
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "查询失败:"+ex;
            }
            return info;
        }

        ///<summary>
        ///删除模板信息
        /// </summary>
        public Information DeleteTemplateInfo(int Id)
        {
            Information info = new Information();
            try
            {
                string sql = "update  shortmsgtemplate set IsDelete=0 where Id=" + Id;
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    conn.Execute(sql);
                    info.flg = "1";
                    info.Msg = "删除成功!";
                }
            }
            catch (Exception ex)
            {
                info.flg = "-1";
                info.Msg = "删除失败:"+ex;
            }
            return info;
        }

        ///<summary>
        ///发送短信
        /// </summary>
        public Information SendoutMessage(SendoutMessagePara para)
        {
            string tempcontent = string.Empty;
            string content = string.Empty;
            Information info = new Information();
            DateTime time = DateTime.Now;
            ShortMessgeDto sm = new ShortMessgeDto();
            string URL = string.Empty;
            string accountid = string.Empty;//判断运营商
            StringBuilder strinsertlog = new StringBuilder("insert into shortmessagelog(Card_ID,Content,SendStatus,SendTime,Company_ID,TaskName) values");
            string sqltempcontent = "select TemplateContent from shortmsgtemplate where TemplateNum='" + para.TemplateNum + "' and Company_ID='" + para.Company_ID + "'";
            if (para.cards.Count() > 0)
            {
                foreach (var item in para.cards)
                {
                    try
                    {
                        using (IDbConnection conn = DapperService.MySqlConnection())
                        {
                            tempcontent = conn.Query<shortmsgtemplate>(sqltempcontent).Select(t => t.TemplateContent).FirstOrDefault();
                            if (!string.IsNullOrWhiteSpace(tempcontent))
                            {
                                content = tempcontent + para.Message;
                            }
                            else
                            {
                                content = para.Message;
                            }
                            string sqlaccount = "select accountsID from card where Card_ID='" + para.cards[0].Card_ID + "'";
                            accountid = conn.Query<Card>(sqlaccount).Select(t => t.accountsID).FirstOrDefault();
                            if (accountid == "1568967912431")//惠州移动新平台
                            {
                                URL = "http://101.200.35.208:9101/sms/sendSms/card/send?";
                            }
                            if (accountid == "1574151885283")//西安移动新平台
                            {
                                URL = "http://101.200.35.208:9102/sms/sendSms/card/send?";
                            }
                            URL = URL + "msisdn=" + item.Card_ID + "&content=" + content;
                            Encoding encoding = Encoding.UTF8;
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                            request.Method = "GET";
                            request.Accept = "text/html, application/xhtml+xml, */*";
                            request.ContentType = "application/json";
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                            {
                                sm = JsonConvert.DeserializeObject<ShortMessgeDto>(reader.ReadToEnd());
                                info.flg = "1";
                                info.Msg = "发送成功!";
                                string sqllog = ("insert into shortmessagelog(Card_ID,Content,SendStatus,SendTime,Company_ID,TaskName) values ('" + item.Card_ID + "','" + content + "','" + 1 + "','" + time + "','" + para.Company_ID + "','" + para.TaskName + "')");
                                conn.Execute(sqllog);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        info.flg = "-1";
                        info.Msg = "发送失败:"+ex;
                    } 
                }
            }
            else
            {
                if (para.Value.Length != 13)
                {
                    info.flg = "-1";
                    info.Msg = "只能输入13位卡号下发短信!";
                    return info;
                }
                else
                {
                    try
                    {
                        using (IDbConnection conn = DapperService.MySqlConnection())
                        {
                            tempcontent = conn.Query<shortmsgtemplate>(sqltempcontent).Select(t => t.TemplateContent).FirstOrDefault();
                            if (!string.IsNullOrWhiteSpace(tempcontent))
                            {
                                content = tempcontent + para.Message;
                            }
                            else
                            {
                                content = para.Message;
                            }
                            string sqlaccount = "select accountsID from card where Card_ID='" + para.Value + "'";
                            accountid = conn.Query<Card>(sqlaccount).Select(t=>t.accountsID).FirstOrDefault();
                            if (accountid == "1568967912431")//惠州移动新平台
                            {
                                URL = "http://101.200.35.208:9101/sms/sendSms/card/send?";
                            }
                            if (accountid == "1574151885283")//西安移动新平台
                            {
                                URL = "http://101.200.35.208:9102/sms/sendSms/card/send?";
                            }
                            URL = URL + "msisdn=" + para.Value + "&content=" +content;
                            Encoding encoding = Encoding.UTF8;
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                            request.Method = "GET";
                            request.Accept = "text/html, application/xhtml+xml, */*";
                            request.ContentType = "application/json";
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                            {
                                sm = JsonConvert.DeserializeObject<ShortMessgeDto>(reader.ReadToEnd());
                                info.flg = "1";
                                info.Msg = "发送成功!";
                                content = tempcontent + para.Message;
                                strinsertlog.Append("('" + para.Value + "','" + content + "','" + 1 + "','" + time + "','" + para.Company_ID + "','" + para.TaskName + "'),");
                                string sqllog = strinsertlog.ToString().Substring(0, strinsertlog.ToString().Length - 1);
                                conn.Execute(sqllog);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        info.flg = "-1";
                        info.Msg = "下发短信失败:" + ex;
                    }
                }
            }
            return info;
        }

        ///<summary>
        ///查看下行短信发送记录
        /// </summary>
        public CMIOT_API25C02 GetShotMessage(string Value)
        {
            CMIOT_API25C02 s = new CMIOT_API25C02();
            string Token = string.Empty;
            string sql = "select Card_ID,Platform,accountsID from card where Card_ID='" + Value + "'";
            string sqlaccount = "select * from accounts";
            string APPID = string.Empty;
            string Platform = string.Empty;
            string AccountID = string.Empty;
            string transid = string.Empty;
            string startDate = "20200520";
            string endDate = "20200527";
            try
            {
                using (IDbConnection Conn = DapperService.MySqlConnection())
                {
                    Platform = Conn.Query<Card>(sql).Select(t => t.Platform).FirstOrDefault();
                    AccountID = Conn.Query<Card>(sql).Select(t => t.accountsID).FirstOrDefault();
                    if (Platform == "10")
                    {

                    }
                    if (Platform == "11")
                    {
                        APPID = Conn.Query<accounts>(sqlaccount).Where(t => t.accountID == AccountID).Select(t => t.APPID).FirstOrDefault();
                        transid = APPID + "2019071002415709643582";
                        //获取Token
                        Token = Action.Sim_Action.Token(Value);
                        string URL = @"https://api.iot.10086.cn/v5/ec/query/sim-mt-sms/batch?";
                    //https://api.iot.10086.cn/v5/ec/query/sim-mt-sms/batch?transid=xxx&token=xxx&msisdns=xxxx_xxxx_xxxx&startDate=yyyymmdd&endDate=yyyymmdd-以msisdns查询
                    //https://api.iot.10086.cn/v5/ec/query/sim-mt-sms/batch?transid=xxx&token=xxx&iccids=xxxx_xxxx_xxxx&startDate=yyyymmdd&endDate=yyyymmdd-以iccids查询
                    //https://api.iot.10086.cn/v5/ec/query/sim-mt-sms/batch?transid=xxx&token=xxx&imsis=xxxx_xxxx_xxxx&startDate=yyyymmdd&endDate=yyyymmdd-以imsis查询
                    //string SS = url + "transid=" + Transid + "&token=" + TOKEN + "&msisdn=" + MSISDN;
                        string ss = URL + "transid=" + transid + "&token=" + Token + "&msisdns=" + Value + "&startDate=" + startDate + "&endDate="+endDate;
                        Encoding encoding = Encoding.UTF8;
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ss);
                        request.Method = "GET";
                        request.Accept = "text/html, application/xhtml+xml, */*";
                        request.ContentType = "application/json";
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            s = JsonConvert.DeserializeObject<CMIOT_API25C02>(reader.ReadToEnd());
                            if (s.result.Count > 0)
                            {
                                //t.result[0].status
                                //info.flg = "1";
                                //info.Msg = "成功发送" + cm.result[0].succSvcNum + "条" + "失败" + cm.result[0].failSvcNum + "条";
                            }
                            //if (cm.result.Count == 0)
                            //{
                            //    info.flg = "1";
                            //    info.Msg = "发送短信失败" + cm.result[0].failSvcDetail[0].message;
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            //https://api.iot.10086.cn/v5/ec/query/sim-mt-sms/batch?transid=xxx&token=xxx&msisdns=xxxx_xxxx_xxxx&startDate=yyyymmdd&endDate=yyyymmdd -以msisdns查询
            return s;
        }
        /// <summary>
        /// 获取用户短信发送日志
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public ShortMessageLogDto GetShortMessageLog(GetShortMessagePara para)
        {
            ShortMessageLogDto logDto = new ShortMessageLogDto();
            List<shortmessagelogdto> shortmessagelogs = new List<shortmessagelogdto>();
            string sqllog = "select * from shortmessagelog where Company_ID='" + para.Company_ID + "'";
            string sqloginname = "select LoginName as loginname from cf_user where Company_ID='" + para.Company_ID + "'";
            string UserName = "";
            try
            {
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    UserName = conn.Query<User>(sqloginname).Select(t => t.loginname).FirstOrDefault();
                    var list = conn.Query<shortmessagelogdto>(sqllog).ToList();
                    if (!string.IsNullOrWhiteSpace(para.TaskName))
                    {
                        list = conn.Query<shortmessagelogdto>(sqllog).Where(t=>t.TaskName.Contains(para.TaskName)).ToList();
                    }
                    if (para.StatrEnterTime.Length>0)
                    {
                        string statrtime = para.StatrEnterTime[0];
                        string endtime = para.StatrEnterTime[1];
                        if (statrtime.Length > 10)
                        {
                            statrtime = statrtime.Substring(0, 10);
                        }
                        if (endtime.Length > 10)
                        {
                            endtime = endtime.Substring(0, 10);
                        }
                        DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                        long lTime = long.Parse(statrtime + "0000000");
                        TimeSpan toNow = new TimeSpan(lTime);
                        long ETime = long.Parse(endtime + "0000000");
                        TimeSpan etoNow = new TimeSpan(ETime);
                        DateTime StatrTime = dateTimeStart.Add(toNow);
                        DateTime EndTime = dateTimeStart.Add(etoNow);
                        list = list.Where(t => t.SendTime > StatrTime && t.SendTime <= EndTime.AddDays(1)).ToList();
                        //list = list.Where(t => t.SendTime > Convert.ToDateTime(para.StatrEnterTime[0]) && t.SendTime <= Convert.ToDateTime(para.StatrEnterTime[1]).AddDays(1)).ToList();
                    }
                    var Glist = list.GroupBy(t => t.SendTime).ToList();
                    int Id = 0;
                    foreach (var item in Glist)
                    {
                        Id += 1;
                        shortmessagelogdto dto = new shortmessagelogdto();
                        dto.SendTime = item.Key;
                        dto.loginname = UserName;
                        dto.Company_ID = para.Company_ID;
                        dto.Id = Id;
                        string sqlinfo = "select * from shortmessagelog where SendTime='" + item.Key + "'";

                        var listlog = conn.Query<shortmessagelog>(sqlinfo).FirstOrDefault();
                        if (listlog != null)
                        {
                            dto.Content = listlog.Content;
                            dto.SendStatus = listlog.SendStatus;
                            dto.TaskName = listlog.TaskName;
                        }
                        List<CardNum> cardNums = new List<CardNum>();
                        foreach (var items in item)
                        {
                            CardNum num = new CardNum();
                            num.Card_ID = items.Card_ID;
                            cardNums.Add(num);
                        }
                        dto.cardNums = cardNums;
                        shortmessagelogs.Add(dto);  
                    }
                    logDto.shortmessagelogs = shortmessagelogs;
                    logDto.flg = "1";
                    logDto.Msg = "查询成功!";
                }
            }
            catch (Exception ex)
            {
                logDto.flg = "-1";
                logDto.Msg = "查询失败:" + ex;
            }
            return logDto;
        }

        public CMIOT_API25C03 upshortmessageinfo(string Value, string startDate,string endDate)
        {
            CMIOT_API25C03 r = new CMIOT_API25C03();
            string Token = string.Empty;
            string sql = "select Card_ID,Platform,accountsID from card where Card_ID='" + Value + "'";
            string sqlaccount = "select * from accounts";
            string APPID = string.Empty;
            string Platform = string.Empty;
            string AccountID = string.Empty;
            string transid = string.Empty;
           
            try
            {
                using (IDbConnection Conn = DapperService.MySqlConnection())
                {
                    Platform = Conn.Query<Card>(sql).Select(t => t.Platform).FirstOrDefault();
                    AccountID = Conn.Query<Card>(sql).Select(t => t.accountsID).FirstOrDefault();
                    if (Platform == "10")
                    {

                    }
                    if (Platform == "11")
                    {
                        APPID = Conn.Query<accounts>(sqlaccount).Where(t => t.accountID == AccountID).Select(t => t.APPID).FirstOrDefault();
                        transid = APPID + "2019071002415709643582";
                        //获取Token
                        Token = Action.Sim_Action.Token(Value);
                        string URL = @"https://api.iot.10086.cn/v5/ec/query/sim-mo-sms/batch?";
                        //https://api.iot.10086.cn/v5/ec/query/sim-mt-sms/batch?transid=xxx&token=xxx&msisdns=xxxx_xxxx_xxxx&startDate=yyyymmdd&endDate=yyyymmdd-以msisdns查询
                        //https://api.iot.10086.cn/v5/ec/query/sim-mt-sms/batch?transid=xxx&token=xxx&iccids=xxxx_xxxx_xxxx&startDate=yyyymmdd&endDate=yyyymmdd-以iccids查询
                        //https://api.iot.10086.cn/v5/ec/query/sim-mt-sms/batch?transid=xxx&token=xxx&imsis=xxxx_xxxx_xxxx&startDate=yyyymmdd&endDate=yyyymmdd-以imsis查询
                        //string SS = url + "transid=" + Transid + "&token=" + TOKEN + "&msisdn=" + MSISDN;
                        string ss = URL + "transid=" + transid + "&token=" + Token + "&msisdns=" + Value + "&startDate=" + startDate + "&endDate=" + endDate;
                        Encoding encoding = Encoding.UTF8;
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ss);
                        request.Method = "GET";
                        request.Accept = "text/html, application/xhtml+xml, */*";
                        request.ContentType = "application/json";
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            r = JsonConvert.DeserializeObject<CMIOT_API25C03>(reader.ReadToEnd());
                            if (r.result.Count > 0)
                            {
                                //t.result[0].status
                                //info.flg = "1";
                                //info.Msg = "成功发送" + cm.result[0].succSvcNum + "条" + "失败" + cm.result[0].failSvcNum + "条";
                            }
                            //if (cm.result.Count == 0)
                            //{
                            //    info.flg = "1";
                            //    info.Msg = "发送短信失败" + cm.result[0].failSvcDetail[0].message;
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return r;
        }
        public string SendShortMessge(string CardNum,string Content)
        {
            string t = string.Empty;
            string url = "http://101.200.35.208:9101/sms/sendSms/card/send?";
            url = url + "msisdn="+CardNum+ "&content="+Content;
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                t = reader.ReadToEnd();
            }
            return t;
        }

        ///<summary>
        ///获取回复短信信息
        /// </summary>
        public GetShortMessageDto GetShortMessageInfo(ShortMessagePara para)
        {
            GetShortMessageDto dto = new GetShortMessageDto();
            try
            {
                GetMessageDto r = new GetMessageDto();
                 string url = "http://101.200.35.208:9103/GetMessageInfo";
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "get";
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.ContentType = "application/json";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    r = JsonConvert.DeserializeObject<GetMessageDto>(reader.ReadToEnd());
                    if (r.messages.Count > 0)
                    {
                        r.messages = r.messages.Where(t => t.CardID == "1440230257208").ToList();
                       dto.shortMessageInfos=r.messages;
                        dto.flg = "1";
                        dto.Msg = "成功";
                    }
                }     
            }
            catch (Exception ex)
            {
                dto.shortMessageInfos = null;
                dto.flg = "-1";
                dto.Msg = "失败:"+ex;
            }
            return dto;
        }

        ///<summary>
        ///获取短信详细信息
        /// </summary>
        public getShortMessageInfos GetShortMeaages(GetShotrPara para)
        {
            getShortMessageInfos infos = new getShortMessageInfos();
            List<ShortInfo> shortInfos = new List<ShortInfo>();
            GetMessageDto r = new GetMessageDto();
            List<tb_e_message> messages = new List<tb_e_message>();
            string url = "http://101.200.35.208:9103/GetMessageInfo";
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "get";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            try
            {
                string sql = "select * from shortmessagelog where Company_ID='"+para.Company_ID+ "'ORDER BY SendTime DESC ";
                string sqlcard = "select DISTINCT Card_ID  from shortmessagelog where Company_ID='" + para.Company_ID + "'";
                string usersql = "select LoginName as loginname  from cf_user where Company_ID='" + para.Company_ID + "' and status=1";
                string loginname = string.Empty;
                using (IDbConnection CONN = DapperService.MySqlConnection())
                {
                    var listsend = CONN.Query<shortmessagelog>(sql).ToList();//发送
                    var getcard = CONN.Query<ShortInfo>(sqlcard).ToList();//接收
                    loginname = CONN.Query<User>(usersql).Select(t => t.loginname).FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(para.Card_ID))
                    {
                        listsend = listsend.Where(t => t.Card_ID == para.Card_ID).ToList();
                        messages = messages.Where(t => t.CardID == para.Card_ID).ToList();
                        foreach (var items in messages)
                        {
                            ShortInfo info = new ShortInfo();
                            info.Card_ID = items.CardID;
                            info.Content = items.content;
                            info.loginname = loginname;
                            info.SendStatus = "1";
                            info.ShortType = 1;
                            info.Time = items.addTime;
                            shortInfos.Add(info);
                        }
                    }
                    if (para.ShortType == "0")//发送的短信
                    {
                        getcard = null;
                        if (para.Times.Length != 0 && para.Times != null)
                        {
                            string statrtime = para.Times[0];
                            string endtime = para.Times[1];
                            if (statrtime.Length > 10)
                            {
                                statrtime = statrtime.Substring(0, 10);
                            }
                            if (endtime.Length > 10)
                            {
                                endtime = endtime.Substring(0, 10);
                            }
                            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                            long lTime = long.Parse(statrtime + "0000000");
                            TimeSpan toNow = new TimeSpan(lTime);
                            long ETime = long.Parse(endtime + "0000000");
                            TimeSpan etoNow = new TimeSpan(ETime);
                            DateTime StatrTime = dateTimeStart.Add(toNow);
                            DateTime EndTime = dateTimeStart.Add(etoNow);
                            if (statrtime.Length > 10)
                            {
                                statrtime = statrtime.Substring(0, 10);
                            }
                            if (endtime.Length > 10)
                            {
                                endtime = endtime.Substring(0, 10);
                            }
                            if (para.ShortType == "0")//发送的
                            {
                                listsend = listsend.Where(t => t.SendTime > StatrTime && t.SendTime <= EndTime.AddDays(1)).ToList();
                            }
                        }
                    }
                    if (para.ShortType == "1")//接收的短信
                    {
                        listsend = null;
                        using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            r = JsonConvert.DeserializeObject<GetMessageDto>(reader.ReadToEnd());
                            r.messages = r.messages;
                            if (getcard != null)
                            {
                                if (para.Times.Length == 0 || para.Times == null)
                                {
                                    foreach (var item in getcard)
                                    {
                                        messages = r.messages.ToList().Where(t => t.CardID == item.Card_ID).ToList();
                                        foreach (var items in messages)
                                        {
                                            ShortInfo info = new ShortInfo();
                                            info.Card_ID = items.CardID;
                                            info.Content = items.content;
                                            info.loginname = loginname;
                                            info.SendStatus = "1";
                                            info.ShortType = 1;
                                            info.Time = items.addTime;
                                            shortInfos.Add(info);
                                        }
                                    }
                                }
                                if (para.Times.Length != 0 && para.Times != null)
                                {
                                    string statrtime = para.Times[0];
                                    string endtime = para.Times[1];
                                    if (statrtime.Length > 10)
                                    {
                                        statrtime = statrtime.Substring(0, 10);
                                    }
                                    if (endtime.Length > 10)
                                    {
                                        endtime = endtime.Substring(0, 10);
                                    }
                                    DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                                    long lTime = long.Parse(statrtime + "0000000");
                                    TimeSpan toNow = new TimeSpan(lTime);
                                    long ETime = long.Parse(endtime + "0000000");
                                    TimeSpan etoNow = new TimeSpan(ETime);
                                    DateTime StatrTime = dateTimeStart.Add(toNow);
                                    DateTime EndTime = dateTimeStart.Add(etoNow);
                                    messages = r.messages.Where(t => t.addTime > StatrTime && t.addTime <= EndTime.AddDays(1)).ToList();
                                    foreach (var items in messages)
                                    {
                                        ShortInfo info = new ShortInfo();
                                        info.Card_ID = items.CardID;
                                        info.Content = items.content;
                                        info.loginname = loginname;
                                        info.SendStatus = "1";
                                        info.ShortType = 1;
                                        info.Time = items.addTime;
                                        shortInfos.Add(info);
                                    }
                                }
                            }
                        }
                    }
                    if (para.Times.Length != 0 && para.Times != null && string.IsNullOrWhiteSpace(para.ShortType))
                    {
                        string statrtime = para.Times[0];
                        string endtime = para.Times[1];
                        if (statrtime.Length > 10)
                        {
                            statrtime = statrtime.Substring(0, 10);
                        }
                        if (endtime.Length > 10)
                        {
                            endtime = endtime.Substring(0, 10);
                        }
                        DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                        long lTime = long.Parse(statrtime + "0000000");
                        TimeSpan toNow = new TimeSpan(lTime);
                        long ETime = long.Parse(endtime + "0000000");
                        TimeSpan etoNow = new TimeSpan(ETime);
                        DateTime StatrTime = dateTimeStart.Add(toNow);
                        DateTime EndTime = dateTimeStart.Add(etoNow);
                        listsend = listsend.Where(t => t.SendTime > StatrTime && t.SendTime <= EndTime.AddDays(1)).ToList();//发送
                        using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            r = JsonConvert.DeserializeObject<GetMessageDto>(reader.ReadToEnd());
                            r.messages = r.messages;
                            if (getcard != null)
                            {
                                messages = r.messages.Where(t => t.addTime > StatrTime && t.addTime <= EndTime.AddDays(1)).ToList();
                                foreach (var items in messages)
                                {
                                    ShortInfo info = new ShortInfo();
                                    info.Card_ID = items.CardID;
                                    info.Content = items.content;
                                    info.loginname = loginname;
                                    info.SendStatus = "1";
                                    info.ShortType = 1;
                                    info.Time = items.addTime;
                                    shortInfos.Add(info);
                                }
                            }
                        }

                    }
                    if (listsend != null)
                    {
                        foreach (var item in listsend)
                        {
                            ShortInfo info = new ShortInfo();
                            info.Card_ID = item.Card_ID;
                            info.Content = item.Content;
                            info.SendStatus = item.SendStatus;
                            info.ShortType = 0;
                            info.TaskName = item.TaskName;
                            info.Time = item.SendTime;
                            info.loginname = loginname;
                            shortInfos.Add(info);
                        }
                    }
                    if (string.IsNullOrWhiteSpace(para.Card_ID) && string.IsNullOrWhiteSpace(para.ShortType) && para.Times.Length == 0 ||  para.Times == null)
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            r = JsonConvert.DeserializeObject<GetMessageDto>(reader.ReadToEnd());
                            r.messages = r.messages;
                            if (getcard!=null)
                            {
                                foreach (var item in getcard)
                                {
                                    messages = r.messages.ToList().Where(t => t.CardID == item.Card_ID).ToList();
                                    foreach (var items in messages)
                                    {
                                        ShortInfo info = new ShortInfo();
                                        info.Card_ID = items.CardID;
                                        info.Content = items.content;
                                        info.loginname = loginname;
                                        info.SendStatus = "1";
                                        info.ShortType = 1;
                                        info.Time = items.addTime;
                                        shortInfos.Add(info);
                                    }
                                }
                            }
                        }
                    }
                    infos.shortInfos = shortInfos;
                    infos.flg = "1";
                    infos.Msg = "成功!";
                }
            }
            catch (Exception ex)
            {
                infos.shortInfos = null;
                infos.flg = "-1";
                infos.Msg = "失败:"+ex;
            }
            return infos;
        }

        public GetShortMessageDetailDto GetShortMeaages(string Company_ID, string CardID)
        {
            GetShortMessageDetailDto dto = new GetShortMessageDetailDto();
            List<ShortMessageDetail> messageDetails = new List<ShortMessageDetail>();
            // 发送的信息 
            List<shortmessagelog> sendmessage = new List<shortmessagelog>();
            // 接收的信息
            List<tb_e_message> getmessage = new List<tb_e_message>();
            try
            {
                //获取登录用户的卡信息
                string sql = "select DISTINCT Card_ID from shortmessagelog where Company_ID='" + Company_ID + "'";
                string sqlusername = "select LoginName from cf_user where Company_ID='" + Company_ID + "'";
                using (IDbConnection conn = DapperService.MySqlConnection())
                {
                    string loginname = conn.Query<ShortMessageDetail>(sqlusername).Select(t => t.loginname).FirstOrDefault();
                    var cardidlist = conn.Query<ShortMessagePara>(sql).ToList();
                    foreach (var item in cardidlist)
                    {
                        ShortMessageDetail detail = new ShortMessageDetail();
                        detail.CardID = item.Card_ID;
                        detail.loginname = loginname;
                        string sendmessagesql = "select * FROM shortmessagelog where Card_ID='" + item.Card_ID + "'";
                        var sendlist = conn.Query<shortmessagelog>(sendmessagesql).ToList();
                        detail.TaskName = sendlist.Select(t => t.TaskName).FirstOrDefault();
                        detail.SendTime = sendlist.Max(t => t.SendTime);
                        foreach (var items in sendlist)
                        {
                            shortmessagelog Shortmessagelog = new shortmessagelog();
                            Shortmessagelog.Content = items.Content;

                            sendmessage.Add(Shortmessagelog);
                            detail.sendmessage = sendmessage;
                        }
                        //收到的回复信息
                        GetMessageDto r = new GetMessageDto();
                        string url = "http://101.200.35.208:9103/GetMessageInfo";
                        Encoding encoding = Encoding.UTF8;
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        request.Method = "get";
                        request.Accept = "text/html, application/xhtml+xml, */*";
                        request.ContentType = "application/json";
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            r = JsonConvert.DeserializeObject<GetMessageDto>(reader.ReadToEnd());
                            if (r.messages.Count > 0)
                            {
                                r.messages = r.messages.Where(t => t.CardID == item.Card_ID).ToList();
                                getmessage = r.messages;
                                detail.getmessage = getmessage;
                            }
                        }
                        messageDetails.Add(detail);
                    }
                    dto.messageDetails = messageDetails;
                    dto.flg = "1";
                    dto.Msg = "成功!";
                }
            }
            catch (Exception ex)
            {
                dto.flg = "-1";
                dto.Msg = "失败!";
            }
            return dto;
        }

    }
}