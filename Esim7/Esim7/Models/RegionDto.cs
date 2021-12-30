using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Models
{
    public class Regions : Information
    {
        public List<RegionDto> dtos { get; set; }
    }
    public class RegionDto
    {
        /// <summary>
        /// 0没有超出管控  1超出管控
        /// </summary>
        public string RegionLimitStatus { get; set; }
        public string Card_ICCID { get; set; }
        public string Card_IMSI { get; set; }
        public string Card_ID { get; set; }
        public string Card_CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string Card_Remarks { get; set; }
    }
}