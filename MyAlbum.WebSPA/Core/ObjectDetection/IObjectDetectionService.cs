using System.Collections.Generic;
using MyAlbum.WebSPA.Core.ObjectDetection.YoloParser;

namespace MyAlbum.WebSPA.Core.ObjectDetection
{
    public interface IObjectDetectionService
    {
        IDictionary<string, IList<YoloBoundingBox>> DetectObjectsFromImages(List<string> imageFilePaths, string uploadFolderPath, string outputFolderPath);
    }
}