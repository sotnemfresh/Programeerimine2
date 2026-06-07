using Xunit;
using KooliProjekt.Application.Features.Tellimused;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.UnitTests;

namespace KooliProjekt.UnitTests.Features.Tellimused
{
    public class GetTellimusQueryHandlerTests : ServiceTestBase
    {
        [Fact]
        public void Constructor_should_throw_if_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new GetTellimusQueryHandler(null));
        }

        [Fact]
        public async Task Handle_should_return_null_value_if_request_is_null()
        {
            var handler = new GetTellimusQueryHandler(DbContext);
            var result = await handler.Handle(null, CancellationToken.None);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Handle_should_return_object_with_all_includes_if_it_exists()
        {
            // Arrange
            var klient = new Klient { FirstName = "Mati", LastName = "Maasikas", Email = "mati@test.ee" };
            var toode = new Toode { Name = "Test Toode", Price = 10, StockQuantity = 100 };

            var tellimus = new Tellimus
            {
                OrderDate = DateTime.Now,
                Klient = klient,
                Status = "Uus"
            };

            var rida = new TellimuseRida
            {
                Quantity = 5,
                UnitPrice = 10,
                LineTotal = 50,
                Toode = toode,
                Tellimus = tellimus
            };

            var arve = new Arve
            {
                InvoiceNumber = "TEL-123",
                InvoiceDate = DateTime.Now,
                Klient = klient,
                Tellimus = tellimus
            };

            await DbContext.Kliendid.AddAsync(klient);
            await DbContext.Tooted.AddAsync(toode);
            await DbContext.Tellimused.AddAsync(tellimus);
            await DbContext.TellimusedRida.AddAsync(rida);
            await DbContext.Arved.AddAsync(arve);
            await DbContext.SaveChangesAsync();

            var query = new GetTellimusQuery { Id = tellimus.Id };
            var handler = new GetTellimusQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(tellimus.Id, result.Value.Id);
            Assert.Equal("Mati", result.Value.Klient.FirstName);
            Assert.Single(result.Value.TellimuseRead); // Kontrollime, et rida tuli kaasa
            Assert.NotNull(result.Value.Arve); // Kontrollime, et arve tuli kaasa
        }
    }
}