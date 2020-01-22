using Newtonsoft.Json;

namespace StudentHelper.Models
{
    public class Comment
    {

        public int Id { get; set; }
        public string Content { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public virtual UserDetails UserDetails { get; set; }
        [JsonIgnore]
        public virtual Post Post { get; set; }
        public int PostId { get; set; }

    }
}