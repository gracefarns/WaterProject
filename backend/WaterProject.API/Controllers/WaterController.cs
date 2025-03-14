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
        public IEnumerable<Project> GetProjects()
        {
            return _waterContext.Projects.ToList();
        }

        [HttpGet("FunctionalProjects")]
        public IEnumerable<Project> GetFunctionalProjecsts()
        {
            var something = _waterContext.Projects.Where(p => p.ProjectFunctionalityStatus == "Functional").ToList();
            return something;
        }
        
    }
}
