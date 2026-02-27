using UnityEngine;
using UnityEngine.EventSystems;
public class MenuReturn : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
       
        if (eventData.pointerDrag != null)
        {
            DropSlots slot = eventData.pointerDrag.GetComponent<DropSlots>();

           
            if (slot != null && slot.isFull)
            {
                slot.ResetSlot();
            }
        }
    }
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
