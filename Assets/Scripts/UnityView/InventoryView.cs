using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyGame.UnityView
{
    /// <summary>
    /// UI Toolkit implementation of IInventoryView.
    /// Acts as a pure View in the MVP pattern without containing business logic.
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public class InventoryView : MonoBehaviour, IInventoryView
    {
        public event Action<string, int> OnAddButtonClicked;

        private UIDocument _uiDocument;
        private Button _btnAddPotion;
        private Label _lblError;
        private VisualElement _slotsContainer;
        private readonly Dictionary<int, Label> _slotLabels = new Dictionary<int, Label>();
        private int _configuredSlotCount = 0;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            BindVisualElements();
        }

        private void OnDisable()
        {
            UnbindVisualElements();
        }

        public void InitializeSlots(int slotCount)
        {
            _configuredSlotCount = slotCount;

            var root = GetRootVisualElement();
            if (root == null)
            {
                return;
            }

            _slotsContainer = root.Q<VisualElement>("slotsContainer") ?? root.Q<VisualElement>(className: "slots-container");
            if (_slotsContainer == null)
            {
                return;
            }

            _slotsContainer.Clear();
            _slotLabels.Clear();

            for (int i = 0; i < slotCount; i++)
            {
                var slotBox = new VisualElement();
                slotBox.AddToClassList("slot-box");
                slotBox.name = $"slot-box-{i}";

                var slotLabel = new Label($"Slot {i + 1}: [Empty]");
                slotLabel.AddToClassList("slot-label");
                slotLabel.name = $"slot-label-{i}";

                slotBox.Add(slotLabel);
                _slotsContainer.Add(slotBox);
                _slotLabels[i] = slotLabel;
            }
        }

        public void UpdateSlot(int slotIndex, string itemId, int amount)
        {
            if (_slotLabels.TryGetValue(slotIndex, out var label))
            {
                label.text = string.IsNullOrEmpty(itemId) || amount <= 0
                    ? $"Slot {slotIndex + 1}: [Empty]"
                    : $"Slot {slotIndex + 1}: {itemId} x{amount}";
            }
            else
            {
                var root = GetRootVisualElement();
                var dynamicLabel = root?.Q<Label>($"slot-label-{slotIndex}");
                if (dynamicLabel != null)
                {
                    _slotLabels[slotIndex] = dynamicLabel;
                    dynamicLabel.text = string.IsNullOrEmpty(itemId) || amount <= 0
                        ? $"Slot {slotIndex + 1}: [Empty]"
                        : $"Slot {slotIndex + 1}: {itemId} x{amount}";
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

        private VisualElement GetRootVisualElement()
        {
            if (_uiDocument == null)
            {
                _uiDocument = GetComponent<UIDocument>();
            }

            return _uiDocument?.rootVisualElement;
        }

        private void BindVisualElements()
        {
            var root = GetRootVisualElement();
            if (root == null)
            {
                return;
            }

            _btnAddPotion = root.Q<Button>("btnAddPotion");
            if (_btnAddPotion != null)
            {
                _btnAddPotion.clicked -= HandleAddPotionClicked;
                _btnAddPotion.clicked += HandleAddPotionClicked;
            }

            _lblError = root.Q<Label>("lblError");
            ClearError();

            if (_configuredSlotCount > 0)
            {
                InitializeSlots(_configuredSlotCount);
            }
        }

        private void UnbindVisualElements()
        {
            if (_btnAddPotion != null)
            {
                _btnAddPotion.clicked -= HandleAddPotionClicked;
            }
        }

        private void HandleAddPotionClicked()
        {
            OnAddButtonClicked?.Invoke("Health Potion", 5);
        }
    }
}
