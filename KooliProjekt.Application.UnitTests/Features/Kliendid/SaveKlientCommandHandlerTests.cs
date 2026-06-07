using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Kliendid;
using KooliProjekt.Application.UnitTests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.UnitTests.Features.Kliendid
{
    public class SaveKlientCommandHandlerTests : ServiceTestBase
    {
        [Fact]
        public void Constructor_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new SaveKlientCommandHandler(null));
        }

        [Fact]
        public async Task Handle_should_add_new_klient()
        {
            var command = new SaveKlientCommand
            {
                Id = 0,
                FirstName = "Juhan",
                LastName = "Juurekas",
                Email = "juhan@test.ee"
            };
            var handler = new SaveKlientCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);
            var savedKlient = await DbContext.Kliendid.FirstOrDefaultAsync(k => k.Email == "juhan@test.ee");

            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedKlient);
            Assert.Equal("Juhan", savedKlient.FirstName);
        }

        [Fact]
        public async Task Handle_should_update_existing_klient()
        {
            var klient = new Klient { FirstName = "Malle", LastName = "K", Email = "malle@test.ee" };
            await DbContext.Kliendid.AddAsync(klient);
            await DbContext.SaveChangesAsync();

            var command = new SaveKlientCommand
            {
                Id = klient.Id,
                FirstName = "Malle-Liis",
                LastName = "K",
                Email = "malle@test.ee"
            };
            var handler = new SaveKlientCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);
            var updatedKlient = await DbContext.Kliendid.FindAsync(klient.Id);

            Assert.False(result.HasErrors);
            Assert.Equal("Malle-Liis", updatedKlient.FirstName);
        }

        // --- VALIDAATORI TESTID ---

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Validator_should_fail_when_LastName_is_missing(string lastName)
        {
            var validator = new SaveKlientCommandValidator();
            var command = new SaveKlientCommand { FirstName = "Test", LastName = lastName };

            var result = validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(SaveKlientCommand.LastName));
        }
    }
}