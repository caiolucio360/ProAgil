using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public interface IProAgilRepository
    {
         void Add<T>(T entity) where T : class;
         void Update<T>(T entity) where T : class;
         void Delete<T>(T entity) where T : class;
         Task<bool> SaveChagesAsync();

         //Eventos  
         Task<Evento[]> GetAllEventosAsync(bool IncludePalestrantes);
         Task<Evento> GetEventoById(int EventoId, bool IncludePalestrantes);
         Task<Evento[]> GetAllEventosAsyncByTema(string Tema, bool IncludePalestrantes);      
         
         //Palestrantes
         Task<Palestrante[]> GetAllPalestrantesAsync(bool IncludeEventos);
         Task<Palestrante> GetPalestranteAsyncById(int PalestranteId, bool IncludeEventos);
         Task<Palestrante[]> GetAllPalestrantesAsyncByName(string name, bool IncludeEventos);         
    }
}