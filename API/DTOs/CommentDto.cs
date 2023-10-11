using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public required string Body { get; set; }
        public DateTime CreationDate { get; set; }

    }
}