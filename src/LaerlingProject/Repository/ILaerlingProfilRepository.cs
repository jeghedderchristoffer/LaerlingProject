using LaerlingProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaerlingProject.Repository
{
    public interface ILaerlingProfilRepository
    {
        LaerlingProfil GetLaerlingProfil(int id);

        IList<LaerlingProfil> GetAllLaerlingProfil();

        LaerlingProfil CreateLaerlingProfil(LaerlingProfil newLaerlingProfil, List<int> selectedList, ApplicationUser currentUser);

        LaerlingProfil UpdateLaerlingProfil(LaerlingProfil newLaerlingProfil, List<int> selectedList);

        bool DeleteLaerlingProfil(int id); 
    }
}
