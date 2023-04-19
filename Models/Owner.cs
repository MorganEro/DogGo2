using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DogGo2.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        public string Address { get; set; }
        [Phone]
        public string Phone { get; set; }
        public List<Dog> dogs { get; set; }
        public int NeighborhoodId { get; set; }
        public Neighborhood neighborhood { get; set; }

    }

   
}
