using UnityEngine;

public class FallingRock : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
    
        if (other.CompareTag("Player"))
        {
            Debug.Log("Taş tam isabet!");
            
            PixelPlayer playerScript = other.GetComponent<PixelPlayer>();

       
            if (playerScript != null)
            {
                playerScript.HasarAl(1);
            }

            Destroy(gameObject);
        }

        
        if (!other.CompareTag("Enemy") && !other.CompareTag("Bird") && !other.CompareTag("Tas"))
        {
            Destroy(gameObject);
        }
    }
}