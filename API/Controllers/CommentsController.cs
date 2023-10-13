using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class CommentDtoValidator : AbstractValidator<CommentDto>
    {
        public CommentDtoValidator()
        {
            RuleFor(comment => comment.Body).NotNull().NotEmpty().WithMessage("Body is required.");
        }
    }

[ApiController]
[Route("api/album/{albumId}/images/{imageId}/comments")]
public class CommentsController : ControllerBase
{
    private readonly GudAppContext context;
    private readonly CommentDtoValidator commentDtoValidator;

    public CommentsController(GudAppContext context)
    {
        this.context = context;
        this.commentDtoValidator = new CommentDtoValidator();
    }

    [HttpGet]
    public IActionResult GetComments(int albumId, int imageId)
    {
        // make async
        var comments = context.Comments
            .Where(c => c.Image.Album.Id == albumId && c.Image.Id == imageId)
            .ToList();

        if (comments.Count == 0)
        {
            return NotFound();
        }

        var commentsDto = comments.Select(comment => new CommentDto
        {
            Id = comment.Id,
            Body = comment.Body,
            CreationDate = comment.CreationDate
        }).ToList();

        return Ok(commentsDto);
    }

    [HttpGet("{commentId}")]
    public IActionResult GetComment(int albumId, int imageId, int commentId)
    {
        // make async
        var comment = context.Comments
            .SingleOrDefault(c => c.Image.Album.Id == albumId && c.Image.Id == imageId && c.Id == commentId);

        if (comment == null)
        {
            return NotFound("Comment not found");
        }

        var commentDto = new CommentDto
        {
            Id = comment.Id,
            Body = comment.Body,
            CreationDate = comment.CreationDate
        };

        return Ok(commentDto);
    }

    [HttpPost]
    public IActionResult CreateComment(int albumId, int imageId, CommentDto commentDto)
    {
        var validationResult = commentDtoValidator.Validate(commentDto);
        if (!validationResult.IsValid)
        {
            return UnprocessableEntity(validationResult.Errors);
        }
        // make async
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

        var createdCommentDto = new CommentDto
        {
            Id = comment.Id,
            Body = comment.Body,
            CreationDate = comment.CreationDate
        };

        return CreatedAtAction("GetComment", new { albumId, imageId, commentId = comment.Id }, createdCommentDto);
    }

    [HttpPut("{commentId}")]
    public IActionResult UpdateComment(int albumId, int imageId, int commentId, CommentDto commentDto)
    {
        var validationResult = commentDtoValidator.Validate(commentDto);
        if (!validationResult.IsValid)
        {
            return UnprocessableEntity(validationResult.Errors);
        }
        // make async
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var comment = context.Comments.SingleOrDefault(c => c.Image.Album.Id == albumId && c.Image.Id == imageId && c.Id == commentId);

        if (comment == null)
        {
            return NotFound("Comment not found");
        }


        comment.Body = commentDto.Body;
        context.SaveChanges();
        // return DTO
        return Ok(comment);
    }

    [HttpDelete("{commentId}")]
    public IActionResult DeleteComment(int albumId, int imageId, int commentId)
    {
        // make async
        var comment = context.Comments.SingleOrDefault(c => c.Image.Album.Id == albumId && c.Image.Id == imageId && c.Id == commentId);

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