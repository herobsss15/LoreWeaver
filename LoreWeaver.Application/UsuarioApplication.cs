using WorldForge.Dominio.Entidades;
using LoreWeaver.Repository.Interfaces;
using System.Collections.Generic;

namespace LoreWeaver.Application.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public IEnumerable<Usuario> GetAllUsuarios()
        {
            return _usuarioRepository.GetAll();
        }

        public Usuario GetUsuarioById(int id)
        {
            return _usuarioRepository.GetById(id);
        }

        public void CreateUsuario(Usuario usuario)
        {
            _usuarioRepository.Add(usuario);
        }

        public void UpdateUsuario(Usuario usuario)
        {
            _usuarioRepository.Update(usuario);
        }

        public void DeleteUsuario(int id)
        {
            _usuarioRepository.Delete(id);
        }
    }
}