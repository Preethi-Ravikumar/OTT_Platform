using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OTT_Platform.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string MovieCategory { get; set; }
        public string Language { get; set; }    
        public string Actors { get; set; }

    }
}
