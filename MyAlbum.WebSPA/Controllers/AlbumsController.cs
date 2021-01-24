using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyAlbum.Core;
using MyAlbum.WebSPA.Controllers.Resources;
using Microsoft.AspNetCore.Authorization;
using MyAlbum.Core.Models;
using System.Security.Claims;
using System;

namespace MyAlbum.WebSPA.Controllers
{
    [ApiExplorerSettings(GroupName = "Albums")]
    [Route("/api/albums")]
    public class AlbumsController : Controller
    {
        private readonly IMapper mapper;
        private readonly IAlbumRepository albumRepository;
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly string uploadsFolderUrl;

        public AlbumsController(IMapper mapper, IAlbumRepository albumRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.albumRepository = albumRepository;
            this.uploadsFolderUrl = "/uploads";
        }

        /// <summary>
        /// Get an album by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType((int)System.Net.HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(AlbumResource), (int)System.Net.HttpStatusCode.OK)]
        public async Task<IActionResult> GetAlbum([FromRoute] int id)
        {
            var album = await albumRepository.GetAsync(id);

            if (album == null)
                return NotFound();

            var albumResource = mapper.Map<Album, AlbumResource>(album);
            foreach (var photo in albumResource.Photos)
            {
                photo.FilePath = string.Format("{0}/{1}", this.uploadsFolderUrl, photo.FilePath);
            }
            return Ok(albumResource);
        }

        /// <summary>
        /// Get a list of albums by filter
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AlbumResource>), (int)System.Net.HttpStatusCode.OK)]
        public async Task<IEnumerable<AlbumResource>> GetAlbums([FromQuery] AlbumQueryResource filterResource)
        {
            var filter = this.mapper.Map<AlbumQueryResource, AlbumQuery>(filterResource);
            var albums = await this.albumRepository.GetAlbums(filter);
            var albumResources = mapper.Map<IEnumerable<Album>, IEnumerable<AlbumResource>>(albums);
            foreach (var album in albumResources)
            {
                foreach (var photo in album.Photos)
                {
                    photo.FilePath = string.Format("{0}/{1}", this.uploadsFolderUrl, photo.FilePath);
                }
            }
            return albumResources;
        }

        /// <summary>
        /// Create a new album
        /// </summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType((int)System.Net.HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)System.Net.HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(AlbumResource), (int)System.Net.HttpStatusCode.OK)]
        public async Task<IActionResult> CreateAlbum([FromForm] AlbumResource albumResource)
        {
            if (string.IsNullOrEmpty(albumResource.Name))
            {
                return NoContent();
            }
            else
            {
                var album = this.mapper.Map<AlbumResource, Album>(albumResource);
                var currentUser = new User()
                {
                    UserName = User.FindFirstValue(ClaimTypes.Name)
                };
                album.Author = this.userRepository.GetOrAdd(currentUser);
                this.albumRepository.Add(album);
                await this.unitOfWork.CompleteAsync();
                return Ok(mapper.Map<Album, AlbumResource>(album));
            }
        }

        /// <summary>
        /// Update an album by ID
        /// </summary>
        [HttpPost("{id}")]
        [Authorize]
        [ProducesResponseType((int)System.Net.HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)System.Net.HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)System.Net.HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)System.Net.HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(AlbumResource), (int)System.Net.HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateAlbum([FromRoute] int id, [FromForm] AlbumResource albumResource)
        {
            Album album = await albumRepository.GetAsync(id);
            if (album != null)
            {
                var userName = User.FindFirstValue(ClaimTypes.Name);
                if (album.Author.UserName != userName)
                    return Forbid();
                if (string.IsNullOrEmpty(albumResource.Name))
                    return BadRequest();

                album.Name = albumResource.Name;
                album.ModifiedDate = DateTime.UtcNow;
                await this.unitOfWork.CompleteAsync();

                var outputAlbumResource = mapper.Map<Album, AlbumResource>(album);
                return Ok(outputAlbumResource);
            }
            else
                return NotFound();
        }

        /// <summary>
        /// Delete an album by ID
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize()]
        [ProducesResponseType((int)System.Net.HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)System.Net.HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)System.Net.HttpStatusCode.NotFound)]
        [ProducesResponseType((int)System.Net.HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteAlbum([FromRoute] int id)
        {
            Album album = await albumRepository.GetAsync(id);
            if (album != null)
            {
                var userName = User.FindFirstValue(ClaimTypes.Name);
                if (album.Author.UserName != userName)
                    return Forbid();

                foreach (Photo photo in album.Photos)
                    photo.Album = null;
                this.albumRepository.Delete(album);
                await this.unitOfWork.CompleteAsync();
                return Ok();
            }
            else
                return NotFound();
        }
    }
}