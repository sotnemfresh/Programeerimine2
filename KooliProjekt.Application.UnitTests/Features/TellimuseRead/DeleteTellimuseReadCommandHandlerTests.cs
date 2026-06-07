using Xunit;
using KooliProjekt.Application.Features.TellimuseRead;
using KooliProjekt.Application.Data;
using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.UnitTests;

namespace KooliProjekt.UnitTests.Features.TellimuseRead
{
    public class DeleteTellimuseReadCommandHandlerTests : ServiceTestBase
    {
        [Fact]
        public void Constructor_should_throw_if_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new DeleteTellimuseReadCommandHandler(null));
        }

        [Fact]
        public async Task Handle_should_throw_ArgumentNullException_if_request_is_null()
        {
            var handler = new DeleteTellimuseReadCommandHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Handle_should_return_success_without_db_call_if_id_is_invalid(int id)
        {
            var command = new DeleteTellimuseReadCommand { Id = id };
            var handler = new DeleteTellimuseReadCommandHandler(GetFaultyDbContext());

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Handle_should_delete_entity_if_exists()
        {
            var rida = new TellimuseRida { };
            await DbContext.TellimusedRida.AddAsync(rida);
            await DbContext.SaveChangesAsync();

            var command = new DeleteTellimuseReadCommand { Id = rida.Id };
            var handler = new DeleteTellimuseReadCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);
            var deletedRida = await DbContext.TellimusedRida.FindAsync(rida.Id);

            Assert.NotNull(result);
            Assert.Null(deletedRida);
        }

        [Fact]
        public async Task Handle_should_not_fail_if_entity_does_not_exist()
        {
            var command = new DeleteTellimuseReadCommand { Id = 9999 };
            var handler = new DeleteTellimuseReadCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }
    }
}