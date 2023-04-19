
using System;
using System.Collections.Generic;

namespace DogGo2.Models.ViewModels
{
    public class WalkerViewModel
    {
        public Owner Owner { get; set; }
        public List<Walker> Walkers { get; set; }
        public Walk walk { get; set; }
        public Dog dog { get; set; }

        public List<Walk> walks { get; set; }
        public Walker walker { get; set; }
        public List<Dog> Dogs { get; set; }
    }
}