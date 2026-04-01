using UnityEngine;

public class Fireball : MonoBehaviour
{
    [Header("Ayar")]
    public float ucusHizi = 500f; 
    public float yasamSuresi = 4f; 

    void Start()
    {
        Destroy(gameObject, yasamSuresi); 
    }

    void Update()
    {
        transform.Translate(Vector3.left * ucusHizi * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D temas)
    {
        if (temas.CompareTag("Player"))
        {
            Destroy(gameObject); 
            
            Debug.Log(("Öldün"));
        }
    }
}