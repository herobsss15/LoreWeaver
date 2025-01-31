using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorldForge.Dominio.Entidades;

namespace LoreWeaver.Repository.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(u => u.UsuarioId);

            builder.Property(u => u.NomeUsuario)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.EmailUsuario)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.SenhaUsuario)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Ativo)
                   .IsRequired();
        }
    }
}