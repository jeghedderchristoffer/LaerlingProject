using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaerlingProject.Models;
using LaerlingProject.Data;

namespace LaerlingProject.Repository
{
    public class CityRepository : ICityRepository
    {
        ApplicationDbContext _context;

        public CityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<City> GetAllCity()
        {
            return _context.Citys.ToList(); 
        }

        public City GetCity(int id)
        {
            return _context.Citys.SingleOrDefault(x => x.CityId == id); 
        }

        public City GetCityName(string name)
        {
            return _context.Citys.SingleOrDefault(x => x.Name == name); 
        }
    }
}
