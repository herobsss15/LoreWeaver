using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorldForge.Dominio.Entidades;

namespace LoreWeaver.Repository.Configurations
{
    public class PersonagemConfiguration : IEntityTypeConfiguration<Personagem>
    {
        public void Configure(EntityTypeBuilder<Personagem> builder)
        {
            builder.HasKey(p => p.PersonagemId);

            builder.Property(p => p.NomePersonagem)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.DescricaoPersonagem)
                   .HasMaxLength(500);

            builder.Property(p => p.Ativo)
                   .IsRequired();

            builder.HasOne(p => p.Mundo)
                   .WithMany(m => m.Personagens)
                   .HasForeignKey(p => p.MundoId);

            builder.HasOne(p => p.Evento)
                   .WithMany(e => e.Personagens)
                   .HasForeignKey(p => p.EventoId);
        }
    }
}