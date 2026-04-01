using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class MarketManager : MonoBehaviour
{
    public int currentHearts = 0;
    public TMP_Text heartUIText;
    public TMP_Text heartUITextBig;

    [Header("UI Referansları")]
    public Transform ownedParentSmall;    
    public Transform equippedParentSmall;
    public GameObject cardPrefabSmall; 
    public GameObject ownedEmptyTextSmall;     
    public GameObject equippedEmptyTextSmall;  

    [Header("UI Referansları")]
    public Transform ownedParentBig;      
    public Transform equippedParentBig;
    public GameObject cardPrefabBig;
    public GameObject ownedEmptyTextBig;       
    public GameObject equippedEmptyTextBig;

    [Header("Veritabanı")]
    
    public List<ItemData> allAvailableItems; 
    
    // Hafıza
    private List<ItemData> ownedItems = new List<ItemData>();
    private List<ItemData> equippedItems = new List<ItemData>();

    void Start()
    {
        
        currentHearts = PlayerPrefs.GetInt("SavedHearts", 0);
        UpdateMoneyUI();

       
        LoadMarketData();
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
            
            UpdateMoneyUI();

            ownedItems.Add(item);

            
            if(ownedParentSmall) CreateCard(item, ownedParentSmall, cardPrefabSmall, "Owned");
            if(ownedParentBig) CreateCard(item, ownedParentBig, cardPrefabBig, "Owned");
            
            
            SaveMarketData();
            
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
        
      
        SaveMarketData();

        CheckEmptyUI();
        RefreshAllCards();
    }
    
 
    
    void SaveMarketData()
    {
        
        string ownedString = "";
        foreach (ItemData item in ownedItems) ownedString += item.itemName + ",";
        PlayerPrefs.SetString("SavedOwnedItems", ownedString);

        
        string equippedString = "";
        foreach (ItemData item in equippedItems) equippedString += item.itemName + ",";
        PlayerPrefs.SetString("SavedEquippedItems", equippedString);

        PlayerPrefs.Save();
    }

    void LoadMarketData()
    {
       
        string ownedString = PlayerPrefs.GetString("SavedOwnedItems", "");
        string equippedString = PlayerPrefs.GetString("SavedEquippedItems", "");
        
        string[] ownedNames = ownedString.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
        string[] equippedNames = equippedString.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
        
        foreach (string itemName in ownedNames)
        {
            ItemData foundItem = allAvailableItems.Find(x => x.itemName == itemName);
            if (foundItem != null && !ownedItems.Contains(foundItem))
            {
                ownedItems.Add(foundItem);
                if(ownedParentSmall) CreateCard(foundItem, ownedParentSmall, cardPrefabSmall, "Owned");
                if(ownedParentBig) CreateCard(foundItem, ownedParentBig, cardPrefabBig, "Owned");
            }
        }

        foreach (string itemName in equippedNames)
        {
            ItemData foundItem = allAvailableItems.Find(x => x.itemName == itemName);
            if (foundItem != null && !equippedItems.Contains(foundItem))
            {
                equippedItems.Add(foundItem);
                if(equippedParentSmall) CreateCard(foundItem, equippedParentSmall, cardPrefabSmall, "Equipped");
                if(equippedParentBig) CreateCard(foundItem, equippedParentBig, cardPrefabBig, "Equipped");
            }
        }

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
        if (ownedEmptyTextSmall != null) ownedEmptyTextSmall.SetActive(ownedItems.Count == 0);
        if (ownedEmptyTextBig != null) ownedEmptyTextBig.SetActive(ownedItems.Count == 0);
        if (equippedEmptyTextSmall != null) equippedEmptyTextSmall.SetActive(equippedItems.Count == 0);
        if (equippedEmptyTextBig != null) equippedEmptyTextBig.SetActive(equippedItems.Count == 0);
    }
}