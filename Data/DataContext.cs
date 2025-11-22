using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using UniConnect.Enums;
using UniConnect.Models;

    namespace UniConnect.Data
    {
        public class DataContext : DbContext
        {
            public DataContext(DbContextOptions<DataContext> options) : base(options) { }
            public DbSet<Usuario> Usuarios { get; set; }
            public DbSet<ComunidadeTematica> ComunidadesTematicas { get; set; }
            public DbSet<ParticipacaoComunidade> ParticipacoesComunidades { get; set; }
            public DbSet<Postagem> Postagens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TPH: usa coluna Tipo (enum) como discriminator
            modelBuilder.Entity<Usuario>()
                .HasDiscriminator<TipoUsuario>("Tipo")
                .HasValue<Usuario>(TipoUsuario.Usuario)
                .HasValue<Aluno>(TipoUsuario.Aluno)
                .HasValue<Professor>(TipoUsuario.Professor)
                .HasValue<Admin>(TipoUsuario.Admin);

            // Configurações de propriedades opcionais/maxlength (exemplos)
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Comunidade -> Usuario (criação): 1 Usuario pode criar várias comunidades
            modelBuilder.Entity<ComunidadeTematica>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.ComunidadesCriadas)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Participacao: Usuario <-> Comunidade (N - N com payload)
            modelBuilder.Entity<ParticipacaoComunidade>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Participacoes)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ParticipacaoComunidade>()
                .HasOne(p => p.ComunidadeTematica)
                .WithMany(c => c.Participacoes)
                .HasForeignKey(p => p.ComunidadeTematicaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Postagem -> Usuario & Comunidade
            modelBuilder.Entity<Postagem>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Postagens)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Postagem>()
                .HasOne(p => p.ComunidadeTematica)
                .WithMany(c => c.Postagens)
                .HasForeignKey(p => p.ComunidadeTematicaId)
                .OnDelete(DeleteBehavior.Cascade);

            // opcional: restrições e tamanhos
            modelBuilder.Entity<ComunidadeTematica>()
                .Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<Postagem>()
                .Property(p => p.Conteudo)
                .IsRequired();
        }

    }

        
    }    

