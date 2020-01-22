using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentHelper.Models
{
    public class Popularity
    {
        [Key, Column(Order = 0)]
        public int CourseId { get; set; }
        [Key, Column(Order = 1)]
        public int UserDetailsId { get; set; }
        [JsonIgnore]
        public virtual Course Course { get; set; }
        [JsonIgnore]
        public virtual UserDetails UserDetails { get; set; }
        public int Votes { get; set; }
    }
}