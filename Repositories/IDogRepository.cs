using DogGo2.Controllers;
using DogGo2.Models;
using System.Collections.Generic;

namespace DogGo2.Repositories
{
    public interface IDogRepository
    {
        List<Dog> GetAllDogs ();

        Dog GetDogById (int id);
        void AddDog(Dog dog);
        void RemoveDog(int id);

        void UpdateDog(Dog dog);
        List<Dog> GetDogsByOwnerId(int ownerId);



    }
}
