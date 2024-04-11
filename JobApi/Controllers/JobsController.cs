using Microsoft.AspNetCore.Mvc;
using JobApi.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace JobApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly JobContext _context;

        private readonly ILogger<JobsController> _logger;

        public JobsController(JobContext context, ILogger<JobsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /jobs
        [HttpGet]
        public ActionResult<List<Job>> GetAll()
        {
            try
            {
                var jobs = _context.Jobs.ToList();

                if (jobs == null || jobs.Count == 0)
                {
                    return NotFound(new { message = "Nenhum emprego encontrado." });
                }

                return Ok(jobs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao buscar todos os empregos.");

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Ocorreu um erro ao processar sua solicitação." });
            }
        }


        // GET: /jobs/{id}
        [HttpGet("{id}")]
        public ActionResult<Job> GetById(int id)
        {
            try
            {
                var job = _context.Jobs.Find(id);

                if (job == null)
                {
                    return NotFound(new { message = $"Emprego com o ID: {id} não encontrado." });
                }

                return Ok(job);
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, "Ocorreu um erro ao buscar o emprego com ID {JobId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Ocorreu um erro ao processar sua solicitação." });

            }
        }

        // POST: /jobs
        [HttpPost]
        public ActionResult<Job> Create(Job job)
        {
            _context.Jobs.Add(job);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = job.Id }, job);
        }

        // PUT: /jobs/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, Job job)
        {
            if (id != job.Id)
            {
                return BadRequest();
            }

            var existingJob = _context.Jobs.Find(id);
            if (existingJob == null)
            {
                return NotFound();
            }

            existingJob.Title = job.Title;
            existingJob.Description = job.Description;
            existingJob.Location = job.Location;
            existingJob.Salary = job.Salary;

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: /jobs/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var job = _context.Jobs.Find(id);

            if (job == null)
            {
                return NotFound();
            }

            _context.Jobs.Remove(job);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
