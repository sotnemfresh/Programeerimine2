using Xunit;
using KooliProjekt.Application.Features.Tooted;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Data;
using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.UnitTests;

namespace KooliProjekt.UnitTests.Features.Tooted
{
    public class GetToodeQueryHandlerTests : ServiceTestBase
    {
        [Fact]
        public void Constructor_should_throw_if_dbContext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new GetToodeQueryHandler(null));
        }

        [Fact]
        public async Task Handle_should_return_null_value_if_request_is_null()
        {
            // Arrange
            var handler = new GetToodeQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(null, CancellationToken.None);

            // Assert
            Assert.Null(result.Value);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Handle_should_return_object_if_it_exists()
        {
            // Arrange
            var toode = new Toode
            {
                Name = "Test Toode",
                Price = 15,
                StockQuantity = 10,
                Description = "Kirjeldus",
                FotoURL = "http://foto.ee"
            };

            await DbContext.Tooted.AddAsync(toode);
            await DbContext.SaveChangesAsync();

            var query = new GetToodeQuery { Id = toode.Id };
            var handler = new GetToodeQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(toode.Id, result.Value.Id);
            Assert.Equal("Test Toode", result.Value.Name);
        }
    }
}