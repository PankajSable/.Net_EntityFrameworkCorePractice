using Microsoft.EntityFrameworkCore;

namespace DbOperationsWithEFCore.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currencies>().HasData(
                new Currencies() { Id = 1, Title = "INR", Description = "Indian INR" },
                new Currencies() { Id = 2, Title = "Dollar", Description = "Dollar" },
                new Currencies() { Id = 3, Title = "Euro", Description = "Euro" },
                new Currencies() { Id = 4, Title = "Dinar", Description = "Dinar" }
                ) ;
            modelBuilder.Entity<Language>().HasData(
                new Language() { Id = 1, Title = "Hindi", Description = "Hindi" },
                new Language() { Id = 2, Title = "English", Description = "English" },
                new Language() { Id = 3, Title = "Marathi", Description = "Marathi" },
                new Language() { Id = 4, Title = "Spenish", Description = "Spenish" }
                );
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Currencies> Currencies { get; set; }
        public DbSet<BookPrice> BookPrice { get; set; }
        public DbSet<Author> Authors { get; set; }
       
    }
}
