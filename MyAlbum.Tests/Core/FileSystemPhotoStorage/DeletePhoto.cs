using System;
using System.IO;
using Moq;
using MyAlbum.Core;
using Xunit;

namespace MyAlbum.Tests.Core
{
    public class FileSystemPhotoStorage_Test2
    {
        [Fact]
        public void DeletePhoto()
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
            File.Copy(originalFilePath, outputFilePath);
            var filePhotoStorage = new FileSystemPhotoStorage();
            // Assert #1
            Assert.True(File.Exists(uploadFilePath));
            Assert.True(File.Exists(outputFilePath));            
            // Act
            filePhotoStorage.DeletePhoto(fileName, uploadsFolderPath, outputFolderPath);
            // Assert #2
            Assert.False(File.Exists(uploadFilePath));
            Assert.False(File.Exists(outputFilePath));
        }
    }
}