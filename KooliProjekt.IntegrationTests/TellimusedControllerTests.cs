using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.Tellimused;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Linq;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class TellimusedControllerTests : TestBase
    {
        // ===== LIST =====

        [Fact]
        public async Task List_should_return_paged_result()
        {
            // Arrange
            var url = "/api/Tellimused/List/?page=1&pageSize=10";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<TellimusListDto>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        // ===== GET =====

        [Fact]
        public async Task Get_should_return_tellimus()
        {
            // Arrange
            var klient = new Klient { FirstName = "Test", LastName = "User", Email = "test@test.ee" };
            await DbContext.Kliendid.AddAsync(klient);
            await DbContext.SaveChangesAsync();

            var tellimus = new Tellimus
            {
                OrderDate = DateTime.Now,
                Status = "Pending",
                KlientId = klient.Id
            };
            await DbContext.Tellimused.AddAsync(tellimus);
            await DbContext.SaveChangesAsync();

            var url = $"/api/Tellimused/Get/?id={tellimus.Id}";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<TellimusDto>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_tellimus()
        {
            // Arrange
            var url = "/api/Tellimused/Get/?id=9999";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        // ===== DELETE =====

        [Fact]
        public async Task Delete_should_remove_existing_tellimus()
        {
            // Arrange
            var klient = new Klient { FirstName = "Test", LastName = "User", Email = "test@test.ee" };
            await DbContext.Kliendid.AddAsync(klient);
            await DbContext.SaveChangesAsync();

            var tellimus = new Tellimus
            {
                OrderDate = DateTime.Now,
                Status = "Pending",
                KlientId = klient.Id
            };
            await DbContext.Tellimused.AddAsync(tellimus);
            await DbContext.SaveChangesAsync();

            var url = "/api/Tellimused/Delete/";

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { id = tellimus.Id })
            };
            using var response = await Client.SendAsync(request);
            var tellimusFromDb = await DbContext.Tellimused
           .Where(t => t.Id == tellimus.Id)
          .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Null(tellimusFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_work_with_missing_tellimus()
        {
            // Arrange
            var url = "/api/Tellimused/Delete/";

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
        public async Task Save_should_add_new_tellimus()
        {
            // Arrange
            var klient = new Klient { FirstName = "Test", LastName = "User", Email = "test@test.ee" };
            await DbContext.Kliendid.AddAsync(klient);
            await DbContext.SaveChangesAsync();

            var url = "/api/Tellimused/Save/";
            var command = new SaveTellimusCommand
            {
                OrderDate = DateTime.Now,
                Status = "Pending",
                KlientId = klient.Id
            };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_work_with_missing_tellimus()
        {
            // Arrange
            var url = "/api/Tellimused/Save/";
            var command = new SaveTellimusCommand { Id = 9999, Status = "Pending" };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content);
            Assert.Contains("error", content.ToLower());
        }

        [Fact]
        public async Task Save_should_work_with_invalid_tellimus()
        {
            // Arrange
            var url = "/api/Tellimused/Save/";
            var command = new SaveTellimusCommand { Id = 0, Status = "" };

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