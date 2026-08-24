using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MyGame.Core;

namespace MyGame.UnityView
{
    /// <summary>
    /// UI Toolkit implementation of IInventoryView.
    /// Acts as a pure View in the MVP pattern without containing business logic.
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public class InventoryView : MonoBehaviour, IInventoryView
    {
        [Header("Bootstrap Configuration (Optional)")]
        [SerializeField] private bool _autoBootstrap = true;
        [SerializeField] private int _initialSlots = 5;
        [SerializeField] private int _potionMaxStack = 100;

        public event Action<string, int> OnAddButtonClicked;

        private UIDocument _uiDocument;
        private Button _btnAddPotion;
        private Label _lblError;
        private readonly Dictionary<int, Label> _slotLabels = new Dictionary<int, Label>();
        private InventoryPresenter _presenter;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
        }

        private void Start()
        {
            if (_autoBootstrap && _presenter == null)
            {
                var database = new ItemDatabase();
                database.RegisterItem(new ItemDefinition("Health Potion", _potionMaxStack));

                var state = new InventoryState(_initialSlots);
                var brain = new InventoryBrain(state, database);
                _presenter = new InventoryPresenter(this, brain);
            }
        }

        private void OnEnable()
        {
            if (_uiDocument == null)
            {
                _uiDocument = GetComponent<UIDocument>();
            }

            var root = _uiDocument?.rootVisualElement;
            if (root == null)
            {
                return;
            }

            // Query Add Potion button
            _btnAddPotion = root.Q<Button>("btnAddPotion");
            if (_btnAddPotion != null)
            {
                _btnAddPotion.clicked += HandleAddPotionClicked;
            }

            // Query Error label
            _lblError = root.Q<Label>("lblError");
            ClearError();

            // Cache slot labels if present in UXML
            _slotLabels.Clear();
            for (int i = 0; i < 20; i++)
            {
                var slotLabel = root.Q<Label>($"slot-label-{i}");
                if (slotLabel != null)
                {
                    _slotLabels[i] = slotLabel;
                }
            }
        }

        private void OnDisable()
        {
            if (_btnAddPotion != null)
            {
                _btnAddPotion.clicked -= HandleAddPotionClicked;
            }

            _presenter?.Dispose();
            _presenter = null;
        }

        private void HandleAddPotionClicked()
        {
            // Simulate adding 5 Health Potions per click matching the Gherkin scenario
            OnAddButtonClicked?.Invoke("Health Potion", 5);
        }

        public void UpdateSlot(int slotIndex, string itemId, int amount)
        {
            if (_slotLabels.TryGetValue(slotIndex, out var label))
            {
                label.text = string.IsNullOrEmpty(itemId) || amount <= 0
                    ? $"Slot {slotIndex}: [Empty]"
                    : $"Slot {slotIndex}: {itemId} x{amount}";
            }
            else
            {
                var root = _uiDocument?.rootVisualElement;
                var dynamicLabel = root?.Q<Label>($"slot-label-{slotIndex}");
                if (dynamicLabel != null)
                {
                    _slotLabels[slotIndex] = dynamicLabel;
                    dynamicLabel.text = string.IsNullOrEmpty(itemId) || amount <= 0
                        ? $"Slot {slotIndex}: [Empty]"
                        : $"Slot {slotIndex}: {itemId} x{amount}";
                }
            }
        }

        public void ShowError(string message)
        {
            if (_lblError != null)
            {
                _lblError.text = message;
                _lblError.style.display = DisplayStyle.Flex;
            }
        }

        public void ClearError()
        {
            if (_lblError != null)
            {
                _lblError.text = string.Empty;
                _lblError.style.display = DisplayStyle.None;
            }
        }
    }
}

