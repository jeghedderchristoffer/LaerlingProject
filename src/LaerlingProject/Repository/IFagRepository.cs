using LaerlingProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaerlingProject.Repository
{
    public interface IFagRepository
    {
        Fag GetFag(int id);

        IEnumerable<Fag> GetAllFag(); 

    }
}
