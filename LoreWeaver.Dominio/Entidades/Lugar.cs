// using System;
// using System.Collections.Generic;
// using WorldForge.Dominio.Entidades;

// namespace WorldForge.Dominio.Entidades
// {
//     public class Lugar
//     {
//         #region Atributos
//         private string _nomeLugar;
//         private string _descricaoLugar;
//         private string _coordenadas;
//         private bool _ativo;
//         #endregion

//         #region Propriedades
//         public int LugarId { get; set; }
//         public string NomeLugar
//         {
//             get { return _nomeLugar; }
//             set
//             {
//                 if (string.IsNullOrEmpty(value))
//                 {
//                     throw new ArgumentException("O nome do lugar não pode ser vazio.");
//                 }
//                 _nomeLugar = value;
//             }
//         }
//         public string DescricaoLugar
//         {
//             get { return _descricaoLugar; }
//             set
//             {
//                 if (string.IsNullOrEmpty(value))
//                 {
//                     throw new ArgumentException("A descrição do lugar não pode ser vazia.");
//                 }
//                 _descricaoLugar = value;
//             }
//         }
//         public string Coordenadas
//         {
//             get { return _coordenadas; }
//             set
//             {
//                 if (string.IsNullOrEmpty(value))
//                 {
//                     throw new ArgumentException("As coordenadas do lugar não podem ser vazias.");
//                 }
//                 _coordenadas = value;
//             }
//         }
//         public bool Ativo
//         {
//             get { return _ativo; }
//             set { _ativo = value; }
//         }
//         public int CriadorId { get; set; }
//         public ICollection<Evento> Eventos { get; set; }
//         #endregion

//         #region Construtor
//         public Lugar(string nomeLugar, string descricaoLugar, string coordenadas, int criadorId)
//         {
//             NomeLugar = nomeLugar;
//             DescricaoLugar = descricaoLugar;
//             Coordenadas = coordenadas;
//             CriadorId = criadorId;
//             Ativo = true; // Ativo por padrão no construtor.
//             Eventos = new List<Evento>();
//         }
//         #endregion

//         #region Métodos
//         public void Deletar()
//         {
//             Ativo = false;
//         }
//         #endregion
//     }
// }