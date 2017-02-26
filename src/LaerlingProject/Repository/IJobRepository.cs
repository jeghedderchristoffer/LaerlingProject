using LaerlingProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaerlingProject.Repository
{
    public interface IJobRepository
    {
        IList<Job> GetAllJobs();
        IList<Job> GetAllCurrentJobs(); 
        IList<Job> GetAllJobsFromCity(int cityId);
        IList<Job> GetAllJobsFromFag(int fagId);
        Job GetJob(int jobId); 
        Job AddJob(Job job);
        Job EditJob(Job job); 
        bool DeleteJob(int jobId);

    }
}
