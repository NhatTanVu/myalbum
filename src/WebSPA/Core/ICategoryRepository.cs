using System.Collections.Generic;
using MyAlbum.Core.Models;

namespace MyAlbum.Core
{
    public interface ICategoryRepository
    {
         IEnumerable<Category> GetByNames(IEnumerable<string> names);
    }
}