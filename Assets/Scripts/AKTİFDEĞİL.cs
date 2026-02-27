using UnityEngine;
using UnityEngine.EventSystems;

public class WindowResizer : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    public RectTransform windowRect; 
    public Vector2 minSize = new Vector2(300, 200);
    public Vector2 maxSize = new Vector2(1600, 900);

    private Canvas canvas;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        windowRect.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (windowRect == null) return;

        Vector2 newSize = windowRect.sizeDelta;
        
       
        newSize += new Vector2(eventData.delta.x, -eventData.delta.y) / canvas.scaleFactor;
        newSize.x = Mathf.Clamp(newSize.x, minSize.x, maxSize.x);
        newSize.y = Mathf.Clamp(newSize.y, minSize.y, maxSize.y);

        
        
        windowRect.sizeDelta = newSize;
    }
}