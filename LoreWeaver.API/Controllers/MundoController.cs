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
    public class MundosController : ControllerBase
    {
        private readonly IMundoRepository _mundoRepository;

        public MundosController(IMundoRepository mundoRepository)
        {
            _mundoRepository = mundoRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<MundoModel>> GetMundos()
        {
            var mundos = _mundoRepository.GetAll().Select(m => new MundoModel
            {
                MundoId = m.MundoId,
                NomeDoMundo = m.NomeDoMundo,
                DescricaoMundo = m.DescricaoMundo
            });

            return Ok(mundos);
        }

        [HttpGet("{id}")]
        public ActionResult<MundoModel> GetMundo(int id)
        {
            var mundo = _mundoRepository.GetById(id);
            if (mundo == null)
            {
                return NotFound();
            }

            var mundoModel = new MundoModel
            {
                MundoId = mundo.MundoId,
                NomeDoMundo = mundo.NomeDoMundo,
                DescricaoMundo = mundo.DescricaoMundo
            };

            return Ok(mundoModel);
        }

        [HttpPost]
        public ActionResult<MundoModel> CreateMundo(MundoModel mundoModel)
        {
            var mundo = new Mundo(mundoModel.NomeDoMundo, mundoModel.DescricaoMundo);

            _mundoRepository.Add(mundo);

            mundoModel.MundoId = mundo.MundoId;

            return CreatedAtAction(nameof(GetMundo), new { id = mundoModel.MundoId }, mundoModel);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMundo(int id, MundoModel mundoModel)
        {
            if (id != mundoModel.MundoId)
            {
                return BadRequest();
            }

            var mundo = _mundoRepository.GetById(id);
            if (mundo == null)
            {
                return NotFound();
            }

            mundo.NomeDoMundo = mundoModel.NomeDoMundo;
            mundo.DescricaoMundo = mundoModel.DescricaoMundo;

            _mundoRepository.Update(mundo);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMundo(int id)
        {
            var mundo = _mundoRepository.GetById(id);
            if (mundo == null)
            {
                return NotFound();
            }

            _mundoRepository.Delete(id);
            return NoContent();
        }
    }
}