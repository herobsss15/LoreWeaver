using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorldForge.Dominio.Entidades;

namespace LoreWeaver.Repository.Configurations
{
    public class MundoConfiguration : IEntityTypeConfiguration<Mundo>
    {
        public void Configure(EntityTypeBuilder<Mundo> builder)
        {
            builder.HasKey(m => m.MundoId);

            builder.Property(m => m.NomeDoMundo)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.DescricaoMundo)
                .HasMaxLength(500);

            builder.Property(m => m.Ativo)
                .IsRequired();

            builder.Property(m => m.CriadorId)
                .IsRequired();
        }
    }
}