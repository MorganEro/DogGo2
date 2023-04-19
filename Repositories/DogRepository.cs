using DogGo2.Models;
using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace DogGo2.Repositories
{
    public class DogRepository : IDogRepository
    {
        private readonly IConfiguration _config;
        public DogRepository(IConfiguration config)
        {
            _config = config;
        }
        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Dog> GetAllDogs()
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                          SELECT d.Id AS DogID, d.Name AS DogName, d.OwnerId, d.Breed, d.Notes, d.ImageUrl, 
                          o.Name as OwnerName, o.Id AS OwnerID 
                          FROM Dog d LEFT JOIN Owner o ON o.Id = d.OwnerId
                    ";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Dog> dogs = new List<Dog>();
                        while (reader.Read())
                        {
                            Owner owner = new Owner
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("OwnerID")),
                                Name = reader.GetString(reader.GetOrdinal("OwnerName")),
                            };
                            Dog dog = new Dog
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("DogID")),
                                Name = reader.GetString(reader.GetOrdinal("DogName")),
                                OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                Breed = reader.GetString(reader.GetOrdinal("Breed")),
                                owner = owner
                            };
                            if (!reader.IsDBNull(reader.GetOrdinal("Notes")))
                            {
                                dog.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                            }
                            if (!reader.IsDBNull(reader.GetOrdinal("ImageUrl")))
                            {
                                dog.ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"));
                            }
                           
                            dogs.Add(dog);                           
                        }
                        return dogs;
                    }
                }
            }
        }
        public List<Dog> GetDogsByOwnerId(int ownerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT Id, Name, Breed, Notes, ImageUrl, OwnerId 
                FROM Dog
                WHERE OwnerId = @ownerId
            ";

                    cmd.Parameters.AddWithValue("@ownerId", ownerId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Dog> dogs = new List<Dog>();

                    while (reader.Read())
                    {
                        Dog dog = new Dog()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId"))
                        };

                        // Check if optional columns are null
                        if (reader.IsDBNull(reader.GetOrdinal("Notes")) == false)
                        {
                            dog.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                        }
                        if (reader.IsDBNull(reader.GetOrdinal("ImageUrl")) == false)
                        {
                            dog.ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"));
                        }

                        dogs.Add(dog);
                    }
                    reader.Close();
                    return dogs;
                }
            }
        }

        public Dog GetDogById(int id)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                          SELECT d.Id AS DogID, d.Name AS DogName, d.OwnerId, d.Breed, d.Notes, d.ImageUrl, 
                          o.Name as OwnerName, o.Id AS OwnerID 
                          FROM Dog d LEFT JOIN Owner o ON o.Id = d.OwnerId
                          WHERE d.Id = @id
                    ";
                    cmd.Parameters.AddWithValue("@id", id);
                    Dog dog = null;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {                 
                        while (reader.Read())
                        {
                            if (dog == null)
                            {
                                Owner dogOwner = new Owner
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("OwnerID")),
                                    Name = reader.GetString(reader.GetOrdinal("OwnerName")),
                                };
                                dog = new Dog
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("DogID")),
                                    Name = reader.GetString(reader.GetOrdinal("DogName")),
                                    OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                    Breed = reader.GetString(reader.GetOrdinal("Breed")),
                                    owner= dogOwner
                                };
                                if (!reader.IsDBNull(reader.GetOrdinal("Notes")))
                                {
                                    dog.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                                }
                                if (!reader.IsDBNull(reader.GetOrdinal("ImageUrl")))
                                {
                                    dog.ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"));
                                }   
                            }                           
                        }
                        return dog;
                    }
                }
            }
        }

      
        public void AddDog(Dog dog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Dog ([Name], ImageUrl, Breed, Notes, OwnerId)
                    OUTPUT INSERTED.ID
                    VALUES (@name, @imageUrl, @breed, @notes, @ownerId);
                ";

                    cmd.Parameters.AddWithValue("@name", dog.Name);
                    cmd.Parameters.AddWithValue("@breed", dog.Breed);
                    if (dog.Notes == null)
                    {
                        cmd.Parameters.AddWithValue("@notes", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@notes", dog.Notes);
                    }
                    cmd.Parameters.AddWithValue("@imageUrl", (object)dog.ImageUrl ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ownerId", dog.OwnerId);
                    int id = (int)cmd.ExecuteScalar();

                    dog.Id = id;
                }
            }
        }
        public void RemoveDog(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Dog
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateDog(Dog dog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Dog
                            SET 
                                [Name] = @name, 
                                ImageUrl = @imageUrl, 
                                Breed = @breed, 
                                Notes = @notes, 
                                OwnerId = @ownerId
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@name", dog.Name);
                    cmd.Parameters.AddWithValue("@breed", dog.Breed);
                    if (dog.Notes == null)
                    {
                        cmd.Parameters.AddWithValue("@notes", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@notes", dog.Notes);
                    }
                    cmd.Parameters.AddWithValue("@imageUrl", (object)dog.ImageUrl ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ownerId", dog.OwnerId);
                    cmd.Parameters.AddWithValue("@id", dog.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
