using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProAgil.WebAPI.Data;

namespace ProAgil.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventoController : ControllerBase
    {       
        public readonly DataContext _context;
        public EventoController(DataContext context)
        {
            _context = context;
        }       

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await _context.Eventos.ToListAsync();
                return Ok(results);  
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Banco de dados falhou");                
            }
            
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        { 
           try
            {
                var results = await _context.Eventos.FirstOrDefaultAsync(m => m.EventoId == id);
                return Ok(results);  
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,"Banco de dados falhou");                
            }          
        }
    }
}
