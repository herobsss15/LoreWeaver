// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
// using WorldForge.Dominio.Entidades;

// namespace LoreWeaver.Repository.Configurations
// {
//     public class VersaoConfiguration : IEntityTypeConfiguration<Versao>
//     {
//         public void Configure(EntityTypeBuilder<Versao> builder)
//         {
//             builder.HasKey(v => v.VersaoId);

//             builder.Property(v => v.NumeroVersao)
//                    .IsRequired()
//                    .HasMaxLength(50);

//             builder.Property(v => v.DescricaoMudancas)
//                    .HasMaxLength(500);

//             builder.Property(v => v.Ativo)
//                    .IsRequired();

//             builder.HasOne(v => v.Mundo)
//                    .WithMany(m => m.Versoes)
//                    .HasForeignKey(v => v.MundoId);
//         }
//     }
// }