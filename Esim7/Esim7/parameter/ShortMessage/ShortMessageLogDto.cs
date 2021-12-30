using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.parameter.ShortMessage
{
    /// <summary>
    /// 短信发送日志信息
    /// </summary>
    public class ShortMessageLogDto: Information
    {
         public List<shortmessagelogdto> shortmessagelogs { get; set; }
    }
    public class shortmessagelogdto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string SendStatus { get; set; }
        public DateTime SendTime { get; set; }
        public string Company_ID { get; set; }
        public string loginname { get; set; }
        public string TaskName { get; set; }
        public string Card_ID { get; set; }
        public List<CardNum> cardNums { get; set; }
    }
    public class CardNum
    {
        public string Card_ID { get; set; }
    }
}