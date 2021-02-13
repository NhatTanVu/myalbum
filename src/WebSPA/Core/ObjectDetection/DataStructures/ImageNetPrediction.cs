using Microsoft.ML.Data;

namespace MyAlbum.WebSPA.Core.ObjectDetection.DataStructures
{
    public class ImageNetPrediction
    {
        [ColumnName("grid")]
        public float[] PredictedLabels;        
    }
}