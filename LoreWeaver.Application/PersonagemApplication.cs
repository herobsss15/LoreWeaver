using WorldForge.Dominio.Entidades;
using LoreWeaver.Repository.Interfaces;
using System.Collections.Generic;

namespace LoreWeaver.Application.Services
{
    public class PersonagemService
    {
        private readonly IPersonagemRepository _personagemRepository;

        public PersonagemService(IPersonagemRepository personagemRepository)
        {
            _personagemRepository = personagemRepository;
        }

        public IEnumerable<Personagem> GetAllPersonagens()
        {
            return _personagemRepository.GetAll();
        }

        public Personagem GetPersonagemById(int id)
        {
            return _personagemRepository.GetById(id);
        }

        public void CreatePersonagem(Personagem personagem)
        {
            _personagemRepository.Add(personagem);
        }

        public void UpdatePersonagem(Personagem personagem)
        {
            _personagemRepository.Update(personagem);
        }

        public void DeletePersonagem(int id)
        {
            _personagemRepository.Delete(id);
        }
    }
}