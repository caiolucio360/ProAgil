using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {
        private readonly ProAgilContext _context;
        public ProAgilRepository(ProAgilContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        //Gerais
        public void Add<T>(T entity) where T : class
        {
           _context.Add(entity);
        }
        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        public async Task<bool> SaveChagesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        //Evento
        public async Task<Evento[]> GetAllEventosAsync(bool IncludePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais);

            if(IncludePalestrantes){
                query =query.Include(pe => pe.PalestranteEventos).ThenInclude(p => p.Palestrante);
            }

            query = query.OrderBy(e => e.Id);

            return await query.ToArrayAsync();
        }
        public async Task<Evento> GetEventoById(int EventoId, bool IncludePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais);

            if(IncludePalestrantes){
                query =query.Include(pe => pe.PalestranteEventos).ThenInclude(p => p.Palestrante);
            }

            query = query.OrderByDescending(e => e.DataEvento)
                        .Where(e => e.Id == EventoId);
            
            return await query.FirstOrDefaultAsync();
        }
        public async Task<Evento[]> GetAllEventosAsyncByTema(string Tema, bool IncludePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais);

            if(IncludePalestrantes){
                query =query.Include(pe => pe.PalestranteEventos).ThenInclude(p => p.Palestrante);
            }

            query = query.OrderByDescending(e => e.DataEvento)
                        .Where(e => e.Tema.ToLower().Contains(Tema.ToLower()));
            
            return await query.ToArrayAsync();
        }
        

        //Palestrante
        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool IncludeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes            
            .Include(p => p.RedesSociais);

            if(IncludeEventos){
                query =query.Include(pe => pe.PalestranteEventos).ThenInclude(e => e.Evento);
            }

            query = query.OrderBy(p => p.Nome);
            
            return await query.ToArrayAsync();
        }
        public async Task<Palestrante> GetPalestranteAsyncById(int PalestranteId, bool IncludeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes            
            .Include(p => p.RedesSociais);

            if(IncludeEventos){
                query =query.Include(pe => pe.PalestranteEventos).ThenInclude(e => e.Evento);
            }

            query = query.OrderBy(p => p.Nome).Where(p => p.Id == PalestranteId);
            
            return await query.FirstOrDefaultAsync();
        }
        public async Task<Palestrante[]> GetAllPalestrantesAsyncByName(string name, bool IncludeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes            
            .Include(p => p.RedesSociais);

            if(IncludeEventos){
                query =query.Include(pe => pe.PalestranteEventos).ThenInclude(e => e.Evento);
            }

            query = query.Where(p => p.Nome.ToLower().Contains(name.ToLower()));
            
            return await query.ToArrayAsync();
        }       
    }
}