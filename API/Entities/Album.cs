using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using API.Auth.Entities;
using API.Auth.Model;

namespace API.Entities
{
    public class Album : IUserOwnedResource
    {
        public int Id { get; set; }
        [Required] 
        public string Title  { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }

        public IList<Image> Images {get; set;} = new List<Image>();
        [Required]
        public string UserId {get;set;}
        public User User {get;set;}
    }
    
}