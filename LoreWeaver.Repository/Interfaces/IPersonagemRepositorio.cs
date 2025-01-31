using WorldForge.Dominio.Entidades;
using System.Collections.Generic;

namespace LoreWeaver.Repository.Interfaces
{
    public interface IPersonagemRepository
    {
        IEnumerable<Personagem> GetAll();
        Personagem GetById(int id);
        void Add(Personagem personagem);
        void Update(Personagem personagem);
        void Delete(int id);
    }
}