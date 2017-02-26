using LaerlingProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaerlingProject.Repository
{
    public interface ICityRepository
    {
        City GetCity(int id);

        IEnumerable<City> GetAllCity();

        City GetCityName(string name); 
    }
}
