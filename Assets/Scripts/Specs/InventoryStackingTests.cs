using NUnit.Framework;
using MyGame.Core;

namespace MyGame.Core.Specs
{
    /// <summary>
    /// Feature: Item Stacking Limits
    ///   Scenario: Attempting to stack items beyond the maximum limit
    ///     Given the Player has a slot with 98 "Health Potion"
    ///     And the stack limit for potions per slot is 100
    ///     When the player attempts to add 5 "Health Potion" to this inventory
    ///     Then the current slot should contain 100 potions
    ///     And the inventory should create a new slot containing 2 potions (or overflow remainder)
    ///     But if there are no empty slots, the system should return an "Inventory Full" error and reject the remaining potions.
    /// </summary>
    [TestFixture]
    public class InventoryStackingTests
    {
        [Test]
        [Description("Given a slot with 98 items, when adding 5 items with max stack 100, fills slot to 100 and creates new slot with remainder.")]
        public void AttemptingToStackItemsBeyondMaxLimit_WithEmptySlotsAvailable_FillsCurrentSlotAndCreatesNewSlot()
        {
            // Given the Player has an inventory with available slots and a slot with 98 "Health Potion"
            var state = new InventoryState(maxSlots: 5);
            state.Slots[0].ItemId = "Health Potion";
            state.Slots[0].Amount = 98;

            var brain = new InventoryBrain(state);

            // And the stack limit for potions per slot is 100
            var potionDefinition = new ItemDefinition("Health Potion", 100);

            // When the player attempts to add 5 "Health Potion" to this inventory (98 + 5 = 103)
            var result = brain.AddItem(potionDefinition, 5);

            // Then the operation succeeds and no items are rejected
            Assert.IsTrue(result.IsSuccess, "Expected AddItem operation to succeed.");
            Assert.AreEqual(0, result.RejectedAmount, "Expected all potions to be stored.");

            // And the current slot should contain 100 potions
            Assert.AreEqual("Health Potion", state.Slots[0].ItemId);
            Assert.AreEqual(100, state.Slots[0].Amount);

            // And the inventory should create a new slot containing 3 potions (remaining 3 of 5)
            Assert.AreEqual("Health Potion", state.Slots[1].ItemId);
            Assert.AreEqual(3, state.Slots[1].Amount);
        }

        [Test]
        [Description("Given a slot with 98 items, when adding 4 items with max stack 100, fills slot to 100 and creates new slot with exactly 2 potions.")]
        public void AttemptingToStackItemsBeyondMaxLimit_AddingFourPotions_FillsSlotAndCreatesNewSlotWithTwo()
        {
            // Given the Player has a slot with 98 "Health Potion"
            var state = new InventoryState(maxSlots: 5);
            state.Slots[0].ItemId = "Health Potion";
            state.Slots[0].Amount = 98;

            var brain = new InventoryBrain(state);
            var potionDefinition = new ItemDefinition("Health Potion", 100);

            // When the player attempts to add 4 "Health Potion" to this inventory (98 + 4 = 102)
            var result = brain.AddItem(potionDefinition, 4);

            // Then the current slot contains 100 potions and the new slot contains exactly 2 potions
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(0, result.RejectedAmount);
            Assert.AreEqual(100, state.Slots[0].Amount);
            Assert.AreEqual(2, state.Slots[1].Amount);
        }

        [Test]
        [Description("Given a full inventory with a slot having 98 items, when adding 5 items, fills slot to 100 and rejects remaining 3 with Inventory Full error.")]
        public void AttemptingToStackItemsBeyondMaxLimit_WithNoEmptySlots_ReturnsInventoryFullErrorAndRejectsRemaining()
        {
            // Given the Player has an inventory with only 1 slot containing 98 "Health Potion" (no empty slots available)
            var state = new InventoryState(maxSlots: 1);
            state.Slots[0].ItemId = "Health Potion";
            state.Slots[0].Amount = 98;

            var brain = new InventoryBrain(state);

            // And the stack limit for potions per slot is 100
            var potionDefinition = new ItemDefinition("Health Potion", 100);

            // When the player attempts to add 5 "Health Potion" to this inventory
            var result = brain.AddItem(potionDefinition, 5);

            // Then the current slot should contain 100 potions
            Assert.AreEqual("Health Potion", state.Slots[0].ItemId);
            Assert.AreEqual(100, state.Slots[0].Amount);

            // But if there are no empty slots, the system should return an "Inventory Full" error and reject the remaining potions
            Assert.IsFalse(result.IsSuccess, "Expected operation to report failure due to lack of space.");
            Assert.AreEqual(3, result.RejectedAmount, "Expected 3 remaining potions to be rejected.");
            Assert.AreEqual("Inventory Full", result.ErrorMessage);
        }

        [Test]
        [Description("Verifies adding items by ItemId using an injected ItemDatabase.")]
        public void AddItem_WithItemDatabase_ResolvesDefinitionAndStacksProperly()
        {
            var database = new ItemDatabase();
            database.RegisterItem(new ItemDefinition("Health Potion", 100));

            var state = new InventoryState(maxSlots: 2);
            state.Slots[0].ItemId = "Health Potion";
            state.Slots[0].Amount = 98;

            var brain = new InventoryBrain(state, database);

            var result = brain.AddItem("Health Potion", 5);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(0, result.RejectedAmount);
            Assert.AreEqual(100, state.Slots[0].Amount);
            Assert.AreEqual(3, state.Slots[1].Amount);
        }
    }
}
