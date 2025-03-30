using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WaterProject.API.Data;

namespace WaterProject.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WaterController : ControllerBase
    {
        private WaterDbContext _waterContext;
        public WaterController(WaterDbContext temp) => _waterContext = temp;

        [HttpGet("AllProjects")]
        public IActionResult GetProjects(int pageHowMany = 10, int pageNum = 1,[FromQuery] List<string> projectTypes = null)
        {

            var query = _waterContext.Projects.AsQueryable();
            if (projectTypes != null)
            {
                query = query.Where(p => projectTypes.Contains(p.ProjectType));
            }

            // set cookies
            HttpContext.Response.Cookies.Append("FavoriteProjectType", "Borehole Well and Hand Pump", new CookieOptions()
            {
                HttpOnly = true, // cookie can only be seen on the server and not in javascript (good for security)
                Secure = true, // only sent through HTTPS
                SameSite = SameSiteMode.Strict,// limit if we can have cookies from other domain names (strict means aboslutely not)
                Expires = DateTime.Now.AddMinutes(1),// say when it expires
            });

            var totalNumProjects = _waterContext.Projects.Count();

            var something = query
                .Skip((pageNum - 1) * pageHowMany)
                .Take(pageHowMany)
                .ToList();

            

            var someObject = new
            {
                Projects = something,
                TotalNumProjects = totalNumProjects
            };

            return Ok(someObject);
        }

        [HttpGet("GetProjectTypes")]
        public IActionResult GetProjectTypes()
        {
            var projectTypes = _waterContext.Projects
                .Select(p => p.ProjectType)
                .Distinct()
                .ToList();

            return Ok(projectTypes);
        }

        [HttpPost("AddProject")]
        public IActionResult AddProject([FromBody] Project newProject)
        {
            _waterContext.Projects.Add(newProject);
            _waterContext.SaveChanges();
            return Ok(newProject);
        }

        [HttpPut("UpdateProject/{projectId}")]
        public IActionResult UpdateProject(int projectId, [FromBody] Project updatedProject) {
            var existingProject = _waterContext.Projects.Find(projectId);

            existingProject.ProjectName = updatedProject.ProjectName;
            existingProject.ProjectType = updatedProject.ProjectType;
            existingProject.ProjectRegionalProgram = updatedProject.ProjectRegionalProgram;
            existingProject.ProjectImpact = updatedProject.ProjectImpact;
            existingProject.ProjectPhase = updatedProject.ProjectPhase;
            existingProject.ProjectFunctionalityStatus = updatedProject.ProjectFunctionalityStatus;

            _waterContext.Projects.Update(existingProject);
            _waterContext.SaveChanges();

            return Ok(existingProject);
        }
        [HttpDelete("DeleteProject/{projectId}")]
        public IActionResult DeleteProject(int projectId) {
            var project = _waterContext.Projects.Find(projectId);

            if (project == null) {
                return NotFound(new {message = "Project not found"});
            }   

            _waterContext.Projects.Remove(project);
            _waterContext.SaveChanges();

            return NoContent();
        }
    }
}
