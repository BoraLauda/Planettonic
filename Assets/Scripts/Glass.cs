using UnityEngine;

public class Glass : MonoBehaviour
{
    
    public int currentDrops = 0;      
    public int targetDrops = 100;     
    public int tolerance = 15;      

   
    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.CompareTag("Liquid"))
        {
            currentDrops++; 
            Debug.Log(" Toplam: " + currentDrops);
            
        }
    }

   
    public void CheckScore()
    {
        int difference = Mathf.Abs(targetDrops - currentDrops);
        float starsToGive = 0f;
        int heartsToGive = 0;

        if (difference <= tolerance)
        {
            starsToGive = 1f;   
            heartsToGive = 2;   
        }
        else if (currentDrops < targetDrops - tolerance)
        {
            starsToGive = 0.5f;
            heartsToGive = 1; 
        }
        else
        {
            starsToGive = 0f;  
            heartsToGive = 0;
        }
        
        brainDate mainDateScript = FindObjectOfType<brainDate>();
        if (mainDateScript != null)
        {
            mainDateScript.EndBartendingGame(starsToGive, heartsToGive, TargetCharacter.Left); 
        }
        
        KokteylManager.Instance.NextPhase();
    }
}