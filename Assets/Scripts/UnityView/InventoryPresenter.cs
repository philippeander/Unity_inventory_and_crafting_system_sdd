using System;
using MyGame.Core;

namespace MyGame.UnityView
{
    /// <summary>
    /// Presenter connecting the UI Toolkit View (IInventoryView) to the Domain controller (InventoryBrain).
    /// Follows the MVP pattern: no engine references, purely mediates events and state updates.
    /// </summary>
    public class InventoryPresenter : IDisposable
    {
        private readonly IInventoryView _view;
        private readonly InventoryBrain _brain;

        public IInventoryView View => _view;
        public InventoryBrain Brain => _brain;

        public InventoryPresenter(IInventoryView view, InventoryBrain brain) : this(view, brain, null)
        {
        }

        public InventoryPresenter(IInventoryView view, InventoryBrain brain, InventoryState state)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _brain = brain ?? throw new ArgumentNullException(nameof(brain));

            _view.InitializeSlots(_brain.State.MaxSlots);
            _view.OnAddButtonClicked += HandleAddButtonClicked;
            RefreshView();
        }

        /// <summary>
        /// Synchronizes the View with current inventory state.
        /// </summary>
        public void RefreshView()
        {
            var slots = _brain.State.Slots;
            for (int i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];
                _view.UpdateSlot(i, slot.ItemId, slot.Amount);
            }
        }

        private void HandleAddButtonClicked(string itemId, int amount)
        {
            InventoryOperationResult result;

            if (_brain.ItemDatabase != null)
            {
                result = _brain.AddItem(itemId, amount);
            }
            else
            {
                var itemDefinition = new ItemDefinition(itemId, 100);
                result = _brain.AddItem(itemDefinition, amount);
            }

            if (!result.IsSuccess)
            {
                _view.ShowError(result.ErrorMessage);
            }
            else
            {
                _view.ClearError();
            }

            RefreshView();
        }

        public void Dispose()
        {
            if (_view != null)
            {
                _view.OnAddButtonClicked -= HandleAddButtonClicked;
            }
        }
    }
}
