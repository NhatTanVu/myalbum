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
using MyAlbum.WebSPA.Core.ObjectDetection;
using System.Linq;
using System;

namespace MyAlbum.WebSPA.Controllers
{
    [Route("/api/photos")]
    public class PhotosController : Controller
    {
        private readonly IMapper mapper;
        private readonly IPhotoRepository photoRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IUserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IPhotoUploadService photoUploadService;
        private readonly IWebHostEnvironment host;
        private readonly string uploadsFolderPath;
        private readonly string uploadsFolderUrl;
        private readonly string outputFolderPath;
        private readonly string outputFolderUrl;        
        public IObjectDetectionService objectDetectionService { get; }

        public PhotosController(IMapper mapper, 
            IPhotoRepository photoRepository, ICategoryRepository categoryRepository, IUserRepository userRepository,
            IUnitOfWork unitOfWork, IPhotoUploadService photoUploadService,
            IWebHostEnvironment host, IObjectDetectionService objectDetectionService)
        {
            this.objectDetectionService = objectDetectionService;
            this.host = host;
            this.photoUploadService = photoUploadService;
            this.unitOfWork = unitOfWork;
            this.photoRepository = photoRepository;
            this.categoryRepository = categoryRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.uploadsFolderPath = Path.Combine(host.WebRootPath, "uploads");
            this.uploadsFolderUrl = "/uploads";
            this.outputFolderPath = Path.Combine(host.WebRootPath, "uploads", "output");
            this.outputFolderUrl = "/uploads/output";
        }

        [HttpGet]
        public async Task<IEnumerable<PhotoResource>> GetPhotos()
        {
            var photos = await this.photoRepository.GetPhotos();
            var photoResources = mapper.Map<IEnumerable<Photo>, IEnumerable<PhotoResource>>(photos);
            photoResources = photoResources.Select(res =>  {
                string orgFilePath = res.FilePath;
                res.FilePath = Path.Combine(this.uploadsFolderUrl, orgFilePath);
                res.BoundingBoxFilePath = string.Format("{0}/{1}", this.outputFolderUrl, orgFilePath);
                return res;
            });

            return photoResources;
        }

        [HttpPost]
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
                string imageFilePath = Path.Combine(this.uploadsFolderPath, photo.FilePath);
                var boxDicts = this.objectDetectionService.DetectObjectsFromImages(new List<string>(){ imageFilePath }, this.uploadsFolderPath, this.outputFolderPath);
                var labels = boxDicts[imageFilePath].Select(b => b.Label);
                if (labels.Any()) {
                    var categories = this.categoryRepository.GetByNames(labels);
                    photo.PhotoCategories = categories.Select(cat => new PhotoCategory(){
                        Category = cat,
                        Photo = photo
                    }).ToList();
                }
                photo.Author = await this.userRepository.GetAsync(MyAlbum.Core.Models.User.AnonymousUser.Id); // TODO: Set correct user
            }
            this.photoRepository.Add(photo);
            await this.unitOfWork.CompleteAsync();

            return Ok(mapper.Map<Photo, PhotoResource>(photo));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photo = await photoRepository.GetAsync(id);

            if (photo == null)
                return NotFound();

            var photoResource = mapper.Map<Photo, PhotoResource>(photo);
            string orgFilePath = photoResource.FilePath;
            photoResource.FilePath = Path.Combine(this.uploadsFolderUrl,  orgFilePath);
            photoResource.BoundingBoxFilePath = string.Format("{0}/{1}", this.outputFolderUrl, orgFilePath);

            return Ok(photoResource);
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