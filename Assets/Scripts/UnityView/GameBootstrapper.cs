using UnityEngine;
using MyGame.Core;

namespace MyGame.UnityView
{
    /// <summary>
    /// Composition Root for the Inventory MVP architecture.
    /// Wires together the Core Domain (InventoryState, ItemDatabase, InventoryBrain) and Presentation (InventoryView, InventoryPresenter).
    /// </summary>
    public class GameBootstrapper : MonoBehaviour
    {
        [Header("View Reference")]
        [SerializeField] private InventoryView _inventoryView;

        [Header("Inventory Setup")]
        [SerializeField] private int _maxSlots = 2;
        [SerializeField] private int _potionMaxStack = 100;

        private InventoryPresenter _presenter;

        public InventoryState State { get; private set; }
        public InventoryBrain Brain { get; private set; }
        public InventoryPresenter Presenter => _presenter;

        private void Start()
        {
            // 1. Resolve View
            if (_inventoryView == null)
            {
                #if UNITY_2023_1_OR_NEWER
                _inventoryView = Object.FindFirstObjectByType<InventoryView>();
                #else
                _inventoryView = Object.FindObjectOfType<InventoryView>();
                #endif
            }

            if (_inventoryView == null)
            {
                Debug.LogError("[GameBootstrapper] Failed to find an InventoryView instance in the scene.", this);
                return;
            }

            // 2. Initialize Domain State (configured with 2 max slots for quick testing)
            State = new InventoryState(_maxSlots);

            // 3. Initialize Item Database with Health Potion definition
            var itemDatabase = new ItemDatabase();
            itemDatabase.RegisterItem(new ItemDefinition("Health Potion", _potionMaxStack));

            // 4. Initialize Domain Controller (Brain)
            Brain = new InventoryBrain(State, itemDatabase);

            // 5. Initialize Presenter (mediating View, Brain, and State)
            _presenter = new InventoryPresenter(_inventoryView, Brain, State);

            Debug.Log($"[GameBootstrapper] Initialized Inventory MVP with {_maxSlots} max slots (Health Potion stack limit: {_potionMaxStack}).");
        }

        private void OnDestroy()
        {
            _presenter?.Dispose();
            _presenter = null;
        }
    }
}

