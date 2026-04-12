using UnityEngine;

public class Bird : MonoBehaviour
{
    [Header("Hareket ve Sinirlar")]
    public float moveSpeed = 150f; 
    public float leftBoundary = -400f; 
    public float rightBoundary = 400f;
    private bool isFacingRight = true;
    
    [Header("Takip Ayarlari")]
    public Transform player; 
    public Transform levelContainer; 
    public float chaseSpeed = 250f; 
    public float radarMesafesi = 250f; 
    private bool isChasing = false;

    [Header("Zeka Ayarlari")]
    public float onuneAtmaMesafesi = 80f; 
    private float sonOyuncuX; 

    [Header("Tas Atma Ayarlari")]
    public GameObject rockPrefab;
    public Transform dropPoint;
    public Transform rockContainer;
    public float dropInterval = 1.5f; 
    
    private float dropTimer;
    private RectTransform rect; 

    void Start()
    {
        dropTimer = dropInterval;
        rect = GetComponent<RectTransform>();
        if (player != null) sonOyuncuX = player.localPosition.x;
    }

    void Update()
    {
        HandleDropping();
    }

    void FixedUpdate()
    {
        if (player == null || levelContainer == null) return;

        float kusunGercekYeriX = levelContainer.localPosition.x + rect.anchoredPosition.x;
        float oyuncununGercekYeriX = player.localPosition.x;
        float aradakiMesafe = Mathf.Abs(oyuncununGercekYeriX - kusunGercekYeriX);

        if (aradakiMesafe < radarMesafesi)
        {
            isChasing = true;
        }
        else 
        {
            isChasing = false;
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }
    
    void HandleDropping()
    {
        dropTimer -= Time.deltaTime;
        if (dropTimer <= 0f)
        {
            DropRock();
            dropTimer = dropInterval; 
        }
    }
    
    void DropRock()
    {
        if (rockPrefab != null && dropPoint != null && rockContainer != null)
        {
            GameObject newLightning = Instantiate(rockPrefab, dropPoint.position, Quaternion.identity, rockContainer);
            
            newLightning.transform.localScale = Vector3.one;
            
            FallingRock lightningScript = newLightning.GetComponent<FallingRock>();
            if (lightningScript != null)
            {
                float dir = isFacingRight ? 1f : -1f; 
                lightningScript.SetDirection(dir);
            }
        }
    }

    void Patrol()
    {
        float step = moveSpeed * Time.fixedDeltaTime;

        if (isFacingRight)
        {
            rect.anchoredPosition += new Vector2(step, 0); 
            if (rect.anchoredPosition.x >= rightBoundary)
            {
                rect.anchoredPosition = new Vector2(rightBoundary, rect.anchoredPosition.y); 
                Flip(); 
            }
        }
        else
        {
            rect.anchoredPosition -= new Vector2(step, 0); 
            if (rect.anchoredPosition.x <= leftBoundary)
            {
                rect.anchoredPosition = new Vector2(leftBoundary, rect.anchoredPosition.y); 
                Flip(); 
            }
        }
    }

    void ChasePlayer()
    {
        float step = chaseSpeed * Time.fixedDeltaTime;
        
        float oyuncuHareketi = player.localPosition.x - sonOyuncuX;
        sonOyuncuX = player.localPosition.x;

        float tahminOffseti = 0f;
        if (oyuncuHareketi > 0.5f) 
        {
            tahminOffseti = onuneAtmaMesafesi;
        }
        else if (oyuncuHareketi < -0.5f) 
        {
            tahminOffseti = -onuneAtmaMesafesi;
        }

        float targetX = (player.localPosition.x + tahminOffseti) - levelContainer.localPosition.x;
        
        float currentX = rect.anchoredPosition.x;
        float nextX = Mathf.MoveTowards(currentX, targetX, step);
        
        nextX = Mathf.Clamp(nextX, leftBoundary, rightBoundary);
        rect.anchoredPosition = new Vector2(nextX, rect.anchoredPosition.y); 
        
        float fark = targetX - rect.anchoredPosition.x;
        if (fark > 2f && !isFacingRight)
            Flip();
        else if (fark < -2f && isFacingRight)
            Flip();
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}