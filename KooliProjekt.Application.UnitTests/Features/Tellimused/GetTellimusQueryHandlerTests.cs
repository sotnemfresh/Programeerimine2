using Xunit;
using KooliProjekt.Application.Features.Tellimused;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.UnitTests;

namespace KooliProjekt.UnitTests.Features.Tellimused
{
    public class GetTellimusQueryHandlerTests : ServiceTestBase
    {
        [Fact]
        public void Constructor_should_throw_if_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new GetTellimusQueryHandler(null));
        }

        [Fact]
        public async Task Handle_should_throw_ArgumentNullException_if_request_is_null()
        {
            var handler = new GetTellimusQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task Handle_should_return_null_value_if_id_is_zero_or_less(int id)
        {
            var dbContext = GetFaultyDbContext();
            var query = new GetTellimusQuery { Id = id };
            var handler = new GetTellimusQueryHandler(dbContext);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Null(result.Value);
        }
    }
}