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
using System;

namespace MyAlbum.WebSPA.Controllers
{
    [ApiExplorerSettings(GroupName = "Photos")] 
    [Route("/api/photos")]
    public class PhotosController : Controller
    {
        private readonly IMapper mapper;
        private readonly IPhotoRepository photoRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IUserRepository userRepository;
        private readonly ICommentRepository commentRepository;
        private readonly IAlbumRepository albumRepository;
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
            ICommentRepository commentRepository, IAlbumRepository albumRepository, IUnitOfWork unitOfWork, IPhotoUploadService photoUploadService,
            IWebHostEnvironment host, IObjectDetectionService objectDetectionService)
        {
            this.objectDetectionService = objectDetectionService;
            this.host = host;
            this.photoUploadService = photoUploadService;
            this.unitOfWork = unitOfWork;
            this.photoRepository = photoRepository;
            this.categoryRepository = categoryRepository;
            this.userRepository = userRepository;
            this.commentRepository = commentRepository;
            this.albumRepository = albumRepository;
            this.mapper = mapper;
            this.uploadsFolderPath = Path.Combine(host.WebRootPath, "uploads");
            this.uploadsFolderUrl = "/uploads";
            this.outputFolderPath = Path.Combine(host.WebRootPath, "uploads", "output");
            this.outputFolderUrl = "/uploads/output";
        }

        public string UploadFolderUrl
        {
            get
            {
                return this.uploadsFolderUrl;
            }
        }

        public string UploadFolderPath
        {
            get
            {
                return this.uploadsFolderPath;
            }
        }

        public string OutputFolderUrl
        {
            get
            {
                return this.outputFolderUrl;
            }
        }      

