using WorldForge.Dominio.Entidades;
using LoreWeaver.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using LoreWeaver.Repository.Data;

namespace LoreWeaver.Repository.Implementations
{
    public class PersonagemRepository : IPersonagemRepository
    {
        private readonly LoreWeaverContext _context;

        public PersonagemRepository(LoreWeaverContext context)
        {
            _context = context;
        }

        public IEnumerable<Personagem> GetAll()
        {
            return _context.Personagens.ToList();
        }

        public Personagem GetById(int id)
        {
            return _context.Personagens.Find(id);
        }

        public void Add(Personagem personagem)
        {
            _context.Personagens.Add(personagem);
            _context.SaveChanges();
        }

        public void Update(Personagem personagem)
        {
            _context.Personagens.Update(personagem);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var personagem = _context.Personagens.Find(id);
            if (personagem != null)
            {
                _context.Personagens.Remove(personagem);
                _context.SaveChanges();
            }
        }
    }
}