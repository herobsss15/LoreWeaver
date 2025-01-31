using LoreWeaver.Application.Services;
using WorldForge.Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LoreWeaver.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonagensController : ControllerBase
    {
        private readonly PersonagemService _personagemService;

        public PersonagensController(PersonagemService personagemService)
        {
            _personagemService = personagemService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Personagem>> GetPersonagens()
        {
            return Ok(_personagemService.GetAllPersonagens());
        }

        [HttpGet("{id}")]
        public ActionResult<Personagem> GetPersonagem(int id)
        {
            var personagem = _personagemService.GetPersonagemById(id);
            if (personagem == null)
            {
                return NotFound();
            }
            return Ok(personagem);
        }

        [HttpPost]
        public ActionResult<Personagem> CreatePersonagem(Personagem personagem)
        {
            _personagemService.CreatePersonagem(personagem);
            return CreatedAtAction(nameof(GetPersonagem), new { id = personagem.PersonagemId }, personagem);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePersonagem(int id, Personagem personagem)
        {
            if (id != personagem.PersonagemId)
            {
                return BadRequest();
            }
            _personagemService.UpdatePersonagem(personagem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePersonagem(int id)
        {
            _personagemService.DeletePersonagem(id);
            return NoContent();
        }
    }
}