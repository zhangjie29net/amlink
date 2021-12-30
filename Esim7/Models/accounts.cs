using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class accounts
    {
        public int Id { get; set; }
        public string APPID { get; set; }
        public string ECID { get; set; }
        public string PASSWORD { get; set; }
        public string cityID { get; set; }
        public string accountName { get; set; }
        public string accountID { get; set; }
        /// <summary>
        /// 供应商类型 移动电信联通奇迹
        /// </summary>
        public string operatorsName { get; set; }
        public string selectaccountID { get; set; }
        //选择的奇迹对接平台名称
        public string selectaccountname { get; set; }
        public string TOKEN { get; set; }
        public string TRANSID { get; set; }
        public int Platform { get; set; }
        /// <summary>
        /// 选择平台标识 只有为1的时候才是选择奇迹
        /// </summary>
        public int PlatformFlg { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string APIkey { get; set; }
        public string Remark { get; set; }
        public string url { get; set; }
        public string Company_ID { get; set; }
        /// <summary>
        /// 供应商公司联系人
        /// </summary>
        public string Contacts { get; set; }
        /// <summary>
        ///职务
        /// </summary>
        public string Job { get; set; }
        public string Email { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string Pic { get; set; }
        /// <summary>
        /// 运营商类型
        /// </summary>
        public string OperatorType { get; set; }
        /// <summary>
        /// 发票名称
        /// </summary>
        public string BillName { get; set; }
        /// <summary>
        /// 税号
        /// </summary>
        public string DutyParagraph { get; set; }
        public string AddressPhone { get; set; }
        /// <summary>
        /// 开户行
        /// </summary>
        public string Blank { get; set; }
        /// <summary>
        /// 银行账号
        /// </summary>
        public string BlankNo { get; set; }
    }

    ///<summary>
    ///奇迹下的运营商信息接收类
    /// </summary>
    public class AccountDto:Information
    {
        public List<accountsinfo> accountsinfos { get; set; }

    }
    public class accountsinfo
    {
        public string accountID { get; set; }
        public string accountName { get; set; }
        public string Company_ID { get; set; }
        public string Remark { get; set; }
    }

    public class GetAccountDtoInfos : Information
    {
        public List<accountdto> accounts { get; set; }
    }

    public class accountdto
    {
        public int id { get; set; }
        public string APPID { get; set; }
        public string ECID { get; set; }
        public string PASSWORD { get; set; }
        public string cityID { get; set; }
        public string accountName { get; set; }
        public string accountID { get; set; }
        public string selectaccountID { get; set; }
        public string selectaccountname { get; set; }
        public string TOKEN { get; set; }
        public string TRANSID { get; set; }
        public int Platform { get; set; }
        /// <summary>
        /// 选择平台标识 只有为1的时候才是选择奇迹
        /// </summary>
        public int PlatformFlg { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string APIkey { get; set; }
        public string Remark { get; set; }
        public string url { get; set; }
        public string Company_ID { get; set; }
        /// <summary>
        /// 供应商公司联系人
        /// </summary>
        public string Contacts { get; set; }
        /// <summary>
        ///职务
        /// </summary>
        public string Job { get; set; }
        public string Email { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string Pic { get; set; }
        /// <summary>
        /// 运营商类型
        /// </summary>
        public string OperatorType { get; set; }
        ///<summary>
        ///运营商名称
        /// </summary>
        public string operatorsName { get; set; }
        /// <summary>
        /// 发票名称
        /// </summary>
        public string BillName { get; set; }
        /// <summary>
        /// 税号
        /// </summary>
        public string DutyParagraph { get; set; }
        public string AddressPhone { get; set; }
        /// <summary>
        /// 开户行
        /// </summary>
        public string Blank { get; set; }
        /// <summary>
        /// 银行账号
        /// </summary>
        public string BlankNo { get; set; }
    }

    public class AccountPara
    {
        public string CompanyName { get; set;}
        public string Company_ID { get; set; }
    }

    ///<summary>
    ///奇迹对接的运营商信息
    /// </summary>
    public class qijiaccountinfo:Information
    {
        public List<qijiaccount> accounts { get; set; }
    }

    public class qijiaccount
    {
        public string accountID { get; set; }
        public string accountName { get; set; }
    }
}