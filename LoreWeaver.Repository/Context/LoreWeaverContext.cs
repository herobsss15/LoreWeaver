using LoreWeaver.Repository.Configurations;
using Microsoft.EntityFrameworkCore;
using WorldForge.Dominio.Entidades;

namespace LoreWeaver.Repository.Data
{
    public class LoreWeaverContext : DbContext
    {
        public LoreWeaverContext(DbContextOptions<LoreWeaverContext> options) : base(options) { }

        public DbSet<Mundo> Mundos { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        // public DbSet<Lugar> Lugares { get; set; }
        public DbSet<Personagem> Personagens { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Versao> Versoes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=HEROBSSS\SQLEXPRESS;Database=LoreWeaver;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new MundoConfiguration());
            modelBuilder.ApplyConfiguration(new EventoConfiguration());
            // modelBuilder.ApplyConfiguration(new LugarConfiguration());
            modelBuilder.ApplyConfiguration(new PersonagemConfiguration());
            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new VersaoConfiguration());
        }
    }
}