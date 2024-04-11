using Microsoft.AspNetCore.Mvc;
using JobApi.Models;

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
                var jobs = _context.Jobs?.ToList();

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
                var job = _context.Jobs?.Find(id);

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
            try
            {
                if (job == null)
                {
                    return BadRequest(new { message = "O objeto Job é nulo." });
                }

                _context.Jobs?.Add(job);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetById), new { id = job.Id }, job);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao criar um novo emprego.");

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Ocorreu um erro ao processar sua solicitação." });
            }
        }

        // PUT: /jobs/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, Job job)
        {
            try
            {
                if (id != job.Id)
                {
                    return BadRequest(new { message = "O ID fornecido na URL não corresponde ao ID do objeto Job." });
                }

                var existingJob = _context.Jobs?.Find(id);
                if (existingJob == null)
                {
                    return NotFound(new { message = $"Emprego com ID {id} não encontrado." });
                }

                existingJob.Title = job.Title;
                existingJob.Description = job.Description;
                existingJob.Location = job.Location;
                existingJob.Salary = job.Salary;

                _context.SaveChanges();

                return NoContent(); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao atualizar o emprego com ID {JobId}", id);

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Ocorreu um erro ao processar sua solicitação." });
            }
        }

        // DELETE: /jobs/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var job = _context.Jobs?.Find(id);

                if (job == null)
                {
                    return NotFound(new { message = $"Emprego com ID {id} não encontrado." });
                }

                _context.Jobs?.Remove(job);
                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao excluir o emprego com ID {JobId}", id);

                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Ocorreu um erro ao processar sua solicitação." });
            }
        }

    }
}
