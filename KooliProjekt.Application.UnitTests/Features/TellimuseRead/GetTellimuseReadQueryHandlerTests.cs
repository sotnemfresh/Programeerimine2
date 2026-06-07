using Xunit;
using KooliProjekt.Application.Features.TellimuseRead;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Data;
using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.UnitTests;

namespace KooliProjekt.UnitTests.Features.TellimuseRead
{
    public class GetTellimuseReadQueryHandlerTests : ServiceTestBase
    {
        [Fact]
        public async Task Handle_should_return_null_value_if_request_is_null()
        {
            // Arrange
            var handler = new GetTellimuseReadQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(null, CancellationToken.None);

            // Assert
            Assert.Null(result.Value);
        }
        [Fact]
        public void Constructor_should_throw_if_dbContext_is_null()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new GetTellimuseReadQueryHandler(null));
        }
        [Fact]
        public async Task Handle_should_return_object_if_it_exists()
        {
            // Arrange
            // 1. Loome kliendi, toote ja tellimuse, et saaksime luua rea
            var klient = new Klient { FirstName = "Test", LastName = "Kasutaja", Email = "test@test.ee" };
            var toode = new Toode { Name = "Sülearvuti", Price = 1000, StockQuantity = 5 };
            var tellimus = new Tellimus { OrderDate = DateTime.Now, Klient = klient };

            // 2. Loome tellimuse rea
            var rida = new TellimuseRida
            {
                Quantity = 2,
                UnitPrice = 1000,
                LineTotal = 2000,
                Toode = toode,
                Tellimus = tellimus
            };

            await DbContext.TellimusedRida.AddAsync(rida);
            await DbContext.SaveChangesAsync();

            var query = new GetTellimuseReadQuery { Id = rida.Id };
            var handler = new GetTellimuseReadQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(rida.Id, result.Value.Id);
            Assert.Equal("Sülearvuti", result.Value.Toode.Name);
            Assert.False(result.HasErrors);
        }
    }
}