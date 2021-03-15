using System.Collections.Generic;
using MyAlbum.Services.Photo.API.Core.Models;

namespace MyAlbum.Services.Photo.API.Core
{
    public interface ICategoryRepository
    {
         IEnumerable<Category> GetByNames(IEnumerable<string> names);
    }
}