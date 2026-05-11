using UnityEngine;

public class RocketCurve : MonoBehaviour
{
    private Vector3 p0; 
    private Vector3 p1; 
    private Vector3 p2; 
    
    private float duration;
    private float t = 0f;

    
    public float yonOfseti = 180f; 

    public void Firlat(Vector3 start, Vector3 control, Vector3 end, float speed)
    {
        p0 = start;
        p1 = control;
        p2 = end;
        
        duration = 1000f / speed; 
    }

    void Update()
    {
        if (t < 1f)
        {
            t += Time.deltaTime / duration;
            
          
            Vector3 yeniPos = Mathf.Pow(1 - t, 2) * p0 + 
                              2 * (1 - t) * t * p1 + 
                              Mathf.Pow(t, 2) * p2;
                          
         
            Vector3 yon = (yeniPos - transform.position).normalized;
            
            if (yon != Vector3.zero && t > 0.01f) 
            {
                
                float angle = Mathf.Atan2(yon.y, yon.x) * Mathf.Rad2Deg;
                
               
                transform.rotation = Quaternion.Euler(0, 0, angle + yonOfseti); 
            }
            
           
            GetComponent<RectTransform>().position = yeniPos;
        }
        else
        {
            Destroy(gameObject); 
        }
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
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