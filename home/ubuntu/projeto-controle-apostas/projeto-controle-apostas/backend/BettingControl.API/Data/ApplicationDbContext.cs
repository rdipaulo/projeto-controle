using BettingControl.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BettingControl.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Entidades Existentes
        public DbSet<User> Users { get; set; } = default!;

        // NOVAS ENTIDADES
        public DbSet<Ciclo> Ciclos { get; set; } = default!;
        public DbSet<Bet> Bets { get; set; } = default!;
        public DbSet<HistoricoBanca> HistoricoBancas { get; set; } = default!;
        public DbSet<FechamentoMensal> FechamentosMensais { get; set; } = default!;
        
        // Opcional: Configurar chaves estrangeiras, se necessário
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configurar a relação de 1:N entre User e Cycle
            modelBuilder.Entity<Ciclo>()
                .HasOne(c => c.User)
                .WithMany() // Ajuste se você adicionar ICollection<Cycle> em User
                .HasForeignKey(c => c.UserId);
                
            // Configurar a relação de 1:N entre Cycle e Bet
            modelBuilder.Entity<Bet>()
                .HasOne(b => b.Ciclo)
                .WithMany(c => c.Bets)
                .HasForeignKey(b => b.CicloId);

            modelBuilder.Entity<HistoricoBanca>()
                .HasOne(h => h.User)
                .WithMany()
                .HasForeignKey(h => h.UserId);

            modelBuilder.Entity<FechamentoMensal>()
                .HasOne(fm => fm.User)
                .WithMany()
                .HasForeignKey(fm => fm.UserId);
            modelBuilder.Entity<Bet>()
                .Property(b => b.ValorApostado)
                .HasPrecision(18, 2);

        }
    }
}