using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCardUI : MonoBehaviour
{
    public ItemData myData;       
    public MarketManager manager; 
    
    public Button actionButton;   
    public TMP_Text buttonText;   
    
    public Image cardImageUI;
    public TMP_Text itemNameText; 
  

    public string cardType = "Market"; 
    
    void OnEnable()
    {
        ForceUpdateState();
    }

    void Start()
    {
        if (actionButton == null) actionButton = GetComponentInChildren<Button>();
        
    
        if (myData != null)
        {
            if (cardImageUI != null && myData.cardImage != null)
            {
                cardImageUI.sprite = myData.cardImage; 
            }

            if (itemNameText != null)
            {
                itemNameText.text = myData.itemName; 
            }
        }
     
        if (actionButton != null)
        {
            actionButton.onClick.RemoveAllListeners();
            
            if (cardType == "Market")
            {
                actionButton.onClick.AddListener(TryBuy);
            }
            else if (cardType == "Owned")
            {
                actionButton.onClick.AddListener(TryEquip);
            }
            else if (cardType == "Equipped")
            {
                actionButton.interactable = false;
                if(buttonText) buttonText.text = "Equipped";
            }
        }

        ForceUpdateState();
    }
    
    public void SetupCopy(ItemData data, MarketManager mngr, string type)
    {
        myData = data;
        manager = mngr;
        cardType = type;
        Start(); 
        
        ForceUpdateState();
    }
    
    public void ForceUpdateState()
    {
        if (manager == null || myData == null) return;

        if (cardType == "Market")
        {
            if (manager.IsOwned(myData))
            {
                if(buttonText) buttonText.text = "Owned"; 
                actionButton.interactable = false;
            }
            else
            {
                if(buttonText) buttonText.text = myData.price.ToString();
                actionButton.interactable = true;
            }
        }
        else if (cardType == "Owned")
        {
            if (manager.IsEquipped(myData))
            {
                if(buttonText) buttonText.text = "Equipped";
                actionButton.interactable = false;
            }
            else
            {
                if(buttonText) buttonText.text = "Equip";
                actionButton.interactable = true; 
            }
        }
        else if (cardType == "Equipped")
        {
            if(buttonText) buttonText.text = "Equipped";
            actionButton.interactable = false;
        }
    }

    void TryBuy()
    {
        if (manager != null && myData != null) 
        {
            manager.BuyItem(myData);
        }
        ForceUpdateState();
    }

    void TryEquip()
    {
        if (manager != null && myData != null) 
        {
            manager.EquipItem(myData);
        }
        ForceUpdateState();
    }
}