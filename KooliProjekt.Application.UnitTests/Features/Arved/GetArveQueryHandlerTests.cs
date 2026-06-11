using Xunit;
using KooliProjekt.Application.Features.Arved;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Data;
using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.UnitTests;

namespace KooliProjekt.UnitTests.Features.Arved
{
    public class GetArveQueryHandlerTests : ServiceTestBase
    {
        [Fact]
        public async Task Handle_should_throw_ArgumentNullException_if_request_is_null()
        {
            var handler = new GetArveQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
        }

        [Fact]
        public void Constructor_should_throw_if_dbContext_is_null()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new GetArveQueryHandler(null));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task Handle_should_return_null_value_if_id_is_zero_or_less(int id)
        {
            var dbContext = GetFaultyDbContext();
            var query = new GetArveQuery { Id = id };
            var handler = new GetArveQueryHandler(dbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Handle_should_return_arve_when_valid_id_is_provided()
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

            var tellimus = new Tellimus
            {
                OrderDate = DateTime.Now,
                Status = "Pending",
                KlientId = klient.Id
            };
            await DbContext.Tellimused.AddAsync(tellimus);

            var arve = new Arve
            {
                InvoiceNumber = "INV-001",
                InvoiceDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                Status = "Pending",
                SubTotal = 100,
                ShippingTotal = 10,
                Discount = 5,
                GrandTotal = 105,
                KlientId = klient.Id,
                TellimusId = tellimus.Id
            };
            await DbContext.Arved.AddAsync(arve);
            await DbContext.SaveChangesAsync();

            var query = new GetArveQuery { Id = arve.Id };
            var handler = new GetArveQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(arve.Id, result.Value.Id);
            Assert.Equal("INV-001", result.Value.InvoiceNumber);
            Assert.Equal("Pending", result.Value.Status);
            Assert.Equal(100, result.Value.SubTotal);
            Assert.Equal(105, result.Value.GrandTotal);
            Assert.NotNull(result.Value.Klient);
            Assert.Equal("Test", result.Value.Klient.FirstName);
            Assert.NotNull(result.Value.Tellimus);
        }

        [Fact]
        public async Task Handle_should_return_null_when_arve_does_not_exist()
        {
            // Arrange
            var query = new GetArveQuery { Id = 9999 };
            var handler = new GetArveQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Value);
        }
    }
}