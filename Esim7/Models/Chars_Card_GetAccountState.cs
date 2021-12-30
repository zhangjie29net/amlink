namespace Esim7.Models
{/// <summary>
/// 物联卡账户状态   echars信息面板界面 
/// </summary>
    public class Chars_Card_GetAccountState
    {



        /// <summary>
        /// 数据库主键ID（卡账户ID
        /// </summary>
        public string CardAcountID { get; set; }
        /// <summary>
        /// 正常 00
        /// </summary>
        public string Normal { get; set; }
        /// <summary>
        /// 停机  02
        /// </summary>
        public string Shutdown { get; set; }
        /// <summary>
        /// 单项停机 01
        /// </summary>
        
        public string Other { get; set; }
        /// <summary>
        /// 外联建 公司ID
        /// </summary>
        public string CompanyID { get; set; }



    }
}