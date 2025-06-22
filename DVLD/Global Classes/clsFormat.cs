using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Global_Classes
{
    public static class clsFormat
    {
        public static string DateToShort(DateTime Date) 
        {
            return Date.ToString("dd, MMM, yyy");
        }
    }
}
