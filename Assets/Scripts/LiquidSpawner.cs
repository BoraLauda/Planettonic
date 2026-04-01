using UnityEngine;

public class LiquidSpawner : MonoBehaviour
{
   
    public GameObject liquidPrefab; 
    public Transform spawnPoint;    
    
   
    public float spawnRate = 0.05f; 
    private float nextSpawnTime = 0f;

    [HideInInspector] public bool isPouring = false;
    public Glass targetGlass;

    void Update()
    {
        if (KokteylManager.Instance.currentPhase != KokteylManager.GamePhase.Pouring) return;
        if (Input.GetMouseButtonDown(0))
        {
            isPouring = true;
        }

       
        if (Input.GetMouseButtonUp(0))
        {
            isPouring = false;
            
        }

       
        if (isPouring && Time.time >= nextSpawnTime)
        {
            Instantiate(liquidPrefab, spawnPoint.position, Quaternion.identity);
            
            nextSpawnTime = Time.time + spawnRate;
        }
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            isPouring = false; 

            if (targetGlass != null)
            {
                targetGlass.CheckScore();
            }
        }
    }
}