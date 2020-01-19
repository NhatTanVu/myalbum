using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Controllers;
using MyAlbum.Controllers.Resources;
using MyAlbum.Mapping;
using MyAlbum.Models;
using MyAlbum.Persistence;
using Xunit;

namespace MyAlbum.Tests.Controllers
{
    public class PhotosController_GetPhotosShould
    {
        private readonly IMapper _mapper;

        public PhotosController_GetPhotosShould()
        {
            var mapperConfig = new MapperConfiguration(cfg => {
                cfg.AddProfile<MappingProfile>();
            });
            this._mapper = mapperConfig.CreateMapper();
        }

        private List<Photo> SeedMockDb_GetPhotos_ReturnPhotos(DbContextOptions<MyAlbumDbContext> options)
        {
            List<Photo> seededPhotos = new List<Photo>();
            seededPhotos.Add(new Photo(){
                Id = 1,
                Name = "Photo 1",
                FilePath = @"C:\Photo\File\Path\1"
            });
            seededPhotos.Add(new Photo(){
                Id = 2,
                Name = "Photo 2",
                FilePath = @"C:\Photo\File\Path\2"
            });
            using (var context = new MyAlbumDbContext(options))
            {
                foreach(Photo photo in seededPhotos)
                {
                    context.Photos.Add(photo);
                    context.SaveChanges();
                }
            }
            return seededPhotos;
        }

        [Fact]
        public async Task GetPhotos_ReturnPhotos()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyAlbumDbContext>()
                .UseInMemoryDatabase(databaseName: "PhotosController_GetPhotosShould_MyAlbumDatabase")
                .Options;
            var seededPhotos = SeedMockDb_GetPhotos_ReturnPhotos(options);
            var seededPhotoResources = this._mapper.Map<IEnumerable<Photo>, IEnumerable<PhotoResource>>(seededPhotos);
            using (var context = new MyAlbumDbContext(options))
            {
                PhotosController controller = new PhotosController(context, this._mapper);
                // Act
                var photos = await controller.GetPhotos();
                // Assert
                Assert.True(seededPhotoResources.SequenceEqual(photos));
            }
        }
    }
}