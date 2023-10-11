using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
 [ApiController]
    [Route("api/album/{albumId}/images/{imageId}/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly GudAppContext context;

        public CommentsController(GudAppContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult GetComments(int albumId, int imageId)
        {
            var image = context.Images
                .Where(i => i.Album.Id == albumId && i.Id == imageId);

            return Ok(image);
        }

        [HttpGet]
        [Route("{commentId}")]
        public IActionResult GetComment(int albumId, int imageId, int commentId)
        {
            var comment = context.Comments
                .SingleOrDefault(c => c.Image.Id == imageId && c.Id == commentId);

            if (comment == null)
            {
                return NotFound("Comment not found");
            }

            return Ok(comment);
        }

        [HttpPost]
        public IActionResult CreateComment(int albumId, int imageId, CommentDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var image = context.Images.SingleOrDefault(i => i.Album.Id == albumId && i.Id == imageId);
            if (image == null)
            {
                return NotFound("Image not found");
            }

            var comment = new Comment
            {
                Body = commentDto.Body,
                Image = image,
                CreationDate = DateTime.UtcNow
            };

            context.Comments.Add(comment);
            context.SaveChanges();

            return Ok(comment);
        }

        [HttpPut]
        [Route("{commentId}")]
        public IActionResult EditComment(int albumId, int imageId, int commentId, CommentDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = context.Comments.SingleOrDefault(c => c.Image.Id == imageId && c.Id == commentId);

            if (comment == null)
            {
                return NotFound("Comment not found");
            }

            comment.Body = commentDto.Body;
            context.SaveChanges();

            return Ok(comment);
        }

        [HttpDelete]
        [Route("{commentId}")]
        public IActionResult DeleteComment(int albumId, int imageId, int commentId)
        {
            var comment = context.Comments.SingleOrDefault(c => c.Image.Id == imageId && c.Id == commentId);

            if (comment == null)
            {
                return NotFound("Comment not found");
            }

            context.Comments.Remove(comment);
            context.SaveChanges();

            return NoContent();
        }
    }
}