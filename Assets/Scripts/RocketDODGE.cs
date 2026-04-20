using UnityEngine;

public class RocketDODGE : MonoBehaviour
{
    public float speed = 500f;
    private Vector2 moveDirection;
    
    public float yokOlmaMesafesi = 1200f; 
    
    private Vector3 dogduguYer;
    
    public void Setup(Vector2 dir, float newSpeed)
    {
        moveDirection = dir;
        speed = newSpeed;
        
       
        dogduguYer = transform.position; 
        
        if (dir == Vector2.up) 
        {
            transform.rotation = Quaternion.Euler(0, 0, -90); 
        }
        else if (dir == Vector2.down) 
        {
            transform.rotation = Quaternion.Euler(0, 0, 90); 
        }
        else if (dir == Vector2.left) 
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); 
        }
        else if (dir == Vector2.right) 
        {
            transform.rotation = Quaternion.Euler(0, 0, 180); 
        }
    }
    
    void Update()
    {
      
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
        
       
        if (Vector3.Distance(dogduguYer, transform.position) > yokOlmaMesafesi)
        {
           
            Destroy(gameObject);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerDODGE playerScript = other.GetComponent<PlayerDODGE>();
            
            if (playerScript != null)
            {
                playerScript.TakeDamage();
            }
            
            Destroy(gameObject);
        }
    }
}