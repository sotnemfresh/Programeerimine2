using Xunit;
using KooliProjekt.Application.Features.Tellimused;
using KooliProjekt.Application.UnitTests;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace KooliProjekt.UnitTests.Features.Tellimused
{
    public class ListTellimusedQueryHandlerTests : ServiceTestBase
    {
        [Fact]
        public void Constructor_should_throw_if_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new ListTellimusedQueryHandler(null));
        }

        [Fact]
        public async Task Handle_should_throw_ArgumentNullException_if_request_is_null()
        {
            var handler = new ListTellimusedQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_should_throw_ArgumentException_if_pageSize_is_too_large()
        {
            var query = new ListTellimusedQuery { Page = 1, PageSize = 101 };
            var handler = new ListTellimusedQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}