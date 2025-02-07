namespace LoreWeaver.API.Models
{
    public class PersonagemModel
    {
        public int PersonagemId { get; set; }
        public int MundoId { get; set; }
        public int EventoId { get; set; }
        public int CriadorId { get; set; }
        public string NomePersonagem { get; set; }
        public string DescricaoPersonagem { get; set; }
        public string PapelPersonagem { get; set; }
        public bool Ativo { get; set; }
    }
}