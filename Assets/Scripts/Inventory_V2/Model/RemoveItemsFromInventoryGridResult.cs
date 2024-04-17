namespace Inventory
{
    public readonly struct RemoveItemsFromInventoryGridResult
    {
        public readonly int InventoryOwnerId;
        public readonly int ItemsRemoveAmount;
        public readonly bool Success;

        public RemoveItemsFromInventoryGridResult(int inventoryOwnerId, int itemsRemoveAmount, bool success)
        {
            InventoryOwnerId = inventoryOwnerId;
            ItemsRemoveAmount = itemsRemoveAmount;
            Success = success;
        }
    }
}
