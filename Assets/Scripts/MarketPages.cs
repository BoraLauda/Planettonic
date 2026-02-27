using UnityEngine;

public class MarketPages : MonoBehaviour
{
    
    public GameObject marketPanelSmall;
    public GameObject ownedPanelSmall;
    public GameObject equippedPanelSmall;

   
    public GameObject marketPanelBig;
    public GameObject ownedPanelBig;
    public GameObject equippedPanelBig;

    void Start()
    {
        ShowMarket();
    }

    public void ShowMarket()
    {
        
        if(marketPanelSmall) marketPanelSmall.SetActive(true);
        if(ownedPanelSmall) ownedPanelSmall.SetActive(false);
        if(equippedPanelSmall) equippedPanelSmall.SetActive(false);

        
        if(marketPanelBig) marketPanelBig.SetActive(true);
        if(ownedPanelBig) ownedPanelBig.SetActive(false);
        if(equippedPanelBig) equippedPanelBig.SetActive(false);
    }

    public void ShowOwned()
    {
      
        if(marketPanelSmall) marketPanelSmall.SetActive(false);
        if(ownedPanelSmall) ownedPanelSmall.SetActive(true);
        if(equippedPanelSmall) equippedPanelSmall.SetActive(false);

     
        if(marketPanelBig) marketPanelBig.SetActive(false);
        if(ownedPanelBig) ownedPanelBig.SetActive(true);
        if(equippedPanelBig) equippedPanelBig.SetActive(false);
    }

    public void ShowEquipped()
    {
        
        if(marketPanelSmall) marketPanelSmall.SetActive(false);
        if(ownedPanelSmall) ownedPanelSmall.SetActive(false);
        if(equippedPanelSmall) equippedPanelSmall.SetActive(true);

        
        if(marketPanelBig) marketPanelBig.SetActive(false);
        if(ownedPanelBig) ownedPanelBig.SetActive(false);
        if(equippedPanelBig) equippedPanelBig.SetActive(true);
    }
}