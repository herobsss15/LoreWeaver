namespace WorldForge.Dominio.Entidades
{
    public class Evento
    {
        #region Atributos
        private string _nomeEvento;
        private string _descricaoEvento;
        private DateTime _dataEvento;
        private bool _ativo;
        #endregion

        #region Propriedades
        public int EventoId { get; set; }
        public int MundoId { get; set; }
        public Mundo Mundo { get; set; }
        public ICollection<Personagem> Personagens { get; set; }


        public int CriadorId { get; set; }

        public string NomeEvento
        {
            get { return _nomeEvento; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("O nome do evento não pode ser vazio.");
                }
                _nomeEvento = value;
            }
        }

        public string DescricaoEvento
        {
            get { return _descricaoEvento; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("A descrição do evento não pode ser vazia.");
                }
                _descricaoEvento = value;
            }
        }

        public DateTime DataEvento
        {
            get { return _dataEvento; }
            set
            {
                if (value == default(DateTime))
                {
                    throw new ArgumentException("A data do evento é inválida.");
                }
                _dataEvento = value;
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
        public Evento(string nomeEvento, string descricaoEvento, DateTime dataEvento, int criadorId)
        {
            NomeEvento = nomeEvento;
            DescricaoEvento = descricaoEvento;
            DataEvento = dataEvento;
            CriadorId = criadorId;
            Ativo = true;
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
