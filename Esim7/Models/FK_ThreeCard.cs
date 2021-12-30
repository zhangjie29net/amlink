using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    /// <summary>
    /// 奇迹物联三合一卡关联表
    /// </summary>
    public class FK_ThreeCard
    {
        public int Id { get; set; }
        public string SN { get; set; }
        public string CMCC_CardId { get; set; }
        public string CUCC_CardId { get; set; }
        public string CT_CardId { get; set; }
        public string CMCC_CardICCID { get; set; }
        public string CUCC_CardICCID { get; set; }
        public string CT_CardICCID { get; set; }
        public string CMCC_IMEI { get; set; }
        public string CUCC_IMEI { get; set; }
        public string CT_IMEI { get; set; }
        public string CMCC_CardState { get; set; }
        public string CUCC_CardState { get; set; }
        public string CT_CardState { get; set; }
        public string CMCC_WorkState { get; set; }
        public string CUCC_WorkState { get; set; }
        public string CT_WorkState { get; set; }
        public string CMCC_Flow { get; set; }
        public string CUCC_Flow { get; set; }
        public string CT_Flow { get; set; }
        public string CopyID { get; set; }
        public DateTime Addtime { get; set; }
    }
}