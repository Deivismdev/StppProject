using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Image
    {
        public int Id { get; set; }
        [Required]
        public string Title  { get; set; }
        [Required]
        public string Url  { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }

        [Required]
        public int AlbumId {get;set;}
        public Album Album {get; set;}

        public IList<Comment> Comments {get; set;} = new List<Comment>();

    }

}