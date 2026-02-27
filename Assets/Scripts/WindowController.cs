using UnityEngine;
using System.Collections;

public class WindowController : MonoBehaviour
{
    public float animationSpeed = 10f; 
    
    private Vector3 normalScale; 
    
    [Header("Maximize Settings")]
    private bool isMaximized = false;
    
    private Vector2 preMaximizeSize; // Boyutu
    
    private Vector2 preMaximizePos;  // Pozisyonu
    
    private RectTransform rectTransform;
    private Canvas canvas;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        normalScale = transform.localScale;
    }

    void OnEnable()
    { 
        transform.localScale = Vector3.zero;
        //animasyon kısmı
        StopAllCoroutines();
        StartCoroutine(AnimateWindow(Vector3.zero, normalScale));
    }

    public void CloseWindow()
    {
        //anim kapatma
        StopAllCoroutines();
        
        StartCoroutine(AnimateWindow(transform.localScale, Vector3.zero, true));
    }
    
    public void ToggleMaximize()
    {
        if (isMaximized)
        {
            rectTransform.sizeDelta = preMaximizeSize;
            rectTransform.anchoredPosition = preMaximizePos;
            isMaximized = false;
        }
        else
        {
            preMaximizeSize = rectTransform.sizeDelta;
            preMaximizePos = rectTransform.anchoredPosition;
            
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            
            rectTransform.sizeDelta = canvasRect.sizeDelta;
            rectTransform.anchoredPosition = new Vector2(-canvasRect.sizeDelta.x / 2, canvasRect.sizeDelta.y / 2);
            
            
            
            isMaximized = true;
        }
    }

   
    IEnumerator AnimateWindow(Vector3 start, Vector3 end, bool disableOnFinish = false)
    {
        float t = 0;
        
        
        while (t < 1)
        {
            t += Time.deltaTime * animationSpeed;
           
            transform.localScale = Vector3.Lerp(start, end, t); 
            yield return null; 
        }

        transform.localScale = end;

       
        if (disableOnFinish)
        {
            gameObject.SetActive(false); //kapat
        }
    }
}