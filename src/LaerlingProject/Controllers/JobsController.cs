using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaerlingProject.Data;
using Microsoft.AspNetCore.Mvc;
using LaerlingProject.Models.JobViewModels;
using LaerlingProject.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using LaerlingProject.Models;

namespace LaerlingProject.Controllers
{
    public class JobsController : Controller
    {
        private IJobRepository _jobRepo;
        private UserManager<ApplicationUser> _userManager; 

        public JobsController(IJobRepository jobRepo, UserManager<ApplicationUser> userManager)
        {
            _jobRepo = jobRepo;
            _userManager = userManager; 
        }

        // GET: /Index/
        [HttpGet]
        public IActionResult Index(JobsMessageId? message = null)
        {
            ViewData["Message"] =
                message == JobsMessageId.NotAWorkerError ? "Du skal registrer dig som arbejder for at søge jobs."
                : "";

            return View(); 
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Info(int id)
        {
            var user = await GetCurrentUserAsync(); 

            if (user.LaerlingProfilID == null)
            {
                return RedirectToAction(nameof(Index), new { Message = JobsMessageId.NotAWorkerError }); 
            }
    

            var job = _jobRepo.GetJob(id); 

            return View(job); 
        }




        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        public enum JobsMessageId
        {
            NotAWorkerError, 
        }

    }
}
