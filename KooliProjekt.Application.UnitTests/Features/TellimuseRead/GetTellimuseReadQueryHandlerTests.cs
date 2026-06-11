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
        public void Constructor_should_throw_if_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new GetTellimuseReadQueryHandler(null));
        }

        [Fact]
        public async Task Handle_should_throw_ArgumentNullException_if_request_is_null()
        {
            var handler = new GetTellimuseReadQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task Handle_should_return_null_value_if_id_is_zero_or_less(int id)
        {
            var dbContext = GetFaultyDbContext();
            var query = new GetTellimuseReadQuery { Id = id };
            var handler = new GetTellimuseReadQueryHandler(dbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Handle_should_return_tellimuserida_when_valid_id_is_provided()
        {
            // Arrange
            var klient = new Klient
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@test.ee"
            };
            await DbContext.Kliendid.AddAsync(klient);

            var toode = new Toode
            {
                Name = "Test Product",
                Price = 100,
                StockQuantity = 10
            };
            await DbContext.Tooted.AddAsync(toode);

            var tellimus = new Tellimus
            {
                OrderDate = DateTime.Now,
                Status = "Pending",
                KlientId = klient.Id
            };
            await DbContext.Tellimused.AddAsync(tellimus);

            var rida = new TellimuseRida
            {
                TellimusId = tellimus.Id,
                ToodeId = toode.Id,
                Quantity = 2,
                UnitPrice = 100,
                LineTotal = 200,
                VatRate = 0.2m,
                VatAmount = 40
            };
            await DbContext.TellimusedRida.AddAsync(rida);
            await DbContext.SaveChangesAsync();

            var query = new GetTellimuseReadQuery { Id = rida.Id };
            var handler = new GetTellimuseReadQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(rida.Id, result.Value.Id);
            Assert.Equal(2, result.Value.Quantity);
            Assert.Equal(100, result.Value.UnitPrice);
            Assert.Equal(200, result.Value.LineTotal);
            Assert.NotNull(result.Value.Toode);
            Assert.Equal("Test Product", result.Value.Toode.Name);
        }

        [Fact]
        public async Task Handle_should_return_null_when_rida_does_not_exist()
        {
            // Arrange
            var query = new GetTellimuseReadQuery { Id = 9999 };
            var handler = new GetTellimuseReadQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Value);
        }
    }
}