using LoreWeaver.Application.Services;
using WorldForge.Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LoreWeaver.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly EventoService _eventoService;

        public EventosController(EventoService eventoService)
        {
            _eventoService = eventoService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Evento>> GetEventos()
        {
            return Ok(_eventoService.GetAllEventos());
        }

        [HttpGet("{id}")]
        public ActionResult<Evento> GetEvento(int id)
        {
            var evento = _eventoService.GetEventoById(id);
            if (evento == null)
            {
                return NotFound();
            }
            return Ok(evento);
        }

        [HttpPost]
        public ActionResult<Evento> CreateEvento(Evento evento)
        {
            _eventoService.CreateEvento(evento);
            return CreatedAtAction(nameof(GetEvento), new { id = evento.EventoId }, evento);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEvento(int id, Evento evento)
        {
            if (id != evento.EventoId)
            {
                return BadRequest();
            }
            _eventoService.UpdateEvento(evento);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEvento(int id)
        {
            _eventoService.DeleteEvento(id);
            return NoContent();
        }
    }
}