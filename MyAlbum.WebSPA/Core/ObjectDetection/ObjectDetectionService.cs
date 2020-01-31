using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using Microsoft.ML;
using MyAlbum.Core;
using MyAlbum.WebSPA.Core.ObjectDetection.DataStructures;
using MyAlbum.WebSPA.Core.ObjectDetection.YoloParser;

namespace MyAlbum.WebSPA.Core.ObjectDetection
{
    public class ObjectDetectionService : IObjectDetectionService
    {
        private static readonly string assetsPath = @"C:\_code\MyAlbum\MyAlbum.WebSPA\Core\ObjectDetection\assets";
        private static readonly string modelFilePath = Path.Combine(assetsPath, "Model", @"tiny_yolov2\Model.onnx");

        public IDictionary<string, IList<YoloBoundingBox>> DetectObjectsFromImages(List<string> imageFilePaths, string uploadFolderPath)
        {
            MLContext mlContext = new MLContext();
            IDictionary<string, IList<YoloBoundingBox>> detectedObjectsDict = new Dictionary<string, IList<YoloBoundingBox>>();
            string outputFolder = Path.Combine(uploadFolderPath, "output");

            try
            {
                IEnumerable<ImageNetData> images = ImageNetData.ReadFromFiles(imageFilePaths);
                IDataView imageDataView = mlContext.Data.LoadFromEnumerable(images);
                var modelScorer = new OnnxModelScorer(modelFilePath, mlContext);

                YoloOutputParser parser = new YoloOutputParser();
                IEnumerable<float[]> probabilities = modelScorer.Score(imageDataView); // Use model to score data
                var boundingBoxes =
                    probabilities
                    .Select(probability => parser.ParseOutputs(probability))
                    .Select(boxes => parser.FilterBoundingBoxes(boxes, 5, .5F));

                for (var i = 0; i < images.Count(); i++)
                {
                    string imageFileName = images.ElementAt(i).Label;
                    string imageFilePath = images.ElementAt(i).ImagePath;
                    IList<YoloBoundingBox> detectedObjects = boundingBoxes.ElementAt(i);
                    detectedObjectsDict.Add(imageFilePath, detectedObjects);
                    DrawBoundingBox(imageFilePath, outputFolder, imageFileName, detectedObjects);
                }

                return detectedObjectsDict;
            }
            catch
            {
                throw;
            }            
        }

        private void DrawBoundingBox(string imageFilePath, string outputImageLocation, string imageName, IList<YoloBoundingBox> filteredBoundingBoxes)
        {
            Image image = Image.FromFile(imageFilePath);

            var originalImageHeight = image.Height;
            var originalImageWidth = image.Width;

            foreach (var box in filteredBoundingBoxes)
            {
                var x = (uint)Math.Max(box.Dimensions.X, 0);
                var y = (uint)Math.Max(box.Dimensions.Y, 0);
                var width = (uint)Math.Min(originalImageWidth - x, box.Dimensions.Width);
                var height = (uint)Math.Min(originalImageHeight - y, box.Dimensions.Height);

                x = (uint)originalImageWidth * x / OnnxModelScorer.ImageNetSettings.imageWidth;
                y = (uint)originalImageHeight * y / OnnxModelScorer.ImageNetSettings.imageHeight;
                width = (uint)originalImageWidth * width / OnnxModelScorer.ImageNetSettings.imageWidth;
                height = (uint)originalImageHeight * height / OnnxModelScorer.ImageNetSettings.imageHeight;

                string text = $"{box.Label} ({(box.Confidence * 100).ToString("0")}%)";

                using (Graphics thumbnailGraphic = Graphics.FromImage(image))
                {
                    thumbnailGraphic.CompositingQuality = CompositingQuality.HighQuality;
                    thumbnailGraphic.SmoothingMode = SmoothingMode.HighQuality;
                    thumbnailGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    // Define Text Options
                    Font drawFont = new Font("Arial", 12, FontStyle.Bold);
                    SizeF size = thumbnailGraphic.MeasureString(text, drawFont);
                    SolidBrush fontBrush = new SolidBrush(Color.Black);
                    Point atPoint = new Point((int)x, (int)y - (int)size.Height - 1);

                    // Define BoundingBox options
                    Pen pen = new Pen(box.BoxColor, 3.2f);
                    SolidBrush colorBrush = new SolidBrush(box.BoxColor);

                    thumbnailGraphic.FillRectangle(colorBrush, (int)x, (int)(y - size.Height - 1), (int)size.Width, (int)size.Height);
                    thumbnailGraphic.DrawString(text, drawFont, fontBrush, atPoint);
                    // Draw bounding box on image
                    thumbnailGraphic.DrawRectangle(pen, x, y, width, height);
                }
            }

            if (!Directory.Exists(outputImageLocation))
            {
                Directory.CreateDirectory(outputImageLocation);
            }

            image.Save(Path.Combine(outputImageLocation, imageName));
        }
    }
}