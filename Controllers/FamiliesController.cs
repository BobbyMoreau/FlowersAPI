using flowers.api.Data;
using flowers.api.Entities;
using flowers.api.ViewModels.Families;
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


        [HttpPost()]
        public async Task<IActionResult> Add([FromBody]FamilyPostView family)
        {
            if(!ModelState.IsValid) return BadRequest("No I need comlete information.");
            
            if(await _context.Families.SingleOrDefaultAsync(f => f.Name.ToUpper() == family.Name.ToUpper()) is not null){
                return BadRequest($"The flower {family.Name} allready exist");
            }

             
            var newFamily = new Family{
                Name = family.Name,

            };

            try 
            {
            await _context.Families.AddAsync(newFamily);
            if (await _context.SaveChangesAsync() > 0) 
            { 
                return CreatedAtAction(nameof(GetById), new{id = newFamily.Id},
                new 
                {
  
                    Name = newFamily.Name,

                });
            }
            }
            catch (Exception e) 
            {    
                Console.WriteLine(e.Message);               
            }


            return StatusCode(500, "Internal Server Error");
            
        }

    }
}