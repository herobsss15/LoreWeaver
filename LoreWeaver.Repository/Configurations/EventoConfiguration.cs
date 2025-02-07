// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
// using WorldForge.Dominio.Entidades;

// namespace LoreWeaver.Repository.Configurations
// {
//     public class EventoConfiguration : IEntityTypeConfiguration<Evento>
//     {
//         public void Configure(EntityTypeBuilder<Evento> builder)
//         {
//             builder.HasKey(e => e.EventoId);

//             builder.Property(e => e.NomeEvento)
//                    .IsRequired()
//                    .HasMaxLength(100);

//             builder.Property(e => e.DescricaoEvento)
//                    .HasMaxLength(500);

//             builder.Property(e => e.Ativo)
//                    .IsRequired();

//             builder.HasOne(e => e.Mundo)
//                    .WithMany(m => m.Eventos)
//                    .HasForeignKey(e => e.MundoId);
//         }
//     }
// }