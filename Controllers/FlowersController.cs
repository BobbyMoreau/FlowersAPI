using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using flowers.api.Data;
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
            var result = await _context.Flowers.ToListAsync();

            return Ok(result);
        }

        [HttpGet("{id}")] // http://localhost:5000/api/flowers
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _context.Flowers.FindAsync(id);

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

        
    }
}