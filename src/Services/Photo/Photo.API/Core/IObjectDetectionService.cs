using System.Collections.Generic;
using MyAlbum.Services.Photo.API.Services.ObjectDetection.YoloParser;

namespace MyAlbum.Services.Photo.API.Core
{
    public interface IObjectDetectionService
    {
        IDictionary<string, IList<YoloBoundingBox>> DetectObjectsFromImages(List<string> imageFilePaths, string uploadFolderPath, string outputFolderPath);
    }
}