using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactLibrary
{
    public class Address
    {
        public long Pid { get; set; }
        public string HouseNum { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }

        public override string ToString()
        {
            return HouseNum + " " + Street + ", " + City +
                ", " + ZipCode + " " + State + ", " + Country;
        }
    }
}
