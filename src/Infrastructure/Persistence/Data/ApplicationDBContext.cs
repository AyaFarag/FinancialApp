using FinancialApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace FinancialApp.Infrastructure.Persistence.Data
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser , IdentityRole, string>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Statement>()
                .HasIndex(s => new { s.UserId, s.StatementMonth }).IsUnique();

            modelBuilder.Entity<Statement>()
                .Property(s => s.RowVersion)
                .IsRowVersion();
          
        }

        public DbSet<Statement> Statements { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

    }
}
