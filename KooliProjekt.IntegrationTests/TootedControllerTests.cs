using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.Tooted;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Linq;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class TootedControllerTests : TestBase
    {
        // ===== LIST =====

        [Fact]
        public async Task List_should_return_paged_result()
        {
            // Arrange
            var url = "/api/Tooted/List/?page=1&pageSize=10";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<ToodeListDto>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        // ===== GET =====

        [Fact]
        public async Task Get_should_return_toode()
        {
            // Arrange
            var toode = new Toode { Name = "Test Product", Price = 99.99m, StockQuantity = 50 };
            await DbContext.Tooted.AddAsync(toode);
            await DbContext.SaveChangesAsync();

            var url = $"/api/Tooted/Get/?id={toode.Id}";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<ToodeDto>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_toode()
        {
            // Arrange
            var url = "/api/Tooted/Get/?id=9999";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        // ===== DELETE =====

        [Fact]
        public async Task Delete_should_remove_existing_toode()
        {
            // Arrange
            var toode = new Toode { Name = "DeleteMe", Price = 10 };
            await DbContext.Tooted.AddAsync(toode);
            await DbContext.SaveChangesAsync();

            var url = "/api/Tooted/Delete/";

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { id = toode.Id })
            };
            using var response = await Client.SendAsync(request);
            var toodeFromDb = await DbContext.Tooted
          .Where(t => t.Id == toode.Id)
        .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Null(toodeFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_work_with_missing_toode()
        {
            // Arrange
            var url = "/api/Tooted/Delete/";

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
        public async Task Save_should_add_new_toode()
        {
            // Arrange
            var url = "/api/Tooted/Save/";
            var command = new SaveToodeCommand
            {
                Name = "New Product",
                Description = "Test desc",
                Price = 25.50m,
                StockQuantity = 100
            };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);
            var toodeFromDb = await DbContext.Tooted
      .Where(t => t.Name == "New Product")
                      .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(toodeFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_work_with_missing_toode()
        {
            // Arrange
            var url = "/api/Tooted/Save/";
            var command = new SaveToodeCommand { Id = 9999, Name = "Ghost" };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);

            // Assert - ErrorHandlingBehavior catches the error and returns 200 with error message
            // OR it could be BadRequest if validation/handler returns error
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content);
            Assert.Contains("error", content.ToLower());
        }

        [Fact]
        public async Task Save_should_work_with_invalid_toode()
        {
            // Arrange
            var url = "/api/Tooted/Save/";
            var command = new SaveToodeCommand { Id = 0, Name = "", Price = -1 };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content);
            // Validation errors should be present
            Assert.True(
        response.StatusCode == HttpStatusCode.BadRequest || content.Contains("error", System.StringComparison.OrdinalIgnoreCase),
$"Expected error response but got {response.StatusCode}: {content}"
            );
        }
    }
}