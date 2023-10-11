using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class ImageDto
    {
        public int Id { get; set; }
        public required string Title  { get; set; }
        public required string Url  { get; set; }
        public required string Description { get; set; }
        public DateTime CreationDate { get; set; }

        // public required AlbumDto Album {get; set;}

    }
}