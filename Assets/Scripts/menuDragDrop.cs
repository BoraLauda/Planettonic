using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(CanvasGroup))]
public class menuDragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    [HideInInspector] public Transform parentBe4Drag;
    [HideInInspector] public Image myImage;
    [HideInInspector] public Vector3 startPosition;
  
    private CanvasGroup canvas;
    private Canvas mainCanvas;
    
    private RectTransform rectTransform;
    private RectTransform canvasRectTransform;
    
    private brainMenu manager;
    
    private Vector2 offsetPos;
    
    void Awake()
    {
        myImage = GetComponent<Image>();
        canvas = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        
        manager = FindObjectOfType<brainMenu>();
        
        UpdateCanvas();
    }
    
    void UpdateCanvas()
    {
    
            if (mainCanvas == null)
            {
                Canvas foundCanvas = GetComponentInParent<Canvas>();
                if (foundCanvas != null)
                {
                    mainCanvas = foundCanvas.rootCanvas;
                    canvasRectTransform = mainCanvas.GetComponent<RectTransform>();
                }
                else
                {
                    Debug.LogError("Canvasın altında deil");
                }
            }
    
    
    }
    
    
    
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (manager != null) manager.PlayPickUpSound();
        
        parentBe4Drag = transform.parent;
        startPosition = transform.localPosition;
        transform.SetParent(mainCanvas.transform);
        transform.SetAsLastSibling();
        canvas.blocksRaycasts = false;
        canvas.alpha = 0.6f;
        
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform, 
            eventData.position, 
            eventData.pressEventCamera, 
            out Vector2 mousePosOnCanvas
        );
        
        offsetPos = (Vector2)rectTransform.localPosition - mousePosOnCanvas;
    }
    
    public void OnDrag(PointerEventData eventData)
    
    {
        Vector2 localPointerPosition;
        
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRectTransform, 
                eventData.position, 
                eventData.pressEventCamera, 
                out localPointerPosition
            ))
        
        {
            rectTransform.localPosition = localPointerPosition + offsetPos;
        }
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        
        canvas.blocksRaycasts = true;
        canvas.alpha = 1f;
        
       
        if (mainCanvas != null && transform.parent == mainCanvas.transform)
        {
            transform.SetParent(parentBe4Drag);
            transform.localPosition = startPosition;
            
            transform.localScale = Vector3.one;
        }
    }
    
}
