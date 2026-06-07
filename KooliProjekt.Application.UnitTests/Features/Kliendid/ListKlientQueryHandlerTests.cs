using Xunit;
using KooliProjekt.Application.Features.Kliendid;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Data;
using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.UnitTests;

namespace KooliProjekt.UnitTests.Features.Kliendid
{
    public class ListKliendidQueryHandlerTests : ServiceTestBase
    {
        [Fact]
        public void Constructor_should_throw_if_dbcontext_is_null()
        {
            // See test peaks nüüd töötama, sest lisasime Handlerisse kontrolli
            Assert.Throws<ArgumentNullException>(() => new ListKliendidQueryHandler(null));
        }

        [Fact]
        public async Task Handle_should_throw_ArgumentNullException_if_request_is_null()
        {
            var handler = new ListKliendidQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Handle_should_throw_ArgumentException_if_page_is_invalid(int page)
        {
            var query = new ListKliendidQuery { Page = page, PageSize = 10 };
            var handler = new ListKliendidQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(query, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Handle_should_throw_ArgumentException_if_pageSize_is_invalid(int pageSize)
        {
            var query = new ListKliendidQuery { Page = 1, PageSize = pageSize };
            var handler = new ListKliendidQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_should_throw_ArgumentException_if_pageSize_is_too_large()
        {
            var query = new ListKliendidQuery { Page = 1, PageSize = 101 };
            var handler = new ListKliendidQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_should_return_list_if_data_exists()
        {
            // Arrange
            var klient = new Klient { FirstName = "Rainer", LastName = "Tahker", Email = "Rainer@test.ee" };
            await DbContext.Kliendid.AddAsync(klient);
            await DbContext.SaveChangesAsync();

            var query = new ListKliendidQuery { Page = 1, PageSize = 10 };
            var handler = new ListKliendidQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.NotEmpty(result.Value.Results);
        }
    }
}