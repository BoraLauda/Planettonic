using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlanetSelector : MonoBehaviour
{
    [Header("UI")]
    public Image imgLeft;
    public Image imgCenter;
    public Image imgRight;
   
    [System.Serializable]
    public class PlanetData
    {
        public string planetName;
        public Sprite planetIcon;
    }

    
    [Header("Gezegen Verileri")]
    public List<PlanetData> planetList;

    private int currentIndex = 0;

    void Start()
    {
        UpdateUI();
    }

    public void OnNextClicked()
    {
        if (planetList.Count == 0) return;

        currentIndex = (currentIndex + 1) % planetList.Count;
        UpdateUI();
    }

    public void OnPreviousClicked()
    {
        if (planetList.Count == 0) return;

        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = planetList.Count - 1;
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        if (planetList == null || planetList.Count == 0) return;

        
        PlanetData currentPlanet = planetList[currentIndex];
        imgCenter.sprite = currentPlanet.planetIcon;
        imgCenter.rectTransform.localScale = Vector3.one * 1.2f; 
        imgCenter.color = Color.white;
        int leftIndex = (currentIndex - 1 + planetList.Count) % planetList.Count;
        
        imgLeft.gameObject.SetActive(true); 
        imgLeft.sprite = planetList[leftIndex].planetIcon;
        imgLeft.rectTransform.localScale = Vector3.one * 0.7f;
        imgLeft.color = new Color(1, 1, 1, 0.5f);
        int rightIndex = (currentIndex + 1) % planetList.Count;

        imgRight.gameObject.SetActive(true); 
        imgRight.sprite = planetList[rightIndex].planetIcon;
        imgRight.rectTransform.localScale = Vector3.one * 0.7f;
        imgRight.color = new Color(1, 1, 1, 0.5f);
    }
}