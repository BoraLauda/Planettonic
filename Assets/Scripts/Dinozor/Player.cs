using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float kosuHizi = 6f; 
    public float ziplamaGucu = 10f;

    [Header("Zemin Kontrolü")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask zeminLayer; 
    
    private bool isGrounded;
    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
    
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, zeminLayer);
        anim.SetBool("isGrounded", isGrounded);
        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, ziplamaGucu);
            anim.SetTrigger("Jump");
        }
    }

    void FixedUpdate()
    {
    }

    private void OnTriggerEnter2D(Collider2D temas)
    {
        if (temas.CompareTag("Kalp"))
        {
            if (BrainKuzu.Instance != null)
            {
                BrainKuzu.Instance.KalpToplandi();
            }
            
         
            Destroy(temas.gameObject);
        }
        else if (temas.CompareTag("KirikKalp"))
        {
           
            if (BrainKuzu.Instance != null)
            {
                BrainKuzu.Instance.KirikKalbeCarpildi();
            }
            
            Destroy(temas.gameObject);
        }
    }
}
