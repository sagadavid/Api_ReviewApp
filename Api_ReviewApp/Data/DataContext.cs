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
    
    //set tables for models
    public DbSet<Pokemon> Pokemons { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Category> Categories { get; set; }
public DbSet<Review> Reviews { get; set; }
public DbSet<Reviewer> Reviewers { get; set; }
public DbSet<PokemonCategories> PokemonCategories { get; set; }
public DbSet<PokemonOwners> PokemonOwners { get; set; }




    }
}