        public string OutputFolderPath
        {
            get
            {
                return this.outputFolderPath;
            }
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
            photoResource.FilePath = string.Format("{0}/{1}", this.uploadsFolderUrl, orgFilePath);
            photoResource.BoundingBoxFilePath = string.Format("{0}/{1}", this.outputFolderUrl, orgFilePath);

            return Ok(photoResource);
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
            photoResources = photoResources.Select(res =>
            {
                string orgFilePath = res.FilePath;
                res.FilePath = string.Format("{0}/{1}", this.uploadsFolderUrl, orgFilePath);
                res.BoundingBoxFilePath = string.Format("{0}/{1}", this.outputFolderUrl, orgFilePath);
                res.TotalComments = res.Comments.Count();
                ((List<CommentResource>)res.Comments).Clear();
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
                var boxDicts = this.objectDetectionService.DetectObjectsFromImages(new List<string>() { imageFilePath }, this.uploadsFolderPath, this.outputFolderPath);
                var labels = boxDicts[imageFilePath].Select(b => b.Label);
                if (labels.Any())
                {
                    var categories = this.categoryRepository.GetByNames(labels);
                    photo.PhotoCategories = categories.Select(cat => new PhotoCategory()
                    {
                        Category = cat,
                        Photo = photo
                    }).ToList();
                }
                var currentUser = new User()
                {
                    UserName = User.FindFirstValue(ClaimTypes.Name)
                };

                if (photoResource.Album != null && photoResource.Album.Id > 0)
                {
                    var newAlbum =  await this.albumRepository.GetAsync(photoResource.Album.Id, false);
                    if (newAlbum != null)
                        photo.Album = newAlbum;
                    else
                        return BadRequest();
                }
                else
                    photo.Album = null;

                photo.Author = this.userRepository.GetOrAdd(currentUser);

                this.photoRepository.Add(photo);
                await this.unitOfWork.CompleteAsync();

                return Ok(mapper.Map<Photo, PhotoResource>(photo));
            }
            else
                return NoContent();
        }

        /// <summary>
        /// Update a photo by ID
        /// </summary>
        [HttpPost("{id}")]
        [Authorize()]
        public async Task<IActionResult> UpdatePhoto([FromRoute] int id, [FromForm] PhotoResource photoResource)
        {
            Photo photo = await photoRepository.GetAsync(id);
            if (photo != null)
            {
                var userName = User.FindFirstValue(ClaimTypes.Name);
                if (photo.Author.UserName != userName)
                    return Forbid();
                if (string.IsNullOrEmpty(photoResource.Name))
                    return BadRequest();

                if (photoResource.FileToUpload != null)
                {
                    var fileToUpload = photoResource.FileToUpload;
                    var originalFilePath = photo.FilePath;
                    photo.FilePath = await this.photoUploadService.UploadPhoto(fileToUpload, this.uploadsFolderPath);
                    var dimensions = await this.photoRepository.GetImageDimensions(fileToUpload);
                    photo.Height = dimensions.Height;
                    photo.Width = dimensions.Width;
                    string imageFilePath = Path.Combine(this.uploadsFolderPath, photo.FilePath);
                    var boxDicts = this.objectDetectionService.DetectObjectsFromImages(new List<string>() { imageFilePath }, this.uploadsFolderPath, this.outputFolderPath);
                    var labels = boxDicts[imageFilePath].Select(b => b.Label);
                    if (labels.Any())
                    {
                        var categories = this.categoryRepository.GetByNames(labels);
                        photo.PhotoCategories = categories.Select(cat => new PhotoCategory()
                        {
                            Category = cat,
                            Photo = photo
                        }).ToList();
                    }
                    if (!string.IsNullOrEmpty(originalFilePath))
                        this.photoUploadService.DeletePhoto(originalFilePath, this.uploadsFolderPath, this.outputFolderPath);
                }
                photo.Name = photoResource.Name;
                photo.LocLng = photoResource.LocLng;
                photo.LocLat = photoResource.LocLat;
                photo.CenterLng = photoResource.CenterLng;
                photo.CenterLat = photoResource.CenterLat;
                photo.MapZoom = photoResource.MapZoom;

                var currentCategories = photo.PhotoCategories.ToList();
                var currentCategoriesIds = currentCategories.Select(cat => cat.CategoryId);
                var strSelectedCategories = Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString())["PhotoCategories"];
                var selectedCategoryResources = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<CategoryResource>>(strSelectedCategories) ?? new List<CategoryResource>();
                var selectedCategoryIds =  selectedCategoryResources.Select(cat => cat.Id);
                var deletedCategoryIds = photo.PhotoCategories.Where(cat => !selectedCategoryIds.Contains(cat.CategoryId)).Select(cat => cat.CategoryId);
                var newCategoryResources = selectedCategoryResources.Where(selectedCat => !currentCategoriesIds.Contains(selectedCat.Id));
                var newCategories = mapper.Map<IEnumerable<CategoryResource>, IEnumerable<PhotoCategory>>(newCategoryResources).ToList();
                currentCategories.RemoveAll(c => deletedCategoryIds.Contains(c.CategoryId));
                currentCategories.AddRange(newCategories);
                photo.PhotoCategories = currentCategories;

                if (photoResource.Album != null && photoResource.Album.Id > 0)
                {
                    var newAlbum =  await this.albumRepository.GetAsync(photoResource.Album.Id, false);
                    if (newAlbum != null)
                        photo.Album = newAlbum;
                    else
                        return BadRequest();
                }
                else
                    photo.Album = null;
                photo.ModifiedDate = DateTime.UtcNow;
                await this.unitOfWork.CompleteAsync();

                var outputPhotoResource = mapper.Map<Photo, PhotoResource>(photo);
                string orgFilePath = outputPhotoResource.FilePath;
                outputPhotoResource.FilePath = string.Format("{0}/{1}", this.uploadsFolderUrl, orgFilePath);
                outputPhotoResource.BoundingBoxFilePath = string.Format("{0}/{1}", this.outputFolderUrl, orgFilePath);

                return Ok(outputPhotoResource);
            }
            else
                return NotFound();
        }

        /// <summary>
        /// Delete a photo by ID
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize()]
        public async Task<IActionResult> DeletePhoto([FromRoute] int id)
        {
            Photo photo = await photoRepository.GetAsync(id);
            if (photo != null)
            {
                var userName = User.FindFirstValue(ClaimTypes.Name);
                if (photo.Author.UserName != userName)
                    return Forbid();

                var originalFilePath = photo.FilePath;
                this.commentRepository.DeleteAll(photo.Comments.ToList());
                this.photoRepository.Delete(photo);
                await this.unitOfWork.CompleteAsync();
                if (!string.IsNullOrEmpty(originalFilePath))
                    this.photoUploadService.DeletePhoto(originalFilePath, this.uploadsFolderPath, this.outputFolderPath);

                return Ok();
            }
            else
                return NotFound();
        }
    }
}