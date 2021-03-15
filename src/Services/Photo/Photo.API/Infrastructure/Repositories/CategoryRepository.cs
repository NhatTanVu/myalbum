using System.Collections.Generic;
using System.Linq;
using MyAlbum.Services.Photo.API.Core;
using MyAlbum.Services.Photo.API.Core.Models;

namespace MyAlbum.Services.Photo.API.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MyAlbumDbContext context;
        public CategoryRepository(MyAlbumDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Category> GetByNames(IEnumerable<string> names)
        {
            return this.context.Category.Where(cat => names.Contains(cat.Name));
        }
    }
}