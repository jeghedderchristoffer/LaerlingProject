using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaerlingProject.Models
{
    public class LaerlingCity
    {
        public int LaerlingID { get; set; }
        public LaerlingProfil Laerling { get; set; }

        public int CityID { get; set; }
        public City City { get; set; }
    }
}
