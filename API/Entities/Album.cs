using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace API.Entities
{
    public class Album
    {
        public int Id { get; set; }
        [Required] 
        public string Title  { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }

        public IList<Image> Images {get; set;} = new List<Image>();
    }
    
}