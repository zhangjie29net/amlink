namespace Esim7.Action
{   /// <summary>
/// 获取新平台的URL拼接字符串
/// </summary>
    public class ChangeURL
    {
        public class GetURL {
            
            public string URL { get; set; }
            /// <summary>
            ///属于什么类型 query，change，order等
            /// </summary>
            public string Type { get; set; }
            public int  TypeNumber { get; set; }
        }
        public string URL(string APIName)
        {
            string URL = "https://api.iot.10086.cn/v5/ec/";
            string Query = "query/";
            string Change = "change/";
            string Order = "order/";
            string Operate = "operate/";
            string reset = "reset/";
            //用户类
            if (APIName == "API23M10")//物联卡业务批量办理结果查询
            {
                return URL + Query + "sim-batch-result?";
            }
            //https://api.iot.10086.cn/v5/ec/ query/sim-basic-info?transid=xxxxxx&token=xxxxxx &msisdn=xxxxxx -以msisdn进行查询参数只有一个
            else if (APIName == "API23S00")//单卡基本信息查询
            {
                return URL + Query + "sim-basic-info?";
            }
            //https://api.iot.10086.cn/v5/ec/ query/sim-change-history?transid=xxxxxx&token=xxxxxx &msisdn=xxxxxx -以msisdn进行查询参数只有一个
            else if (APIName == "API23S02") //单卡状态变更历史查询
            {
                return URL + Query + "sim-change-history?";
            }
            // https://api.iot.10086.cn/v5/ec/ change/sim-status?transid=xxx&token=xxx &msisdn=xxx&operType=xxx  -以msisdn进行变更参数多了一个  operType
            else if (APIName == "API23S03")  //单卡状态变更
            {
                return URL + Change + "sim-status?";
            }
            //https://api.iot.10086.cn/v5/ec/ query/sim-imei?transid=xxxxxx&token=xxxxxx &msisdn=xxxxxx -以msisdn进行查询查询参数只有一个
            else if (APIName == "API23S00")//单卡基本信息查询
            {
                return URL + Query + "sim-basic-info?";
            }
             
            // https://api.iot.10086.cn/v5/ec/ query/sim-imei?transid=xxxxxx&token=xxxxxx &msisdn=xxxxxx -以msisdn进行查询查询参数只有一个
            else if (APIName == "API23S04")  //单卡绑定IMEI实时查询
            {
                return URL + Query + "sim-imei?";
            }
            // https://api.iot.10086.cn/v5/ec/ change/sim-status/batch?transid=xxxxxx&token=xxxxxx& msisdns=xxxx_xxxx_xxxx_xxxx&operType=xxxx修改批量参数两个
            else if (APIName == "API23S06")  //物联网卡状太 变更批量办理
            {
                return URL + Change + "sim-status/batch?";
            }
            // https://api.iot.10086.cn/v5/ec/ query/sim-status?transid=xxxxxx&token=xxxxxx &msisdn=xxxxxx -以msisdn进行查询查询参数一个 
            else if (APIName == "API25S04")  //单卡状态查询
            {
                return URL + Query + "sim-status?";
            }
            //  https://api.iot.10086.cn/v5/ec/ query/sim-card-info/batch?transid=xxx &token=xxx&msisdns=xxx_xxx_xxx  -以msisdns进行查询查询参数一个 
            else if (APIName == "API25S05")  //集团客户可以通过卡号（msisdn\iccid\imsi三选一，每次不超过100张卡）查询剩余2个码号的信息
            {
                return URL + Query + "sim-card-info/batch?";
            }
            /// https://api.iot.10086.cn/v5/ec/ query/sim-platform/batch?transid=xxxxxx& token=xxxxxx&msisdns=xxx_xxx_xxx –以msisdns进行查询查询批量参数一个 
            else if (APIName == "API25S06")  //物联卡归属平台批量查询 批量查询物联卡对应的OneLink管理平台。
            {
                return URL + Query + "sim-platform/batch?";
            }
            //用量类
            // https://api.iot.10086.cn/v5/ec/ query/group-data-usage?transid=xxxxx&token=xxxxx &groupId=xxxxxx 查询参数一个groupId 
            else if (APIName == "API23U00")
            { //群组本月流量累计使用量实时查询
                return URL + Query + "group-data-usage?";
            }
            //  https://api.iot.10086.cn/v5/ec/ query/sim-voice-usage?transid=xxxxxx&token=xxxxxx &msisdn=xxxxxx –以msisdn进行查询查询参数一个 
            else if (APIName == "API23U01")//单卡本月语音累计使用量实时查询
            {
                return URL + Query + "sim-voice-usage?";
            }
            //https://api.iot.10086.cn/v5/ec/ query/group-data-margin?transid=xxxxx&token=xxxxx &groupId=xxxxx  查询参数一个groupId
            else if (APIName == "API23U04")//群组本月套餐内流量使用量实时查询
            {
                return URL + Query + "group-data-margin?" +"";
            }
            //https://api.iot.10086.cn/v5/ec/ query/sim-voice-margin?transid=xxxxxx&token=xxxxxx &msisdn=xxxxxx –以msisdn进行查询  查询参数一个 
            else if (APIName == "API23U05")//单卡本月套餐内语音使用量实时查询  实时查询物联卡本月套餐内语音使用量。
            {
                return URL + Query + "sim-voice-margin?" +"";
            }
       // https://api.iot.10086.cn/v5/ec/ query/sim-sms-margin?transid=xxxxxx&msisdn=xxxxxx &token=xxxxxx –以msisdn进行查询  查询参数一个 
            else if (APIName == "API23U06")//单卡本月套餐短信用量实时查询 实时查询物联卡本月套餐内短信使用量
            {
                return URL + Query + "sim-sms-margin?" + "";
            }
            //https://api.iot.10086.cn/v5/ec/ query/sim-sms-margin?transid=xxxxxx &msisdn=xxxxxx &token=xxxxxx –以msisdn进行查询    查询 参数一个 
            else if (APIName == "API23U06")//单卡本月套餐短信用量实时查询 实时查询物联卡本月套餐内短信使用量
            {
                return URL + Query + "sim-sms-margin?" +"";
            }
            //https://api.iot.10086.cn/v5/ec/ query/sim-data-margin?transid=xxxxxx &msisdn=xxxxxx&token=xxxxxx –以msisdn进行查询  查询参数一个 
            else if (APIName == "API23U07")//单卡本月套餐流量用量实时查询  实时查询物联卡本月套餐内流量使用量。
            {
                return URL + Query + "sim-data-margin?" +"";
            }
            //https://api.iot.10086.cn/v5/ec/ query/sim-voice-usage-daily/batch?transid=xxxxxx&token=xxxxx &msisdns=xxx_xxx_xxx&queryDate=xxxxxx–以msisdns进行查询  查询参数msisdns   queryDate
            else if (APIName == "API23U08")//物联卡单日语音使用量批量查询 通过此接口可以批量（暂定100张）查询集团客户下所属SIM卡的日语音使用情况。批量查询多个用户、指定日期的语音使用量（仅支持查询近7天中某一天的数据，截止前一天）
            {
                return URL + Query + "sim-voice-usage-daily/batch?";
            }
            //https://api.iot.10086.cn/v5/ec/ query/sim-voice-usage-monthly/batch?transid=xxxxxx&token=xxxxx &msisdns=xxx_xxx_xxx&queryDate=xxxxxx–以msisdns进行查询      查询 参数msisdns   queryDate
            else if (APIName == "API23U09")//物联卡单月语音使用量批量查询  通过此接口可以查询某集团客户下所属SIM卡的月语音使用情况。批量查询多个用户、指定日期的语音使用量，仅支持查询近6个月中某月的使用量，其中本月数据截止为前一天
            {
                return URL + Query + "sim-voice-usage-monthly/batch?";
            }
            //https://api.iot.10086.cn/v5/ec/ query/sim-sms-usage-daily/batch?transid=xxxxxx& token=xxxxxx& msisdns=xxxxxx&queryDate=xxxxxx–以msisdns进行查询 查询参数msisdns   queryDate
            else if (APIName == "API25U00")//物联卡单日短信使用量批量查询 批量（暂定100张）查询物联卡某一天短信使用量（仅支持查询近7天中某一天的数据，截止前一天）
            {
                return URL + Query + "sim-sms-usage-daily/batch?" +"";
            }
            //https://api.iot.10086.cn/v5/ec/ query/sim-data-usage-daily/batch?transid=xxxxxx token=xxxxx&msisdns=xxx_xxx_xxx&queryDate=xxxxxx–以msisdns进行查询  查询参数msisdns   queryDate
            else if (APIName == "API25U01")//物联卡单日GPRS流量使用量批量查询 通过此接口可以批量（暂定100张）查询物联卡某一天GPRS流量使用量（仅支持查询近7天中某一天的数据，截止前一天）
            {
                return URL + Query + "sim-data-usage-daily/batch?" +"";
            }
            //https://api.iot.10086.cn/v5/ec/ query/sim-sms-usage-monthly/batch?transid=xxxxxx &token=xxxxx&msisdns=xxx_xxx_xxx&queryDate=xxxxxx–以msisdns进行查询 查询参数msisdns   queryDate
            else if (APIName == "API25U02")//物联卡单月短信使用量批量查询 通过此接口可以查询某集团客户下所属SIM卡的月短信使用情况。批量查询多个用户、指定日期的短信使用量，仅支持查询近6个月中某月的使用量，其中本月数据截止为前一天
            {
                return URL + Query + "sim-sms-usage-monthly/batch?" +"";
            }
            //https://api.iot.10086.cn/v5/ec/ query/sim-data-usage-monthly/batch?transid=xxxxxx &token=xxxxx&msisdns=xxx_xxx_xxx&queryDate=xxxxxx–以msisdns进行查询  查询参数msisdns   queryDate
            else if (APIName == "API25U03")//物联卡单月GPRS流量使用量批量查询 通过此接口可以批量（暂定100张）查询集团客户下所属SIM卡的月数据使用情况。批量查询多个用户、指定日期的数据使用量，仅支持查询近6个月中某月的使用量，其中本月数据截止为前一天
            {
                return URL + Query + "sim-data-usage-monthly/batch?" +"";
            }
            //https://api.iot.10086.cn/v5/ec/ query/sim-data-usage?transid=xxxxxx&token=xxxxxx &msisdn=xxxxxx –以msisdn进行查询  查询 参数一个 
            else if (APIName == "API25U04")//单卡本月流量累计使用量查询  查询集团所属物联卡当月的GPRS使用量，PB号段为截至前一天24点流量，CT号段为实时流量。（单位：KB）。
            {
                return URL + Query + "sim-data-usage?" +"";
            }
            //https://api.iot.10086.cn/v5/ec/ query/sim-sms-usage?transid=xxxxxx&token=xxxxxx &msisdn=xxxxxx –以msisdn进行查询  查询 参数一个
            else if (APIName == "API25U05")//单卡本月短信累计使用量查询
            // 查询集团所属物联卡当月短信使用情况，PB号段为截至前一天24点短信用量，CT号段为实时短信用量。（单位：条）。
            {
                return URL + Query + "sim-sms-usage?" +"";
            }
        //套餐类
       // https://api.iot.10086.cn/v5/ec/ query/ordered-offerings?transid=xxxxxx&token=xxxxxx  &queryType=xxx&msisdn=xxxxxx –以msisdn进行查询 查询 参数 msisdn   queryType 查询场景标识类型（1：客户接入类型；2：群组接入类型；3：sim接入类型）
            else if (APIName == "API23R00")
            //               资费订购实时查询
            //根据用户类型（企业、群组、sim卡）查询已订购的所有资费列表。
            {
                return URL + Query + "ordered-offerings?" +"" ;
            }
            //https://api.iot.10086.cn/v5/ec/ query/offerings-detail?transid=xxxxxx&token=xxxx      &offeringId=xxxx  查询 offeringId   资费ID
            else if (APIName == "API23R01")
            //           资费详情实时查询
            //查询指定资费的详细信息。
            {
                return URL + Query + "offerings-detail?" +"";
            }
            //https://api.iot.10086.cn/v5/ec/query/purchasable-offerings?transid=xxxxxx&token=xxxxxx& queryType=xxx  &msisdn=xxxxxx& pageSize=xxxxxx& startNum=xxxxxx& catalogId=xxx& categoryId=xxxxxx  –以msisdn进行查询                                    查询 多参数  复杂  
            //queryType   必填  查询场景标识类型（3：集团用户附属资费） 只填3 
            //pageSize    是    每页查询数目 不大于50
            //startNum     是    从1开始
            //catalogId    是           目录参考CMIOT_API23R07目录节点实时查询 
            //categoryId   是           节点ID 多节点用下划线隔开 参考CMIOT_API23R07目录节点实时查询
        
            else if (APIName == "API23R02")
            //            可订购资费实时查询
            //查询指定用户类型（企业、群组、sim卡）可订购的所有附属资费列表。
            {
                return URL + Query + "purchasable-offerings?" +"";
            }
            //https://api.iot.10086.cn/v5/ec/query/changeable-offerings?transid=xxxxxx&token=xxxxxx &queryType=xxx &msisdn=xxxxxx &descOfferingId &pageSize=xxxxxx &startNum=xxxxxx –以msisdn进行查询
            //  queryType 查询场景标识类型（1：集团群组接入类型；2：集团用户接入类型）   值 只选 2
            //descOfferingId   原资费ID
            //pageSize 每页查询数目不大于50
            //startNum  开始页 从1开始
            else if (APIName == "API23R03")
            //         可变更资费实时查询
            //查询指定用户类型（群组、sim卡）可变更的所有附属资费列表。
            {
                return URL + Query + "changeable-offerings?" +"";
            }
            //https://api.iot.10086.cn/v5/ec/query/categories?transid=xxxx&token=xxxx &queryScenes=xxx  只选3
            else if (APIName == "API23R07")
            //        目录节点实时查询
            //根据不同应用场景查询资费的目录节点。
            {
                return URL + Query + "categories?" +"";
            }
            //https://api.iot.10086.cn/v5/ec/order/gprspackage-order?transid=xxxxxx& token=xxxxxx&msisdn=xxxx  &maincommoDity=xxxx& packageType=xxxx
            //https://api.iot.10086.cn/v5/ec/order/gprspackage-order?transid=xxxxxx& token=xxxxxx&msisdn=xxxx  &maincommoDity  加油包主商品： 0：物联卡个人（11000001） 1：车联网个人（11100001) =xxxx&packageType=xxxx加油包套餐： 0:流量加油包 10元套餐（物联卡） 1:流量加油包10元套餐（车联网） 2:流量加油包 30元套餐（物联卡） 3:流量加油包30元套餐（车联网）
            else if (APIName == "API23R08")
            //           物联卡流量叠加包订购
            //集团客户可以通过卡号调用该接口办理订购流量叠加包业务，同一张卡在10分钟内不能重复调用该接口
            {
                return URL + Order + "gprspackage-order?" +"";
            }
            //通信类
            //https://api.iot.10086.cn/v5/ec/query/ec-message-white-list?transid=xxxxxx &token=xxxxxx&pageSize=xxx&startNum=xxxxxx
            else if (APIName == "API23M02")
            //           集团客户短信白名单查询
            //查询企业客户的短信白名单列表。
            {
                return URL + Order + "ec-message-white-list?" +"";
            }
            //https://api.iot.10086.cn/v5/ec/query/apn-info?transid=xxx &token=xxx&msisdn=xxx -以msisdn进行查询
            else if (APIName == "API23M03")
            //          单卡已开通APN信息查询
            //查询SIM卡已开通APN服务的APN信息
            {
                return URL + Order + "apn-info?" +"";
            }
            //https://api.iot.10086.cn/v5/ec/operate/sim-call-function?transid=xxx &token=xxx&msisdn=xxx&operType=xxx  -以msisdn进行办理
            else if (APIName == "API23M05")
            //           单卡语音功能开停
            //集团客户可以通过卡号（msisdn\iccid\imsi三选一，单卡）办理集团归属物联卡的语音功能开 / 停（同一卡号10分钟内不能通过此类接口重复办理业务）
            {
                return URL + Operate + "sim-call-function?" +"";
            }
            //https://api.iot.10086.cn/v5/ec/operate/sim-sms-function?transid=xxx &token=xxx&msisdn=xxx&operType=xxx  -以msisdn进行办理
            else if (APIName == "API23M06")
            //            单卡短信功能开停
            //集团客户可以通过卡号（msisdn\iccid\imsi三选一，单卡）办理集团归属物联卡的短信功能开 / 停（同一卡号10分钟内不能通过此类接口重复办理业务）
            {
                return URL + Operate + "sim-sms-function?" +"";
            }
            // https://api.iot.10086.cn/v5/ec/operate/sim-apn-function?transid=xxx &token=xxx&msisdn=xxx&operType=xxx&apnName=xxx  -以msisdn进行办理
            else if (APIName == "API23M07")
            //            单卡APN功能开停
            //集团客户可以通过卡号（msisdn\iccid\imsi三选一，单卡）办理集团归属物联卡的APN功能开 / 停（同一卡号10分钟内不能通过此类接口重复办理业务）。
            {
                return URL + Operate + "sim-apn-function?" +"";
            }
            //https://api.iot.10086.cn/v5/ec/query/sim-communication-function-status?transid=xxxxxx& token=xxxxxx& msisdn=xxxxxx–以msisdn进行查询
            else if (APIName == "API23M08")
            //           单卡通信功能开通查询
            //客户通过卡号查询物联卡通信功能开通情况。
            {
                return URL + Query + "sim-communication-function-status?" +"" ;
            }
            //https://api.iot.10086.cn/v5/ec/query/sim-communication-function-status?transid=xxxxxx& token=xxxxxx& msisdn=xxxxxx–以msisdn进行查询
            else if (APIName == "API23M08")
            //           单卡通信功能开通查询
            //客户通过卡号查询物联卡通信功能开通情况。
            {
                return URL + Query + "sim-communication-function-status?" +"";
            }
            //https://api.iot.10086.cn/v5/ec/operate/sim-communication-function/batch?transid=xxxxxx& token=xxxxxx&msisdns=xxxx_xxxx_xxxx_xxxx&serviceType=xxxx&operType=xxxx
            else if (APIName == "API23M09")
            //          物联卡通信功能开停批量办理
            //集团客户可以通过卡号调用该接口批量办理物联卡的通信功能（语音、短信、国际漫游、数据通信服务）开停，
            //每次不超过100张卡，同一卡号十分钟内不得重复调用该接口（批量办理中若一批有10个卡号，
            //其中有一个卡号是在十分钟内有成功调用的，则这次不允许调用，这一批卡号中其余9个卡号本次调用失败，
            //不记录在十分钟内有调用的记录中）。如需查询办理结果则根据该接口返回的任务流水号调“CMIOT_API23M10 - 物联卡
            {
                return URL + Operate + "sim-communication-function/batch?" +"";
            }
            //https://api.iot.10086.cn/v5/ec/query/on-off-status?transid=xxxxxx &token=xxxxxx&msisdn=xxxxxx –以msisdn进行查询
            else if (APIName == "API25M00")
            //            单卡开关机状态实时查询
            //查询终端的开关机信息
            {
                return URL + Query + "on-off-status?" +"";
            }
            // https://api.iot.10086.cn/v5/ec/query/sim-session?transid=xxxxxx &token=xxxxxx&msisdn=xxxxxx –以msisdn进行查询
            else if (APIName == "API25M01")
            //           单卡在线信息实时查询
            //查询物联卡的在线信息，区分APN，返回APN信息、IP地址、会话开始时间。
            {
                return URL + Query + "sim-session?" +"";
            }
            //https://api.iot.10086.cn/v5/ec/reset/sim-sms-status?transid=xxxxxx& token=xxxxxx& msisdn=xxxxxx –以msisdn进行操作
            else if (APIName == "API25M02")
            //            物联卡短信状态重置
            //集团客户可重置HLR上短信的状态，以保证短信正常使用
            {
                return URL + reset + "sim-sms-status?";                                      
            }
            else
            {
                return "error";
            }
        }
        /// <summary>
        /// 根据APIName返回不同的URL地址和参数
        /// </summary>
        /// <param name="APIName"></param>
        /// <returns></returns>
        public static  GetURL URL2(string APIName)
        {
            GetURL r = new GetURL();
            string URL = "https://api.iot.10086.cn/v5/ec/";
            string Query = "query/";
            string Change = "change/";
            string Order = "order/";
            string Operate = "operate/";
            string reset = "reset/";
            //用户类
            if (APIName == "API23M10")//物联卡业务批量办理结果查询
            {
                r.Type = Query;
                r.URL = URL + Query + "sim-batch-result?";
                r.TypeNumber = 1;
            }
            //https://api.iot.10086.cn/v5/ec/ query/sim-basic-info?transid=xxxxxx&token=xxxxxx &msisdn=xxxxxx -以msisdn进行  查询参数只有一个
            else if (APIName == "API23S00")//单卡基本信息查询
            {
                r.Type = Query;
                r.URL = URL + Query + "sim-basic-info?";
                r.TypeNumber = 1;
            }
            //https://api.iot.10086.cn/v5/ec/ query/sim-change-history?transid=xxxxxx&token=xxxxxx &msisdn=xxxxxx -以msisdn进行 查询参数只有一个
            else if (APIName == "API23S02") //单卡状态变更历史查询
            {
                r.Type = Query;
                r.URL = URL + Query + "sim-change-history?";
                r.TypeNumber = 1;
            }
            // https://api.iot.10086.cn/v5/ec/ change/sim-status?transid=xxx&token=xxx &msisdn=xxx&operType=xxx  -以msisdn进行 变更参数多了一个  operType
            else if (APIName == "API23S03")  //单卡状态变更
            {
                r.Type = Change;
                r.URL = URL + Change + "sim-status?";
                r.TypeNumber = 2;
            }
            //https://api.iot.10086.cn/v5/ec/ query/sim-imei?transid=xxxxxx&token=xxxxxx &msisdn=xxxxxx -以msisdn进行查询  查询参数只有一个
            else if (APIName == "API23S00")//单卡基本信息查询
            {
                r.Type = Query;
                r.URL = URL + Query + "sim-basic-info?";
                r.TypeNumber = 1;
            }
            // https://api.iot.10086.cn/v5/ec/ query/sim-imei?transid=xxxxxx&token=xxxxxx &msisdn=xxxxxx -以msisdn进行查询  查询参数只有一个
            else if (APIName == "API23S04")  //单卡绑定IMEI实时查询
            {
                r.Type = Query;
                r.URL = URL + Query + "sim-imei?";
                r.TypeNumber = 1;
            }
            // https://api.iot.10086.cn/v5/ec/ change/sim-status/batch?transid=xxxxxx&token=xxxxxx& msisdns=xxxx_xxxx_xxxx_xxxx&operType=xxxx      修改  批量参数两个
            else if (APIName == "API23S06")  //物联网卡状太 变更批量办理
            {
                r.Type = Change + "#msisdns#operType";
                r.URL = URL + Change + "sim-status/batch?";
                r.TypeNumber = 2;
            }
            // https://api.iot.10086.cn/v5/ec/ query/sim-status?transid=xxxxxx&token=xxxxxx &msisdn=xxxxxx -以msisdn进行查询  查询 参数一个 
            else if (APIName == "API25S04")  //单卡状态查询
            {
                r.Type = Query;
                r.URL = URL + Query + "sim-status?";
                r.TypeNumber = 1;
            }
            //  https://api.iot.10086.cn/v5/ec/ query/sim-card-info/batch?transid=xxx &token=xxx&msisdns=xxx_xxx_xxx  -以msisdns进行查询 查询  批量 参数一个 
            else if (APIName == "API25S05")  //集团客户可以通过卡号（msisdn\iccid\imsi三选一，每次不超过100张卡）查询剩余2个码号的信息
            {
                r.Type = Query + "#msisdns";
                r.URL = URL + Query + "sim-card-info/batch?";
                r.TypeNumber = 1;
            }
            /// https://api.iot.10086.cn/v5/ec/ query/sim-platform/batch?transid=xxxxxx& token=xxxxxx&msisdns=xxx_xxx_xxx –以msisdns进行查询 查询批量 参数一个 
            else if (APIName == "API25S06")  //物联卡归属平台批量查询 批量查询物联卡对应的OneLink管理平台。
            {
                r.Type = Query + "#msisdns";
                r.URL = URL + Query + "sim-platform/batch?";
                r.TypeNumber = 1;
            }
            //用量类
            // https://api.iot.10086.cn/v5/ec/ query/group-data-usage?transid=xxxxx&token=xxxxx &groupId=xxxxxx 查询 参数一个groupId 
            //else if (APIName == "API23U00")
            //{ //群组本月流量累计使用量实时查询
            //    return URL + Query + "group-data-usage?";
            //}
            //  https://api.iot.10086.cn/v5/ec/ query/sim-voice-usage?transid=xxxxxx&token=xxxxxx &msisdn=xxxxxx –以msisdn进行查询  查询 参数一个 
            else if (APIName == "API23U01")//单卡本月语音累计使用量实时查询
            {
                r.Type = Query;
                r.URL = URL + Query + "sim-voice-usage?";
                r.TypeNumber = 1;
            }
            //https://api.iot.10086.cn/v5/ec/ query/group-data-margin?transid=xxxxx&token=xxxxx &groupId=xxxxx 查询 参数一个groupId
            else if (APIName == "API23U04")//群组本月套餐内流量使用量实时查询
            {
                r.Type = Query + "#groupId";
                r.URL = URL + Query + "group-data-margin?";
                r.TypeNumber = 1;
            }
            //https://api.iot.10086.cn/v5/ec/ query/sim-voice-margin?transid=xxxxxx&token=xxxxxx &msisdn=xxxxxx –以msisdn进行查询 查询参数一个 
            else if (APIName == "API23U05")//单卡本月套餐内语音使用量实时查询  实时查询物联卡本月套餐内语音使用量。
            {
                r.Type = Query;
                r.URL = URL + Query + "sim-voice-margin?";
                r.TypeNumber = 1;
            }
            // https://api.iot.10086.cn/v5/ec/ query/sim-sms-margin?transid=xxxxxx&msisdn=xxxxxx &token=xxxxxx –以msisdn进行查询查询 参数一个 
            else if (APIName == "API23U06")//单卡本月套餐短信用量实时查询 实时查询物联卡本月套餐内短信使用量
            {
                r.Type = Query;
                r.URL = URL + Query + "sim-sms-margin?";
                r.TypeNumber = 1;
            }        
            //https://api.iot.10086.cn/v5/ec/ query/sim-data-margin?transid=xxxxxx &msisdn=xxxxxx&token=xxxxxx –以msisdn进行查询 查询参数一个 
            else if (APIName == "API23U07")//单卡本月套餐流量用量实时查询  实时查询物联卡本月套餐内流量使用量。
            {
                r.Type = Query;
                r.URL = URL + Query + "sim-data-margin?";
                r.TypeNumber = 1;
            }
            //https://api.iot.10086.cn/v5/ec/ query/sim-voice-usage-daily/batch?transid=xxxxxx&token=xxxxx &msisdns=xxx_xxx_xxx&queryDate=xxxxxx–以msisdns进行查询 查询参数msisdns   queryDate
            else if (APIName == "API23U08")//物联卡单日语音使用量批量查询 通过此接口可以批量（暂定100张）查询集团客户下所属SIM卡的日语音使用情况。批量查询多个用户、指定日期的语音使用量（仅支持查询近7天中某一天的数据，截止前一天）
            {
                r.Type = Query + "#msisdns#queryDate";
                r.URL = URL + Query + "sim-voice-usage-daily/batch?";
                r.TypeNumber = 2;
            }
            //https://api.iot.10086.cn/v5/ec/ query/sim-voice-usage-monthly/batch?transid=xxxxxx&token=xxxxx &msisdns=xxx_xxx_xxx&queryDate=xxxxxx–以msisdns进行查询 查询 参数msisdns   queryDate
            else if (APIName == "API23U09")//物联卡单月语音使用量批量查询  通过此接口可以查询某集团客户下所属SIM卡的月语音使用情况。批量查询多个用户、指定日期的语音使用量，仅支持查询近6个月中某月的使用量，其中本月数据截止为前一天
            {
                r.Type = Query + "#msisdns#queryDate";
                r.URL = URL + Query + "sim-voice-usage-monthly/batch?";
                r.TypeNumber = 2;
            }
            //https://api.iot.10086.cn/v5/ec/ query/sim-sms-usage-daily/batch?transid=xxxxxx& token=xxxxxx& msisdns=xxxxxx&queryDate=xxxxxx–以msisdns进行查询  查询 参数msisdns   queryDate
            else if (APIName == "API25U00")//物联卡单日短信使用量批量查询 批量（暂定100张）查询物联卡某一天短信使用量（仅支持查询近7天中某一天的数据，截止前一天）
            {
                r.Type = Query + "#msisdns#queryDate";
                r.URL = URL + Query + "sim-sms-usage-daily/batch?";
                r.TypeNumber = 2;
            }
            //https://api.iot.10086.cn/v5/ec/ query/sim-data-usage-daily/batch?transid=xxxxxx token=xxxxx&msisdns=xxx_xxx_xxx&queryDate=xxxxxx–以msisdns进行查询 查询 参数msisdns   queryDate
            else if (APIName == "API25U01")//物联卡单日GPRS流量使用量批量查询 通过此接口可以批量（暂定100张）查询物联卡某一天GPRS流量使用量（仅支持查询近7天中某一天的数据，截止前一天）
            {
                r.Type = Query + "#msisdns#queryDate";
                r.URL = URL + Query + "sim-data-usage-daily/batch?";
                r.TypeNumber = 2;
            }
            //https://api.iot.10086.cn/v5/ec/ query/sim-sms-usage-monthly/batch?transid=xxxxxx &token=xxxxx&msisdns=xxx_xxx_xxx&queryDate=xxxxxx–以msisdns进行查询  查询 参数msisdns   queryDate
            else if (APIName == "API25U02")//物联卡单月短信使用量批量查询 通过此接口可以查询某集团客户下所属SIM卡的月短信使用情况。批量查询多个用户、指定日期的短信使用量，仅支持查询近6个月中某月的使用量，其中本月数据截止为前一天
            {
                r.Type = Query + "#msisdns#queryDate";
                r.URL = URL + Query + "sim-sms-usage-monthly/batch?";
                r.TypeNumber = 2;
            }
            //https://api.iot.10086.cn/v5/ec/ query/sim-data-usage-monthly/batch?transid=xxxxxx &token=xxxxx&msisdns=xxx_xxx_xxx&queryDate=xxxxxx–以msisdns进行查询 查询参数msisdns   queryDate
            else if (APIName == "API25U03")//物联卡单月GPRS流量使用量批量查询 通过此接口可以批量（暂定100张）查询集团客户下所属SIM卡的月数据使用情况。批量查询多个用户、指定日期的数据使用量，仅支持查询近6个月中某月的使用量，其中本月数据截止为前一天
            {
                r.Type = Query + "#msisdns#queryDate";
                r.URL = URL + Query + "sim-data-usage-monthly/batch?";
                r.TypeNumber = 2;
            }
            //https://api.iot.10086.cn/v5/ec/ query/sim-data-usage?transid=xxxxxx&token=xxxxxx &msisdn=xxxxxx –以msisdn进行查询   查询参数一个 
            else if (APIName == "API25U04")//单卡本月流量累计使用量查询  查询集团所属物联卡当月的GPRS使用量，PB号段为截至前一天24点流量，CT号段为实时流量。（单位：KB）。
            {
                r.Type = Query;
                r.URL = URL + Query + "sim-data-usage?";
                r.TypeNumber = 1;
            }
            //https://api.iot.10086.cn/v5/ec/ query/sim-sms-usage?transid=xxxxxx&token=xxxxxx &msisdn=xxxxxx –以msisdn进行查询 查询参数一个
            else if (APIName == "API25U05")//单卡本月短信累计使用量查询
            // 查询集团所属物联卡当月短信使用情况，PB号段为截至前一天24点短信用量，CT号段为实时短信用量。（单位：条）。
            {
                r.Type = Query;
                r.URL = URL + Query + "sim-sms-usage?";
                r.TypeNumber = 1;
            }
            //套餐类
            // https://api.iot.10086.cn/v5/ec/ query/ordered-offerings?transid=xxxxxx&token=xxxxxx  &queryType=xxx&msisdn=xxxxxx –以msisdn进行查询 查询参数 msisdn   queryType 查询场景标识类型（1：客户接入类型；2：群组接入类型；3：sim接入类型）
            else if (APIName == "API23R00")
            //               资费订购实时查询
            //根据用户类型（企业、群组、sim卡）查询已订购的所有资费列表。
            {
                r.Type = Query + "#msisdn#queryType";
                r.URL = URL + Query + "ordered-offerings?";
                r.TypeNumber = 2;
            }
            //https://api.iot.10086.cn/v5/ec/ query/offerings-detail?transid=xxxxxx&token=xxxx      &offeringId=xxxx  查询offeringId   资费ID
            else if (APIName == "API23R01")
            //           资费详情实时查询
            //查询指定资费的详细信息。
            {
                r.Type = Query + "#offeringId";
                r.URL = URL + Query + "offerings-detail?";
                r.TypeNumber = 1;
            }
            //https://api.iot.10086.cn/v5/ec/query/purchasable-offerings?transid=xxxxxx&token=xxxxxx& queryType=xxx  &msisdn=xxxxxx& pageSize=xxxxxx& startNum=xxxxxx& catalogId=xxx& categoryId=xxxxxx  –以msisdn进行查询 查询 多参数  复杂  
            //queryType   必填  查询场景标识类型（3：集团用户附属资费） 只填3 
            //pageSize    是    每页查询数目 不大于50
            //startNum     是    从1开始
            //catalogId    是           目录参考CMIOT_API23R07目录节点实时查询 
            //categoryId   是           节点ID 多节点用下划线隔开 参考CMIOT_API23R07目录节点实时查询

            else if (APIName == "API23R02")
            //            可订购资费实时查询
            //查询指定用户类型（企业、群组、sim卡）可订购的所有附属资费列表。
            {
                r.Type = Query + "#queryType#msisdn#pageSize#startNum#catalogId#categoryId";
                r.URL = "purchasable-offerings?";
                r.TypeNumber = 6;
            }
            //https://api.iot.10086.cn/v5/ec/query/changeable-offerings?transid=xxxxxx&token=xxxxxx &queryType=xxx &msisdn=xxxxxx &descOfferingId &pageSize=xxxxxx &startNum=xxxxxx –以msisdn进行查询
            //  queryType 查询场景标识类型（1：集团群组接入类型；2：集团用户接入类型）   值 只选 2
            //descOfferingId   原资费ID
            //pageSize 每页查询数目不大于50
            //startNum  开始页 从1开始
            else if (APIName == "API23R03")
            //         可变更资费实时查询
            //查询指定用户类型（群组、sim卡）可变更的所有附属资费列表。
            {
                r.Type = Query + "#queryType#msisdn#descOfferingId#pageSize#startNum";
                r.URL = URL + Query + "changeable-offerings?";
                r.TypeNumber = 5;
            }
            //https://api.iot.10086.cn/v5/ec/query/categories?transid=xxxx&token=xxxx &queryScenes=xxx 只选3
            else if (APIName == "API23R07")
            //        目录节点实时查询
            //根据不同应用场景查询资费的目录节点。
            {
                r.Type = Query + "#queryScenes=3";
                r.URL = URL + Query + "categories?";
                r.TypeNumber = 0; 
            }
            //https://api.iot.10086.cn/v5/ec/order/gprspackage-order?transid=xxxxxx& token=xxxxxx&msisdn=xxxx  &maincommoDity=xxxx&  packageType=xxxx
            //https://api.iot.10086.cn/v5/ec/order/gprspackage-order?transid=xxxxxx& token=xxxxxx&msisdn=xxxx  &maincommoDity  加油包主商品： 0：物联卡个人（11000001） 1：车联网个人（11100001)    =xxxx&packageType=xxxx加油包套餐： 0:流量加油包 10元套餐（物联卡） 1:流量加油包10元套餐（车联网） 2:流量加油包 30元套餐（物联卡） 3:流量加油包30元套餐（车联网）
            else if (APIName == "API23R08")
            //           物联卡流量叠加包订购
            //集团客户可以通过卡号调用该接口办理订购流量叠加包业务，同一张卡在10分钟内不能重复调用该接口
            {
                r.Type = Query + "#msisdn#maincommoDity#packageType";
                r.URL = URL + Order + "gprspackage-order?";
                r.TypeNumber = 3;
            }
            //通信类
            //https://api.iot.10086.cn/v5/ec/query/ec-message-white-list?transid=xxxxxx &token=xxxxxx&pageSize=xxx&startNum=xxxxxx
            else if (APIName == "API23M02")
            //           集团客户短信白名单查询
            //查询企业客户的短信白名单列表。
            {
                r.Type = Query + "#pageSize#startNum";
                r.URL = URL + Query + "ec-message-white-list?";
                r.TypeNumber = 2;
            }
            //https://api.iot.10086.cn/v5/ec/query/apn-info?transid=xxx &token=xxx&msisdn=xxx -以msisdn进行查询
            else if (APIName == "API23M03")
            //          单卡已开通APN信息查询
            //查询SIM卡已开通APN服务的APN信息
            {
                r.Type = Query ;
                r.URL = URL + Query + "apn-info?";
                r.TypeNumber = 1;
            }
            //https://api.iot.10086.cn/v5/ec/operate/sim-call-function?transid=xxx &token=xxx&msisdn=xxx&operType=xxx  -以msisdn进行办理
            else if (APIName == "API23M05")
            //           单卡语音功能开停
            //集团客户可以通过卡号（msisdn\iccid\imsi三选一，单卡）办理集团归属物联卡的语音功能开 / 停（同一卡号10分钟内不能通过此类接口重复办理业务）
            {
                r.Type = Query;
                r.URL = URL + Operate + "sim-call-function?";
                r.TypeNumber = 1;
            }
            //https://api.iot.10086.cn/v5/ec/operate/sim-sms-function?transid=xxx &token=xxx&msisdn=xxx&operType=xxx  -以msisdn进行办理
            else if (APIName == "API23M06")
            //            单卡短信功能开停
            //集团客户可以通过卡号（msisdn\iccid\imsi三选一，单卡）办理集团归属物联卡的短信功能开 / 停（同一卡号10分钟内不能通过此类接口重复办理业务）
            {
                r.Type = Query;
                r.URL = URL + Operate + "sim-sms-function?";
                r.TypeNumber = 1; 
            }
            // https://api.iot.10086.cn/v5/ec/operate/sim-apn-function?transid=xxx &token=xxx&msisdn=xxx&operType=xxx&apnName=xxx  -以msisdn进行办理
            else if (APIName == "API23M07")
            //            单卡APN功能开停
            //集团客户可以通过卡号（msisdn\iccid\imsi三选一，单卡）办理集团归属物联卡的APN功能开 / 停（同一卡号10分钟内不能通过此类接口重复办理业务）。
            {
                r.Type = Query;
                r.URL = URL + Operate + "sim-apn-function?";
                r.TypeNumber = 1;
            }
            //https://api.iot.10086.cn/v5/ec/query/sim-communication-function-status?transid=xxxxxx& token=xxxxxx& msisdn=xxxxxx–以msisdn进行查询
            else if (APIName == "API23M08")
            //           单卡通信功能开通查询
            //客户通过卡号查询物联卡通信功能开通情况。
            {
                r.Type = Query;
                r.URL = URL + Query + "sim-communication-function-status?";
                r.TypeNumber = 1;
            }
            //https://api.iot.10086.cn/v5/ec/query/sim-communication-function-status?transid=xxxxxx& token=xxxxxx& msisdn=xxxxxx–以msisdn进行查询
            else if (APIName == "API23M08")
            //           单卡通信功能开通查询
            //客户通过卡号查询物联卡通信功能开通情况。
            {
                r.Type = Query;
                r.URL = URL + Query + "sim-communication-function-status?";
                r.TypeNumber = 1;               
            }
            //https://api.iot.10086.cn/v5/ec/operate/sim-communication-function/batch?transid=xxxxxx& token=xxxxxx&msisdns=xxxx_xxxx_xxxx_xxxx&serviceType=xxxx&operType=xxxx
            else if (APIName == "API23M09")
            //          物联卡通信功能开停批量办理
            //集团客户可以通过卡号调用该接口批量办理物联卡的通信功能（语音、短信、国际漫游、数据通信服务）开停，
            //每次不超过100张卡，同一卡号十分钟内不得重复调用该接口（批量办理中若一批有10个卡号，
            //其中有一个卡号是在十分钟内有成功调用的，则这次不允许调用，这一批卡号中其余9个卡号本次调用失败，
            //不记录在十分钟内有调用的记录中）。如需查询办理结果则根据该接口返回的任务流水号调“CMIOT_API23M10 - 物联卡
            {
                r.Type = Query+ "#msisdns#serviceType#operType";
                r.URL = URL + Operate + "sim-communication-function/batch?";
                r.TypeNumber = 3;               
            }
            //https://api.iot.10086.cn/v5/ec/query/on-off-status?transid=xxxxxx &token=xxxxxx&msisdn=xxxxxx –以msisdn进行查询
            else if (APIName == "API25M00")
            //            单卡开关机状态实时查询
            //查询终端的开关机信息
            {
                r.Type = Query ;
                r.URL = URL + Query + "on-off-status?";
                r.TypeNumber = 1;               
            }
            // https://api.iot.10086.cn/v5/ec/query/sim-session?transid=xxxxxx &token=xxxxxx&msisdn=xxxxxx –以msisdn进行查询
            else if (APIName == "API25M01")
            //           单卡在线信息实时查询
            //查询物联卡的在线信息，区分APN，返回APN信息、IP地址、会话开始时间。
            {
                r.Type = Query;
                r.URL = URL + Query + "sim-session?";
                r.TypeNumber = 1;
            }
            //https://api.iot.10086.cn/v5/ec/reset/sim-sms-status?transid=xxxxxx& token=xxxxxx& msisdn=xxxxxx –以msisdn进行操作
            else if (APIName == "API25M02")
            //            物联卡短信状态重置
            //集团客户可重置HLR上短信的状态，以保证短信正常使用
            {
                r.Type = Query;
                r.URL = URL + reset + "sim-sms-status?";
                r.TypeNumber = 1;
            }
            else
            {
                r.Type = "error";
                r.URL = "error";
                r.TypeNumber = 0;
            }
            return r;
        }
    }
}
