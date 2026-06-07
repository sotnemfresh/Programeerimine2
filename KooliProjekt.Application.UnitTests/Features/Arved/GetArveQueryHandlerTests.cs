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
        public async Task Handle_should_return_null_value_if_request_is_null()
        {
            // Arrange
            var handler = new GetArveQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(null, CancellationToken.None);

            // Assert
            Assert.Null(result.Value);
        }
        [Fact]
        public void Constructor_should_throw_if_dbContext_is_null()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new GetArveQueryHandler(null));
        }

        [Fact]
        public async Task Handle_should_return_object_if_it_exists()
        {
            // Arrange
            var klient = new Klient { FirstName = "Rainer", LastName = "Puur", Email = "Rainer@test.ee" };
            var tellimus = new Tellimus { OrderDate = DateTime.Now, Klient = klient };

            var arve = new Arve
            {
                InvoiceNumber = "INV-2026-01",
                InvoiceDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(14),
                Klient = klient,
                Tellimus = tellimus
            };

            await DbContext.Arved.AddAsync(arve);
            await DbContext.SaveChangesAsync();

            var query = new GetArveQuery { Id = arve.Id };
            var handler = new GetArveQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(arve.InvoiceNumber, result.Value.InvoiceNumber);
        }
    }
}