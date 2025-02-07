using WorldForge.Dominio.Entidades;
using LoreWeaver.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using LoreWeaver.Repository.Data;

namespace LoreWeaver.Repository.Implementations
{
    public class MundoRepository : IMundoRepository
    {
        private readonly LoreWeaverContext _context;

        public MundoRepository(LoreWeaverContext context)
        {
            _context = context;
        }

        public IEnumerable<Mundo> GetAll()
        {
            return _context.Mundos.ToList();
        }

        public Mundo GetById(int id)
        {
            return _context.Mundos.Find(id);
        }

        public void Add(Mundo mundo)
        {
            _context.Mundos.Add(mundo);
            _context.SaveChanges();
        }

        public void Update(Mundo mundo)
        {
            _context.Mundos.Update(mundo);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var mundo = _context.Mundos.Find(id);
            if (mundo != null)
            {
                _context.Mundos.Remove(mundo);
                _context.SaveChanges();
            }
        }
    }
}