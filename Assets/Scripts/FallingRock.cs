using UnityEngine;
using System.Collections;
public class FallingRock : MonoBehaviour
{
    [Header("Şimşek Ayarları")]
    public float fallSpeed = 45f; 
    
    private bool hasStruck = false;

    void Update()
    {
        
        if (hasStruck) return;

        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
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
            
            StartCoroutine(FlashAndDestroy());
        }
    }
    
    IEnumerator FlashAndDestroy()
    {
       
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}