using UnityEngine;
using System.Collections;
public class FallingRock : MonoBehaviour
{
    [Header("Şimşek Ayarları")]
    public float fallSpeed = 45f; 
    
    public float horizontalSpeed = 10f; 
    
    private bool hasStruck = false;
    private float direction = 0f; 

  
    public void SetDirection(float dir)
    {
        direction = dir;
    }

    void Update()
    {
        if (hasStruck) return;
        
        Vector3 moveVector = new Vector3(direction * horizontalSpeed, -fallSpeed, 0f);
        transform.Translate(moveVector * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasStruck) return;

        if (collision.CompareTag("Player"))
        {
            PixelPlayer playerObj = collision.GetComponent<PixelPlayer>();
            if (playerObj != null)
            {
                playerObj.HasarAl(1); 
            }
            
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Zemin"))
        {
            hasStruck = true; 
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}