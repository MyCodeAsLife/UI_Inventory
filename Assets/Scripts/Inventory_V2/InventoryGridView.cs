using UnityEngine;

namespace Inventory
{
    public class InventoryGridView : MonoBehaviour
    {
        // Можно получить ссылку на префаб

        public void Setup(IReadOnlyInventoryGrid inventory)
        {
            var slots = inventory.GetSlots();
            var size = inventory.Size;              //+

            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    var slot = slots[i, j];
                    // Далее можно в префаб ложить слоты

                    Debug.Log($"Slot ({i}:{j}). Item ID: {slot.ItemId}, amount: {slot.Amount}.");
                }
            }
        }
    }
}