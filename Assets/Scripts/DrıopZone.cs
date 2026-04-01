using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    [Header("Kabul Edilen Obje")]
    public string acceptedItemName = "Shaker"; 
    
    public bool isFilled = false; 

    public void OnDrop(PointerEventData eventData)
    {
        if (!isFilled && eventData.pointerDrag != null)
        {
            DraggableItem draggedItem = eventData.pointerDrag.GetComponent<DraggableItem>();

            if (draggedItem != null && draggedItem.itemName == acceptedItemName)
            {
                
                draggedItem.isLocked = true;
                
               
                draggedItem.GetComponent<RectTransform>().position = transform.position;
                
               
                draggedItem.GetComponent<CanvasGroup>().blocksRaycasts = true;
                draggedItem.GetComponent<CanvasGroup>().alpha = 1f;
                draggedItem.enabled = false; 
                
                isFilled = true;
            }
        }
    }
}