using UnityEngine;
using UnityEngine.EventSystems;

public class UISlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)      // ��� ���������� ���������������� ������� ��� ������� ������
    {
        var dragItem = eventData.pointerDrag.transform; // �������� ������ �� ��������������� ������
        dragItem.SetParent(transform);                  // ���������������� ������� ��������� ��������� ������ ����
        dragItem.localPosition = Vector3.zero;          // �������� ������� ���������������� �������, ����� �� ����� � ������ ����.
    }
}
