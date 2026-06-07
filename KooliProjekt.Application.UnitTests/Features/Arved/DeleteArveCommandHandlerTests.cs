using Xunit;
using KooliProjekt.Application.Features.Arved;
using KooliProjekt.Application.Data;
using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.UnitTests;

namespace KooliProjekt.UnitTests.Features.Arved
{
    public class DeleteArveCommandHandlerTests : ServiceTestBase
    {
        [Fact]
        public void Constructor_should_throw_if_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new DeleteArveCommandHandler(null));
        }

        [Fact]
        public async Task Handle_should_throw_ArgumentNullException_if_request_is_null()
        {
            var handler = new DeleteArveCommandHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Handle_should_return_success_without_db_call_if_id_is_invalid(int id)
        {
            var command = new DeleteArveCommand { Id = id };
            var handler = new DeleteArveCommandHandler(GetFaultyDbContext());

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Handle_should_delete_entity_if_exists()
        {
            var arve = new Arve 
            {   InvoiceNumber = "123",
                InvoiceDate = DateTime.Now,
                TellimusId = 1,
            };
            await DbContext.Arved.AddAsync(arve);
            await DbContext.SaveChangesAsync();

            var command = new DeleteArveCommand { Id = arve.Id };
            var handler = new DeleteArveCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);
            var deletedArve = await DbContext.Arved.FindAsync(arve.Id);

            Assert.NotNull(result);
            Assert.Null(deletedArve);
        }

        [Fact]
        public async Task Handle_should_not_fail_if_entity_does_not_exist()
        {
            var command = new DeleteArveCommand { Id = 9999 };
            var handler = new DeleteArveCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }
    }
}