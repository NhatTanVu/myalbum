using Microsoft.ML.Data;

namespace MyAlbum.Services.Photo.API.Services.ObjectDetection.DataStructures
{
    public class ImageNetPrediction
    {
        [ColumnName("grid")]
        public float[] PredictedLabels;        
    }
}