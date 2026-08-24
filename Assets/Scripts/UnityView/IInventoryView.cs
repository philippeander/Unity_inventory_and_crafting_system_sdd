using System;

namespace MyGame.UnityView
{
    public interface IInventoryView
    {
        event Action<string, int> OnAddButtonClicked;

        void UpdateSlot(int slotIndex, string itemId, int amount);
        void ShowError(string message);
        void ClearError();
    }
}

