using System.ComponentModel.DataAnnotations;
using API.Auth;

namespace API.Entities
{
    public class Comment : IUserOwnedResource
    {
        public int Id { get; set; }
        [Required]
        public string Body { get; set; }
        public DateTime CreationDate { get; set; }

        public int ImageId{get;set;}
        public Image Image { get; set; }
        [Required]
        public string UserId {get;set;}
        public User User {get;set;}
    }
}