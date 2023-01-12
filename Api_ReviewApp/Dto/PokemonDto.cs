namespace Api_ReviewApp.Dto
{
    public class PokemonDto
    {
        //dto's are ment to limit data shared with requests
        //map pokemon to dto with automapper pack
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }


    }
}
