using UnityEngine;
using UnityEngine.UI;

public class PlayerDODGE : MonoBehaviour
{
    public float moveSpeed = 500f;
    private Rigidbody2D rb;
    private Vector2 movement;
    
    private Image PlayerImage;
    
    private brainDODGE gameManager;
    private BrainDodgeCurve curveGameManager; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        gameManager = FindFirstObjectByType<brainDODGE>();
        curveGameManager = FindFirstObjectByType<BrainDodgeCurve>();
        
        PlayerImage = GetComponent<Image>();
    }

    
    bool IsGameStopped()
    {
        if (curveGameManager != null && curveGameManager.gameObject.activeInHierarchy)
        {
            return !curveGameManager.isGameActive;
        }
        if (gameManager != null && gameManager.gameObject.activeInHierarchy)
        {
            return !gameManager.isGameActive;
        }
        return false;
    }

    void Update()
    {
        
        if (IsGameStopped())
        {
            movement = Vector2.zero; 
            ChangeAlpha(0.5f);
            return; 
        }
        else
        {
            ChangeAlpha(1f);
        }
        
        float mx = Input.GetAxisRaw("Horizontal");
        float my = Input.GetAxisRaw("Vertical");
        
        movement = new Vector2(mx, my).normalized;
    }
    
    void FixedUpdate()
    {
        if (IsGameStopped())
        {
            rb.linearVelocity = Vector2.zero; 
            return;
        }
        rb.linearVelocity = movement * moveSpeed;
    }
    
    public void TakeDamage()
    {
        Debug.Log("Çarpışma");
        
      
        if (curveGameManager != null && curveGameManager.gameObject.activeInHierarchy)
        {
            curveGameManager.TakeDamage();
        }
        else if (gameManager != null && gameManager.gameObject.activeInHierarchy)
        {
            gameManager.TakeDamage();
        }
    }

    void ChangeAlpha(float alphaValue)
    {
        if (PlayerImage != null)
        {
            Color tempColor = PlayerImage.color;

            if (Mathf.Abs(tempColor.a - alphaValue) > 0.01f)
            {
                tempColor.a = alphaValue;
                PlayerImage.color = tempColor;
            }
        }
    }
}