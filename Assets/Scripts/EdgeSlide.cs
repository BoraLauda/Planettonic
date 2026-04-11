using UnityEngine;

public class EdgeSlide : MonoBehaviour
{
    public static bool isDraggingItem = false;    
    public RectTransform viewWindow;
    public RectTransform contentToPan; 
    
    public float panSpeed = 800f; 
    public float edgeThreshold = 100f; 
    
    public float minX = -1500f; 
    public float maxX = 1500f;  

    private Canvas parentCanvas;
    
    public float resetSpeed = 10f;
    private float baslangicX;

    void Start()
    {
        if (viewWindow != null)
        {
            parentCanvas = viewWindow.GetComponentInParent<Canvas>();
        }
        
        if (contentToPan != null)
        {
            baslangicX = contentToPan.anchoredPosition.x;
        }
    }

    void Update()
    {
        if (contentToPan == null || viewWindow == null || parentCanvas == null) return;
        
        if (KokteylManager.Instance != null && KokteylManager.Instance.currentPhase != KokteylManager.GamePhase.Preparation)
        {
            Vector2 targetPos = new Vector2(baslangicX, contentToPan.anchoredPosition.y);
            contentToPan.anchoredPosition = Vector2.Lerp(contentToPan.anchoredPosition, targetPos, Time.deltaTime * resetSpeed);
            
            return; 
        }

        if (isDraggingItem) return; 
        
        Vector2 mousePos = Input.mousePosition;
        Camera cam = (parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : parentCanvas.worldCamera;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(viewWindow, mousePos, cam, out Vector2 localMousePos))
        {
            Rect rect = viewWindow.rect;
            if (localMousePos.y >= rect.yMin && localMousePos.y <= rect.yMax)
            {
                Vector2 currentPos = contentToPan.anchoredPosition;
                bool isTryingToMove = false;

                if (localMousePos.x > rect.xMin && localMousePos.x < rect.xMin + edgeThreshold)
                {
                    currentPos.x += panSpeed * Time.deltaTime;
                    isTryingToMove = true;
                }
                else if (localMousePos.x < rect.xMax && localMousePos.x > rect.xMax - edgeThreshold)
                {
                    currentPos.x -= panSpeed * Time.deltaTime;
                    isTryingToMove = true;
                }

                if (isTryingToMove)
                {
                    currentPos.x = Mathf.Clamp(currentPos.x, minX, maxX);
                    contentToPan.anchoredPosition = currentPos;
                }
            }
        }
    }
}
