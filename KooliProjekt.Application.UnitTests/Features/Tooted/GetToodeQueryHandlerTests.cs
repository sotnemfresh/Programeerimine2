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
        public void Constructor_should_throw_if_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new GetToodeQueryHandler(null));
        }

        [Fact]
        public async Task Handle_should_throw_ArgumentNullException_if_request_is_null()
        {
            var handler = new GetToodeQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task Handle_should_return_null_value_if_id_is_zero_or_less(int id)
        {
            var dbContext = GetFaultyDbContext();
            var query = new GetToodeQuery { Id = id };
            var handler = new GetToodeQueryHandler(dbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Handle_should_return_toode_when_valid_id_is_provided()
        {
            // Arrange
            var toode = new Toode
            {
                Name = "Test Product",
                Description = "Test Description",
                FotoURL = "http://example.com/photo.jpg",
                Price = 99.99m,
                StockQuantity = 50
            };
            await DbContext.Tooted.AddAsync(toode);
            await DbContext.SaveChangesAsync();

            var query = new GetToodeQuery { Id = toode.Id };
            var handler = new GetToodeQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(toode.Id, result.Value.Id);
            Assert.Equal("Test Product", result.Value.Name);
            Assert.Equal("Test Description", result.Value.Description);
            Assert.Equal(99.99m, result.Value.Price);
            Assert.Equal(50, result.Value.StockQuantity);
        }

        [Fact]
        public async Task Handle_should_return_null_when_toode_does_not_exist()
        {
            // Arrange
            var query = new GetToodeQuery { Id = 9999 };
            var handler = new GetToodeQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Value);
        }
    }
}