namespace Inventory
{
    public class InventorySlotController
    {
        private const string ItemIdText = "Item ID: ";
        private readonly InventorySlotView View;

        private int _capacity;

        public InventorySlotController(IReadOnlyInventorySlot slot, InventorySlotView view)
        {
            View = view;
            _capacity = slot.Capacity;

            slot.ItemIdChanged += OnSlotItemIdChanged;
            slot.ItemAmountChanged += OnSlotItemAmountChanged;

            View.TitleId = ItemIdText + slot.ItemId.ToString();
            View.Amount = slot.Amount.ToString() + "\\" + _capacity;    // Дубляж
        }

        private void OnSlotItemAmountChanged(int newAmount)
        {
            View.Amount = newAmount.ToString() + "\\" + _capacity;      // Дубляж
        }

        private void OnSlotItemIdChanged(int newItemId)
        {
            View.TitleId = ItemIdText + newItemId.ToString();
        }
    }
}
