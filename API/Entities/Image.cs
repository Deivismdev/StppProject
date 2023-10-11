using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public required string Title  { get; set; }
        public required string Url  { get; set; }
        public required string Description { get; set; }
        public DateTime CreationDate { get; set; }

        public required Album Album {get; set;}

    }

}