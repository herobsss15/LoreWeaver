using LoreWeaver.Application.Interfaces;
using WorldForge.Dominio.Entidades;
using LoreWeaver.Repository.Interfaces;
using System.Collections.Generic;

namespace LoreWeaver.Application.Implementations
{
    public class MundoService : IMundoService
    {
        private readonly IMundoRepository _mundoRepository;

        public MundoService(IMundoRepository mundoRepository)
        {
            _mundoRepository = mundoRepository;
        }

        public IEnumerable<Mundo> GetAllMundos()
        {
            return _mundoRepository.GetAll();
        }

        public Mundo GetMundoById(int id)
        {
            return _mundoRepository.GetById(id);
        }

        public void CreateMundo(Mundo mundo)
        {
            _mundoRepository.Add(mundo);
        }

        public void UpdateMundo(Mundo mundo)
        {
            _mundoRepository.Update(mundo);
        }

        public void DeleteMundo(int id)
        {
            _mundoRepository.Delete(id);
        }
    }
}