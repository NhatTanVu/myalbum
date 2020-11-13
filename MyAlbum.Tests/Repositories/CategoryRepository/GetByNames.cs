using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Core.Models;
using MyAlbum.Persistence;
using Xunit;

namespace MyAlbum.Tests.Repositories
{
    public class CategoryRepository_Test1
    {
        private IEnumerable<Category> SeedCategories(MyAlbumDbContext context)
        {
            int numOfCategories = new Random().Next(1, 20);
            var seedCategories = new List<Category>();
            for (int i = 0; i < numOfCategories; i++)
            {
                var category = new Category(){
                    Id = new Random().Next(i * 20, (i + 1) * 20),
                    Name = Guid.NewGuid().ToString()
                };
                seedCategories.Add(category);
                context.Category.Add(category);
            }
            context.SaveChanges();            
            return seedCategories;
        }        

        [Fact]
        public void GetByNames()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<MyAlbumDbContext>()
                .UseInMemoryDatabase(databaseName: "CategoryRepository_GetByNames_MyAlbumDatabase")
                .Options;
            using (var context = new MyAlbumDbContext(options))
            {
                var seedCategories = SeedCategories(context);
                var categoryRepository = new CategoryRepository(context);
                // Act
                var result = categoryRepository.GetByNames(seedCategories.Select(c => c.Name));
                // Assert
                Assert.Equal(seedCategories, result);
            }            
        }
    }
}