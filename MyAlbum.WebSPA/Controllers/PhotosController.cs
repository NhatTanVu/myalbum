using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyAlbum.WebSPA.Controllers.Resources;
using MyAlbum.Core.Models;
using MyAlbum.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace MyAlbum.WebSPA.Controllers
{
    public class PhotosController : Controller
    {
        private readonly IMapper mapper;
        private readonly IPhotoRepository photoRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IPhotoUploadService photoUploadService;
        private readonly IWebHostEnvironment host;
        private readonly string uploadsFolderPath;

        public PhotosController(IMapper mapper, IPhotoRepository photoRepository,
            IUnitOfWork unitOfWork, IPhotoUploadService photoUploadService, IWebHostEnvironment host)
        {
            this.host = host;
            this.photoUploadService = photoUploadService;
            this.unitOfWork = unitOfWork;
            this.photoRepository = photoRepository;
            this.mapper = mapper;
            this.uploadsFolderPath = Path.Combine(host.WebRootPath, "uploads");
        }

        [HttpGet("/api/Photos")]
        public async Task<IEnumerable<PhotoResource>> GetPhotos()
        {
            var photos = await this.photoRepository.GetPhotos();
            return mapper.Map<IEnumerable<Photo>, IEnumerable<PhotoResource>>(photos);
        }
        
        [HttpPost("/api/Photo")]
        public async Task<IActionResult> CreatePhoto([FromForm] PhotoResource photoResource)
        {            
            var photo = this.mapper.Map<PhotoResource, Photo>(photoResource);
            if (photoResource.FileToUpload != null)
            {
                var fileToUpload = photoResource.FileToUpload;
                photo.FilePath = await this.photoUploadService.UploadPhoto(fileToUpload, this.uploadsFolderPath);
                var dimensions = await GetImageDimensions(fileToUpload);
                photo.Height = dimensions.Height;
                photo.Width = dimensions.Width;
            }
            this.photoRepository.Add(photo);
            await this.unitOfWork.CompleteAsync();
            return Ok(mapper.Map<Photo, PhotoResource>(photo));
        }

        private async Task<(int Height, int Width)> GetImageDimensions(IFormFile file)
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
    }
}