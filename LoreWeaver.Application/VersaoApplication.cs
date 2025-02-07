using WorldForge.Dominio.Entidades;
using LoreWeaver.Repository.Interfaces;
using System.Collections.Generic;

namespace LoreWeaver.Application.Services
{
    public class VersaoService
    {
        private readonly IVersaoRepository _versaoRepository;

        public VersaoService(IVersaoRepository versaoRepository)
        {
            _versaoRepository = versaoRepository;
        }

        public IEnumerable<Versao> GetAllVersoes()
        {
            return _versaoRepository.GetAll();
        }

        public Versao GetVersaoById(int id)
        {
            return _versaoRepository.GetById(id);
        }

        public void CreateVersao(Versao versao)
        {
            _versaoRepository.Add(versao);
        }

        public void UpdateVersao(Versao versao)
        {
            _versaoRepository.Update(versao);
        }

        public void DeleteVersao(int id)
        {
            _versaoRepository.Delete(id);
        }
    }
}