using LoreWeaver.Application.Interfaces;
using LoreWeaver.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WorldForge.Dominio.Entidades;

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
        public ActionResult<IEnumerable<MundoModel>> GetMundos()
        {
            var mundos = _mundoService.GetAllMundos().Select(m => new MundoModel
            {
                MundoId = m.MundoId,
                NomeDoMundo = m.NomeDoMundo,
                DescricaoMundo = m.DescricaoMundo,
                Ativo = m.Ativo,
                CriadorId = m.CriadorId
            });
            return Ok(mundos);
        }

        [HttpGet("{id}")]
        public ActionResult<MundoModel> GetMundo(int id)
        {
            var mundo = _mundoService.GetMundoById(id);
            if (mundo == null)
            {
                return NotFound();
            }
            var mundoModel = new MundoModel
            {
                MundoId = mundo.MundoId,
                NomeDoMundo = mundo.NomeDoMundo,
                DescricaoMundo = mundo.DescricaoMundo,
                Ativo = mundo.Ativo,
                CriadorId = mundo.CriadorId
            };
            return Ok(mundoModel);
        }

        [HttpPost]
        public ActionResult Add(MundoModel mundoModel)
        {
            var mundo = new Mundo(mundoModel.NomeDoMundo, mundoModel.DescricaoMundo, mundoModel.CriadorId)
            {
                Ativo = mundoModel.Ativo
            };
            _mundoService.CreateMundo(mundo);
            return CreatedAtAction(nameof(GetMundo), new { id = mundo.MundoId }, mundoModel);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, MundoModel mundoModel)
        {
            if (id != mundoModel.MundoId)
            {
                return BadRequest();
            }
            var mundo = new Mundo(mundoModel.NomeDoMundo, mundoModel.DescricaoMundo, mundoModel.CriadorId)
            {
                MundoId = mundoModel.MundoId,
                Ativo = mundoModel.Ativo
            };
            _mundoService.UpdateMundo(mundo);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _mundoService.DeleteMundo(id);
            return NoContent();
        }
    }
}