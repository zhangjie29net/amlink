using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.ReturnMessage
{
    public class Return_Chars_Card_GetAccountState
    {
        public List<Chars_Card_GetAccountState> card_CommunicationStates { get; set; }
        public string status { get; set; }
        public string Message { get; set; }




    }
}