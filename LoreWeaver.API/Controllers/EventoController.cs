using LoreWeaver.Application.Interfaces;
using LoreWeaver.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using LoreWeaver.Application.Services;
using WorldForge.Dominio.Entidades;

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
        public ActionResult<IEnumerable<EventoModel>> GetEventos()
        {
            var eventos = _eventoService.GetAllEventos().Select(e => new EventoModel
            {
                EventoId = e.EventoId,
                NomeEvento = e.NomeEvento,
                DescricaoEvento = e.DescricaoEvento,
                DataEvento = e.DataEvento,
                Ativo = e.Ativo,
                MundoId = e.MundoId,
                CriadorId = e.CriadorId
            });
            return Ok(eventos);
        }

        [HttpGet("{id}")]
        public ActionResult<EventoModel> GetEvento(int id)
        {
            var evento = _eventoService.GetEventoById(id);
            if (evento == null)
            {
                return NotFound();
            }
            var eventoModel = new EventoModel
            {
                EventoId = evento.EventoId,
                NomeEvento = evento.NomeEvento,
                DescricaoEvento = evento.DescricaoEvento,
                DataEvento = evento.DataEvento,
                Ativo = evento.Ativo,
                MundoId = evento.MundoId,
                CriadorId = evento.CriadorId
            };
            return Ok(eventoModel);
        }

        [HttpPost]
        public ActionResult Add(EventoModel eventoModel)
        {
            var evento = new Evento(eventoModel.NomeEvento, eventoModel.DescricaoEvento, eventoModel.DataEvento, eventoModel.MundoId)
            {
                CriadorId = eventoModel.CriadorId,
                Ativo = eventoModel.Ativo
            };
            _eventoService.CreateEvento(evento);
            return CreatedAtAction(nameof(GetEvento), new { id = evento.EventoId }, eventoModel);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, EventoModel eventoModel)
        {
            if (id != eventoModel.EventoId)
            {
                return BadRequest();
            }
            var evento = new Evento(eventoModel.NomeEvento, eventoModel.DescricaoEvento, eventoModel.DataEvento, eventoModel.MundoId)
            {
                EventoId = eventoModel.EventoId,
                Ativo = eventoModel.Ativo
            };
            _eventoService.UpdateEvento(evento);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _eventoService.DeleteEvento(id);
            return NoContent();
        }
    }
}