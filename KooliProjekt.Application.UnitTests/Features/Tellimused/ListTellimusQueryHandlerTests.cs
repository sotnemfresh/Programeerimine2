using Xunit;
using KooliProjekt.Application.Features.Tellimused;
using KooliProjekt.Application.UnitTests;
using System;
using System.Threading.Tasks;
using System.Threading;
using KooliProjekt.Application.Data;

namespace KooliProjekt.UnitTests.Features.Tellimused
{
    public class ListTellimusedQueryHandlerTests : ServiceTestBase
    {
        [Fact]
        public void Constructor_should_throw_if_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new ListTellimusedQueryHandler(null));
        }

        [Fact]
        public async Task Handle_should_throw_ArgumentNullException_if_request_is_null()
        {
            var handler = new ListTellimusedQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_should_throw_ArgumentException_if_page_is_zero_or_less()
        {
            var query = new ListTellimusedQuery { Page = 0, PageSize = 10 };
            var handler = new ListTellimusedQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_should_throw_ArgumentException_if_pageSize_is_zero_or_less()
        {
            var query = new ListTellimusedQuery { Page = 1, PageSize = 0 };
            var handler = new ListTellimusedQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_should_throw_ArgumentException_if_pageSize_is_too_large()
        {
            var query = new ListTellimusedQuery { Page = 1, PageSize = 101 };
            var handler = new ListTellimusedQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_should_return_paged_result_when_valid_request()
        {
            // Arrange
            var klient = new Klient
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@test.ee"
            };
            await DbContext.Kliendid.AddAsync(klient);
            await DbContext.SaveChangesAsync();

            for (int i = 1; i <= 15; i++)
            {
                var tellimus = new Tellimus
                {
                    OrderDate = DateTime.Now.AddDays(-i),
                    Status = i % 2 == 0 ? "Completed" : "Pending",
                    KlientId = klient.Id
                };
                await DbContext.Tellimused.AddAsync(tellimus);
            }
            await DbContext.SaveChangesAsync();

            var query = new ListTellimusedQuery { Page = 1, PageSize = 10 };
            var handler = new ListTellimusedQueryHandler(DbContext);

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
            var query = new ListTellimusedQuery { Page = 1, PageSize = 10 };
            var handler = new ListTellimusedQueryHandler(DbContext);

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