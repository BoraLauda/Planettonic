using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MarketManager : MonoBehaviour
{
  public int currentHearts = 0;
    public TMP_Text heartUIText;
    public TMP_Text heartUITextBig;

    
    public Transform ownedParentSmall;    
    public Transform equippedParentSmall;
    public GameObject cardPrefabSmall; 

    
    public Transform ownedParentBig;      
    public Transform equippedParentBig;
    public GameObject cardPrefabBig;
    
    public GameObject ownedEmptyTextSmall;     
    public GameObject equippedEmptyTextSmall;  
    public GameObject ownedEmptyTextBig;       
    public GameObject equippedEmptyTextBig;
    
    // Hafıza
    private List<ItemData> ownedItems = new List<ItemData>();
    private List<ItemData> equippedItems = new List<ItemData>();

    void Start()
    {
      /*  currentHearts = 200; 
        PlayerPrefs.SetInt("SavedHearts", currentHearts); 
        PlayerPrefs.Save();
        

        UpdateMoneyUI();
        RefreshAllCards();*/
        
        currentHearts = PlayerPrefs.GetInt("SavedHearts", 0);
        UpdateMoneyUI();
    }
    public bool IsOwned(ItemData item)
    {
        return ownedItems.Contains(item);
    }

    public bool IsEquipped(ItemData item)
    {
        return equippedItems.Contains(item);
    }

    public void BuyItem(ItemData item)
    {
        if (ownedItems.Contains(item)) return; 

        if (currentHearts >= item.price)
        {
            currentHearts -= item.price;
            
           
            PlayerPrefs.SetInt("SavedHearts", currentHearts);
            PlayerPrefs.Save();
            
            UpdateMoneyUI();

            ownedItems.Add(item);

            if(ownedParentSmall) CreateCard(item, ownedParentSmall, cardPrefabSmall, "Owned");
            if(ownedParentBig) CreateCard(item, ownedParentBig, cardPrefabBig, "Owned");
            
            Debug.Log("Ürün alındı. Kalan Para: " + currentHearts);
            
            CheckEmptyUI();
            
            RefreshAllCards();
        }
    }

    public void EquipItem(ItemData item)
    {
        if (equippedItems.Contains(item)) return;

        equippedItems.Add(item);

        
        if(equippedParentSmall) CreateCard(item, equippedParentSmall, cardPrefabSmall, "Equipped");
        
       
        if(equippedParentBig) CreateCard(item, equippedParentBig, cardPrefabBig, "Equipped");
        
        CheckEmptyUI();
        
        RefreshAllCards();
    }
    
    public void RefreshAllCards()
    {
        ItemCardUI[] allCards = FindObjectsByType<ItemCardUI>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (ItemCardUI card in allCards)
        {
            card.ForceUpdateState();
        }
    }

    
    void CreateCard(ItemData item, Transform parent, GameObject prefabToUse, string state)
    {
        if (prefabToUse == null || parent == null) return;

        GameObject newCard = Instantiate(prefabToUse, parent);
        ItemCardUI ui = newCard.GetComponent<ItemCardUI>();
        
        if(ui != null) 
        {
            ui.SetupCopy(item, this, state);
            ui.ForceUpdateState();
        }
    }

    void UpdateMoneyUI()
    {
        if(heartUIText) heartUIText.text = currentHearts.ToString();
        if(heartUITextBig) heartUITextBig.text = currentHearts.ToString();
    }
    
    void CheckEmptyUI()
    {
       
        if (ownedEmptyTextSmall != null)
        {
           
            bool isEmpty = (ownedItems.Count == 0);
            ownedEmptyTextSmall.SetActive(isEmpty);
        }
        
        if (ownedEmptyTextBig != null)
        {
            bool isEmpty = (ownedItems.Count == 0);
            ownedEmptyTextBig.SetActive(isEmpty);
        }
        
        if (equippedEmptyTextSmall != null)
        {
            bool isEmpty = (equippedItems.Count == 0);
            equippedEmptyTextSmall.SetActive(isEmpty);
        }

        if (equippedEmptyTextBig != null)
        {
            bool isEmpty = (equippedItems.Count == 0);
            equippedEmptyTextBig.SetActive(isEmpty);
        }
    }
}