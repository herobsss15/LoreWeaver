namespace LoreWeaver.API.Models
{
    public class UsuarioModel
    {
        public int UsuarioId { get; set; }
        public string NomeUsuario { get; set; }
        public string EmailUsuario { get; set; }
        public string SenhaUsuario { get; set; }
        public bool Ativo { get; set; }
    }
}