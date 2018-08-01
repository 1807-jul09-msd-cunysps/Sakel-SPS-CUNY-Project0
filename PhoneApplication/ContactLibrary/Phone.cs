using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactLibrary
{
    public class Phone
    {
        public long Pid { get; set; }
        public string CountryCode { get; set; }
        public string AreaCode { get; set; }
        public string Number { get; set; }

        public override string ToString()
        {
            return CountryCode + "-" + AreaCode + "-" + Number;
        }
    } 
}
