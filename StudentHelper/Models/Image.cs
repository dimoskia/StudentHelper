namespace StudentHelper.Models
{
    public class Image
    {
        public int Id { get; set; }
        public byte[] ImageData { get; set; }

        public static int? ExtractImageId(string ImageUrl)
        {
            if (!string.IsNullOrEmpty(ImageUrl))
            {
                string[] parts = ImageUrl.Split('/');
                return int.Parse(parts[parts.Length - 1]);
            }
            return null;
        }
    }
}