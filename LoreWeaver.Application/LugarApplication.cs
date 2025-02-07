// using WorldForge.Dominio.Entidades;
// using LoreWeaver.Repository.Interfaces;
// using System.Collections.Generic;

// namespace LoreWeaver.Application.Services
// {
//     public class LugarService
//     {
//         private readonly ILugarRepository _lugarRepository;

//         public LugarService(ILugarRepository lugarRepository)
//         {
//             _lugarRepository = lugarRepository;
//         }

//         public IEnumerable<Lugar> GetAllLugares()
//         {
//             return _lugarRepository.GetAll();
//         }

//         public Lugar GetLugarById(int id)
//         {
//             return _lugarRepository.GetById(id);
//         }

//         public void CreateLugar(Lugar lugar)
//         {
//             _lugarRepository.Add(lugar);
//         }

//         public void UpdateLugar(Lugar lugar)
//         {
//             _lugarRepository.Update(lugar);
//         }

//         public void DeleteLugar(int id)
//         {
//             _lugarRepository.Delete(id);
//         }
//     }
// }