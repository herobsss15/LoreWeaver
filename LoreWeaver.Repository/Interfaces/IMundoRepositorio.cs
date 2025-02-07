using WorldForge.Dominio.Entidades;
using System.Collections.Generic;

namespace LoreWeaver.Repository.Interfaces
{
    public interface IMundoRepository
    {
        IEnumerable<Mundo> GetAll();
        Mundo GetById(int id);
        void Add(Mundo mundo);
        void Update(Mundo mundo);
        void Delete(int id);
    }
}