using UnityEngine;

public class Takip : MonoBehaviour
{
    [Header("Takip Ayarları")]
    public Transform hedef; 
    public float takipMesafesi = 3f; 
    public float takipHizi = 6f; 

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
       
        if (!HeartSpawner.isGameActive)
        {
            if (anim != null) anim.speed = 0; 
            return; 
        }
        else
        {
          
            if (anim != null) anim.speed = 1;
        }

        if (hedef == null) return;
        
        Vector3 hedefPos = new Vector3(hedef.position.x - takipMesafesi, transform.position.y, transform.position.z);
        
        transform.position = Vector3.Lerp(transform.position, hedefPos, Time.deltaTime * takipHizi);
    }
}