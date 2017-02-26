using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaerlingProject.Models;
using LaerlingProject.Data;

namespace LaerlingProject.Repository
{
    public class JobRepository : IJobRepository
    {
        ApplicationDbContext _context;

        public JobRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public Job AddJob(Job job)
        {
            _context.Job.Add(job);
            _context.SaveChanges();
            return job;
        }

        public bool DeleteJob(int jobId)
        {
            try
            {
                _context.Job.SingleOrDefault(j => j.Id == jobId);
                _context.SaveChanges();
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }

            return true;
        }

        public Job EditJob(Job job)
        {
            var eJob = _context.Job.FirstOrDefault(j => j.Id == job.Id);
            eJob.FagId = job.FagId;
            eJob.CityId = job.CityId;
            eJob.JobBeskrivelse = job.JobBeskrivelse;
            eJob.JobDato = job.JobDato;
            eJob.Løn = job.Løn;

            _context.SaveChanges();

            return eJob;
        }

        public IList<Job> GetAllCurrentJobs()
        {
            return _context.Job.Where(x => x.JobDato > DateTime.Now).ToList();
        }

        public IList<Job> GetAllJobs()
        {
            return _context.Job.ToList();
        }

        public IList<Job> GetAllJobsFromCity(int cityId)
        {
            return _context.Job.Where(x => x.CityId == cityId).ToList();
        }

        public IList<Job> GetAllJobsFromFag(int fagId)
        {
            return _context.Job.Where(x => x.FagId == fagId).ToList();
        }

        public Job GetJob(int jobId)
        {
            return _context.Job.SingleOrDefault(x => x.Id == jobId);
        }
    }
}
