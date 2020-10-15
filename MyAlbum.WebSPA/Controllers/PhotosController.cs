using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyAlbum.WebSPA.Controllers.Resources;
using MyAlbum.Core.Models;
using MyAlbum.Core;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using MyAlbum.WebSPA.Core.ObjectDetection;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

        /// <summary>
        /// Get a list of photos by filter
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<PhotoResource>> GetPhotos([FromQuery] PhotoQueryResource filterResource)
        {
            var filter = mapper.Map<PhotoQueryResource, PhotoQuery>(filterResource);
            var photos = await this.photoRepository.GetPhotos(filter);
            var photoResources = mapper.Map<IEnumerable<Photo>, IEnumerable<PhotoResource>>(photos);
            photoResources = photoResources.Select(res =>  {
                string orgFilePath = res.FilePath;
                res.FilePath = Path.Combine(this.uploadsFolderUrl, orgFilePath);
                res.BoundingBoxFilePath = string.Format("{0}/{1}", this.outputFolderUrl, orgFilePath);
                return res;
            });

            return photoResources;
        }

        /// <summary>
        /// Create a new photo
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePhoto([FromForm] PhotoResource photoResource)
        {
            if (photoResource.FileToUpload != null)
            {
                var photo = this.mapper.Map<PhotoResource, Photo>(photoResource);                
                var fileToUpload = photoResource.FileToUpload;
                photo.FilePath = await this.photoUploadService.UploadPhoto(fileToUpload, this.uploadsFolderPath);
                var dimensions = await this.photoRepository.GetImageDimensions(fileToUpload);
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
                var currentUser = new User() {
                    UserName = User.FindFirstValue(ClaimTypes.Name)
                };
                photo.Author = this.userRepository.GetOrAdd(currentUser);

                this.photoRepository.Add(photo);
                await this.unitOfWork.CompleteAsync();

                return Ok(mapper.Map<Photo, PhotoResource>(photo));
            }
            else
                return NoContent();
        }

        /// <summary>
        /// Get a photo by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPhoto([FromRoute] int id)
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

        /// <summary>
        /// Save a photo by ID
        /// </summary>
        [HttpPost("{id}")]
        public async Task<IActionResult> GetPhoto([FromRoute] int id, [FromForm] PhotoResource photoResource)
        {

            return Ok();
        }        
    }
}