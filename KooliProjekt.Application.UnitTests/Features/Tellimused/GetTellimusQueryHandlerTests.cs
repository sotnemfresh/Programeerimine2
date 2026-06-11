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
        public async Task Handle_should_throw_ArgumentNullException_if_request_is_null()
        {
            var handler = new GetTellimusQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task Handle_should_return_null_value_if_id_is_zero_or_less(int id)
        {
            var dbContext = GetFaultyDbContext();
            var query = new GetTellimusQuery { Id = id };
            var handler = new GetTellimusQueryHandler(dbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Handle_should_return_tellimus_when_valid_id_is_provided()
        {
            // Arrange
            var klient = new Klient
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@test.ee",
                Address = "Test Address",
                Phone = "12345678",
                Discount = 0
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

            var arve = new Arve
            {
                InvoiceNumber = "INV-001",
                InvoiceDate = DateTime.Now,
                Status = "Pending",
                SubTotal = 200,
                GrandTotal = 240,
                KlientId = klient.Id,
                TellimusId = tellimus.Id
            };
            await DbContext.Arved.AddAsync(arve);
            await DbContext.SaveChangesAsync();

            var query = new GetTellimusQuery { Id = tellimus.Id };
            var handler = new GetTellimusQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(tellimus.Id, result.Value.Id);
            Assert.Equal("Pending", result.Value.Status);
            Assert.NotNull(result.Value.Klient);
            Assert.Equal("Test", result.Value.Klient.FirstName);
            Assert.NotNull(result.Value.Arve);
            Assert.Equal("INV-001", result.Value.Arve.InvoiceNumber);
            Assert.NotNull(result.Value.TellimuseRead);
            Assert.Single(result.Value.TellimuseRead);
        }

        [Fact]
        public async Task Handle_should_return_null_when_tellimus_does_not_exist()
        {
            // Arrange
            var query = new GetTellimusQuery { Id = 9999 };
            var handler = new GetTellimusQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Handle_should_return_tellimus_without_arve_when_arve_does_not_exist()
        {
            // Arrange
            var klient = new Klient
            {
                FirstName = "Test",
                LastName = "User",
                Email = "test@test.ee"
            };
            await DbContext.Kliendid.AddAsync(klient);

            var tellimus = new Tellimus
            {
                OrderDate = DateTime.Now,
                Status = "Pending",
                KlientId = klient.Id
            };
            await DbContext.Tellimused.AddAsync(tellimus);
            await DbContext.SaveChangesAsync();

            var query = new GetTellimusQuery { Id = tellimus.Id };
            var handler = new GetTellimusQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Null(result.Value.Arve);
        }
    }
}