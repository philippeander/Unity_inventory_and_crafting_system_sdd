using System;

namespace MyGame.Core
{
    public class InventoryBrain
    {
        private readonly InventoryState _state;
        private readonly IItemDatabase _itemDatabase;

        public InventoryState State => _state;
        public IItemDatabase ItemDatabase => _itemDatabase;

        public InventoryBrain(InventoryState state, IItemDatabase itemDatabase = null)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));
            _itemDatabase = itemDatabase;
        }

        public InventoryOperationResult AddItem(ItemDefinition item, int amount)
        {
            if (string.IsNullOrEmpty(item.Id) || amount <= 0)
            {
                return InventoryOperationResult.Success();
            }

            int remaining = amount;
            int maxStack = item.MaxStackSize > 0 ? item.MaxStackSize : 1;

            // 1. Fill existing slots of this item that have available space
            foreach (var slot in _state.Slots)
            {
                if (remaining <= 0)
                {
                    break;
                }

                if (!slot.IsEmpty && slot.ItemId == item.Id && slot.Amount < maxStack)
                {
                    int spaceAvailable = maxStack - slot.Amount;
                    int toAdd = Math.Min(spaceAvailable, remaining);
                    slot.Amount += toAdd;
                    remaining -= toAdd;
                }
            }

            // 2. Fill empty slots if there is still remaining amount
            if (remaining > 0)
            {
                foreach (var slot in _state.Slots)
                {
                    if (remaining <= 0)
                    {
                        break;
                    }

                    if (slot.IsEmpty)
                    {
                        int toAdd = Math.Min(maxStack, remaining);
                        slot.ItemId = item.Id;
                        slot.Amount = toAdd;
                        remaining -= toAdd;
                    }
                }
            }

            // 3. If no empty slots were available and there is remaining amount
            if (remaining > 0)
            {
                return InventoryOperationResult.Failure(remaining, "Inventory Full");
            }

            // 4. Everything was added successfully
            return InventoryOperationResult.Success();
        }

        public InventoryOperationResult AddItem(string itemId, int amount)
        {
            if (_itemDatabase == null)
            {
                throw new InvalidOperationException("ItemDatabase must be configured to add items by ID.");
            }

            if (!_itemDatabase.TryGetItem(itemId, out var item))
            {
                return InventoryOperationResult.Failure(amount, $"Item '{itemId}' not found in database.");
            }

            return AddItem(item, amount);
        }
    }
}
