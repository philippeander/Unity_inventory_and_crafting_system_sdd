using System.Collections.Generic;

namespace MyGame.Core
{
    public class ItemDatabase : IItemDatabase
    {
        private readonly Dictionary<string, ItemDefinition> _items = new Dictionary<string, ItemDefinition>();

        public void RegisterItem(ItemDefinition item)
        {
            _items[item.Id] = item;
        }

        public bool TryGetItem(string itemId, out ItemDefinition item)
        {
            if (itemId == null)
            {
                item = default;
                return false;
            }

            return _items.TryGetValue(itemId, out item);
        }

        public ItemDefinition GetItem(string itemId)
        {
            if (TryGetItem(itemId, out var item))
            {
                return item;
            }

            throw new KeyNotFoundException($"Item with ID '{itemId}' not found in database.");
        }
    }
}

