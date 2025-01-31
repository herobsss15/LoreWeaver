namespace WorldForge.Dominio.Entidades
{
    public class Personagem
    {
        #region Atributos
        private string _nomePersonagem;
        private string _descricaoPersonagem;
        private string _papelPersonagem;
        private bool _ativo;
        #endregion

        #region Propriedades
        public int PersonagemId { get; set; }
        public int MundoId { get; set; }
        public Mundo Mundo { get; set; }
        public int EventoId { get; set; }
        public Evento Evento { get; set; }

        public int CriadorId { get; set; }

        public string NomePersonagem
        {
            get { return _nomePersonagem; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("O nome do personagem não pode ser vazio.");
                }
                _nomePersonagem = value;
            }
        }

        public string DescricaoPersonagem
        {
            get { return _descricaoPersonagem; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("A descrição do personagem não pode ser vazia.");
                }
                _descricaoPersonagem = value;
            }
        }

        public string PapelPersonagem
        {
            get { return _papelPersonagem; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("O papel do personagem não pode ser vazio.");
                }
                _papelPersonagem = value;
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
        #endregion

        #region Construtor
        public Personagem(string nomePersonagem, string descricaoPersonagem, string papelPersonagem, int criadorId)
        {
            // As propriedades são configuradas através dos setters, que já realizam as validações necessárias.
            NomePersonagem = nomePersonagem;
            DescricaoPersonagem = descricaoPersonagem;
            PapelPersonagem = papelPersonagem;
            CriadorId = criadorId;
            Evento = null;
            Ativo = true; // Definido como ativo por padrão no construtor.
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
