// using LoreWeaver.Application.Interfaces;
// using LoreWeaver.API.Models;
// using WorldForge.Dominio.Entidades;
// using Microsoft.AspNetCore.Mvc;
// using System.Collections.Generic;
// using System.Linq;
// using LoreWeaver.Application.Services;

// namespace LoreWeaver.API.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class LugaresController : ControllerBase
//     {
//         private readonly LugarService _lugarService;

//         public LugaresController(LugarService lugarService)
//         {
//             _lugarService = lugarService;
//         }

//         [HttpGet]
//         public ActionResult<IEnumerable<LugarModel>> GetLugares()
//         {
//             var lugares = _lugarService.GetAllLugares().Select(l => new LugarModel
//             {
//                 LugarId = l.LugarId,
//                 NomeLugar = l.NomeLugar,
//                 DescricaoLugar = l.DescricaoLugar,
//                 Coordenadas = l.Coordenadas,
//                 Ativo = l.Ativo,
//                 CriadorId = l.CriadorId
//             });
//             return Ok(lugares);
//         }

//         [HttpGet("{id}")]
//         public ActionResult<LugarModel> GetLugar(int id)
//         {
//             var lugar = _lugarService.GetLugarById(id);
//             if (lugar == null)
//             {
//                 return NotFound();
//             }
//             var lugarModel = new LugarModel
//             {
//                 LugarId = lugar.LugarId,
//                 NomeLugar = lugar.NomeLugar,
//                 DescricaoLugar = lugar.DescricaoLugar,
//                 Coordenadas = lugar.Coordenadas,
//                 Ativo = lugar.Ativo,
//                 CriadorId = lugar.CriadorId
//             };
//             return Ok(lugarModel);
//         }

//         [HttpPost]
//         public ActionResult Add(LugarModel lugarModel)
//         {
//             var lugar = new Lugar(lugarModel.NomeLugar, lugarModel.DescricaoLugar, lugarModel.Coordenadas, lugarModel.CriadorId)
//             {
//                 Ativo = lugarModel.Ativo
//             };
//             _lugarService.CreateLugar(lugar);
//             return CreatedAtAction(nameof(GetLugar), new { id = lugar.LugarId }, lugarModel);
//         }

//         [HttpPut("{id}")]
//         public ActionResult Update(int id, LugarModel lugarModel)
//         {
//             if (id != lugarModel.LugarId)
//             {
//                 return BadRequest();
//             }
//             var lugar = new Lugar(lugarModel.NomeLugar, lugarModel.DescricaoLugar, lugarModel.Coordenadas, lugarModel.CriadorId)
//             {
//                 LugarId = lugarModel.LugarId,
//                 Ativo = lugarModel.Ativo
//             };
//             _lugarService.UpdateLugar(lugar);
//             return NoContent();
//         }

//         [HttpDelete("{id}")]
//         public ActionResult Delete(int id)
//         {
//             _lugarService.DeleteLugar(id);
//             return NoContent();
//         }
//     }
// }