using LoreWeaver.Application.Services;
using WorldForge.Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LoreWeaver.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LugaresController : ControllerBase
    {
        private readonly LugarService _lugarService;

        public LugaresController(LugarService lugarService)
        {
            _lugarService = lugarService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Lugar>> GetLugares()
        {
            return Ok(_lugarService.GetAllLugares());
        }

        [HttpGet("{id}")]
        public ActionResult<Lugar> GetLugar(int id)
        {
            var lugar = _lugarService.GetLugarById(id);
            if (lugar == null)
            {
                return NotFound();
            }
            return Ok(lugar);
        }

        [HttpPost]
        public ActionResult<Lugar> CreateLugar(Lugar lugar)
        {
            _lugarService.CreateLugar(lugar);
            return CreatedAtAction(nameof(GetLugar), new { id = lugar.LugarId }, lugar);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateLugar(int id, Lugar lugar)
        {
            if (id != lugar.LugarId)
            {
                return BadRequest();
            }
            _lugarService.UpdateLugar(lugar);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteLugar(int id)
        {
            _lugarService.DeleteLugar(id);
            return NoContent();
        }
    }
}