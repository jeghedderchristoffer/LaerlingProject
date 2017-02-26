using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaerlingProject.Models.JobViewModels
{
    public class IndexViewModel
    {
        public int CityId { get; set; }
        public int FagId { get; set; }
        public DateTime JobDato { get; set; }
        public int Løn { get; set; }
        public string JobBeskrivelse { get; set; }
    }
}
