using System;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Xunit;

namespace Testing
{
    public class UnitTest1
    {
        DbContextOptions<TheStore_DbContext> options = new DbContextOptionsBuilder<TheStore_DbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        [Fact]
        public void Test1()
        {
            // ARRANGE
            using(var context = new TheStore_DbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                
            }

            // ACT

            // ASSERT
            //Assert.Equal(first, second);
        }
    }
}
