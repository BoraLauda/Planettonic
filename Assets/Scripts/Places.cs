using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CarouselWithDetail : MonoBehaviour
{
   
    public Image imgLeft;
    public Image imgCenter;
    public Image imgRight;

    
    public Image imgDetailDisplay; 

    [System.Serializable]
    public class ItemData
    {
        public string itemName;
        public Sprite iconSprite;   
        public Sprite detailSprite; 
    }

    [Header("Veriler")]
    public List<ItemData> dataList;

    private int currentIndex = 0;

    void Start()
    {
        UpdateUI();
    }

    public void OnNextClicked()
    {
        if (dataList.Count == 0) return;

       
        currentIndex = (currentIndex + 1) % dataList.Count;
        UpdateUI();
    }

    public void OnPreviousClicked()
    {
        if (dataList.Count == 0) return;

        currentIndex--;
      
        if (currentIndex < 0)
        {
            currentIndex = dataList.Count - 1;
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        if (dataList == null || dataList.Count == 0) return;

       
        ItemData currentItem = dataList[currentIndex];
        
        imgCenter.sprite = currentItem.iconSprite;
        imgCenter.rectTransform.localScale = Vector3.one * 1.2f; 
        imgCenter.color = Color.white; 

      
        if(imgDetailDisplay != null)
        {
            imgDetailDisplay.sprite = currentItem.detailSprite;
        }

        
        int leftIndex = (currentIndex - 1 + dataList.Count) % dataList.Count;
        
        imgLeft.gameObject.SetActive(true); 
        imgLeft.sprite = dataList[leftIndex].iconSprite;
        imgLeft.rectTransform.localScale = Vector3.one * 0.7f; 
        imgLeft.color = new Color(1, 1, 1, 0.5f); 

        
        int rightIndex = (currentIndex + 1) % dataList.Count;

        imgRight.gameObject.SetActive(true); 
        imgRight.sprite = dataList[rightIndex].iconSprite;
        imgRight.rectTransform.localScale = Vector3.one * 0.7f; 
        imgRight.color = new Color(1, 1, 1, 0.5f); 
    }
}