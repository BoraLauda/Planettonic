using UnityEngine;
using UnityEngine.UI;
public class PlayerDODGE : MonoBehaviour
{
    public float moveSpeed = 500f;
    private Rigidbody2D rb;
    private Vector2 movement;
    
    private Image PlayerImage;
    
    private brainDODGE gameManager;

    private RectTransform rect;
       void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        gameManager = FindFirstObjectByType<brainDODGE>();
        
        PlayerImage = GetComponent<Image>();
    }

    
    void Update()
    {
        
        if (gameManager != null && gameManager.isGameActive == false)
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
        if (gameManager != null && gameManager.isGameActive == false)
        {
            rb.linearVelocity = Vector2.zero; 
            return;
        }
        rb.linearVelocity = movement * moveSpeed;
    }
    
    public void TakeDamage()
    {
        Debug.Log("Çarpışma");
        if(gameManager != null)
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
