using Api_ReviewApp.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_ReviewApp.Data
{
    public class DataContext:DbContext
    {
        //constructor
        public DataContext(DbContextOptions<DataContext>options) : base(options)
        {
        }
    
    //set tables into databasels asin correspondance of models
    public DbSet<Pokemon> Pokemon { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Reviewer> Reviewers { get; set; }
    public DbSet<PokemonCategory> PokemonCategories { get; set; }
    public DbSet<PokemonOwner> PokemonOwners { get; set; }

        //some customization of your tables esp on relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            //commenting review: define composite key here
            // //commenting review: define joining tables (for many to many)
            
            //many to many's
            //tell composite key to entityframework
            modelBuilder.Entity<PokemonCategory>()
                .HasKey(pc => new { pc.PokemonId, pc.CategoryId });
            //tell joining table relations to establish many to many 
            modelBuilder.Entity<PokemonCategory>()
                .HasOne(p => p.Pokemon)
                .WithMany(p => p.PokemonCategories)
                .HasForeignKey(p => p.PokemonId);
            modelBuilder.Entity<PokemonCategory>()
              .HasOne(p => p.Category)
              .WithMany(p => p.PokemonCategories)
              .HasForeignKey(p => p.CategoryId);

            //many to many's
            //tell composite key to entityframework
            modelBuilder.Entity<PokemonOwner>()
                .HasKey(pc => new { pc.PokemonId, pc.OwnerId });
            //tell joining table relations to establish many to many 
            modelBuilder.Entity<PokemonOwner>()
                .HasOne(p => p.Pokemon)
                .WithMany(p => p.PokemonOwners)
                .HasForeignKey(p => p.PokemonId);
            modelBuilder.Entity<PokemonOwner>()
              .HasOne(p => p.Owner)
              .WithMany(p => p.PokemonOwners)
              .HasForeignKey(p => p.OwnerId);




        }




    }
}
