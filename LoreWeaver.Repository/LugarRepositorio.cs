// using WorldForge.Dominio.Entidades;
// using LoreWeaver.Repository.Interfaces;
// using System.Collections.Generic;
// using System.Linq;
// using LoreWeaver.Repository.Data;

// namespace LoreWeaver.Repository.Implementations
// {
//     public class LugarRepository : ILugarRepository
//     {
//         private readonly LoreWeaverContext _context;

//         public LugarRepository(LoreWeaverContext context)
//         {
//             _context = context;
//         }

//         public IEnumerable<Lugar> GetAll()
//         {
//             return _context.Lugares.ToList();
//         }

//         public Lugar GetById(int id)
//         {
//             return _context.Lugares.Find(id);
//         }

//         public void Add(Lugar lugar)
//         {
//             _context.Lugares.Add(lugar);
//             _context.SaveChanges();
//         }

//         public void Update(Lugar lugar)
//         {
//             _context.Lugares.Update(lugar);
//             _context.SaveChanges();
//         }

//         public void Delete(int id)
//         {
//             var lugar = _context.Lugares.Find(id);
//             if (lugar != null)
//             {
//                 _context.Lugares.Remove(lugar);
//                 _context.SaveChanges();
//             }
//         }
//     }
// }