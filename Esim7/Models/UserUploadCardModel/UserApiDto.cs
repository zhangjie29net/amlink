using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Esim7.Model_Stock.Model_Stock_Config;

namespace Esim7.Models.UserUploadCardModel
{
    public class UserApiDto
    {
        public string flg { get; set; }
        public string Msg { get; set; }
        public List<Account> accounts { get; set; }
    }
}