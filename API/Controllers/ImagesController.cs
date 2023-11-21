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

// /api/album/{albumId}/images                                     GET         list    200
// /api/album/{albumId}/images/{imageId}                           GET         one     200
// /api/album/{albumId}/images                                     POST        create  201
// /api/album/{albumId}/images/{imageId}                           PUT/PATCH   edit    200 
// /api/album/{albumId}/images/{imageId}                           DELETE      remove  200/204 (nieko negrazini(204) / grazint istrinta (200))


namespace API.Controllers
{

    public class ImageDtoValidator : AbstractValidator<ImageDto>
    {
        public ImageDtoValidator()
        {
            RuleFor(image => image.Title).NotNull().NotEmpty().WithMessage("Title is required.");
            RuleFor(image => image.Url).NotNull().NotEmpty().WithMessage("Url is required.");
            RuleFor(image => image.Description).NotNull().NotEmpty().WithMessage("Description is required.");
        }
    }

    [ApiController]
    [Route("api/album/{albumId}/images")]
    public class ImagesController : ControllerBase
    {
        private readonly GudAppContext context;
        private readonly ImageDtoValidator imageDtoValidator;
        private readonly IAuthorizationService authorizationService;

        public ImagesController(GudAppContext context, IAuthorizationService authorizationService)
        {
            this.context = context;
            this.imageDtoValidator = new ImageDtoValidator();
            this.authorizationService = authorizationService;
        }


        [HttpGet]
        public async Task<IActionResult> GetImages(int albumId)
        {
            var images = await context.Images
                .Where(i => i.Album.Id == albumId)
                .ToListAsync();

            if (images.Count == 0)
            {
                return NotFound();
            }

            var imageDto = images.Select(image => new ImageDto
            {
                Id = image.Id,
                Title = image.Title,
                Url = image.Url,
                Description = image.Description,
                CreationDate = image.CreationDate

            }).ToList();

            return Ok(imageDto);
        }

        [HttpGet("{imageId}")]
        public async Task<IActionResult> GetImage(int albumId, int imageId)
        {
            var image = await context.Images
                .Where(i => i.Album.Id == albumId && i.Id == imageId)
                .FirstOrDefaultAsync();

            if (image == null)
            {
                return NotFound();
            }

            var imageDto = new ImageDto
            {
                Id = image.Id,
                Title = image.Title,
                Url = image.Url,
                Description = image.Description,
                CreationDate = image.CreationDate,
            };

            return Ok(imageDto);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Member)]
        public async Task<IActionResult> CreateImage(int albumId, ImageDto imageDto)
        {
            var validationResult = imageDtoValidator.Validate(imageDto);
            if (!validationResult.IsValid)
            {
                return UnprocessableEntity(validationResult.Errors);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var album = await context.Albums.FindAsync(albumId);
            if (album == null)
            {
                return NotFound("Album not found");
            }

            var image = new Image
            {
                Title = imageDto.Title,
                Url = imageDto.Url,
                Description = imageDto.Description,
                Album = album,
                CreationDate = DateTime.UtcNow,
                UserId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
            };

            context.Images.Add(image);
            await context.SaveChangesAsync();

            var createdImageDto = new ImageDto
            {
                Id = image.Id,
                Title = image.Title,
                Url = image.Url,
                Description = image.Description,
                CreationDate = image.CreationDate
            };

            return CreatedAtAction("GetImage", new { albumId, imageId = image.Id }, createdImageDto);
        }

        [HttpPut("{imageId}")]
        [Authorize(Roles = UserRoles.Member)]
        public async Task<IActionResult> UpdateImage(int albumId, int imageId, ImageDto imageDto)
        {
            var validationResult = imageDtoValidator.Validate(imageDto);
            if (!validationResult.IsValid)
            {
                return UnprocessableEntity(validationResult.Errors);
            }

            var image = await context.Images
                .Where(i => i.Album.Id == albumId && i.Id == imageId)
                .FirstOrDefaultAsync();

            if (image == null)
            {
                return NotFound();
            }
            var authorizationResult = await authorizationService.AuthorizeAsync(User, image, PolicyNames.ResourceOwner);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            image.Title = imageDto.Title;
            image.Url = imageDto.Url;
            image.Description = imageDto.Description;

            await context.SaveChangesAsync();

            var response = new ImageDto
            {
                Id = image.Id,
                Title = image.Title,
                Url = image.Url,
                Description = image.Description,
                CreationDate = image.CreationDate
            };

            return Ok(response);
        }

        [HttpDelete("{imageId}")]
        [Authorize(Roles = UserRoles.Member)]
        public async Task<IActionResult> DeleteImage(int albumId, int imageId)
        {
            var image = await context.Images
                .Where(i => i.Album.Id == albumId && i.Id == imageId)
                .FirstOrDefaultAsync();

            if (image == null)
            {
                return NotFound();
            }
            var authorizationResult = await authorizationService.AuthorizeAsync(User, image, PolicyNames.ResourceOwner);
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            context.Images.Remove(image);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}