using WorldForge.Dominio.Entidades;
using System.Collections.Generic;

namespace LoreWeaver.Repository.Interfaces
{
    public interface ILugarRepository
    {
        IEnumerable<Lugar> GetAll();
        Lugar GetById(int id);
        void Add(Lugar lugar);
        void Update(Lugar lugar);
        void Delete(int id);
    }
}