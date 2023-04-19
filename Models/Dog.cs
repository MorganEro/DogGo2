using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DogGo2.Models
{
    public class Dog
    {
        public int Id { get; set; }
        [DisplayName("Dog Name")]
        public string Name { get; set; }
        [Required]
        public int OwnerId { get; set; }
        public Owner owner { get; set; }

        public string ImageUrl { get; set; }
        public string Breed { get; set; }
        [Required(ErrorMessage = "We need to know is your Dog is Active, VeryActive, or Inactive")]
        public string Notes { get; set; }

        public List<Dog> Dogs { get; set; }



    }
}