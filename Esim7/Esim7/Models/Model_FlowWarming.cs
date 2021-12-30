using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    public class Model_FlowWarming
    {


        public IEnumerable<object> CardMessage { get; set; }

        public string status { get; set; }
        public string Message { get; set; }


    }

    ///<summary>
    ///设置流量资费规则入参
    /// </summary>
    public class SetRulesPara
    {
        public string CompanyID { get; set; }
        public string Phone { get; set; }
        /// <summary>
        /// 设置类型 1:流量2:资费 3：机卡分离
        /// </summary>
        public string SetType { get; set; }
        /// <summary>
        /// 运营商类型 1:移动 2:电信 3:联通
        /// </summary>
        public string OperatorsType { get; set; }
        /// <summary>
        /// 提醒值 设置类型为流量时为提醒阀值  设置类型为资费时为提醒天数
        /// </summary>
        public string WaringNum { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        public List<SetRulesCardInfos> cards { get; set; }
    }

    public class SetRulesCardInfos
    {
        public string ICCID { get; set; }
        public string SN { get; set; }
    }

    ///<summary>
    ///查看设置流量信息
    /// </summary>
    public class GetSetRulesCardInfo
    {
        public string RelationCode { get; set; }
        public string WaringNum { get; set; }
    }

    ///<summary>
    ///查看设置流量资费信息列表入参
    /// </summary>
    public class SetRulesInfoPara
    {
        public string CompanyID { get; set; }
        public string RuleName { get; set; }
    }

    ///<summary>
    ///查看设置流量资费信息列表
    /// </summary>
    public class SetRulesInfoDto:Information
    {
      public List<SetRules> rules { get; set; }
    }
    public class SetRules
    {
        public int id { get; set; }
        public string CompanyID { get; set; }
        public string RelationCode { get; set; }
        /// <summary>
        /// 规则名称
        /// </summary>
        public string RuleName { get; set; }
        public string WaringNum { get; set; }
        /// <summary>
        /// 规则类型
        /// </summary>
        public string RuleType { get; set; }
        public string SetType { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string Phone { get; set; }
        public DateTime AddTime { get; set; }
        public List<SendLog> sendLogs { get; set; }
    }

    ///<summary>
    ///查看流量资费发送日志
    /// </summary>
    public class SendLog
    {
        public int id { get; set; }
        /// <summary>
        /// 规则名称
        /// </summary>
        public string RuleName { get; set; }
        /// <summary>
        /// 规则类型
        /// </summary>
        public string RuleType { get; set; }
        ///<summary>
        ///通知时间
        /// </summary>
        public DateTime SendTime { get; set; }
        ///<summary>
        ///提醒方式
        /// </summary>
        public string ReminderMode = "短信";
        public List<SendCardInfos> cardInfos { get; set; }

    }

    ///<summary>
    ///发送日志卡信息
    /// </summary>
    public class SendCardInfos
    {
        public string Card_ID { get; set; }
        public string ICCID { get; set; }
        public string Flow { get; set; }
    }

    ///<summary>
    ///设置了提示规则的卡的详细信息
    /// </summary>
    public class RulesDetailInfos:Information
    {
        public List<SetRulesDetailInfo> setRules { get; set; }
    }
    public class SetRulesDetailInfo
    {
        public string ICCID { get; set; }
        public string SN { get; set; }
        public string OperatorsType { get; set; }
        public string SetType { get; set; }
    }

    ///<summary>
    ///编辑流量资费规则信息
    /// </summary>
    public class UpdateRulesPara
    {
        /// <summary>
        /// 关联码
        /// </summary>
        public string RelationCode { get; set; }
        ///<summary>
        ///提醒值
        /// </summary>
        public string WaringNum { get; set; }
        ///<summary>
        ///备注
        /// </summary>
        public string Remark { get; set; }
        ///<summary>
        ///电话
        /// </summary>
        public string Phone { get; set; }
    }

    public class GetFenLiNum:Information
    {
        public int Number { get; set; }
    }



}