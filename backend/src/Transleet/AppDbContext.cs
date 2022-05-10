using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Transleet.Models;

namespace Transleet
{
    public class AppDbContext:IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }


        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<Translation> Translations { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Term> Terms { get; set; }
        public DbSet<Label> Labels{ get; set; }
        public DbSet<Locale> Locales { get; set; }
    }
}
