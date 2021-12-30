using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.ReturnMessage
{
    public class Return_CardMessage
    {

   public  List<Card> Cards { get; set; }
        public string Msg { get; set; }
        public string flg { get; set; }
       
    }
}