using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentHelper.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        [JsonIgnore]
        public string Salt { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        [JsonIgnore]
        public string ConfirmationCode { get; set; }
        public virtual UserDetails UserDetails { get; set; }
        [JsonIgnore]
        public virtual ICollection<Course> Favorites { get; set; }
        [NotMapped]
        public List<int> FavouritesIds { get; set; }

    }
}