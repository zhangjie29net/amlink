using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.parameter.ShortMessage
{
    /// <summary>
    /// 添加短信模板入参
    /// </summary>
    public class CreateTemplatePara
    {
        public string TemplateName { get; set; }
        public string TemplateContent { get; set; }
        public string Remark { get; set; }
        public string Company_ID { get; set; }
    }

    ///<summary>
    ///编辑短信模板
    /// </summary>
    public class EditTemplatePara:CreateTemplatePara
    {
        public int Id { get; set; }
    }

    public class SendoutMessagePara
    {
        public string Value { get; set; }//单个卡号
        public string Message { get; set; }//信息内容
        public string TemplateNum { get; set; }//模板编码
        public string Company_ID { get; set; }//公司编码
        public string TaskName { get; set; }//任务名称
        public List<Card_IDS> cards { get; set; }//多个卡号
    }

    public class Card_IDS
    {
        public string Card_ID { get; set; }
    }
}