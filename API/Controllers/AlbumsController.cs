using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// /api/album             GET          list    200
// /api/album/{albumId}   GET          one     200
// /api/album             POST         create  201
// /api/album/{albumId}   PUT/PATCH    edit    200
// /api/album/{albumId}   DELETE       remove  200/204 (return nothing(204) / return deleted (200))


namespace API.Controllers
{
    [ApiController]
    [Route("api/album")]
    public class AlbumsController : ControllerBase
    {
        private readonly GudAppContext context;
        public AlbumsController(GudAppContext context)
        {
            this.context = context;
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
        public IActionResult CreateAlbum(AlbumDto albumDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var album = new Album
            {
                Title = albumDto.Title,
                Description = albumDto.Description,
                CreationDate = DateTime.UtcNow
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
        public async Task<IActionResult> UpdateAlbum(int albumId, AlbumDto albumDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var album = await context.Albums.FirstOrDefaultAsync(a => a.Id == albumId);
            if (album == null)
            {
                return NotFound();
            }

            album.Title = albumDto.Title;
            album.Description = albumDto.Description;

            var updatedAlbumDto = new AlbumDto
            {
                Id = album.Id,
                Title = album.Title,
                Description = album.Description,
                CreationDate = album.CreationDate
            };

            await context.SaveChangesAsync();

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

            context.Albums.Remove(album);
            await context.SaveChangesAsync();

            return NoContent();
        }

    }
}