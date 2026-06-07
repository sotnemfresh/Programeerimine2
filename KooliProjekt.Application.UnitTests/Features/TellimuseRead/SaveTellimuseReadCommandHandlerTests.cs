using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.TellimuseRead;
using KooliProjekt.Application.UnitTests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.UnitTests.Features.TellimuseRead
{
    public class SaveTellimuseReadCommandTests : ServiceTestBase
    {
        [Fact]
        public void Constructor_should_throw_if_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new SaveTellimuseReadCommandHandler(null));
        }

        [Fact]
        public async Task Handle_should_throw_ArgumentNullException_if_request_is_null()
        {
            var handler = new SaveTellimuseReadCommandHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_should_return_error_when_id_is_negative()
        {
            // Arrange
            var command = new SaveTellimuseReadCommand { Id = -1 };
            var handler = new SaveTellimuseReadCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Handle_should_add_new_tellimuse_rida()
        {
            // Arrange
            var command = new SaveTellimuseReadCommand
            {
                Id = 0,
                Quantity = 5,
                UnitPrice = 10,
                LineTotal = 50,
                VatRate = 0.2m,
                VatAmount = 10,
                TellimusId = 1,
                ToodeId = 1
            };
            var handler = new SaveTellimuseReadCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(1, await DbContext.TellimusedRida.CountAsync());
        }

        [Fact]
        public async Task Handle_should_update_existing_tellimuse_rida()
        {
            // Arrange
            var rida = new TellimuseRida
            {
                TellimusId = 1,
                ToodeId = 1,
                Quantity = 5,
                UnitPrice = 10,
                LineTotal = 50
            };
            await DbContext.TellimusedRida.AddAsync(rida);
            await DbContext.SaveChangesAsync();

            var command = new SaveTellimuseReadCommand
            {
                Id = rida.Id,
                Quantity = 10,
                UnitPrice = 15,
                LineTotal = 150,
                VatRate = 0.2m,
                VatAmount = 30,
                TellimusId = 1,
                ToodeId = 1
            };
            var handler = new SaveTellimuseReadCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var updated = await DbContext.TellimusedRida.FindAsync(rida.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(10, updated.Quantity);
            Assert.Equal(150, updated.LineTotal);
        }

        [Fact]
        public async Task Handle_should_return_error_when_updating_non_existent_rida()
        {
            // Arrange
            var command = new SaveTellimuseReadCommand { Id = 9999, Quantity = 5 };
            var handler = new SaveTellimuseReadCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.HasErrors);
        }
    }
}