﻿using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Tooted;
using KooliProjekt.Application.UnitTests;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KooliProjekt.UnitTests.Features.Tooted
{
    public class SaveToodeCommandTests : ServiceTestBase
    {
        [Fact]
        public void Constructor_should_throw_if_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => new SaveToodeCommandHandler(null));
        }

        [Fact]
        public async Task Handle_should_throw_ArgumentNullException_if_request_is_null()
        {
            var handler = new SaveToodeCommandHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_should_return_error_when_id_is_negative()
        {
            // Arrange
            var command = new SaveToodeCommand { Id = -1 };
            var handler = new SaveToodeCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.HasErrors);
        }

        [Fact]
        public async Task Handle_should_add_new_toode()
        {
            // Arrange
            var command = new SaveToodeCommand
            {
                Id = 0,
                Name = "Test Toode",
                Description = "Test Description",
                FotoURL = "http://example.com/photo.jpg",
                Price = 10.5m,
                StockQuantity = 100
            };
            var handler = new SaveToodeCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal(1, await DbContext.Tooted.CountAsync());
        }

        [Fact]
        public async Task Handle_should_update_existing_toode()
        {
            // Arrange
            var toode = new Toode
            {
                Name = "Original Name",
                Price = 50,
                StockQuantity = 10
            };
            await DbContext.Tooted.AddAsync(toode);
            await DbContext.SaveChangesAsync();

            var command = new SaveToodeCommand
            {
                Id = toode.Id,
                Name = "Updated Name",
                Description = "Updated Description",
                Price = 75,
                StockQuantity = 20
            };
            var handler = new SaveToodeCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var updated = await DbContext.Tooted.FindAsync(toode.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.Equal("Updated Name", updated.Name);
            Assert.Equal(75, updated.Price);
            Assert.Equal(20, updated.StockQuantity);
        }

        [Fact]
        public async Task Handle_should_return_error_when_product_not_found()
        {
            // Arrange
            var command = new SaveToodeCommand { Id = 9999, Name = "Missing" };
            var handler = new SaveToodeCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.HasErrors);
        }
    }
}