﻿namespace Api_ReviewApp.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gym { get; set; }

        //one to many
        public Country Country { get; set; }

        //has many
        public ICollection<PokemonOwner> PokemonOwners { get; set; }
    }
}
