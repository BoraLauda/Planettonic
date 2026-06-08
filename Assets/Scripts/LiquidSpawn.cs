using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LiquidSpawn : MonoBehaviour
{
    public Image liquidImage; 
    public float fillSpeed = 0.5f; 
    
    [Range(0, 1)] public float targetFill = 0f; 
    private float currentFill = 0f;
    
    private bool hasStartedPouring = false; 
    private float idleTimer = 0f;           
    private bool isFinished = false;        

    void OnEnable()
    {
        if (liquidImage == null) liquidImage = GetComponent<Image>();
        
        currentFill = 0f;
        targetFill = 0f;
        
        hasStartedPouring = false;
        idleTimer = 0f;
        isFinished = false;
        
        if (liquidImage != null) 
        {
            liquidImage.fillAmount = 0f; 
            
            if (KokteylManager.Instance != null)
            {
                liquidImage.color = KokteylManager.Instance.GetKokteylRengi();
            }
        }
    }

    void Update()
    {
        if (isFinished) return;

        
        if (Input.GetMouseButton(0))
        {
            hasStartedPouring = true;
            idleTimer = 0f; 
            targetFill += 0.25f * Time.deltaTime;
        }
        else if (hasStartedPouring)
        {
           
            idleTimer += Time.deltaTime;
            
           
            if (idleTimer >= 1f)
            {
                FinishPouring();
            }
        }

        targetFill = Mathf.Clamp(targetFill, 0f, 1f);

       
        if (currentFill < targetFill)
        {
            currentFill += fillSpeed * Time.deltaTime;
            liquidImage.fillAmount = currentFill;
        }
    }

    private void FinishPouring()
    {
        isFinished = true;

        if (KokteylManager.Instance != null)
        {
            
            KokteylManager.Instance.MasadakiBardagiGuncelle(liquidImage.fillAmount);
          
            KokteylManager.Instance.SadeceMasayiBirakVeSifirla(); 
        }
    }
}