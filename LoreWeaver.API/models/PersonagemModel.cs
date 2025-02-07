namespace LoreWeaver.API.Models
{
    public class PersonagemModel
    {
        public int PersonagemId { get; set; }
        public int MundoId { get; set; }
        public string NomePersonagem { get; set; }
        public string Descricao { get; set; } 
        public string Papel { get; set; } 
    }
}