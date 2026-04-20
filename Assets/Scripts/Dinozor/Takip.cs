using UnityEngine;

public class Takip : MonoBehaviour
{
    [Header("Takip Ayarları")]
    public Transform hedef; 
    public float takipMesafesi = 3f; 
    public float takipHizi = 6f; 

    void Update()
    {
        if (hedef == null) return;
        
        Vector3 hedefPos = new Vector3(hedef.position.x - takipMesafesi, transform.position.y, transform.position.z);
        
        transform.position = Vector3.Lerp(transform.position, hedefPos, Time.deltaTime * takipHizi);
    }
}
