using UnityEngine;
using UnityEngine.EventSystems;

public class UIItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform _rectTransform;           // Ссылка на себя UIItem, для перемещения себя.
    private Canvas _mainCanvas;                     // Ссылка на родителя
    private CanvasGroup _group;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _mainCanvas = GetComponentInParent<Canvas>();
        _group = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var slotUI = _rectTransform.parent;         // Получаем слот в котором лежим
        slotUI.SetAsLastSibling();                  // Делаем наш слот последним в списке отрисовки, для того чтобы перетаскиваемый UIItem отрисовывался поверх других слотов
        _group.blocksRaycasts = false;              // Отключаем у данного элемента взаимодействие с reycast, чтобы последний мог фиксировать объекты под текущим, пока текущий перетаскивается
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _mainCanvas.scaleFactor; // Непосредственное перемещение перетаскиваемого объекта. scaleFactor - для поправки на текущее разрешение экрана.
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;         // Возвращение UIItem в изначальное положение
        _group.blocksRaycasts = true;                   // Сновы включаем взаимодействие данного объекта с reycast
    }
}
