using WorldForge.Dominio.Entidades;
using System.Collections.Generic;

namespace LoreWeaver.Application.Interfaces
{
    public interface IMundoService
    {
        IEnumerable<Mundo> GetAllMundos();
        Mundo GetMundoById(int id);
        void CreateMundo(Mundo mundo);
        void UpdateMundo(Mundo mundo);
        void DeleteMundo(int id);
    }
}