using WorldForge.Dominio.Entidades;
using LoreWeaver.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using LoreWeaver.Repository.Data;

namespace LoreWeaver.Repository.Implementations
{
    public class VersaoRepository : IVersaoRepository
    {
        private readonly LoreWeaverContext _context;

        public VersaoRepository(LoreWeaverContext context)
        {
            _context = context;
        }

        public IEnumerable<Versao> GetAll()
        {
            return _context.Versoes.ToList();
        }

        public Versao GetById(int id)
        {
            return _context.Versoes.Find(id);
        }

        public void Add(Versao versao)
        {
            _context.Versoes.Add(versao);
            _context.SaveChanges();
        }

        public void Update(Versao versao)
        {
            _context.Versoes.Update(versao);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var versao = _context.Versoes.Find(id);
            if (versao != null)
            {
                _context.Versoes.Remove(versao);
                _context.SaveChanges();
            }
        }
    }
}