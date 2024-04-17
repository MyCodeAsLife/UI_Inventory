namespace Inventory
{
    public class ScreenController
    {
        private readonly InventoryService InventoryService;
        private readonly ScreenView View;

        private InventoryGridController _currentInventoryGridController;

        public ScreenController(InventoryService inventoryService, ScreenView view)
        {
            InventoryService = inventoryService;
            View = view;
        }

        public void OpenInventory(int ownerId)
        {
            var inventory = InventoryService.GetInventory(ownerId); //+
            var inventoryView = View.InventoryView;                 //+

            _currentInventoryGridController = new InventoryGridController(inventory, inventoryView);
            inventoryView.OwnerId = ownerId.ToString();
        }
    }
}
