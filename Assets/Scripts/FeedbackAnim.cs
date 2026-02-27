using UnityEngine;
using TMPro;
public class FeedbackAnim : MonoBehaviour
{
    public float moveSpeed = 30f;   
    public float displayTime = 1.5f; 
    
    private Vector3 startPos;
    private CanvasGroup canvasGroup; 

    void Awake()
    {
        startPos = transform.localPosition; 
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        transform.localPosition = startPos; 
        canvasGroup.alpha = 1f;
        
        
        CancelInvoke();
        Invoke("HideMe", displayTime);
    }

    void Update()
    {
        transform.localPosition += Vector3.up * moveSpeed * Time.deltaTime;
        
       
        if (canvasGroup != null)
        {
            canvasGroup.alpha -= Time.deltaTime / displayTime;
        }
    }

    void HideMe()
    {
        gameObject.SetActive(false);
    }
}
