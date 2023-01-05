namespace Api_ReviewApp.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //has many
        public ICollection<Owner> Owners { get; set;}
    }
}
