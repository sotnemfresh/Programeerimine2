using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.TellimuseRead;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Linq;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class TellimuseReadControllerTests : TestBase
    {
        // ===== LIST =====

        [Fact]
        public async Task List_should_return_paged_result()
        {
            // Arrange
            var url = "/api/TellimuseRead/List/?page=1&pageSize=10";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<PagedResult<TellimuseRidaListDto>>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        // ===== GET =====

        [Fact]
        public async Task Get_should_return_tellimuserida()
        {
            // Arrange
            var klient = new Klient { FirstName = "Test", LastName = "User", Email = "test@test.ee" };
            await DbContext.Kliendid.AddAsync(klient);
            await DbContext.SaveChangesAsync();

            var toode = new Toode { Name = "Test Product", Price = 100, StockQuantity = 50 };
            await DbContext.Tooted.AddAsync(toode);
            await DbContext.SaveChangesAsync();

            var tellimus = new Tellimus
            {
                OrderDate = DateTime.Now,
                Status = "Pending",
                KlientId = klient.Id
            };
            await DbContext.Tellimused.AddAsync(tellimus);
            await DbContext.SaveChangesAsync();

            var rida = new TellimuseRida
            {
                TellimusId = tellimus.Id,
                ToodeId = toode.Id,
                Quantity = 2,
                UnitPrice = 100,
                LineTotal = 200,
                VatRate = 0.2m,
                VatAmount = 40
            };
            await DbContext.TellimusedRida.AddAsync(rida);
            await DbContext.SaveChangesAsync();

            var url = $"/api/TellimuseRead/Get/?id={rida.Id}";

            // Act
            var response = await Client.GetFromJsonAsync<OperationResult<TellimuseRidaDto>>(url);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.HasErrors);
        }

        [Fact]
        public async Task Get_should_return_not_found_for_missing_rida()
        {
            // Arrange
            var url = "/api/TellimuseRead/Get/?id=9999";

            // Act
            var response = await Client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        // ===== DELETE =====

        [Fact]
        public async Task Delete_should_remove_existing_rida()
        {
            // Arrange
            var klient = new Klient { FirstName = "Test", LastName = "User", Email = "test@test.ee" };
            await DbContext.Kliendid.AddAsync(klient);
            await DbContext.SaveChangesAsync();

            var toode = new Toode { Name = "Product", Price = 100, StockQuantity = 50 };
            await DbContext.Tooted.AddAsync(toode);
            await DbContext.SaveChangesAsync();

            var tellimus = new Tellimus
            {
                OrderDate = DateTime.Now,
                Status = "Pending",
                KlientId = klient.Id
            };
            await DbContext.Tellimused.AddAsync(tellimus);
            await DbContext.SaveChangesAsync();

            var rida = new TellimuseRida
            {
                TellimusId = tellimus.Id,
                ToodeId = toode.Id,
                Quantity = 2,
                UnitPrice = 100,
                LineTotal = 200
            };
            await DbContext.TellimusedRida.AddAsync(rida);
            await DbContext.SaveChangesAsync();

            var url = "/api/TellimuseRead/Delete/";

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = JsonContent.Create(new { id = rida.Id })
            };
            using var response = await Client.SendAsync(request);
            var ridaFromDb = await DbContext.TellimusedRida
         .Where(r => r.Id == rida.Id)
                .FirstOrDefaultAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Null(ridaFromDb);
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_work_with_missing_rida()
        {
            // Arrange
            var url = "/api/TellimuseRead/Delete/";

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
        public async Task Save_should_add_new_rida()
        {
            // Arrange
            var klient = new Klient { FirstName = "Test", LastName = "User", Email = "test@test.ee" };
            await DbContext.Kliendid.AddAsync(klient);
            await DbContext.SaveChangesAsync();

            var toode = new Toode { Name = "Product", Price = 100, StockQuantity = 50 };
            await DbContext.Tooted.AddAsync(toode);
            await DbContext.SaveChangesAsync();

            var tellimus = new Tellimus
            {
                OrderDate = DateTime.Now,
                Status = "Pending",
                KlientId = klient.Id
            };
            await DbContext.Tellimused.AddAsync(tellimus);
            await DbContext.SaveChangesAsync();

            var url = "/api/TellimuseRead/Save/";
            var command = new SaveTellimuseReadCommand
            {
                TellimusId = tellimus.Id,
                ToodeId = toode.Id,
                Quantity = 3,
                UnitPrice = 100,
                LineTotal = 300,
                VatRate = 0.2m,
                VatAmount = 60
            };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OperationResult>();
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Save_should_work_with_missing_rida()
        {
            // Arrange
            var url = "/api/TellimuseRead/Save/";
            var command = new SaveTellimuseReadCommand { Id = 9999, TellimusId = 1, ToodeId = 1, Quantity = 1 };

            // Act
            using var response = await Client.PostAsJsonAsync(url, command);

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content);
            Assert.Contains("error", content.ToLower());
        }

        [Fact]
        public async Task Save_should_work_with_invalid_rida()
        {
            // Arrange
            var url = "/api/TellimuseRead/Save/";
            var command = new SaveTellimuseReadCommand { Id = 0, TellimusId = 0, ToodeId = 0, Quantity = 0 };

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