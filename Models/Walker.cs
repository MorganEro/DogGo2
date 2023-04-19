using System.Collections.Generic;

namespace DogGo2.Models
{
    public class Walker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NeighborhoodId { get; set; } 
        public string ImageUrl { get; set; }
        public Neighborhood neighborhood { get; set; }

        public List<Walk> walks { get; set; }

        public string TotalWalkTime
        {
            get
            {
                int totalSeconds = 0;
                foreach (Walk walk in walks)
                {
                    totalSeconds += walk.Duration;
                }
                int hours = totalSeconds / 3600;
                int mins = (totalSeconds % 3600) / 60;
                return string.Format("{0:D2}hr {1:D2}min", hours, mins);
            }
        }



    }
}
