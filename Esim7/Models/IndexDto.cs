using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    /// <summary>
    /// 主界面数据接收类
    /// </summary>
    public class IndexDto:Information
    {
        //移动卡总数量
        public int CMCCCardTotal { get; set; }
        //电信卡总数量
        public int CTCardTotal { get; set; }
        //联通卡总数量
        public int CUCCCardTotal { get; set; }
        //全网通卡总数量
        public int ThreeCardTotal { get; set; }
        public IndexData DtoList { get; set; }
        //public IndexData CtList { get; set; }
        //public IndexData CuccList { get; set; }

    }
    //主页卡数据
    public class IndexData
    {
        //NB卡数量
        public int NBNum { get; set; }
        //2G/4G数量
        public int NoNBNum { get; set; }
        //月流量使用 单位MB
        public decimal MonthFlowTotal { get; set; }
        //月活跃度
        public decimal ActiveNum { get; set; }
        //物联卡通信状态在线数量
        public int Online { get; set; }
        //物联卡通信状态离线数量
        public int Offline { get; set; }
        //物联卡账户状态 正常(再用 已激活)
        public int Normal { get; set; }
        //物联卡账户状态 停机
        public int Shutdown { get; set; }
        //物联卡账户状态 其他
        public int Other { get; set; }
        //物联卡计费状态 正常
        public int NormalUse { get; set; }
        //物联卡计费状态 沉默期
        public int SilentPeriod { get; set; }
        //物联卡计费状态 测试期
        public int TestPeriod { get; set; }
    }

    ///<summary>
    ///主界面统计近七天流量
    /// </summary>
    public class IndexCardFlowDto : Information
    {
        public List<FlowDays> CmccData { get; set; }
        public List<FlowDays> CtData { get; set; }
        public List<FlowDays> CuccData { get; set; }
    }
    public class FlowDays
    {
        public List<decimal> flow { get; set; }
        public List<string> date { get; set; }
    }


    ///<summary>
    ///返回十张月使用流量最大的卡
    /// </summary>
    public class MaxFlowDto : Information
    {
        public List<FlowDto> flowDtos { get; set; }
    }

    public class FlowDto
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}