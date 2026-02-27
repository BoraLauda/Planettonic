using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropSlots : MonoBehaviour, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public DropSlots partnerSlot; 
    public Image slotImage; 
    
    [HideInInspector] public bool isFull = false;
    [HideInInspector] public FoodType currentFoodType; 
    
    private menuDragDrop currentItem;
    private brainMenu manager;
    private GameObject dragIcon;
    private Canvas mainCanvas;

    void Start()
    {
        manager = FindObjectOfType<brainMenu>();
        mainCanvas = GetComponentInParent<Canvas>();
        if(mainCanvas != null) mainCanvas = mainCanvas.rootCanvas;
        ResetSlot();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (isFull) return; 
        
        menuDragDrop droppedItem = eventData.pointerDrag.GetComponent<menuDragDrop>();
        
        if (droppedItem != null)
        {
            Foods foodInfo = droppedItem.GetComponent<Foods>();

            if (foodInfo != null)
            {
                bool incomingIsDrink = IsDrink(foodInfo.type);

                if (partnerSlot != null && partnerSlot.isFull)
                {
                    bool partnerIsDrink = IsDrink(partnerSlot.currentFoodType);

                    if (incomingIsDrink == partnerIsDrink)
                    {
                        Debug.Log("HATA: Bir kişiye iki aynı türden veremezsin!");
                        return; 
                    }
                }
                
                currentFoodType = foodInfo.type;
            }

            slotImage.sprite = droppedItem.myImage.sprite;
            slotImage.color = Color.white;
            isFull = true;
            
            droppedItem.gameObject.SetActive(false); 
            currentItem = droppedItem; 
            manager.CheckAllSlots();
        }
    }

    bool IsDrink(FoodType type)
    {
        return (type == FoodType.Wine || type == FoodType.SoftDrink || type == FoodType.Beer);
    }

    public void ResetSlot()
    {
        slotImage.sprite = null;
        slotImage.color = Color.clear; 
        isFull = false;
        currentFoodType = FoodType.SafeFood; 

        if (currentItem != null)
        {
            currentItem.gameObject.SetActive(true);
            CanvasGroup group = currentItem.GetComponent<CanvasGroup>();
            if (group != null)
            {
                group.alpha = 1f;            
                group.blocksRaycasts = true;  
            }
            currentItem.transform.SetParent(currentItem.parentBe4Drag);
            currentItem.transform.localPosition = currentItem.startPosition;
            currentItem = null; 
        }
        
        if(manager != null) manager.CheckAllSlots();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isFull) return;
        dragIcon = new GameObject("TempIcon");
        dragIcon.transform.SetParent(mainCanvas.transform);
        dragIcon.transform.localScale = Vector3.one;
        dragIcon.transform.SetAsLastSibling(); 
        Image iconImg = dragIcon.AddComponent<Image>();
        iconImg.sprite = slotImage.sprite;
        iconImg.raycastTarget = false; 
        RectTransform iconRect = dragIcon.GetComponent<RectTransform>();
        iconRect.sizeDelta = GetComponent<RectTransform>().sizeDelta;
        slotImage.color = new Color(1, 1, 1, 0.5f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragIcon != null)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)mainCanvas.transform, eventData.position, mainCanvas.worldCamera, out Vector2 movePos);
            dragIcon.transform.position = mainCanvas.transform.TransformPoint(movePos);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragIcon != null) Destroy(dragIcon);
        if (isFull) slotImage.color = Color.white;
    }
}
