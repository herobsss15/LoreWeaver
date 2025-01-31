using Microsoft.EntityFrameworkCore;
using WorldForge.Dominio.Entidades;
using Microsoft.EntityFrameworkCore.SqlServer;
using LoreWeaver.Repository.Configurations;

namespace LoreWeaver.Repository.Data
{
    public class LoreWeaverContext : DbContext
    {
        private readonly DbContextOptions _options;

        public LoreWeaverContext(DbContextOptions<LoreWeaverContext> options) : base(options)
        {
            _options = options;
        }

        public DbSet<Mundo> Mundos { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Lugar> Lugares { get; set; }
        public DbSet<Personagem> Personagens { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Versao> Versoes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_options == null)
                optionsBuilder.UseSqlServer(@"erver=HEROBSSS\SQLEXPRESS;Database=LoreWeaver;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new MundoConfiguration());
            // modelBuilder.ApplyConfiguration(new EventoConfiguracoes());
            // modelBuilder.ApplyConfiguration(new LugarConfiguracoes());
            // modelBuilder.ApplyConfiguration(new PersonagemConfiguracoes());
            // modelBuilder.ApplyConfiguration(new UsuarioConfiguracoes());
            // modelBuilder.ApplyConfiguration(new VersaoConfiguracoes());

            // Configurações adicionais de mapeamento podem ser adicionadas aqui
        }
    }
}