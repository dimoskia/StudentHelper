using System.Collections.Generic;

namespace StudentHelper.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Salt { get; set; }
        public string Password { get; set; }
        public bool Confirmed { get; set; }
        public virtual UserDetails UserDetails { get; set; }
        public virtual ICollection<Course> Favorites { get; set; }

    }
}