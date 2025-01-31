using WorldForge.Dominio.Entidades;
using LoreWeaver.Repository.Interfaces;
using System.Collections.Generic;

namespace LoreWeaver.Application.Services
{
    public class EventoService
    {
        private readonly IEventoRepository _eventoRepository;

        public EventoService(IEventoRepository eventoRepository)
        {
            _eventoRepository = eventoRepository;
        }

        public IEnumerable<Evento> GetAllEventos()
        {
            return _eventoRepository.GetAll();
        }

        public Evento GetEventoById(int id)
        {
            return _eventoRepository.GetById(id);
        }

        public void CreateEvento(Evento evento)
        {
            _eventoRepository.Add(evento);
        }

        public void UpdateEvento(Evento evento)
        {
            _eventoRepository.Update(evento);
        }

        public void DeleteEvento(int id)
        {
            _eventoRepository.Delete(id);
        }
    }
}