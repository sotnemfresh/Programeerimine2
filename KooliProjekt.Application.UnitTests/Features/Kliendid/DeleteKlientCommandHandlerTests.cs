using Xunit;
using KooliProjekt.Application.Features.Kliendid;
using KooliProjekt.Application.Data;
using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.UnitTests;

namespace KooliProjekt.UnitTests.Features.Kliendid
{
    public class DeleteKlientCommandHandlerTests : ServiceTestBase
    {
        [Fact]
        public void Constructor_should_throw_if_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new DeleteKlientCommandHandler(null));
        }

        [Fact]
        public async Task Handle_should_throw_ArgumentNullException_if_request_is_null()
        {
            var handler = new DeleteKlientCommandHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Handle_should_return_success_without_db_call_if_id_is_invalid(int id)
        {
            var command = new DeleteKlientCommand { Id = id };
            // Kasutame FaultyDbContexti, et veenduda, et DB poole ei pöörduta
            var handler = new DeleteKlientCommandHandler(GetFaultyDbContext());

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Handle_should_delete_entity_if_exists()
        {
            // Arrange
            var klient = new Klient { FirstName = "Test", LastName = "Delete", Email = "del@test.ee" };
            await DbContext.Kliendid.AddAsync(klient);
            await DbContext.SaveChangesAsync();

            var command = new DeleteKlientCommand { Id = klient.Id };
            var handler = new DeleteKlientCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var deletedKlient = await DbContext.Kliendid.FindAsync(klient.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Null(deletedKlient); // Peab olema andmebaasist kadunud
        }

        [Fact]
        public async Task Handle_should_not_fail_if_entity_does_not_exist()
        {
            var command = new DeleteKlientCommand { Id = 9999 };
            var handler = new DeleteKlientCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }
    }
}