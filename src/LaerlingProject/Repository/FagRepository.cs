using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaerlingProject.Models;
using LaerlingProject.Data;

namespace LaerlingProject.Repository
{
    public class FagRepository : IFagRepository
    {
        ApplicationDbContext _context;

        public FagRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Fag> GetAllFag()
        {
            return _context.Fag.ToList(); 
        }

        public Fag GetFag(int id)
        {
            return _context.Fag.SingleOrDefault(x => x.FagID == id); 
        }
    }
}
