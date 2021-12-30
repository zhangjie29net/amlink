using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.SimModel
{
    public class Token
    {

        public class ResultItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string token { get; set; }
        }

        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public string status { get; set; }
            /// <summary>
            /// 正确
            /// </summary>
            public string message { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<ResultItem> result { get; set; }
        }


    }


}