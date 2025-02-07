using WorldForge.Dominio.Entidades;
using System.Collections.Generic;

namespace LoreWeaver.Repository.Interfaces
{
    public interface IUsuarioRepository
    {
        IEnumerable<Usuario> GetAll();
        Usuario GetById(int id);
        void Add(Usuario usuario);
        void Update(Usuario usuario);
        void Delete(int id);
    }
}