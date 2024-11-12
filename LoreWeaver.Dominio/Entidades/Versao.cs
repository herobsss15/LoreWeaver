namespace WorldForge.Dominio.Entidades
{
    public class Versao
    {
        #region Atributos
        private string _numeroVersao;
        private string _descricaoMudancas;
        private bool _ativo;
        #endregion

        #region Propriedades
        public int VersaoId { get; set; }
        public int MundoId { get; set; }
        public Mundo Mundo { get; set; }
        public int CriadorId { get; set; }

        public string NumeroVersao
        {
            get { return _numeroVersao; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("O número da versão não pode ser vazio.");
                }
                _numeroVersao = value;
            }
        }

        public string DescricaoMudancas
        {
            get { return _descricaoMudancas; }
            set
            {
                if (string.IsNullOrEmpty(value) || value.Length < 30)
                {
                    throw new ArgumentException("A descrição das mudanças deve ter pelo menos 30 caracteres.");
                }
                _descricaoMudancas = value;
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
        public Versao(string numeroVersao, string descricaoMudancas, int criadorId)
        {
            NumeroVersao = numeroVersao;
            DescricaoMudancas = descricaoMudancas;
            CriadorId = criadorId;
            Ativo = true;
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
