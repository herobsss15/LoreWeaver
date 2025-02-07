using LoreWeaver.API.Models;
using LoreWeaver.Repository.Interfaces;
using WorldForge.Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace LoreWeaver.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonagensController : ControllerBase
    {
        private readonly IPersonagemRepository _personagemRepository;

        public PersonagensController(IPersonagemRepository personagemRepository)
        {
            _personagemRepository = personagemRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PersonagemModel>> GetPersonagens()
        {
            var personagens = _personagemRepository.GetAll().Select(p => new PersonagemModel
            {
                PersonagemId = p.PersonagemId,
                MundoId = p.MundoId,
                EventoId = p.EventoId,
                CriadorId = p.CriadorId,
                NomePersonagem = p.NomePersonagem,
                DescricaoPersonagem = p.DescricaoPersonagem,
                PapelPersonagem = p.PapelPersonagem,
                Ativo = p.Ativo
            });

            return Ok(personagens);
        }

        [HttpGet("{id}")]
        public ActionResult<PersonagemModel> GetPersonagem(int id)
        {
            var personagem = _personagemRepository.GetById(id);
            if (personagem == null)
            {
                return NotFound();
            }

            var personagemModel = new PersonagemModel
            {
                PersonagemId = personagem.PersonagemId,
                MundoId = personagem.MundoId,
                EventoId = personagem.EventoId,
                CriadorId = personagem.CriadorId,
                NomePersonagem = personagem.NomePersonagem,
                DescricaoPersonagem = personagem.DescricaoPersonagem,
                PapelPersonagem = personagem.PapelPersonagem,
                Ativo = personagem.Ativo
            };

            return Ok(personagemModel);
        }

        [HttpPost]
        public ActionResult<PersonagemModel> CreatePersonagem(PersonagemModel personagemModel)
        {
            var personagem = new Personagem(
                personagemModel.NomePersonagem,
                personagemModel.DescricaoPersonagem,
                personagemModel.PapelPersonagem,
                personagemModel.CriadorId
            )
            {
                MundoId = personagemModel.MundoId,
                EventoId = personagemModel.EventoId,
                Ativo = personagemModel.Ativo
            };

            _personagemRepository.Add(personagem);

            personagemModel.PersonagemId = personagem.PersonagemId;

            return CreatedAtAction(nameof(GetPersonagem), new { id = personagemModel.PersonagemId }, personagemModel);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePersonagem(int id, PersonagemModel personagemModel)
        {
            if (id != personagemModel.PersonagemId)
            {
                return BadRequest();
            }

            var personagem = _personagemRepository.GetById(id);
            if (personagem == null)
            {
                return NotFound();
            }

            personagem.NomePersonagem = personagemModel.NomePersonagem;
            personagem.DescricaoPersonagem = personagemModel.DescricaoPersonagem;
            personagem.PapelPersonagem = personagemModel.PapelPersonagem;
            personagem.MundoId = personagemModel.MundoId;
            personagem.EventoId = personagemModel.EventoId;
            personagem.Ativo = personagemModel.Ativo;

            _personagemRepository.Update(personagem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePersonagem(int id)
        {
            var personagem = _personagemRepository.GetById(id);
            if (personagem == null)
            {
                return NotFound();
            }

            _personagemRepository.Delete(id);
            return NoContent();
        }
    }
}