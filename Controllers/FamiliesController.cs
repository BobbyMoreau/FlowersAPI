using flowers.api.Data;
using flowers.api.Entities;
using flowers.api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace flowers.api.Controllers
{
    [ApiController]
    [Route("api/families")]

    public class FamiliesController : ControllerBase
    {
    
        private readonly FlowersContext _context;

        private readonly IConfiguration _config;
        public FamiliesController(FlowersContext context, IConfiguration config)
        {          
            _context = context;
            _config = config; 
        }

        [HttpGet]
        public async Task<IActionResult> ListAll()
        {
            var result = await _context.Families.ToListAsync();
            return Ok(result);
        }

        [HttpGet("{id}")] 
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _context.Families.FindAsync(id);

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