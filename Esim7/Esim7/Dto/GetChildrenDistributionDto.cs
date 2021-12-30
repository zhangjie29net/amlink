using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Dto
{
    public class GetChildrenDistributionDto: Information
    {
        public List<GetChildrenDistribution> getChildrens { get; set; }
    }

    public class GetChildrenDistribution: distributionratio
    {
       
        ///</summary>
        public string CompanyName { get; set; }
        public string OperatorName{ get; set; }
        ///<summary>
        ///卡形态
        /// </summary>
        public string CardXTName{ get; set; }
        ///<summary>
        ///卡类型
        /// </summary>
        public string CardTypeName { get; set; }
        ///<summary>
        ///套餐
        /// </summary>
        public string SetmealName { get; set; }
       
    }

    ///<summary>
    ///获取子级用户的套餐卡类型卡形态运营商价格组合
    /// </summary>
    public class AvailableSetmeal: Information
    {
        public List<OperatorList> operatorLists { get; set; }
        public List<CardXTList> cardXTLists { get; set; }
        public List<CardTypeList> cardTypeLists { get; set; }
        public List<SetmealList> setmealLists { get; set; }
    }
    public class OperatorList
    {
        public string OperatorID { get; set; }
        public string OperatorName { get; set; }
    }
    public class CardXTList
    {
        public string CardXTID { get; set; }
        public string CardXTName { get; set; }
    }
    public class CardTypeList
    {
        public string CardTypeID { get; set; }
        public string CardTypeName { get; set; }
    }
    public class SetmealList
    {
        public string SetmealID { get; set; }
        public string SetmealName { get; set; }
    }

    public class SetMealInfoDto: Information
    {
        public List<SetmealList> SetmealLists { get; set; }
    }

}