using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Auth.Entities;
using API.Data;
using API.Dtos;
using API.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// /api/album             GET          list    200
// /api/album/{albumId}   GET          one     200
// /api/album             POST         create  201
// /api/album/{albumId}   PUT/PATCH    edit    200
// /api/album/{albumId}   DELETE       remove  200/204 (return nothing(204) / return deleted (200))


namespace API.Controllers
{
    public class AlbumDtoValidator : AbstractValidator<AlbumDto>
    {
        public AlbumDtoValidator()
        {
            RuleFor(album => album.Title).NotNull().NotEmpty().WithMessage("Title is required.");
            RuleFor(album => album.Description).NotNull().NotEmpty().WithMessage("Description is required.");
        }
    }

    [ApiController]
    [Route("api/album")]
    public class AlbumsController : ControllerBase
    {
        // should add _ to private fields
        private readonly IAuthorizationService authorizationService;
        private readonly GudAppContext context;
        private readonly AlbumDtoValidator albumDtoValidator;
        public AlbumsController(GudAppContext context, IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
            this.context = context;
            this.albumDtoValidator = new AlbumDtoValidator();
        }

        // GET: /api/album
        [HttpGet]
        public IActionResult GetAlbums()
        {
            var albums = context.Albums.ToList();

            var albumDtos = albums.Select(album => new AlbumDto
            {
                Id = album.Id,
                Title = album.Title,
                Description = album.Description,
                CreationDate = album.CreationDate
            }).ToList();

            return Ok(albumDtos);
        }

        // GET: /api/album/{albumId}
        [HttpGet("{albumId}")]
        public IActionResult GetAlbum(int albumId)
        {
            var album = context.Albums.FirstOrDefault(a => a.Id == albumId);
            if (album == null)
            {
                return NotFound();
            }

            var albumDto = new AlbumDto
            {
                Id = album.Id,
                Title = album.Title,
                Description = album.Description,
                CreationDate = album.CreationDate
            };

            return Ok(albumDto);
        }

        // POST: /api/album
        [HttpPost]
        [Authorize(Roles = UserRoles.Member)]
        public IActionResult CreateAlbum(AlbumDto albumDto)
        {

            var validationResult = albumDtoValidator.Validate(albumDto);
            if (!validationResult.IsValid)
            {
                return UnprocessableEntity(validationResult.Errors);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var album = new Album
            {
                Title = albumDto.Title,
                Description = albumDto.Description,
                CreationDate = DateTime.UtcNow,
                UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
            };

            context.Albums.Add(album);
            context.SaveChanges();

            var createdAlbumDto = new AlbumDto
            {
                Id = album.Id,
                Title = album.Title,
                Description = album.Description,
                CreationDate = album.CreationDate
            };

            return CreatedAtAction(nameof(GetAlbum), new { albumId = album.Id }, createdAlbumDto);
        }

        // PUT: /api/album/{albumId}
        [HttpPut("{albumId}")]
        [Authorize(Roles = UserRoles.Member)]
        public async Task<IActionResult> UpdateAlbum(int albumId, AlbumDto albumDto)
        {
            var validationResult = albumDtoValidator.Validate(albumDto);
            if (!validationResult.IsValid)
            {
                return UnprocessableEntity(validationResult.Errors);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var album = await context.Albums.FirstOrDefaultAsync(a => a.Id == albumId);
            if (album == null)
            {
                return NotFound();
            }

            var authorizationResult = await authorizationService.AuthorizeAsync(User, album, PolicyNames.ResourceOwner); // this is what triggers ResourceOwnerAuthorizationHandler
            if (!authorizationResult.Succeeded)
            {
                // arba 404 jeigu nenori parodyt kad toks yra
                return Forbid();
            }

            album.Title = albumDto.Title;
            album.Description = albumDto.Description;
            await context.SaveChangesAsync();

            var updatedAlbumDto = new AlbumDto
            {
                Id = album.Id,
                Title = album.Title,
                Description = album.Description,
                CreationDate = album.CreationDate
            };

            return Ok(updatedAlbumDto);
        }
        // DELETE: /api/album/{albumId}
        [HttpDelete("{albumId}")]
        public async Task<IActionResult> DeleteAlbum(int albumId)
        {
            var album = await context.Albums.FirstOrDefaultAsync(a => a.Id == albumId);
            if (album == null)
            {
                return NotFound();
            }

            var authorizationResult = await authorizationService.AuthorizeAsync(User, album, PolicyNames.ResourceOwner);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            context.Albums.Remove(album);
            await context.SaveChangesAsync();

            return NoContent();
        }

    }
}