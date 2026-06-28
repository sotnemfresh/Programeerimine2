using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.Arved;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Linq;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class ArvedControllerTests : TestBase
    {
        // ===== LIST =====

        [Fact]
        public async Task List_should_return_paged_result()
        {
            // Arrange
            var url = "/api/Arved/List/?page=1&pageSize=10";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<ArveListDto>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        // ===== GET =====

        [Fact]
        public async Task Get_should_return_arve()
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

            var arve = new Arve
            {
                InvoiceNumber = "INV-001",
                InvoiceDate = DateTime.Now,
                Status = "Pending",
                SubTotal = 100,
                GrandTotal = 120,
                KlientId = klient.Id,
                TellimusId = tellimus.Id
            };
            await DbContext.Arved.AddAsync(arve);
            await DbContext.SaveChangesAsync();

            var url = $"/api/Arved/Get/?id={arve.Id}";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<ArveDto>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_arve()
        {
            // Arrange
            var url = "/api/Arved/Get/?id=9999";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        // ===== DELETE =====

        [Fact]
        public async Task Delete_should_remove_existing_arve()
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

            var arve = new Arve
            {
                InvoiceNumber = "INV-DEL",
                InvoiceDate = DateTime.Now,
                Status = "Pending",
                SubTotal = 100,
                GrandTotal = 120,
                KlientId = klient.Id,
                TellimusId = tellimus.Id
            };
            await DbContext.Arved.AddAsync(arve);
            await DbContext.SaveChangesAsync();

            var url = "/api/Arved/Delete/";

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { id = arve.Id })
            };
            using var response = await Client.SendAsync(request);
            var arveFromDb = await DbContext.Arved
           .Where(a => a.Id == arve.Id)
        .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Null(arveFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_work_with_missing_arve()
        {
            // Arrange
            var url = "/api/Arved/Delete/";

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
        public async Task Save_should_add_new_arve()
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

            var url = "/api/Arved/Save/";
            var command = new SaveArveCommand
            {
                InvoiceNumber = "INV-NEW",
                InvoiceDate = DateTime.Now,
                Status = "Pending",
                SubTotal = 100,
                GrandTotal = 120,
                KlientId = klient.Id,
                TellimusId = tellimus.Id
            };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_work_with_missing_arve()
        {
            // Arrange
            var url = "/api/Arved/Save/";
            var command = new SaveArveCommand { Id = 9999, InvoiceNumber = "GHOST", TellimusId = 1 };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content);
            Assert.Contains("error", content.ToLower());
        }

        [Fact]
        public async Task Save_should_work_with_invalid_arve()
        {
            // Arrange
            var url = "/api/Arved/Save/";
            var command = new SaveArveCommand { Id = 0, InvoiceNumber = "", TellimusId = 0 };

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