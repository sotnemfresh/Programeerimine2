using Xunit;
using KooliProjekt.Application.Features.Tooted;
using KooliProjekt.Application.UnitTests;
using System;
using System.Threading.Tasks;
using System.Threading;
using KooliProjekt.Application.Data;

namespace KooliProjekt.UnitTests.Features.Tooted
{
    public class ListTootedQueryHandlerTests : ServiceTestBase
    {
        [Fact]
        public void Constructor_should_throw_if_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new ListTootedQueryHandler(null));
        }

        [Fact]
        public async Task Handle_should_throw_ArgumentNullException_if_request_is_null()
        {
            var handler = new ListTootedQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_should_throw_ArgumentException_if_page_is_zero_or_less()
        {
            var query = new ListTootedQuery { Page = 0, PageSize = 10 };
            var handler = new ListTootedQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_should_throw_ArgumentException_if_pageSize_is_zero_or_less()
        {
            var query = new ListTootedQuery { Page = 1, PageSize = 0 };
            var handler = new ListTootedQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_should_throw_ArgumentException_if_pageSize_is_too_large()
        {
            var query = new ListTootedQuery { Page = 1, PageSize = 101 };
            var handler = new ListTootedQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_should_return_paged_result_when_valid_request()
        {
            // Arrange
            for (int i = 1; i <= 15; i++)
            {
                var toode = new Toode
                {
                    Name = $"Product {i}",
                    Description = $"Description {i}",
                    Price = 10 * i,
                    StockQuantity = 100
                };
                await DbContext.Tooted.AddAsync(toode);
            }
            await DbContext.SaveChangesAsync();

            var query = new ListTootedQuery { Page = 1, PageSize = 10 };
            var handler = new ListTootedQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(15, result.Value.RowCount);
            Assert.Equal(10, result.Value.Results.Count);
            Assert.Equal(2, result.Value.PageCount);
        }

        [Fact]
        public async Task Handle_should_return_empty_paged_result_when_no_data()
        {
            // Arrange
            var query = new ListTootedQuery { Page = 1, PageSize = 10 };
            var handler = new ListTootedQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(0, result.Value.RowCount);
            Assert.Empty(result.Value.Results);
        }
    }
}