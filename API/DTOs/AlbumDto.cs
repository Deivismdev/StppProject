using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class AlbumDto
    {
        public int Id { get; set; }
        public required string Title  { get; set; }
        public required string Description { get; set; }
        public DateTime CreationDate { get; set; }
       
    }
}

        // maybe rewrite to use this black magic?
        // public record AlbumDto(int Id, string Title, string Description, DateTime CreationDate);