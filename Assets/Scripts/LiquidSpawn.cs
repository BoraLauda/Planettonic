using UnityEngine;
using UnityEngine.UI;

public class LiquidSpawn : MonoBehaviour
{
    public Image liquidImage; 
    public float fillSpeed = 0.5f; 
    
    [Range(0, 1)] public float targetFill = 0f; 
    private float currentFill = 0f;
    
    void OnEnable()
    {
        if (liquidImage == null) liquidImage = GetComponent<Image>();
        
        currentFill = 0f;
        targetFill = 0f;
        
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
        if (Input.GetMouseButton(0))
        {
            targetFill += 0.25f * Time.deltaTime;
        }

        targetFill = Mathf.Clamp(targetFill, 0f, 1f);

        if (currentFill < targetFill)
        {
            currentFill += fillSpeed * Time.deltaTime;
            liquidImage.fillAmount = currentFill;
        }
    }
}