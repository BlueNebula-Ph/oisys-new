using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using OisysNew;
using OisysNew.Helpers;
using OisysNew.Models;
using OisysNew.Services;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Oisys.UnitTests.Services
{
    public class InventoryServiceTests
    {
        private readonly Fixture fixture = new Fixture();
        private readonly DbSet<Item> itemSet = Substitute.For<DbSet<Item>, IQueryable<Item>>();
        private readonly IOisysDbContext oisysDbContext = Substitute.For<IOisysDbContext>();
        private readonly ILogger<InventoryService> logger = Substitute.For<ILogger<InventoryService>>();
        private readonly List<InventoryAdjustment> adjustments = new List<InventoryAdjustment>();
        private readonly InventoryAdjustment adjustment;
        private readonly Item item;
        private readonly InventoryService service;

        public InventoryServiceTests()
        {
            // Setup the db context
            this.item = fixture.Build<Item>()
                .With(a => a.Quantity, 10)
                .Without(a => a.Adjustments)
                .Create();
            this.oisysDbContext.Items.Returns(itemSet);
            this.oisysDbContext.Items.FindAsync(Arg.Any<int>()).Returns(item);

            // Setup the adjustment
            this.adjustment = fixture.Build<InventoryAdjustment>()
                .With(a => a.Quantity, 5)
                .Create();

            this.service = new InventoryService(oisysDbContext, logger);
        }

        [Fact]
        public async void AdjustItemQuantities_Shoud_Add_Quantities()
        {
            // Arrange
            this.adjustment.AdjustmentType = AdjustmentType.Add;
            this.adjustments.Add(adjustment);

            // Act
            await this.service.AdjustItemQuantities(adjustments);
            
            // Assert
            this.oisysDbContext.Received(1).Update(Arg.Is<Item>(a => a.Quantity == 15));
        }

        [Fact]
        public async void AdjustItemQuantities_Shoud_Deduct_Quantities()
        {
            // Arrange
            this.adjustment.AdjustmentType = AdjustmentType.Deduct;
            this.adjustments.Add(adjustment);

            // Act
            await this.service.AdjustItemQuantities(adjustments);

            // Assert
            this.oisysDbContext.Received(1).Update(Arg.Is<Item>(a => a.Quantity == 5));
        }

        [Fact]
        public async void AdjustItemQuantities_Should_Save_Adjustment_When_Necessary()
        {
            // Arrange
            this.adjustment.SaveAdjustmentDetails = true;
            this.adjustments.Add(this.adjustment);

            // Act
            await this.service.AdjustItemQuantities(this.adjustments);

            // Assert
            this.oisysDbContext.Received(1).Add(Arg.Any<Adjustment>());
        }
    }
}
