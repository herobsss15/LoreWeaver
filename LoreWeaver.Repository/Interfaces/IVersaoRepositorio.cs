using WorldForge.Dominio.Entidades;
using System.Collections.Generic;

namespace LoreWeaver.Repository.Interfaces
{
    public interface IVersaoRepository
    {
        IEnumerable<Versao> GetAll();
        Versao GetById(int id);
        void Add(Versao versao);
        void Update(Versao versao);
        void Delete(int id);
    }
}