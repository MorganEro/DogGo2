using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DogGo2.Models
{
    public class Walk
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Must enter how many minutes you would liKe the walk to be")]
        [DisplayName("Duration in Minutes")]
        public int Duration { get; set; }
        [Required]

        public DateTime Date { get; set; }
        public int WalkerId { get; set; }
        public Walker walker { get; set; }
        public int DogId { get; set; }
        public Dog dog { get; set; }   
        public int formatedDuration => Duration/ 60;
        public string formatedDate => Date.ToString("dd/MM/yyyy");
       

       

        
       

    }
}