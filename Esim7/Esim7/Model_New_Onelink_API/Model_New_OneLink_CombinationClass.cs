namespace Esim7.Model_New_Onelink_API
{      
    /// <summary>
    /// 新平台 onelink  综合功能结合
    /// </summary>
    public class Model_New_OneLink_CombinationClass
    {
        /// <summary>
        /// 会话信息  对应新平台单张卡查询的会话信息   直接调取的未经处理的 字符
        /// </summary>
        public class ConversationMessage
        {
            /// <summary>
            /// 开关
            /// </summary>
            public On_OFF.Root On_OFF { get; set; }
            /// <summary>
            /// 终端上报的IMEI
            /// </summary>
            public GetIMEI.Root GETIMEI { get; set; }
            /// <summary>
            /// 获取在线信息  APN IP等
            /// </summary>
            public OnLine.Root GetOnLine { get; set; }            
        }

        public class ConversationMessage2
        {
            /// <summary>
            /// 开关机状态的标志位
            /// </summary>
            public string On_OFF_status { get; set; }

                /// <summary>
                /// 开关机状态 解读
                /// </summary>
                /// <returns></returns>
            public string On_OFF_Name() {
                   
                switch (On_OFF_status)
                {
                    case "0":
                        return "关机";
                    case "1":
                        return "开机";
                    default:
                        return "非正常状态";
                }                
            }  
        }
    }
}