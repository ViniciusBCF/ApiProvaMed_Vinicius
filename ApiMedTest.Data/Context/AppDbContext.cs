using ApiMedTest.Domain.Entities;
using ApiMedTest.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace ApiMedTest.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Contato> Contatos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contato>(builder =>
            {
                builder.ToTable("Contatos");
                builder.HasKey(contato => contato.Id);

                builder.Property(contato => contato.NomeContato)
                    .HasMaxLength(128)
                    .IsRequired();

                builder.Property(contato => contato.DataNascimento)
                    .IsRequired();

                builder.Property(contato => contato.Sexo)
                    .IsRequired();

                builder.Property(contato => contato.StatusContato)
                    .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<Entity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(nameof(Entity.DataCriacao)).CurrentValue = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(Entity.DataCriacao)).IsModified = false;
                    entry.Property(nameof(Entity.DataAtualizacao)).CurrentValue = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
