using LoreWeaver.Application.Services;
using WorldForge.Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LoreWeaver.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersoesController : ControllerBase
    {
        private readonly VersaoService _versaoService;

        public VersoesController(VersaoService versaoService)
        {
            _versaoService = versaoService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Versao>> GetVersoes()
        {
            return Ok(_versaoService.GetAllVersoes());
        }

        [HttpGet("{id}")]
        public ActionResult<Versao> GetVersao(int id)
        {
            var versao = _versaoService.GetVersaoById(id);
            if (versao == null)
            {
                return NotFound();
            }
            return Ok(versao);
        }

        [HttpPost]
        public ActionResult<Versao> CreateVersao(Versao versao)
        {
            _versaoService.CreateVersao(versao);
            return CreatedAtAction(nameof(GetVersao), new { id = versao.VersaoId }, versao);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateVersao(int id, Versao versao)
        {
            if (id != versao.VersaoId)
            {
                return BadRequest();
            }
            _versaoService.UpdateVersao(versao);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVersao(int id)
        {
            _versaoService.DeleteVersao(id);
            return NoContent();
        }
    }
}