using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyAlbum.Services.Album.API.Controllers.Resources;
using Models = MyAlbum.Services.Album.API.Core.Models;
using MyAlbum.Services.Album.API.Core;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using Microsoft.Extensions.Configuration;

namespace MyAlbum.Services.Album.API.Controllers
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
        private readonly IConfiguration configuration;

        public AlbumsController(IMapper mapper, IAlbumRepository albumRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.albumRepository = albumRepository;
            
            this.configuration = configuration;
            var photoUrl = this.configuration.GetValue<string>("PhotoUrl");
            this.uploadsFolderUrl = photoUrl + "/uploads";
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

            var albumResource = mapper.Map<Models.Album, AlbumResource>(album);
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
            var filter = this.mapper.Map<AlbumQueryResource, Models.AlbumQuery>(filterResource);
            var albums = await this.albumRepository.GetAlbums(filter);
            var albumResources = mapper.Map<IEnumerable<Models.Album>, IEnumerable<AlbumResource>>(albums);
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
        /// Create an album
        /// </summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType((int)System.Net.HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)System.Net.HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(AlbumResource), (int)System.Net.HttpStatusCode.OK)]
        public async Task<IActionResult> CreateAlbum([FromForm] AddAlbumResource albumResource)
        {
            if (string.IsNullOrEmpty(albumResource.Name))
            {
                return BadRequest();
            }
            else
            {
                var album = this.mapper.Map<AddAlbumResource, Models.Album>(albumResource);
                var currentUser = new Models.User()
                {
                    UserName = User.FindFirstValue(ClaimTypes.Name)
                };
                album.Author = this.userRepository.GetOrAdd(currentUser);
                this.albumRepository.Add(album);
                await this.unitOfWork.CompleteAsync();
                return Ok(mapper.Map<Models.Album, AlbumResource>(album));
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
            Models.Album album = await albumRepository.GetAsync(id);
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

                var outputAlbumResource = mapper.Map<Models.Album, AlbumResource>(album);
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
            Models.Album album = await albumRepository.GetAsync(id);
            if (album != null)
            {
                var userName = User.FindFirstValue(ClaimTypes.Name);
                if (album.Author.UserName != userName)
                    return Forbid();

                foreach (Models.Photo photo in album.Photos)
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