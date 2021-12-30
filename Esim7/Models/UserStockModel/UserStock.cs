using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models.UserStockModel
{
    /// <summary>
    /// 用户库存表
    /// </summary>
    public class UserStock
    {
        public int Id { get; set; }
        public string Company_ID { get; set; }
        //操作人
        public string personnel { get; set; }
        //库存类型 1入库 2出库
        public int StockType { get; set; }
        //卡库存数量
        public int CardNumber { get; set; }
        //卡出库数量
        public int CardOutNumber { get; set; }
        //卡入库数量
        public int CardEnterNumber { get; set; }
        //套餐信息 关联setmeal表OperatorID字段
        public string OperatorID { get; set; }
        //物料编码
        public string MaterielCode { get; set; }
        //套餐名称
        public string SetmealName { get; set; }
        //套餐编号
        public string SetmealID { get; set; }
        //库存所在城市
        public string StockCity { get; set; }
        //库存具体位置
        public string StockAdderss { get; set; }
        //运营商名称
        public string OperatorName { get; set; }
        //开始的ICCID
        public string StartICCID { get; set; }
        //结束的ICCID
        public string EndICCID { get; set; }
        //起始卡号
        public string StartCardID { get; set; }
        //结束卡号
        public string EndCardID { get; set; }
        //卡形态名称
        public string CardXTName { get; set; }
        //卡类型名称
        public string CardTypeName { get; set; }
        //对接运营商
        public string OperatorsName { get; set; }
        //沉默期
        public int silentDate { get; set; }
        //测试期
        public int TestDate { get; set; }
        //流量
        public string Flow { get; set; }
        public string Remark { get; set; }
        //出库用途
        public string OutPurpose { get; set; }
        //入库单号
        public string EnterCode { get; set; }
        //出库单号
        public string OutCode { get; set; }
        //卡价格
        public decimal CardPrice { get; set; }
        //批次
        public string pici { get; set; }
        //入库类型 1：一键导入方式入库 2：上传excel方式入库
        public int EnterStockType { get; set; }
        public DateTime EnterTime { get; set; }
        public DateTime OutTime { get; set; }
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 是否进入正式期 或者快要进入正式期 小于30:是 大于30:否
        /// </summary>
        public int IsFormal { get; set; }
    }
}