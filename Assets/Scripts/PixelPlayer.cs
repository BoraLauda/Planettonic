using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PixelPlayer : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public float jumpForce = 10f;
    
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    public Transform levelContainer; 
    public float sagSinir = 150f;  
    public float solSinir = -150f; 
    public float levelKaymaHizi = 300f; 
    
    public Transform attackPoint;
    public float attackRange = 0.5f; 
    public LayerMask enemyLayers; 

    public bool anahtarVarMi = false;
    public GameObject miniGameAnaObje; 
    
    [Header("UI Kafes Ayarları")]
    public Image kafesImage;        
    public Sprite acikKafesGorseli; 
    public float bitisGecikmesi = 1.5f; 

    [Header("Can Sistemi")]
    public int maxCan = 2;
    private int mevcutCan;
    private bool hasarAlabilirMi = true;
    public float hasarGormezlikSuresi = 1.5f;

    [Header("Kalp UI Ayarları")]
    public Image[] kalpIkonlari; 
    
    private SpriteRenderer sr;
    private Color orijinalRenk;
   
    private Rigidbody2D rb;
    private Animator anim;
    private float horizontalInput;
    private bool isGrounded;
    private bool isFacingRight = true;
    private bool isCrouching = false;
    private bool isAttacking = false;
    
    public Vector2 crouchColliderSize;  
    public Vector2 crouchColliderOffset; 

    private BoxCollider2D col;
    private Vector2 normalColliderSize;
    private Vector2 normalColliderOffset;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        col = GetComponent<BoxCollider2D>(); 
        if (col != null)
        {
            normalColliderSize = col.size;
            normalColliderOffset = col.offset;
        }
        
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            orijinalRenk = sr.color; 
        }

        mevcutCan = maxCan;
        KalpleriGuncelle();
    }

    void Update()
    {
        if (isAttacking) return;

        horizontalInput = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetAxisRaw("Vertical") < 0 && isGrounded)
        {
            isCrouching = true;
            horizontalInput = 0; 
            
            if (col != null)
            {
                col.size = crouchColliderSize;
                col.offset = crouchColliderOffset;
            }
        }
        else 
        {
            isCrouching = false;
            
            if (col != null)
            {
                col.size = normalColliderSize;
                col.offset = normalColliderOffset;
            }
        }

        if (isGrounded && Input.GetButtonDown("Jump") && !isCrouching)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Z)) && isGrounded && !isCrouching)
        {
            Attack();
        }

        anim.SetFloat("Speed", Mathf.Abs(horizontalInput)); 
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isCrouching", isCrouching);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    void FixedUpdate()
    {
        if (isAttacking) return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (!isCrouching)
        {
            float myX = transform.localPosition.x;

            if (horizontalInput > 0 && myX >= sagSinir)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); 
                levelContainer.localPosition += Vector3.left * levelKaymaHizi * Time.fixedDeltaTime; 
            }
            else if (horizontalInput < 0 && myX <= solSinir)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); 
                levelContainer.localPosition += Vector3.right * levelKaymaHizi * Time.fixedDeltaTime; 
            }
            else
            {
                rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        if (horizontalInput > 0 && !isFacingRight) Flip();
        else if (horizontalInput < 0 && isFacingRight) Flip();
    }

    void Attack()
    {
        isAttacking = true;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); 
        anim.SetTrigger("Attack");
        Invoke("EndAttack", 0.4f);
    }
    
    public void SwordHit()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<Dragon>() != null)
            {
                enemy.GetComponent<Dragon>().HasarAl();
            }
        }
    }

    void EndAttack() { isAttacking = false; }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    public void HasarAl(int miktar)
    {
        if (!hasarAlabilirMi) return;

        mevcutCan -= miktar;
        
        KalpleriGuncelle();

        if (mevcutCan <= 0)
        {
            StartCoroutine(OlumVeBitir());
        }
        else
        {
            StartCoroutine(HasarEfekti());
        }
    }

    void KalpleriGuncelle()
    {
        for (int i = 0; i < kalpIkonlari.Length; i++)
        {
            if (i < mevcutCan)
            {
                kalpIkonlari[i].enabled = true;
            }
            else
            {
                kalpIkonlari[i].enabled = false;
            }
        }
    }

    IEnumerator HasarEfekti()
    {
        hasarAlabilirMi = false;
        if (sr != null)
        {
            for (int i = 0; i < 5; i++)
            {
                sr.color = new Color(1f, 1f, 1f, 0.2f);
                yield return new WaitForSeconds(hasarGormezlikSuresi / 10);
                sr.color = orijinalRenk;
                yield return new WaitForSeconds(hasarGormezlikSuresi / 10);
            }
        }
        hasarAlabilirMi = true;
    }
    
    private void OnTriggerEnter2D(Collider2D temas)
    {
        if (temas.gameObject.CompareTag("Anahtar"))
        {
            anahtarVarMi = true; 
            Destroy(temas.gameObject); 
        }

        if (temas.gameObject.CompareTag("Death"))
        {
            StartCoroutine(OlumVeBitir());
        }

        if (temas.gameObject.CompareTag("Tas") || temas.gameObject.CompareTag("Enemy"))
        {
            HasarAl(1);
        }
        
        if (temas.gameObject.CompareTag("Kalp"))
        {
            if (anahtarVarMi)
            {
                StartCoroutine(KafesiAcVeBitir());
            }
        }
    }
    
    IEnumerator KafesiAcVeBitir()
    {
        if (kafesImage != null && acikKafesGorseli != null)
        {
            kafesImage.sprite = acikKafesGorseli;
        }

        rb.linearVelocity = Vector2.zero;
        anim.SetFloat("Speed", 0);
        this.enabled = false;
        
        yield return new WaitForSeconds(bitisGecikmesi);
        
        brainDate bd = FindFirstObjectByType<brainDate>(); 
        if (bd == null) 
        {
            bd = FindObjectOfType<brainDate>(); 
        }
        
        if (bd != null)
        {
            Debug.Log("PİKSEL: ZAFER! -> +1 Yıldız, +20 Kalp");
            bd.EndPixelGame(1f, 20, TargetCharacter.Both); 
        }

        if (miniGameAnaObje != null)
        {
            miniGameAnaObje.SetActive(false); 
        }
    }
    
    IEnumerator OlumVeBitir()
    {
        if (sr != null)
        {
            sr.color = new Color(1f, 1f, 1f, 0.4f); 
        }
        
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }
        
        rb.linearVelocity = new Vector2(0f, jumpForce * 0.7f);
        
        anim.speed = 0f;
        this.enabled = false; 
        
        yield return new WaitForSeconds(bitisGecikmesi);
        
        brainDate bd = FindFirstObjectByType<brainDate>(); 
        if (bd == null) 
        {
            bd = FindObjectOfType<brainDate>(); 
        }
        
        if (bd != null)
        {
            Debug.Log("PİKSEL: ÖLÜM! -> 0 Yıldız, 0 Kalp");
            bd.EndPixelGame(0f, 0, TargetCharacter.Both); 
        }
        
        if (sr != null) sr.color = orijinalRenk;
        foreach (Collider2D col in colliders) col.enabled = true;
        anim.speed = 1f;
        
        if (miniGameAnaObje != null)
        {
            miniGameAnaObje.SetActive(false); 
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        
        if (attackPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}