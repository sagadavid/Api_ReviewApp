using Api_ReviewApp.Data;
using Api_ReviewApp.Interfaces;
using Api_ReviewApp.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_ReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        //field
        private readonly DataContext _datacontext;

        //ctor
        public CategoryRepository(DataContext dataContext)
        {
            _datacontext = dataContext;
        }

        //implement interface
        public bool CategoryExists(int categoryId)
        {
            return _datacontext.Categories.Any(c=>c.Id== categoryId);
        }

        public bool CreateCategory(Category category)
        {
            //creating requires a tracker.. which is .add()
            //tracks add, update, modify
            //state can be connected or
            //disconnected which is enttitystate.added ??? (being not connected to database context)
            _datacontext.Add(category);
            return Save();//save defined below
        }

        public ICollection<Category> GetCategories()
        {
            //only 3 .. no need to orderby
            return _datacontext.Categories.ToList();
        }

        public Category GetCategory(int categoryId)
        {
            //returning only one, use firstofdefault is good practice
            return _datacontext.Categories
                .Where(c => c.Id == categoryId).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonByCategory(int categoryId)
        {
            //because we deal with nested entity which means ..
            //a navigational property (actually a joning table)..
            //as entity framewaork required..
            //we need to explicitly show what we ask..
            //and we do that by .select (or include)..
            //saying.. we are giving id categoryId and asking corresponding pokemon via joining table..
            //because pokemoncategories navigate both!!

            return _datacontext
                .PokemonCategories
                .Where(c=>c.CategoryId==categoryId)
                .Select(c=>c.Pokemon).ToList();  
        }

        public bool Save()
        {
            //savechanges results changetracker.deteckchanges
            //returns number of result changes.. derfor er det lurt a sammenligne med 0
            var saved = _datacontext.SaveChanges();//notice saved is integer..
                                                   //cause savehanges tracks changes in amount
            return saved > 0;

            //error#02:SqlException: Cannot insert explicit value for identity column in table 'Categories' when IDENTITY_INSERT is set to OFF.
            //solution#02:mssql server/reviewappdb/newquery-->SET IDENTITY_INSERT Categories ON; .. and execute query..refresh sqlconnection
        //..or
        //dont post with id.. delete it just post name of category
        
        }
    }
}
