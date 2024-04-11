using UnityEngine;
using UnityEngine.EventSystems;

public class UISlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)      // При отпускании перетаскиваемого объекта над текущим слотом
    {
        var dragItem = eventData.pointerDrag.transform; // Получаем ссылку на перетаскиваемый объект
        dragItem.SetParent(transform);                  // Перетаскиваемому объекту назначаем родителем данный слот
        dragItem.localPosition = Vector3.zero;          // Обнуляем позицию перетаскиваемого объекта, чтобы он встал в данный слот.
    }
}
