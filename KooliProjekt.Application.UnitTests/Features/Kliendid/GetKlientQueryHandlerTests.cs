using Xunit;
using KooliProjekt.Application.Features.Kliendid;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Data;
using System;
using System.Threading;
using System.Threading.Tasks;
// TÄHTIS: See rida peab ühtima Sinu ServiceTestBase faili namespace-iga
using KooliProjekt.Application.UnitTests;

namespace KooliProjekt.UnitTests.Features.Kliendid
{
    // Nüüd pärime õigest klassist: ServiceTestBase
    public class GetKlientQueryHandlerTests : ServiceTestBase
    {
        [Fact]
        public void Constructor_should_throw_if_dbcontext_is_null()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new GetKlientQueryHandler(null));
        }

        [Fact]
        public async Task Handle_should_return_null_value_if_request_is_null()
        {
            // Arrange
            // DbContext on kättesaadav, sest pärime ServiceTestBase-st
            var handler = new GetKlientQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(null, CancellationToken.None);

            // Assert
            Assert.Null(result.Value);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Handle_should_return_object_if_it_exists()
        {
            // Arrange
            var klient = new Klient
            {
                FirstName = "Test",
                LastName = "Kasutaja",
                Email = "test@test.ee"
            };

            await DbContext.Kliendid.AddAsync(klient);
            await DbContext.SaveChangesAsync();

            var query = new GetKlientQuery { Id = klient.Id };
            var handler = new GetKlientQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(klient.Id, result.Value.Id);
        }
    }
}