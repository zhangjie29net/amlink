using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.parameter.PowerManage
{
    /// <summary>
    /// 给用户分配和撤销和
    /// </summary>
    public class AssignCardPara
    {
        /// <summary>
        /// 二级用户Id
        /// </summary>
        public string CompanyId { get; set; }
        ///<summary>
        ///被分配用户的Id
        /// </summary>
        public string UserCompanyId { get; set; }
        public List<CardInfos> cardInfos { get; set; }
    }

    public class CardInfos
    {
        public string Card_ICCID { get; set; }
    }
}