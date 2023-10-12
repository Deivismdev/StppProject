using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// /api/album/{albumId}/images                                     GET         list    200
// /api/album/{albumId}/images/{imageId}                           GET         one     200
// /api/album/{albumId}/images                                     POST        create  201
// /api/album/{albumId}/images/{imageId}                           PUT/PATCH   edit    200 
// /api/album/{albumId}/images/{imageId}                           DELETE      remove  200/204 (nieko negrazini(204) / grazint istrinta (200))


namespace API.Controllers
{
   [ApiController]
    [Route("api/album/{albumId}/images")]
    public class ImagesController : ControllerBase
    {
        private readonly GudAppContext context;

        public ImagesController(GudAppContext context)
        {
            this.context = context;
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
                CreationDate = image.CreationDate
            };

            return Ok(imageDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateImage(int albumId, ImageDto imageDto)
        {
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
                CreationDate = DateTime.UtcNow
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
        public async Task<IActionResult> UpdateImage(int albumId, int imageId, ImageDto imageDto)
        {
            var image = await context.Images
                .Where(i => i.Album.Id == albumId && i.Id == imageId)
                .FirstOrDefaultAsync();

            if (image == null)
            {
                return NotFound();
            }

            image.Title = imageDto.Title;
            image.Url = imageDto.Url;
            image.Description = imageDto.Description;

            await context.SaveChangesAsync();

            var response = new ImageDto{
                Id = image.Id,
                Title = image.Title,
                Url = image.Url,
                Description = image.Description,
                CreationDate = image.CreationDate
            };

            return Ok(response);
        }

        [HttpDelete("{imageId}")]
        public async Task<IActionResult> DeleteImage(int albumId, int imageId)
        {
            var image = await context.Images
                .Where(i => i.Album.Id == albumId && i.Id == imageId)
                .FirstOrDefaultAsync();

            if (image == null)
            {
                return NotFound();
            }

            context.Images.Remove(image);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}