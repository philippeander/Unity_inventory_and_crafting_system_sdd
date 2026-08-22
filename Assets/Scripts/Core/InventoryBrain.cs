using System;

namespace MyGame.Core
{
    public class InventoryBrain
    {
        private readonly InventoryState _state;

        public InventoryState State => _state;

        public InventoryBrain(InventoryState state)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));
        }

        public InventoryOperationResult AddItem(ItemDefinition item, int amount)
        {
            throw new NotImplementedException();
        }
    }
}

