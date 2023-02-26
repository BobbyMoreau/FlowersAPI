using flowers.api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace flowers.api.Controllers
{
    [ApiController]
    [Route("api/families")]

    public class FamiliesController : ControllerBase
    {
    
        private readonly FlowersContext _context;
        public FamiliesController(FlowersContext context)
        {          
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ListAll()
        {
            var result = await _context.Families.ToListAsync();
            return Ok(result);
        }
        [HttpGet("{name}/flowers")]

        public async Task<IActionResult> ListFlowersInThisFamily(string name)
        {
            var result = await _context.Families
            .Select(fa => new {
                Name = fa.Name,
                Flowers = fa.Flowers.Select(fl => new {
                    Id = fl.Id,
                    Name = fl.Name,
                    Color = fl.Color,
                    Height = fl.Height
                }).ToList()
            }).SingleOrDefaultAsync(c => c.Name.ToUpper() == name.ToUpper());

            return Ok(result);
        }
    }
}