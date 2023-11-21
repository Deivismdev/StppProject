using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using API.Entities;

namespace API.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime CreationDate { get; set; }

    }
}