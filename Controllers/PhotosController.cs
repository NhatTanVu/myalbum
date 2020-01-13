using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Models;
using MyAlbum.Persistence;

namespace MyAlbum.Controllers
{
    public class PhotosController: Controller
    {
        private readonly MyAlbumDbContext context;

        public PhotosController(MyAlbumDbContext context)
        {
            this.context = context;
        }

        [HttpGet("/api/Photos")]
        public async Task<IEnumerable<Photo>> GetPhotos()
        {
            return await this.context.Photos.Include(p => p.Comments).Include(p => p.PhotoCategories).ToListAsync();
        }
    }
}