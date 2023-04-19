
using System;
using System.Collections.Generic;

namespace DogGo2.Models.ViewModels
{
    public class ProfileViewModel
    {
        public Owner owner { get; set; }
        public List<Walker> walkers { get; set; }
        public List<Dog> dogs { get; set; }
    }
}