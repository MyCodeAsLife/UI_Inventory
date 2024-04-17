namespace Inventory
{
    public readonly struct AddItemsToInventoryGridResult
    {
        public readonly int InventoryOwnerId;
        public readonly int ItemsAddAmount;
        public readonly int ItemsAddedAmount;

        public int ItemsNotAddedAmount => ItemsAddAmount - ItemsAddedAmount;

        public AddItemsToInventoryGridResult(int inventoryOwnerId, int itemsAddAmount, int itemsAddedAmount)
        {
            InventoryOwnerId = inventoryOwnerId;
            ItemsAddAmount = itemsAddAmount;
            ItemsAddedAmount = itemsAddedAmount;
        }
    }
}
