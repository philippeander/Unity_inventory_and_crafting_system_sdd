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
    ///     And the inventory should create a new slot containing 2 potions
    ///     But if there are no empty slots, the system should return an "Inventory Full" error and reject the remaining 2 potions.
    /// </summary>
    [TestFixture]
    public class InventoryStackingTests
    {
        [Test]
        [Description("Given a slot with 98 items, when adding 5 items with max stack 100, fills slot to 100 and creates new slot with 2.")]
        public void AttemptingToStackItemsBeyondMaxLimit_WithEmptySlotsAvailable_FillsCurrentSlotAndCreatesNewSlot()
        {
            // Given the Player has an inventory with available slots and a slot with 98 "Health Potion"
            var state = new InventoryState(maxSlots: 5);
            state.Slots[0].ItemId = "Health Potion";
            state.Slots[0].Amount = 98;

            var brain = new InventoryBrain(state);

            // And the stack limit for potions per slot is 100
            var potionDefinition = new ItemDefinition("Health Potion", 100);

            // When the player attempts to add 5 "Health Potion" to this inventory
            var result = brain.AddItem(potionDefinition, 5);

            // Then the current slot should contain 100 potions
            Assert.IsTrue(result.Success, "Expected AddItem operation to succeed.");
            Assert.AreEqual(0, result.RemainingAmount, "Expected all 5 potions to be stored.");
            Assert.AreEqual("Health Potion", state.Slots[0].ItemId);
            Assert.AreEqual(100, state.Slots[0].Amount);

            // And the inventory should create a new slot containing 2 potions
            Assert.AreEqual("Health Potion", state.Slots[1].ItemId);
            Assert.AreEqual(2, state.Slots[1].Amount);
        }

        [Test]
        [Description("Given a full inventory with a slot having 98 items, when adding 5 items, fills slot to 100 and rejects remaining 2 with Inventory Full error.")]
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

            // But if there are no empty slots, the system should return an "Inventory Full" error and reject the remaining 2 potions
            Assert.IsFalse(result.Success, "Expected operation to report failure/partial rejection due to lack of space.");
            Assert.AreEqual(2, result.RemainingAmount, "Expected 2 remaining potions to be rejected.");
            StringAssert.Contains("Inventory Full", result.ErrorMessage);
        }
    }
}

