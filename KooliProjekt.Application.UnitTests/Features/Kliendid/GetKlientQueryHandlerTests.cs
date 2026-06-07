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
        public async Task Handle_should_throw_ArgumentNullException_if_request_is_null()
        {
            // Arrange
            var handler = new GetKlientQueryHandler(DbContext);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task Handle_should_return_null_value_if_id_is_zero_or_less(int id)
        {
            // Arrange
            // Kasutame "Faulty" contexti - kui kood üritaks andmebaasi pöörduda, siis see context viskaks errori.
            // Tõestab, et andmebaasi päringut ei tehta.
            var dbContext = GetFaultyDbContext();
            var query = new GetKlientQuery { Id = id };
            var handler = new GetKlientQueryHandler(dbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.Value);
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