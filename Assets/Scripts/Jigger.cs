using UnityEngine;
using UnityEngine.UI;

public class Jigger : MonoBehaviour
{
    [Header("UI Referansları")]
    public RectTransform cursor;        
    public RectTransform targetZone;    
    public RectTransform barBackground; 

    [Header("Ayarlar")]
    public float cursorSpeed = 300f;    
    private float direction = 1f;
    private bool isFinished = false;      

    private float minX;
    private float maxX;

    void Start()
    {
        
        float halfWidth = barBackground.rect.width / 2f;
        minX = -halfWidth;
        maxX = halfWidth;
    }

    void Update()
    {
        if (isFinished || KokteylManager.Instance.currentPhase != KokteylManager.GamePhase.Jigger) return;
        
        cursor.anchoredPosition += new Vector2(direction * cursorSpeed * Time.deltaTime, 0);

        if (cursor.anchoredPosition.x >= maxX)
        {
            cursor.anchoredPosition = new Vector2(maxX, cursor.anchoredPosition.y);
            direction = -1f;
        }
        else if (cursor.anchoredPosition.x <= minX)
        {
            cursor.anchoredPosition = new Vector2(minX, cursor.anchoredPosition.y);
            direction = 1f;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            CheckWinCondition();
        }
    }

    void CheckWinCondition()
    {
       
        float targetHalfWidth = targetZone.rect.width / 2f;
        float targetLeft = targetZone.anchoredPosition.x - targetHalfWidth;
        float targetRight = targetZone.anchoredPosition.x + targetHalfWidth;

        float cursorX = cursor.anchoredPosition.x;

        
        if (cursorX >= targetLeft && cursorX <= targetRight)
        {
            
            isFinished = false; 
            KokteylManager.Instance.NextPhase();
            
        }
        else
        {
            Debug.Log("KAÇIRDIN!");
            
        }
    }
}