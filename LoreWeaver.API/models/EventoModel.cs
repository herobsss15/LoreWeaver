namespace LoreWeaver.API.Models
{
    public class EventoModel
    {
        public int EventoId { get; set; }
        public string NomeEvento { get; set; }
        public string DescricaoEvento { get; set; }
        public DateTime DataEvento { get; set; }
        public bool Ativo { get; set; }
        public int MundoId { get; set; }
        public int CriadorId { get; set; }
    }
}