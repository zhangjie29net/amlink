using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.ReturnMessage
{
    public class GetUserSetmealDto
    {
        public string flg { get; set; }
        public string Msg { get; set; }
       public List<UserSetmeal> userSetmeals { get; set; }
    }

    public class UserSetmeal
    {
        public string CardTypeID { get; set; }
        public string CardTypeName { get; set; }
        public string OperatorID { get; set; }
        public string OperatorName { get; set; }
        public string CardXTID { get; set; }
        public string CardXTName { get; set; }
        public int Flow { get; set; }
        public string PackageDescribe { get; set; }
        public string PartNumber { get; set; }
        public string Remark { get; set; }
        public string SetMealID { get; set; }
    }
}