using UnityEngine;

public class RocketDODGE : MonoBehaviour
{
    public float speed = 500f;
    private Vector2 moveDirection;
    
  
    
    
    public void Setup(Vector2 dir, float newSpeed)
    {
        moveDirection = dir;
        speed = newSpeed;
        
        if (dir == Vector2.up) // YUKARI GİDERKEN
        {
            transform.rotation = Quaternion.Euler(0, 0, -90); 
        }
        else if (dir == Vector2.down) // AŞAĞI GİDERKEN
        {
            transform.rotation = Quaternion.Euler(0, 0, 90); 
        }
        else if (dir == Vector2.left) // SOLA GİDERKEN
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); 
        }
        else if (dir == Vector2.right) // SAĞA GİDERKEN
        {
            transform.rotation = Quaternion.Euler(0, 0, 180); 
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
        
      
        
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
    
    void OnTriggerExit2D(Collider2D other)
    {
        
        if (other.tag == "Background")
        {
            Destroy(gameObject);
        }
    }
}
