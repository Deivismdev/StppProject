using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class AlbumDto
    {
        public int Id { get; set; }
        public string Title  { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
       
    }
}

        // maybe rewrite to use this black magic?
        // public record AlbumDto(int Id, string Title, string Description, DateTime CreationDate);