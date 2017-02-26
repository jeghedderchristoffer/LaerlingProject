using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaerlingProject.Models;
using LaerlingProject.Data;
using Microsoft.EntityFrameworkCore;

namespace LaerlingProject.Repository
{
    public class LaerlingProfilRepository : ILaerlingProfilRepository
    {
        ApplicationDbContext _context;

        public LaerlingProfilRepository(ApplicationDbContext context)
        {
            _context = context; 
        }

        public LaerlingProfil CreateLaerlingProfil(LaerlingProfil newLaerlingProfil, List<int> selectedList, ApplicationUser currentUser)
        {
            _context.LaerlingProfil.Add(newLaerlingProfil);
            _context.SaveChanges();

            currentUser.LaerlingProfilID = newLaerlingProfil.LaerlingID; 

            foreach(var c in selectedList)
            {
                newLaerlingProfil.LaerlingCity.Add(new LaerlingCity()
                {
                    CityID = c,
                    LaerlingID = newLaerlingProfil.LaerlingID
                });
            }

            _context.SaveChanges(); 

            return newLaerlingProfil; 
        }

        public bool DeleteLaerlingProfil(int id)
        {
            var lp = _context.LaerlingProfil.SingleOrDefault(x => x.LaerlingID == id);
            _context.Remove(lp);
            _context.SaveChanges();

            return (_context.LaerlingProfil.SingleOrDefault(x => x.LaerlingID == id) == null);            
        }

        public IList<LaerlingProfil> GetAllLaerlingProfil()
        {
            return _context.LaerlingProfil.ToList(); 
        }

        public LaerlingProfil GetLaerlingProfil(int id)
        {
            return _context.LaerlingProfil.SingleOrDefault(x => x.LaerlingID == id); 
        }

        public LaerlingProfil UpdateLaerlingProfil(LaerlingProfil newLaerlingProfil, List<int> selectedList)
        {
            var originalLP = _context.LaerlingProfil.Include(c => c.LaerlingCity).FirstOrDefault(x => x.LaerlingID == newLaerlingProfil.LaerlingID);

            originalLP.FagId = newLaerlingProfil.FagId;
            originalLP.Speciale = newLaerlingProfil.Speciale;
            originalLP.Firma = newLaerlingProfil.Firma;
            originalLP.Hovedforløb = newLaerlingProfil.Hovedforløb;
            originalLP.ProfilTekst = newLaerlingProfil.ProfilTekst;

            if (selectedList != null)
            {
                originalLP.LaerlingCity.Clear();

                _context.SaveChanges(); 

                foreach(var v in selectedList)
                {
                    originalLP.LaerlingCity.Add(new LaerlingCity()
                    {
                        CityID = v, 
                        LaerlingID = originalLP.LaerlingID
                    }); 
                }

            }

            _context.SaveChanges();



            return newLaerlingProfil; 
        }

    }
}
