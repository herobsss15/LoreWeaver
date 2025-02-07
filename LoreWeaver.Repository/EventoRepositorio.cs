// using WorldForge.Dominio.Entidades;
// using LoreWeaver.Repository.Interfaces;
// using System.Collections.Generic;
// using System.Linq;
// using LoreWeaver.Repository.Data;

// namespace LoreWeaver.Repository.Implementations
// {
//     public class EventoRepository : IEventoRepository
//     {
//         private readonly LoreWeaverContext _context;

//         public EventoRepository(LoreWeaverContext context)
//         {
//             _context = context;
//         }

//         public IEnumerable<Evento> GetAll()
//         {
//             return _context.Eventos.ToList();
//         }

//         public Evento GetById(int id)
//         {
//             return _context.Eventos.Find(id);
//         }

//         public void Add(Evento evento)
//         {
//             _context.Eventos.Add(evento);
//             _context.SaveChanges();
//         }

//         public void Update(Evento evento)
//         {
//             _context.Eventos.Update(evento);
//             _context.SaveChanges();
//         }

//         public void Delete(int id)
//         {
//             var evento = _context.Eventos.Find(id);
//             if (evento != null)
//             {
//                 _context.Eventos.Remove(evento);
//                 _context.SaveChanges();
//             }
//         }
//     }
// }