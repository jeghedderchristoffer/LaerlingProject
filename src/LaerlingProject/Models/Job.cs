using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaerlingProject.Models
{
    public class Job
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }
        public int FagId { get; set; }
        public virtual Fag Fag { get; set; }
        public DateTime JobDato { get; set; }
        public int? ForventetTid { get; set; }
        public int Løn { get; set; }
        public string OpgaveType { get; set; } 
        public string JobBeskrivelse { get; set; }

    }
}
 