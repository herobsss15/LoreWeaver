using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorldForge.Dominio.Entidades;

namespace LoreWeaver.Repository.Configurations
{
    public class LugarConfiguration : IEntityTypeConfiguration<Lugar>
    {
        public void Configure(EntityTypeBuilder<Lugar> builder)
        {
            builder.HasKey(l => l.LugarId);

            builder.Property(l => l.NomeLugar)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(l => l.DescricaoLugar)
                   .HasMaxLength(500);

            builder.Property(l => l.Ativo)
                   .IsRequired();

            builder.HasOne(l => l.Mundo)
                   .WithMany(m => m.Lugares)
                   .HasForeignKey(l => l.MundoId);
        }
    }
}