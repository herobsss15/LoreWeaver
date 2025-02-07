using LoreWeaver.API.Models;
using LoreWeaver.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WorldForge.Dominio.Entidades;

namespace LoreWeaver.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonagemController : ControllerBase
    {
        private readonly IPersonagemRepository _personagemRepository;

        public PersonagemController(IPersonagemRepository personagemRepository)
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
                NomePersonagem = p.NomePersonagem,
                Descricao = p.DescricaoPersonagem, // Map Descricao
                Papel = p.PapelPersonagem // Map Papel
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
                NomePersonagem = personagem.NomePersonagem,
                Descricao = personagem.DescricaoPersonagem, // Map Descricao
                Papel = personagem.PapelPersonagem // Map Papel
            };

            return Ok(personagemModel);
        }

        [HttpPost]
        public ActionResult<PersonagemModel> CreatePersonagem(PersonagemModel personagemModel)
        {
            var personagem = new Personagem(
                personagemModel.NomePersonagem,
                personagemModel.MundoId,
                personagemModel.Descricao,
                personagemModel.Papel
            );

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

            personagem.MundoId = personagemModel.MundoId;
            personagem.NomePersonagem = personagemModel.NomePersonagem;
            personagem.DescricaoPersonagem = personagemModel.Descricao; // Update Descricao
            personagem.PapelPersonagem = personagemModel.Papel; // Update Papel

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