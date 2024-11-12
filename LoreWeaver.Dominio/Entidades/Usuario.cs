namespace WorldForge.Dominio.Entidades
{
    public class Usuario
    {
        #region Atributos
        private string _nomeUsuario;
        private string _emailUsuario;
        private string _senhaUsuario;
        private bool _ativo;
        #endregion

        #region Propriedades
        public int UsuarioId { get; set; }

        public string NomeUsuario
        {
            get { return _nomeUsuario; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("O nome do usuário não pode ser vazio.");
                }
                _nomeUsuario = value;
            }
        }

        public string EmailUsuario
        {
            get { return _emailUsuario; }
            set
            {
                if (string.IsNullOrEmpty(value) || !value.Contains('@'))
                {
                    throw new ArgumentException("O email do usuário é inválido.");
                }
                _emailUsuario = value;
            }
        }

        public string SenhaUsuario
        {
            get { return _senhaUsuario; }
            set
            {
                if (string.IsNullOrEmpty(value) || value.Length < 6)
                {
                    throw new ArgumentException("A senha do usuário deve ter pelo menos 6 caracteres.");
                }
                _senhaUsuario = value;
            }
        }

        public bool Ativo
        {
            get { return _ativo; }
            set
            {
                _ativo = value;
            }
        }

        public ICollection<Mundo> Mundos { get; set; }
        public ICollection<Evento> Eventos { get; set; }
        public ICollection<Lugar> Lugares { get; set; }
        public ICollection<Personagem> Personagens { get; set; }
        #endregion

        #region Construtor
        public Usuario(string nomeUsuario, string emailUsuario, string senhaUsuario)
        {
            NomeUsuario = nomeUsuario;
            EmailUsuario = emailUsuario;
            SenhaUsuario = senhaUsuario;
            Ativo = true; // Usuário ativo por padrão no construtor.
            Mundos = new List<Mundo>();
            Eventos = new List<Evento>();
            Lugares = new List<Lugar>();
            Personagens = new List<Personagem>();
        }
        #endregion

        #region Métodos
        public void Deletar()
        {
            Ativo = false;
        }

        public void Restaurar()
        {
            Ativo = true;
        }
        #endregion
    }
}
