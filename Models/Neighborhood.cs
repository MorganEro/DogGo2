using System.ComponentModel;

namespace DogGo2.Models
{
    public class Neighborhood
    {
        public int Id { get; set; }
        [DisplayName("Neighborhood")]
        public string Name { get; set; }
    }
}
