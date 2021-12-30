using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.parameter.DistributionManage
{
    ///<summary>
    ///选卡列表查询参数
    /// </summary>
    public class DistributionPara
    {
        ///<summary>
        ///API标识 
        /// </summary>
        public string ApiFlg { get; set; }

        /// <summary>
        /// 当前登录用户公司编码
        /// </summary>
        public string Company_ID { get; set; }
        /// <summary>
        /// 运营商
        /// </summary>
        public string OperatorID { get; set; }
        ///<summary>
        ///卡形态
        /// </summary>
        public string CardXTID { get; set; }
        ///<summary>
        ///卡类型
        /// </summary>
        public string CardTypeID { get; set; }
        ///<summary>
        ///套餐编号
        /// </summary>
        public string SetmealID { get; set; }
    }

    
   
}