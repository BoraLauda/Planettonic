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

        // --- YENİ EKLENEN COOLDOWN AJANI ---
        bool isCooldownActive = false;
        int currentCooldown = 0;

        // Eşyanın adını tam olarak ScriptableObject'indeki (ItemData) ismiyle aynı yazmalısın.
        if (myData.itemName == "Heart of the Circuit")
        {
            currentCooldown = PlayerPrefs.GetInt("HeartCooldown", 0);
            if (currentCooldown > 0)
            {
                isCooldownActive = true;
            }
        }
        // ------------------------------------

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
            // Eğer sahipsek ve kuşanmak istiyorsak ama Cooldown varsa:
            if (isCooldownActive)
            {
                if(buttonText) buttonText.text = $"Cooldown ({currentCooldown})";
                actionButton.interactable = false; // Tıklamayı engelle
            }
            else if (manager.IsEquipped(myData))
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
            // Zaten kuşanılmış ama date'ten dönünce Cooldown yemişse:
            if (isCooldownActive)
            {
                if(buttonText) buttonText.text = $"Cooldown ({currentCooldown})";
                actionButton.interactable = false;
            }
            else
            {
                if(buttonText) buttonText.text = "Equipped";
                actionButton.interactable = false;
            }
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