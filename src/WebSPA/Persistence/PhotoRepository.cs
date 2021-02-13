using System.Collections.Generic;
using System.Threading.Tasks;
using MyAlbum.Core.Models;
using MyAlbum.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Z.EntityFramework.Plus;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace MyAlbum.Persistence
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly MyAlbumDbContext context;
        public PhotoRepository(MyAlbumDbContext context)
        {
            this.context = context;
        }

        public virtual void Add(Photo photo)
        {
            this.context.Add(photo);
        }

        public virtual async Task<Photo> GetAsync(int id, bool includeRelated = true)
        {
            if (!includeRelated)
                return await context.Photos.FindAsync(id);
            else
            {
                // TODO: Find a better implementation
                var photo = await context.Photos
                    .IncludeFilter(p => p.Author)
                    .IncludeFilter(p => p.Album)
                    .IncludeFilter(p => p.Comments.Where(c => c.ParentId == null))
                        .IncludeFilter(p => p.Comments.Where(c => c.ParentId == null)
                            .Select(c => c.Author))
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (photo == null)
                    return null;

                var photoWithCategories = await context.Photos
                    .Include(p => p.PhotoCategories)
                        .ThenInclude(pc => pc.Category)
                    .FirstOrDefaultAsync(v => v.Id == id);
                var replies = (from c1 in context.Comments
                               where (c1.Photo.Id == id && c1.ParentId == null)
                               join c2 in context.Comments on c1.Id equals c2.ParentId
                               into grpJoin
                               from r in grpJoin
                               group r by r.ParentId
                        into grp
                               select new
                               {
                                   ID = grp.Key,
                                   NumOfReplies = grp.Count()
                               }).ToList();

                foreach (var comment in photo.Comments)
                    if (replies.Any(r => r.ID == comment.Id))
                        comment.NumOfReplies = replies.First(r => r.ID == comment.Id).NumOfReplies;

                return photo;
            }
        }

        public virtual async Task<(int Height, int Width)> GetImageDimensions(IFormFile file)
        {
            // Based on https://stackoverflow.com/questions/50377114/how-to-get-the-image-width-height-when-doing-upload-in-asp-net-core
            if (file != null)
            {
                List<string> AcceptableImageExtentions = new List<string> { ".jpg", ".jpeg", ".png", ".bmp" };

                string fileExtention = System.IO.Path.GetExtension(file.FileName);

                if (AcceptableImageExtentions.Contains(fileExtention))
                {
                    using (System.IO.Stream stream = new System.IO.MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        SixLabors.ImageSharp.Formats.IImageDecoder imageDecoder;

                        if (fileExtention == ".jpeg" || fileExtention == ".jpg")
                        {
                            imageDecoder = new SixLabors.ImageSharp.Formats.Jpeg.JpegDecoder();
                        }
                        else if (fileExtention == ".png")
                        {
                            imageDecoder = new SixLabors.ImageSharp.Formats.Png.PngDecoder();
                        }
                        else
                        {
                            imageDecoder = new SixLabors.ImageSharp.Formats.Bmp.BmpDecoder();
                        }


                        if (stream.Position == stream.Length) //Check this because if your image is a .png, it might just throw an error
                        {
                            stream.Position = stream.Seek(0, SeekOrigin.Begin);
                        }

                        SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32> imageSharp = imageDecoder.Decode<SixLabors.ImageSharp.PixelFormats.Rgba32>(SixLabors.ImageSharp.Configuration.Default, stream);

                        if (imageSharp != null)
                        {
                            return (imageSharp.Height, imageSharp.Width);
                        }
                    }
                }
            }

            return (0, 0);
        }

        public virtual async Task<IEnumerable<Photo>> GetPhotos(PhotoQuery filter)
        {
            IQueryable<Photo> query = this.context.Photos
                .Include(p => p.PhotoCategories)
                .Include(p => p.Author)
                .IncludeFilter(p => p.Comments.Where(c => c.ParentId == null))
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.AuthorUserName))
                query = query.Where(a => a.Author.UserName == filter.AuthorUserName);
            if (filter.CategoryId.HasValue)
                query = query.Where(p => p.PhotoCategories.Select(pc => pc.CategoryId).Any(id => id == filter.CategoryId));
            if (filter.HasLocation.HasValue)
            {
                if (filter.HasLocation.Value)
                    query = query.Where(p => p.LocLat.HasValue && p.LocLng.HasValue);
                else
                    query = query.Where(p => !p.LocLat.HasValue || !p.LocLng.HasValue);
            }
            if (filter.AlbumId.HasValue)
            {
                query = query.Where(p => p.Album.Id == filter.AlbumId.Value);
            }
            var photos = await query.ToListAsync();
            return photos;
        }

        public virtual void Delete(Photo photo)
        {
            this.context.Remove(photo);
        }
    }
}