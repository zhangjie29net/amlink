namespace Esim7.Model_JiHe
{
    public class Model_JiHe_Card_Message
    {
        /// <summary>
        /// 基本信息
        /// </summary>
        public class BasicMess
        {
            /// <summary>
            /// 卡号
            /// </summary>
            public string MSISDN { get; set; }
            /// <summary>
            /// IMSI
            /// </summary>
            public string MISI { get; set; }
            /// <summary>
            /// ICCID
            /// </summary>
            public string ICCID { get; set; }
            /// <summary>
            /// 开户日期
            /// </summary>
            public string OPENDATE { get; set; }
            /// <summary>
            /// 生效日期
            /// </summary>
            public string ACTIVEDATE { get; set; }
            /// <summary>
            /// 网络类型 2G，3G 等
            /// </summary>
            public string CardType { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            public string REMARKS { get; set; }
            /// <summary>
            /// 卡状态 可测试，待激活等状态
            /// </summary>
            public string CARDSTATE { get; set; }
            /// <summary>
            ///   公司名称
            /// </summary>
            public string COMPANYNAME { get; set; }
            /// <summary>
            ///公司ID
            /// </summary>
            public string COMPANYID { get; set; }
            /// <summary>
            /// IMEI
            /// </summary>
            public string IMEI { get; set; }

        }
        /// <summary>
        /// 通讯服务   订购服务 短信数据 语音
        /// </summary>
        public class CommunicationsServices
        {
            /// <summary>
            /// 短信服务
            /// </summary>
            public string Message { get; set; }
            /// <summary>
            /// 语音服务
            /// </summary>
            public string Voice { get; set; }
            /// <summary>
            /// 数据服务
            /// </summary>
            public string Date { get; set; }

        }
        /// <summary>
        /// 会话服务  开关机 APN IP 在线状态 等
        /// </summary>
        public class SessionInformation
        {
            /// <summary>
            /// 开关机状态
            /// </summary>
            public string SwitchingState { get; set; }
            /// <summary>
            /// APN
            /// </summary>
            public string APN { get; set; }
            /// <summary>
            /// IP
            /// </summary>
            public string IP { get; set; }
            /// <summary>
            /// 在线状态
            /// </summary>
            public string State { get; set; }
        }
        /// <summary>
        /// 流量预警
        /// </summary>
        public class FlowWarning
        {
            /// <summary>
            /// ICCID
            /// </summary>
            public string ICCID { get; set; }
            /// <summary>
            /// 总流量
            /// </summary>
            public string TotalFlow { get; set; }
            /// <summary>
            /// 使用流量费
            /// </summary>
            public string UsedFlow { get; set; }
           
        }
    }
}