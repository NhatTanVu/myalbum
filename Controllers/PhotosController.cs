using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Controllers.Resources;
using MyAlbum.Models;
using MyAlbum.Persistence;

namespace MyAlbum.Controllers
{
    public class PhotosController : Controller
    {
        private readonly MyAlbumDbContext context;
        private readonly IMapper mapper;

        public PhotosController(MyAlbumDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }

        [HttpGet("/api/Photos")]
        public async Task<IEnumerable<PhotoResource>> GetPhotos()
        {
            var photos = await this.context.Photos.Include(p => p.Comments).Include(p => p.PhotoCategories).ToListAsync();

            return mapper.Map<List<Photo>, List<PhotoResource>>(photos);
        }
    }
}