using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public required string Body { get; set; }
        public DateTime CreationDate { get; set; }

        public required Image Image { get; set; }
    }
}