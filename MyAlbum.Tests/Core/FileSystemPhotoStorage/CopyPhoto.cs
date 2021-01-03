using System;
using System.IO;
using System.Linq;
using MyAlbum.Core;
using Xunit;

namespace MyAlbum.Tests.Core
{
    public class FileSystemPhotoStorage_Test3
    {
        [Fact]
        public void CopyPhoto()
        {
            // Arrange
            string originalFilePath = @".\car.jpg";
            string fileName = Guid.NewGuid().ToString();
            string uploadsFolderPath = @".\uploads";
            string outputFolderPath = @".\output";
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }
            if (!Directory.Exists(outputFolderPath))
            {
                Directory.CreateDirectory(outputFolderPath);
            }            
            var uploadFilePath = Path.Combine(uploadsFolderPath, fileName);
            var outputFilePath = Path.Combine(outputFolderPath, fileName);
            File.Copy(originalFilePath, uploadFilePath);
            var filePhotoStorage = new FileSystemPhotoStorage();
            // Assert #1
            Assert.True(File.Exists(uploadFilePath));
            Assert.False(File.Exists(outputFilePath));            
            // Act
            filePhotoStorage.CopyPhoto(fileName, uploadsFolderPath, outputFolderPath);
            // Assert #2
            Assert.True(File.Exists(uploadFilePath));
            Assert.True(File.Exists(outputFilePath));
            Assert.True(File.ReadAllBytes(uploadFilePath).SequenceEqual(File.ReadAllBytes(outputFilePath)));
        }
    }
}