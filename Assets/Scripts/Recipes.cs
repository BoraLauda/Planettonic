using UnityEngine;

public class Recipes : MonoBehaviour
{
    public GameObject openBookPanel; 
    public GameObject[] recipePages; 

    [Header("Nefes Alma (Vurgu) Ayarları")]
    public float pulseSpeed = 4f;        
    public float maxScaleAmount = 1.1f;   
    
    private Vector3 originalScale;
    private bool isPulsing = false;

    void Awake()
    {
    
        originalScale = transform.localScale;
    }

    void Start()
    {
        if (openBookPanel != null) openBookPanel.SetActive(false);
    }

    
    void OnEnable()
    {
        isPulsing = true;
    }

   
    void Update()
    {
        if (isPulsing)
        {
            float sinWave = Mathf.Sin(Time.time * pulseSpeed);
            float factor = (sinWave + 1f) * 0.5f; 
            float currentScale = Mathf.Lerp(1f, maxScaleAmount, factor);
            
            transform.localScale = originalScale * currentScale;
        }
    }

    public void ToggleBook()
    {
       
        if (isPulsing)
        {
            isPulsing = false;
            transform.localScale = originalScale; 
        }

        if (openBookPanel == null) return;
        
        bool currentlyActive = openBookPanel.activeSelf;
        openBookPanel.SetActive(!currentlyActive);
        
        if (!currentlyActive)
        {
            ShowPage(0);
        }
    }

    public void ShowPage(int index)
    {
        if (recipePages == null) return;

        for (int i = 0; i < recipePages.Length; i++)
        {
            if (recipePages[i] != null)
                recipePages[i].SetActive(i == index);
        }
    }
}