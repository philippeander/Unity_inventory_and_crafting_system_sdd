namespace MyGame.Core
{
    public class InventorySlot
    {
        public string ItemId { get; set; }
        public int Amount { get; set; }

        public bool IsEmpty => string.IsNullOrEmpty(ItemId) || Amount <= 0;

        public InventorySlot(string itemId = null, int amount = 0)
        {
            ItemId = itemId;
            Amount = amount;
        }

        public void Clear()
        {
            ItemId = null;
            Amount = 0;
        }
    }
}

