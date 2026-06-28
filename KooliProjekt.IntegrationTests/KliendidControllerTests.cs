using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.Kliendid;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Linq;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class KliendidControllerTests : TestBase
    {
        // ===== LIST =====

        [Fact]
        public async Task List_should_return_paged_result()
        {
            // Arrange
            var url = "/api/Kliendid/List/?page=1&pageSize=10";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<KlientListDto>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        // ===== GET =====

        [Fact]
        public async Task Get_should_return_klient()
        {
            // Arrange
            var klient = new Klient { FirstName = "Test", LastName = "User", Email = "test@test.ee" };
            await DbContext.Kliendid.AddAsync(klient);
            await DbContext.SaveChangesAsync();

            var url = $"/api/Kliendid/Get/?id={klient.Id}";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<KlientDto>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_klient()
        {
            // Arrange
            var url = "/api/Kliendid/Get/?id=9999";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        // ===== DELETE =====

        [Fact]
        public async Task Delete_should_remove_existing_klient()
        {
            // Arrange
            var klient = new Klient { FirstName = "Delete", LastName = "Me", Email = "del@test.ee" };
            await DbContext.Kliendid.AddAsync(klient);
            await DbContext.SaveChangesAsync();

            var url = "/api/Kliendid/Delete/";

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { id = klient.Id })
            };
            using var response = await Client.SendAsync(request);
            var klientFromDb = await DbContext.Kliendid
              .Where(k => k.Id == klient.Id)
                         .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Null(klientFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_work_with_missing_klient()
        {
            // Arrange
            var url = "/api/Kliendid/Delete/";

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { id = 9999 })
            };
            using var response = await Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        // ===== SAVE =====

        [Fact]
        public async Task Save_should_add_new_klient()
        {
            // Arrange
            var url = "/api/Kliendid/Save/";
            var command = new SaveKlientCommand
            {
                FirstName = "Uus",
                LastName = "Klient",
                Email = "uus@test.ee"
            };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);
            var klientFromDb = await DbContext.Kliendid
            .Where(k => k.Email == "uus@test.ee").FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(klientFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_work_with_invalid_klient()
        {
            // Arrange
            var url = "/api/Kliendid/Save/";
            var command = new SaveKlientCommand { FirstName = "", LastName = "" };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content);
            Assert.True(
          response.StatusCode == HttpStatusCode.BadRequest || content.Contains("error", System.StringComparison.OrdinalIgnoreCase),
       $"Expected error response but got {response.StatusCode}: {content}"
             );
        }
    }
}