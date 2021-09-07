using System;
using System.Threading.Tasks;
using allinfo.Data;
using allinfo.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace allinfo.UnitTests
{
    public class WritersRepositoryShould
    {
        [Fact]
        public async Task Test1()
        {
            var options = new DbContextOptionsBuilder<NewsContext>().UseInMemoryDatabase(databaseName: "Test").Options;

            using(var context = new NewsContext(options))
            {
                var service = new WritersRepository(context);

                service.AddWriterAsync(new Writer{ LastName = "Johnson", FirstName = "Ben", DOB = DateTime.Now, isManager = true});
                await service.SaveAsync();
            }

            using(var context = new NewsContext(options))
            {
                var writersInDatabase = await context.Writers.CountAsync();
                Assert.Equal(1, writersInDatabase);

                var writer = await context.Writers.FirstAsync();
                Assert.Equal("Johnson", writer.LastName);
                Assert.Equal("Ben", writer.FirstName);
            }
        }
    }
}
