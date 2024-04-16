using System.Collections.Generic;

namespace Inventory
{
    public class InventoryGridController
    {
        private List<InventorySlotController> _slotControllers = new();

        public InventoryGridController(IReadOnlyInventoryGrid inventory, InventoryView view)
        {
            var size = inventory.Size;              //+
            var slots = inventory.GetSlots();       //+
            var lineLength = size.y;                //+

            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    int index = i * lineLength + j;   //+
                    var slotView = view.GetInventorySlotView(index);    //+
                    var slot = slots[i, j]; //+
                    _slotControllers.Add(new InventorySlotController(slot, slotView));
                }
            }                                               // Здесь------------------------------------
        }
    }
}
