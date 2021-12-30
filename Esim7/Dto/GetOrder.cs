using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Dto
{
    public class GetOrder
    {
        public string flg { get; set; }
        public string Msg { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderNum { get; set; }
        public string Body { get; set; }
    }
}