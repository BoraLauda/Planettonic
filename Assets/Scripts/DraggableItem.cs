using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string itemName; 
    
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public Vector2 startPosition;
    private Vector2 pointerOffset;
    
    public static int buzSayaci = 0;

    [HideInInspector] public bool isLocked = false;
    
    public bool isInfiniteSource = false;

  
    private Vector2 homePosition;
    private bool hasHome = false;
   

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    
    
    void Start()
    {
        homePosition = rectTransform.anchoredPosition;
        hasHome = true;
    }
    
    void OnEnable()
    {
        if (hasHome && !isLocked && !isInfiniteSource)
        {
            rectTransform.anchoredPosition = homePosition;
            
            if (canvasGroup != null)
            {
                canvasGroup.blocksRaycasts = true;
                canvasGroup.alpha = 1f;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (KokteylManager.Instance.currentPhase != KokteylManager.GamePhase.Preparation) return;
        if (isLocked) return;

        startPosition = rectTransform.anchoredPosition;
        
        if (isInfiniteSource)
        {
            GameObject masadakiKopya = Instantiate(this.gameObject, transform.parent);
            masadakiKopya.GetComponent<RectTransform>().anchoredPosition = startPosition;
            masadakiKopya.name = this.gameObject.name;
            masadakiKopya.transform.SetSiblingIndex(transform.GetSiblingIndex()); 
        }

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)rectTransform.parent,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPointerPosition
        );
        pointerOffset = localPointerPosition - (Vector2)rectTransform.localPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (KokteylManager.Instance.currentPhase != KokteylManager.GamePhase.Preparation || isLocked) return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)rectTransform.parent,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPointerPosition))
        {
            rectTransform.localPosition = localPointerPosition - pointerOffset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (KokteylManager.Instance.currentPhase != KokteylManager.GamePhase.Preparation) return;
        
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        if (isInfiniteSource)
        {
            if (!isLocked)
            {
                Destroy(gameObject);
            }
            else
            {
                buzSayaci++;
                Debug.Log("Atılan Buz: " + buzSayaci);
            }
            return; 
        }
        
        if (!isLocked) 
        {
            rectTransform.anchoredPosition = startPosition; 
        }
    }
    
    public void ForceTurn()
    {
        rectTransform.anchoredPosition = startPosition; 
        
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        
        isLocked = false;
    }
}