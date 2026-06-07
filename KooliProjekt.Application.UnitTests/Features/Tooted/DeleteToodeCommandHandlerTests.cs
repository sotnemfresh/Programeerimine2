using Xunit;
using KooliProjekt.Application.Features.Tooted;
using KooliProjekt.Application.Data;
using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.UnitTests;

namespace KooliProjekt.UnitTests.Features.Tooted
{
    public class DeleteToodeCommandHandlerTests : ServiceTestBase
    {
        [Fact]
        public void Constructor_should_throw_if_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new DeleteToodeCommandHandler(null));
        }

        [Fact]
        public async Task Handle_should_throw_ArgumentNullException_if_request_is_null()
        {
            var handler = new DeleteToodeCommandHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Handle_should_return_success_without_db_call_if_id_is_invalid(int id)
        {
            var command = new DeleteToodeCommand { Id = id };
            var handler = new DeleteToodeCommandHandler(GetFaultyDbContext());

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Handle_should_delete_entity_if_exists()
        {
            var toode = new Toode { Name = "Test Toode", Price = 10.5m };
            await DbContext.Tooted.AddAsync(toode);
            await DbContext.SaveChangesAsync();

            var command = new DeleteToodeCommand { Id = toode.Id };
            var handler = new DeleteToodeCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);
            var deletedToode = await DbContext.Tooted.FindAsync(toode.Id);

            Assert.NotNull(result);
            Assert.Null(deletedToode);
        }

        [Fact]
        public async Task Handle_should_not_fail_if_entity_does_not_exist()
        {
            var command = new DeleteToodeCommand { Id = 9999 };
            var handler = new DeleteToodeCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }
    }
}