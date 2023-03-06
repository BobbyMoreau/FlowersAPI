using flowers.api.Data;
using flowers.api.Entities;
using flowers.api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace flowers.api.Controllers
{
    [ApiController]
    [Route("api/flowers")]
    public class FlowersController : ControllerBase
    {
        private readonly FlowersContext _context;
        private readonly string _imageBaseUrl;
        private readonly IConfiguration _config;
        public FlowersController(FlowersContext context, IConfiguration config)
        {         
            _config = config; 
            _context = context;
            _imageBaseUrl = _config.GetSection("apiImageUrl").Value;
        }

        [HttpGet()] // http://localhost:4000/api/flowers
        public async Task<IActionResult> ListAll()
        {
            var result = await _context.Flowers
            .OrderBy(f => f.Name)
            .Select(f => new{
                Family = f.Family.Name,
                Name = f.Name,
                Height = f.Height,
                Color = f.Color,
                ImageUrl = _imageBaseUrl + f.ImageUrl ?? "field.png"
            })
            .ToListAsync();

            return Ok(result);
        }

        [HttpGet("{id}")] // http://localhost:4000/api/flowers
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _context.Flowers
            .Select(f => new{
                Id = f.Id,
                Family = f.Family.Name,
                Name = f.Name,
                Height = f.Height,
                Color = f.Color,
                ImageUrl = _imageBaseUrl + f.ImageUrl ?? "field.png"
            })
            .SingleOrDefaultAsync(c => c.Id == id);

            return Ok(result);
        }


        //Space in URL - hov to remove?
        // Did not work = _context.Flowers.SingleOrDefaultAsync(c => c.Name.Trim() == name.Trim());
        [HttpGet("Name/{name}")] 
        public async Task<IActionResult> GetByName(string name)
        {
            
            var result = await _context.Flowers.SingleOrDefaultAsync(c => c.Name == name);

            return Ok(result);
        }


        

        [HttpPost()]
        public async Task<IActionResult> Add(FlowerPostView flower)
        {
            if(!ModelState.IsValid) {return ValidationProblem();}
            
            if(await _context.Flowers.SingleOrDefaultAsync(f => f.Name.ToUpper() == flower.Name.ToUpper()) is not null){
                return BadRequest($"The flower {flower.Name} allready exist");
            }

            var fam = await _context.Families.SingleOrDefaultAsync(f => f.Name.ToUpper() == flower.Family.ToUpper());
            if(fam is null) return NotFound($"Sorry, We couldn't find {flower.Family} ");

            var newFlower = new Flower{
                Name = flower.Name,
                Family = fam,
                Color = flower.Color,
                Height = flower.Height,
                ImageUrl = "flowers.png"

            };

            try 
            {
            await _context.Flowers.AddAsync(newFlower);
            if (await _context.SaveChangesAsync() > 0) 
            { 
                return CreatedAtAction(nameof(GetById), new{id = newFlower.Id},
                new 
                {
                    Id = newFlower.Id,
                    Family = newFlower.Family.Name,
                    Name = newFlower.Name,
                    Height = newFlower.Height,
                    Color = newFlower.Color,
                    ImageUrl = newFlower.ImageUrl
                });
            }
            }
            catch (Exception e) 
            {    
                Console.WriteLine(e.Message);               
            }


            return StatusCode(500, "Internal Server Error");
            
        }
        
        // private async Task<List<FlowerListView>> CreateList()
        // {
        //         var flowers = await _context.Flowers
        //         .OrderBy(f => f.Name)
        //         .Select(b => new FlowerListView
        //         {
        //             Id = b.Id,
        //             Name = b.Name
        //         })
        //         .ToListAsync();

        //     return flowers;
        // }
        
    }
}