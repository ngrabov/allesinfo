using allinfo.Data;
using allinfo;
using System;
using Xunit;
using System.Threading.Tasks;
using allinfo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;

namespace allinfo.UnitTests
{
    public class homeRepositoryShould
    {
        [Fact]
        public async Task Test1()
        {
            var options = new DbContextOptionsBuilder<NewsContext>().UseInMemoryDatabase(databaseName: "Test").Options;

            using(var context = new NewsContext(options))
            {
                var service = new HomeRepository(context);

                await service.CountArticlesAsync();
            }

            using(var context = new NewsContext(options))
            {
                var itemsInDatabase = await context.Articles.CountAsync();
                Assert.Equal(0, itemsInDatabase);
/* 
                var item = await context.Articles.FirstAsync();
                Assert.Equal("Testing?", item.Headline); */
            }
        }
    }
}
