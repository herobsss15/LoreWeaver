using LoreWeaver.Application.Interfaces;
using WorldForge.Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LoreWeaver.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MundosController : ControllerBase
    {
        private readonly IMundoService _mundoService;

        public MundosController(IMundoService mundoService)
        {
            _mundoService = mundoService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Mundo>> GetMundos()
        {
            return Ok(_mundoService.GetAllMundos());
        }

        [HttpGet("{id}")]
        public ActionResult<Mundo> GetMundo(int id)
        {
            var mundo = _mundoService.GetMundoById(id);
            if (mundo == null)
            {
                return NotFound();
            }
            return Ok(mundo);
        }

        [HttpPost]
        public ActionResult<Mundo> CreateMundo(Mundo mundo)
        {
            _mundoService.CreateMundo(mundo);
            return CreatedAtAction(nameof(GetMundo), new { id = mundo.MundoId }, mundo);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMundo(int id, Mundo mundo)
        {
            if (id != mundo.MundoId)
            {
                return BadRequest();
            }
            _mundoService.UpdateMundo(mundo);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMundo(int id)
        {
            _mundoService.DeleteMundo(id);
            return NoContent();
        }
    }
}