using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        public string Body { get; set; }
        public DateTime CreationDate { get; set; }

        public int ImageId{get;set;}
        public Image Image { get; set; }

    }
}