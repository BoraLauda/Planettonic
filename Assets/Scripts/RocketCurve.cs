using UnityEngine;

public class RocketCurve : MonoBehaviour
{
    public float speed = 500f;
    private Vector2 moveDirection;
    
    public float yokOlmaMesafesi = 1500f; 
    private Vector3 dogduguYer;
    
    public float yonOfseti = 180f; 

    public void Setup(Vector2 dir, float newSpeed)
    {
        moveDirection = dir.normalized; 
        speed = newSpeed;
        dogduguYer = transform.position; 
        
        if (dir != Vector2.zero)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle + yonOfseti);
        }
    }

    void Update()
    {
        
        transform.position += (Vector3)(moveDirection * speed * Time.deltaTime);
        
        if (Vector3.Distance(dogduguYer, transform.position) > yokOlmaMesafesi)
        {
            Destroy(gameObject);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            BrainDodgeCurve bc = FindFirstObjectByType<BrainDodgeCurve>();
            if (bc != null && bc.isGameActive)
            {
                bc.TakeDamage();
            }
            Destroy(gameObject);
        }
    }
}