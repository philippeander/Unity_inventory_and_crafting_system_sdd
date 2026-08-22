using System;
using System.Collections.Generic;

namespace MyGame.Core
{
    public class InventoryState
    {
        public int MaxSlots { get; }
        public List<InventorySlot> Slots { get; }

        public InventoryState(int maxSlots)
        {
            if (maxSlots < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxSlots), "MaxSlots cannot be negative.");
            }

            MaxSlots = maxSlots;
            Slots = new List<InventorySlot>(maxSlots);

            for (int i = 0; i < maxSlots; i++)
            {
                Slots.Add(new InventorySlot());
            }
        }
    }
}

