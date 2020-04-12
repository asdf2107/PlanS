using System;
using System.Collections.Generic;
using System.Text;

namespace PlanS
{
    public class DateComp : Comparer<Plan>
    {
        public override int Compare(Plan x, Plan y)
        {
            if (x.DueTime.CompareTo(y.DueTime) != 0)
            {
                return x.DueTime.CompareTo(y.DueTime);
            }
            else
            {
                return x.Name.CompareTo(y.Name);
            }
        }
    }
}