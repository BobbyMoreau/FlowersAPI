using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public FlowersController(FlowersContext context)
        {          
            _context = context;
        }

        [HttpGet()] // http://localhost:5000/api/flowers
        public async Task<IActionResult> ListAll()
        {
            var result = await _context.Flowers
            .OrderBy(f => f.Name)
            .Select(f => new{
                Family = f.Family.Name,
                Name = f.Name,
                Height = f.Height,
                Color = f.Color
            })
            .ToListAsync();

            return Ok(result);
        }

        [HttpGet("{id}")] // http://localhost:5000/api/flowers
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _context.Flowers
            .Select(f => new{
                Id = f.Id,
                Family = f.Family.Name,
                Name = f.Name,
                Height = f.Height,
                Color = f.Color
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


        

        [HttpPost]
        public async Task<IActionResult> Add(FlowerPostView flower)
        {
            if(!ModelState.IsValid) {return ValidationProblem();}
            
            if(await _context.Flowers.SingleOrDefaultAsync(f => f.Name.ToUpper() == flower.Name.ToUpper()) is not null){
                return BadRequest($"The flower {flower.Name} allready exist");
            }

            var fam = await _context.Families.SingleOrDefaultAsync(f => f.Name == flower.Family);
            if(fam is null) return NotFound($"Sorry, We couldn't find {flower.Family} ");

            var newFlower = new Flower{
                Name = flower.Name,
                Family = fam,
                Color = flower.Color,
                Height = flower.Height
                

            };

            try 
            {
            await _context.Flowers.AddAsync(newFlower);
            if (await _context.SaveChangesAsync() > 0) 
            { 
                return CreatedAtAction(nameof(GetByName), new{name=newFlower.Name},
                new 
                {
                    Id = newFlower.Id,
                    Family = newFlower.Family.Name,
                    Name = newFlower.Name,
                    Height = newFlower.Height,
                    Color = newFlower.Color
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