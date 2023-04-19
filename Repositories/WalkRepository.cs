
using DogGo2.Models;
using DogGo2.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo2.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public WalkRepository(IConfiguration config)
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
       

        public List<Walk> GetAllWalksByWalker(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT w.Id AS WalkID, w.Date, w.Duration, w.WalkerId, w.DogId , wr.Id AS WalkerID, wr.Name AS WalkerName, d.Name AS DogName
                        FROM Walks w JOIN Walker wr ON w.WalkerId = wr.Id 
                        JOIN Dog d ON d.Id = w.DogId
                        WHERE wr.Id = @id
                        
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walk> walks = new List<Walk>();
                    Dog dog = null;
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal("DogId")))
                        {
                            dog = new Dog()
                            {
                                Name = reader.GetString(reader.GetOrdinal("DogName")),
                            };
                        }
                        Walker walker = new Walker
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("WalkerID")),
                            Name = reader.GetString(reader.GetOrdinal("WalkerName"))   
                        };
                        Walk walk = new Walk
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("WalkId")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                            WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                            dog = dog,
                            walker= walker     
                        };
                        walks.Add(walk);
                    }
                    reader.Close();
                    return walks;
                }
            }
        }

        public void AddWalk(Walk walk)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                   cmd.CommandText = @"INSERT INTO Walks (Date, Duration, WalkerId, DogId) OUTPUT INSERTED ID VALUES (@date, @duration, @walkerId, @dogId);";

                    cmd.Parameters.AddWithValue("@date", walk.Date);
                    cmd.Parameters.AddWithValue("@duration", walk.Duration);
                    cmd.Parameters.AddWithValue("@walkerId", walk.WalkerId);
                    cmd.Parameters.AddWithValue("@dogId", walk.DogId);
                    int id = (int)cmd.ExecuteScalar();

                    walk.Id= id;
                }
            }

        }

 
    }
}