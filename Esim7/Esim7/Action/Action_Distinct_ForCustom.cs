using Esim7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esim7.Action
{
    public class Action_Distinct_ForCustom : IEqualityComparer<Card2>
    {
        public bool Equals(Card2 x, Card2 y)
        {       //规则定义
            return x.ICCID == y.ICCID;



            
        }

        public int GetHashCode(Card2 obj)
        {
            return obj.ICCID.GetHashCode();
        }
    }
}