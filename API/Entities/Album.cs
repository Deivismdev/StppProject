using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Album
    {
        public int Id { get; set; }
        public required string Title  { get; set; }
        public required string Description { get; set; }
        public DateTime CreationDate { get; set; }

    }
    
}