
using DogGo2.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace DogGo2.Repositories
{
    public interface IOwnerRepository
    {
        List<Owner> GetAllOwners();
        Owner GetOwnerById(int id);

        Owner GetOwnerByEmail(string email);

        void AddOwner(Owner owner);

        void DeleteOwner(int ownerId);

        void UpdateOwner(Owner owner);
    }
}