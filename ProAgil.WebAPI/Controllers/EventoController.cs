using Microsoft.AspNetCore.Mvc;
using ProAgil.Repository;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.WebAPI.Controllers
{
    [ApiController]    
    [Route("[controller]")]
    public class EventoController : ControllerBase
    {       
        public readonly IProAgilRepository _repo;
        public EventoController(IProAgilRepository repo)
        {
            _repo = repo;
        } 


        [HttpGet]
        public async Task<IActionResult> Get(){
            try
            {
                var results = await _repo.GetAllEventosAsync(true);
                return Ok(results); 
            }
            catch (System.Exception)
            {               
               return this.StatusCode(StatusCodes.Status500InternalServerError,"Banco de dados falhou"); 
            }
        } 

        [HttpGet("{EventoId}")]
        public async Task<IActionResult> Get(int eventoId){
            try
            {
                var results = await _repo.GetEventoById(eventoId,true);
                return Ok(results); 
            }
            catch (System.Exception)
            {               
               return this.StatusCode(StatusCodes.Status500InternalServerError,"Banco de dados falhou"); 
            }
        }

         [HttpGet("getByTema/{Tema}")]
        public async Task<IActionResult> Get(string tema){
            try
            {
                var results = await _repo.GetAllEventosAsyncByTema(tema,true);
                return Ok(results); 
            }
            catch (System.Exception)
            {               
               return this.StatusCode(StatusCodes.Status500InternalServerError,"Banco de dados falhou"); 
            }
        }  

        [HttpPost]
        public async Task<IActionResult> Post(Evento model){
            try
            {
                _repo.Add(model);

                if(await _repo.SaveChagesAsync()){
                    return Created($"/evento/{model.Id}",model);
                }                
            }
            catch (System.Exception)
            {               
               return this.StatusCode(StatusCodes.Status500InternalServerError,"Banco de dados falhou"); 
            }

            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Put(int EventoId, Evento model){
            try
            {
                var evento = await _repo.GetEventoById(EventoId,false);
                if(evento == null) return NotFound();

                _repo.Update(model);

                if(await _repo.SaveChagesAsync()){
                    return Created($"/evento/{model.Id}",model);
                }                
            }
            catch (System.Exception)
            {               
               return this.StatusCode(StatusCodes.Status500InternalServerError,"Banco de dados falhou"); 
            }

            return BadRequest();
        } 

        [HttpDelete]
        public async Task<IActionResult> Delete(int EventoId){
            try
            {
                var evento = await _repo.GetEventoById(EventoId,false);
                if(evento == null) return NotFound();
                
                _repo.Update(evento);

                if(await _repo.SaveChagesAsync()){
                    return Ok();
                }                
            }
            catch (System.Exception)
            {               
               return this.StatusCode(StatusCodes.Status500InternalServerError,"Banco de dados falhou"); 
            }

            return BadRequest();
        }            
    }
}
 