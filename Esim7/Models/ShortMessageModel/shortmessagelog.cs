using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models.ShortMessageModel
{
    /// <summary>
    /// 短信发送日志
    /// </summary>
    public class shortmessagelog
    {
        public int Id { get; set; }
        public string Card_ID { get; set; }
        public string Content { get; set; }
        public string SendStatus { get; set; }
        public string Company_ID { get; set; }
        public DateTime SendTime { get; set; }
        public string TaskName { get; set; }
    }
    ///<summary>
    ///短信回复信息
    /// </summary>
    public class GetShortMessageDto : Information
    {
        public List<tb_e_message> shortMessageInfos { get; set; }
    }
    public class ShortMessageInfo
    {
        public string CardID { get; set; }
        public string content { get; set; }
        public string addTime { get; set; }
    }

    ///<summary>
    ///获取短信回复信息入参
    /// </summary>
    public class ShortMessagePara
    {
        public string Card_ID { get; set; }
        public string Company_ID { get; set; }
    }

    ///<summary>
    ///短信回复记录接受类
    /// </summary>
    public class GetMessageDto
    {
        public List<tb_e_message> messages { get; set; }
        public string flg { get; set; }
        public string Msg { get; set; }
    }

    public class tb_e_message
    {
        public int MessageID { get; set; }
        public string CardID { get; set; }
        public string content { get; set; }
        /// <summary>
        /// status 发送状态，0-未发送，1-已提交，2-发送失败
        /// </summary>
        public string status { get; set; }
        public DateTime addTime { get; set; }
    }


    ///<summary>
    ///获取短信信息
    /// </summary>
    public class GetShortMessageDetailDto : Information
    {
        public List<ShortMessageDetail> messageDetails { get; set; }
    }

    public class ShortMessageDetail
    {
        public string CardID { get; set; }
        ///<summary>
        ///任务名称
        /// </summary>
        public string TaskName { get; set; }
        ///<summary>
        ///提交时间
        /// </summary>
        public DateTime SendTime { get; set; }
        ///<summary>
        ///提交用户
        /// </summary>
        public string loginname { get; set; }
        /// <summary>
        /// 发送的信息
        /// </summary>
        public List<shortmessagelog> sendmessage { get; set; }
        /// <summary>
        /// 接收的信息
        /// </summary>
        public List<tb_e_message> getmessage { get; set; }
    }


    ///<summary>
    ///获取短信信息
    /// </summary>
    public class getShortMessageInfos:Information
    {
        public List<ShortInfo> shortInfos { get; set; }
    }

    public class ShortInfo
    {
        public string Card_ID { get; set; }
        public string Content { get; set; }
        public string TaskName { get; set; }
        public string SendStatus { get; set; }
        public DateTime Time { get; set; }
        /// <summary>
        /// 0发送 1接受
        /// </summary>
        public int ShortType { get; set; }
        public string loginname { get; set; }
    }

    ///<summary>
    ///获取短信信息入参
    /// </summary>
    public class GetShotrPara
    {
        public string Card_ID { get; set; }
        public string[] Times { get; set; }
        /// <summary>
        /// 0发送的短信 1回复的短信
        /// </summary>
        public string ShortType { get; set; }
        public string Company_ID { get; set; }
    }

}