using UnityEngine;
using UnityEngine.EventSystems;

public class WindowDragger : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    private RectTransform draggingRectTransform;
    
    private Canvas canvas;

    void Awake()
    {
        draggingRectTransform = transform.parent.GetComponent<RectTransform>();
        
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //ÖNEEE
        draggingRectTransform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
       //KAYYYMAA
        draggingRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
}