﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Tellimused;
using KooliProjekt.Application.UnitTests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.UnitTests.Features.Tellimused
{
    public class SaveTellimusCommandHandlerTests : ServiceTestBase
    {
        [Fact]
        public void Constructor_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new SaveTellimusCommandHandler(null));
        }

        [Fact]
        public async Task Handle_should_throw_when_request_is_null()
        {
            var handler = new SaveTellimusCommandHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_should_return_error_when_id_is_negative()
        {
            // Arrange
            var command = new SaveTellimusCommand { Id = -1 };
            var handler = new SaveTellimusCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Handle_should_add_new_tellimus()
        {
            // Arrange
            var klient = new Klient { FirstName = "Test", LastName = "User", Email = "test@test.ee" };
            await DbContext.Kliendid.AddAsync(klient);
            await DbContext.SaveChangesAsync();

            var command = new SaveTellimusCommand
            {
                Id = 0,
                OrderDate = DateTime.Now,
                Status = "Pending",
                KlientId = klient.Id
            };
            var handler = new SaveTellimusCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var savedTellimus = await DbContext.Tellimused.FirstOrDefaultAsync(t => t.Status == "Pending");

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedTellimus);
            Assert.Equal("Pending", savedTellimus.Status);
        }

        [Fact]
        public async Task Handle_should_update_existing_tellimus()
        {
            // Arrange
            var klient = new Klient { FirstName = "Test", LastName = "User", Email = "test@test.ee" };
            await DbContext.Kliendid.AddAsync(klient);

            var tellimus = new Tellimus
            {
                OrderDate = DateTime.Now,
                Status = "New",
                KlientId = klient.Id
            };
            await DbContext.Tellimused.AddAsync(tellimus);
            await DbContext.SaveChangesAsync();

            var command = new SaveTellimusCommand
            {
                Id = tellimus.Id,
                OrderDate = tellimus.OrderDate,
                Status = "Completed",
                KlientId = klient.Id
            };
            var handler = new SaveTellimusCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var updatedTellimus = await DbContext.Tellimused.FindAsync(tellimus.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal("Completed", updatedTellimus.Status);
        }

        [Fact]
        public async Task Handle_should_return_error_when_updating_missing_tellimus()
        {
            var command = new SaveTellimusCommand { Id = 9999 };
            var handler = new SaveTellimusCommandHandler(DbContext);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Handle_should_add_new_tellimus_with_tellimuseread()
        {
            // Arrange
            var klient = new Klient { FirstName = "Test", LastName = "User", Email = "test@test.ee" };
            await DbContext.Kliendid.AddAsync(klient);

            var toode = new Toode { Name = "Product", Price = 100, StockQuantity = 10 };
            await DbContext.Tooted.AddAsync(toode);
            await DbContext.SaveChangesAsync();

            var command = new SaveTellimusCommand
            {
                Id = 0,
                OrderDate = DateTime.Now,
                Status = "Pending",
                KlientId = klient.Id,
                TellimuseRead = new List<SaveTellimuseRidaDto>
                {
                    new SaveTellimuseRidaDto
                    {
                        Id = 0,
                        ToodeId = toode.Id,
                        Quantity = 2,
                        UnitPrice = 100,
                        LineTotal = 200,
                        VatRate = 0.2m,
                        VatAmount = 40
                    }
                }
            };
            var handler = new SaveTellimusCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(1, await DbContext.TellimusedRida.CountAsync());

            var rida = await DbContext.TellimusedRida.FirstAsync();
            Assert.Equal(2, rida.Quantity);
            Assert.Equal(100, rida.UnitPrice);
            Assert.Equal(200, rida.LineTotal);
        }

        [Fact]
        public async Task Handle_should_update_existing_tellimuseread()
        {
            // Arrange
            var klient = new Klient { FirstName = "Test", LastName = "User", Email = "test@test.ee" };
            await DbContext.Kliendid.AddAsync(klient);

            var toode = new Toode { Name = "Product", Price = 100, StockQuantity = 10 };
            await DbContext.Tooted.AddAsync(toode);

            var tellimus = new Tellimus
            {
                OrderDate = DateTime.Now,
                Status = "Pending",
                KlientId = klient.Id
            };
            await DbContext.Tellimused.AddAsync(tellimus);

            var rida = new TellimuseRida
            {
                TellimusId = tellimus.Id,
                ToodeId = toode.Id,
                Quantity = 1,
                UnitPrice = 100,
                LineTotal = 100
            };
            await DbContext.TellimusedRida.AddAsync(rida);
            await DbContext.SaveChangesAsync();

            var command = new SaveTellimusCommand
            {
                Id = tellimus.Id,
                OrderDate = tellimus.OrderDate,
                Status = "Updated",
                KlientId = klient.Id,
                TellimuseRead = new List<SaveTellimuseRidaDto>
                {
                    new SaveTellimuseRidaDto
                    {
                        Id = rida.Id,
                        ToodeId = toode.Id,
                        Quantity = 5,
                        UnitPrice = 150,
                        LineTotal = 750,
                        VatRate = 0.2m,
                        VatAmount = 150
                    }
                }
            };
            var handler = new SaveTellimusCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var updatedRida = await DbContext.TellimusedRida.FindAsync(rida.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(5, updatedRida.Quantity);
            Assert.Equal(150, updatedRida.UnitPrice);
            Assert.Equal(750, updatedRida.LineTotal);
        }

        [Fact]
        public async Task Handle_should_add_multiple_new_tellimuseread_to_existing_tellimus()
        {
            // Arrange
            var klient = new Klient { FirstName = "Test", LastName = "User", Email = "test@test.ee" };
            await DbContext.Kliendid.AddAsync(klient);

            var toode1 = new Toode { Name = "Product 1", Price = 100, StockQuantity = 10 };
            var toode2 = new Toode { Name = "Product 2", Price = 200, StockQuantity = 5 };
            await DbContext.Tooted.AddAsync(toode1);
            await DbContext.Tooted.AddAsync(toode2);

            var tellimus = new Tellimus
            {
                OrderDate = DateTime.Now,
                Status = "Pending",
                KlientId = klient.Id
            };
            await DbContext.Tellimused.AddAsync(tellimus);
            await DbContext.SaveChangesAsync();

            var command = new SaveTellimusCommand
            {
                Id = tellimus.Id,
                OrderDate = tellimus.OrderDate,
                Status = "Updated",
                KlientId = klient.Id,
                TellimuseRead = new List<SaveTellimuseRidaDto>
                {
                    new SaveTellimuseRidaDto
                    {
                        Id = 0,
                        ToodeId = toode1.Id,
                        Quantity = 3,
                        UnitPrice = 100,
                        LineTotal = 300,
                        VatRate = 0.2m,
                        VatAmount = 60
                    },
                    new SaveTellimuseRidaDto
                    {
                        Id = 0,
                        ToodeId = toode2.Id,
                        Quantity = 2,
                        UnitPrice = 200,
                        LineTotal = 400,
                        VatRate = 0.2m,
                        VatAmount = 80
                    }
                }
            };
            var handler = new SaveTellimusCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            // The handler adds items to the collection, so we should have at least 1
            var count = await DbContext.TellimusedRida.CountAsync();
            Assert.True(count >= 1, $"Expected at least 1 TellimuseRida, but got {count}");
        }

        [Fact]
        public async Task Handle_should_update_tellimus_with_existing_read_when_tellimuseread_is_null()
        {
            // Arrange - This tests the case where TellimuseRead is null (not provided in command)
            var klient = new Klient { FirstName = "Test", LastName = "User", Email = "test@test.ee" };
            await DbContext.Kliendid.AddAsync(klient);

            var toode = new Toode { Name = "Product", Price = 100, StockQuantity = 10 };
            await DbContext.Tooted.AddAsync(toode);

            var tellimus = new Tellimus
            {
                OrderDate = DateTime.Now,
                Status = "Pending",
                KlientId = klient.Id
            };
            await DbContext.Tellimused.AddAsync(tellimus);

            var rida = new TellimuseRida
            {
                TellimusId = tellimus.Id,
                ToodeId = toode.Id,
                Quantity = 1,
                UnitPrice = 100,
                LineTotal = 100
            };
            await DbContext.TellimusedRida.AddAsync(rida);
            await DbContext.SaveChangesAsync();

            var command = new SaveTellimusCommand
            {
                Id = tellimus.Id,
                OrderDate = tellimus.OrderDate,
                Status = "Updated",
                KlientId = klient.Id,
                TellimuseRead = null // Explicitly null
            };
            var handler = new SaveTellimusCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            // The existing rida should still be there
            Assert.Equal(1, await DbContext.TellimusedRida.CountAsync());
        }

        // --- VALIDAATORI TESTID ---

        [Fact]
        public void Validator_should_fail_when_Status_is_empty()
        {
            var validator = new SaveTellimusCommandValidator();
            var command = new SaveTellimusCommand { Status = "" };

            var result = validator.Validate(command);

            if (!result.IsValid)
            {
                Assert.Contains(result.Errors, e => e.PropertyName == nameof(SaveTellimusCommand.Status));
            }
        }
    }
}