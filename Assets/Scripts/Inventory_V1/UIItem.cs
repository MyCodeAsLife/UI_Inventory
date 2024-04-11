using UnityEngine;
using UnityEngine.EventSystems;

public class UIItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform _rectTransform;           // ������ �� ���� UIItem, ��� ����������� ����.
    private Canvas _mainCanvas;                     // ������ �� ��������
    private CanvasGroup _group;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _mainCanvas = GetComponentInParent<Canvas>();
        _group = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var slotUI = _rectTransform.parent;         // �������� ���� � ������� �����
        slotUI.SetAsLastSibling();                  // ������ ��� ���� ��������� � ������ ���������, ��� ���� ����� ��������������� UIItem ������������� ������ ������ ������
        _group.blocksRaycasts = false;              // ��������� � ������� �������� �������������� � reycast, ����� ��������� ��� ����������� ������� ��� �������, ���� ������� ���������������
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _mainCanvas.scaleFactor; // ���������������� ����������� ���������������� �������. scaleFactor - ��� �������� �� ������� ���������� ������.
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;         // ����������� UIItem � ����������� ���������
        _group.blocksRaycasts = true;                   // ����� �������� �������������� ������� ������� � reycast
    }
}
