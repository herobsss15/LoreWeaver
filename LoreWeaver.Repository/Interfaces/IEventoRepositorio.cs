using WorldForge.Dominio.Entidades;
using System.Collections.Generic;

namespace LoreWeaver.Repository.Interfaces
{
    public interface IEventoRepository
    {
        IEnumerable<Evento> GetAll();
        Evento GetById(int id);
        void Add(Evento evento);
        void Update(Evento evento);
        void Delete(int id);
    }
}