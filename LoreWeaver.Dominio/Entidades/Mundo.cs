namespace WorldForge.Dominio.Entidades
{
    public class Mundo
    {
        #region Atributos
        private string _nomeDoMundo;
        private string _descricaoMundo;
        private bool _ativo;
        #endregion

        #region Propriedades
        public int MundoId { get; set; }
        public ICollection<Personagem> Personagens { get; set; }

        public string NomeDoMundo
        {
            get { return _nomeDoMundo; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("O nome do mundo não pode ser vazio.");
                }
                _nomeDoMundo = value;
            }
        }

        public string DescricaoMundo
        {
            get { return _descricaoMundo; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("A descrição do mundo não pode ser vazia.");
                }
                _descricaoMundo = value;
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
        public Mundo(string nomeDoMundo, string descricaoMundo)
        {
            NomeDoMundo = nomeDoMundo;
            DescricaoMundo = descricaoMundo;
            Ativo = true; // Ativo por padrão no construtor.
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