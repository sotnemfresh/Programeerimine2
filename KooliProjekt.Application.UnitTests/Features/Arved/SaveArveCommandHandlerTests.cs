using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Arved;
using KooliProjekt.Application.UnitTests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.UnitTests.Features.Arved
{
    public class SaveArveCommandHandlerTests : ServiceTestBase
    {
        [Fact]
        public void Constructor_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new SaveArveCommandHandler(null));
        }

        [Fact]
        public async Task Handle_should_throw_when_request_is_null()
        {
            var handler = new SaveArveCommandHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_should_return_error_if_id_is_negative()
        {
            var command = new SaveArveCommand { Id = -1 };
            var handler = new SaveArveCommandHandler(GetFaultyDbContext());

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Handle_should_add_new_arve()
        {
            // Arrange
            // Eeldame, et Arve loomisel on vaja neid andmeid
            var command = new SaveArveCommand
            {
                Id = 0,
                InvoiceNumber = "INV-NEW-001",
                InvoiceDate = DateTime.Now,
                TellimusId = 1 // Kuna see on required
            };
            var handler = new SaveArveCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var savedArve = await DbContext.Arved.FirstOrDefaultAsync(a => a.InvoiceNumber == "INV-NEW-001");

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedArve);
            Assert.Equal(command.InvoiceNumber, savedArve.InvoiceNumber);
        }

        [Fact]
        public async Task Handle_should_update_existing_arve()
        {
            // Arrange
            var arve = new Arve { InvoiceNumber = "OLD-123", InvoiceDate = DateTime.Now, TellimusId = 1 };
            await DbContext.Arved.AddAsync(arve);
            await DbContext.SaveChangesAsync();

            var command = new SaveArveCommand
            {
                Id = arve.Id,
                InvoiceNumber = "UPDATED-123",
                InvoiceDate = arve.InvoiceDate,
                TellimusId = 1
            };
            var handler = new SaveArveCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var updatedArve = await DbContext.Arved.FindAsync(arve.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal("UPDATED-123", updatedArve.InvoiceNumber);
        }

        [Fact]
        public async Task Handle_should_return_error_when_updating_missing_arve()
        {
            var command = new SaveArveCommand { Id = 9999, InvoiceNumber = "GHOST" };
            var handler = new SaveArveCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.HasErrors);
            Assert.Contains("Cannot find", result.Errors[0]); // Kontrollime, et viga on õige
        }
    }
}