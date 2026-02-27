using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class APPler : MonoBehaviour
{
    public GameObject backButtonSmall; 
    public GameObject backButtonLarge;
    
    [Header("Sayfalarr")]
    public int currentPageSayı = 0;
    private Stack<int> pageHistory = new Stack<int>();
    
    public Characters IoData;    
    public Characters ElroiData;
    
    public GameObject TamIoName;
    public GameObject TamIoList;
    public GameObject TamIoInfo;
    
    public GameObject TamElroiName;
    public GameObject TamElroiList;
    public GameObject TamElroiInfo;
    
    public GameObject SmallIoName;
    public GameObject SmallIoList;
    public GameObject SmallIoInfo;
    
    public GameObject SmallElroiName;
    public GameObject SmallElroiList;
    public GameObject SmallElroiInfo;

    public GameObject confirmButton;
    

    [Header("UI")]
    public List<Image> onaylaImages; 
    public List<Image> leftHeartImages;   
    public List<Image> rightHeartImages;
    
    
    //public TMPro.TMP_Text NameText;        
   // public TMPro.TMP_Text ListsText;      
   // public TMPro.TMP_Text InfoText;
    
    
    public Sprite defaultIcon;

    [Header("Slotlar (Gri yapmak için)")]
    public List<CharacterSlots> allSlots; 


    private Characters tempCandidate;
    private Characters selectedLeft;  
    private Characters selectedRight;
    
    private int activeSlotToFill = 0; 
    
    public List<GameObject> smallPages; 
    public List<GameObject> largePages; 

    void Start()
    {
        if (currentPageSayı == 0 && pageHistory.Count == 0) OpenPageByIndex(0, false);
        RefreshAllUI();
    }


    public void OpenPageByIndex(int index, bool addToHistory = true)
    {
       
        if (index < 0 || index >= smallPages.Count) return;
        if (addToHistory) pageHistory.Push(currentPageSayı);
        
        for (int i = 0; i < smallPages.Count; i++)
        {
            
            smallPages[i].SetActive(i == index);
        }
        
        for (int i = 0; i < largePages.Count; i++)
        {
           
            if (i < largePages.Count) 
            {
                largePages[i].SetActive(i == index);
            }
        }

        currentPageSayı = index;
        
        bool shouldShowBackButton = (index != 4 && index != 0);  //BACK BUTTON SAYFALARI INDEX

        if (backButtonSmall != null) backButtonSmall.SetActive(shouldShowBackButton);
        if (backButtonLarge != null) backButtonLarge.SetActive(shouldShowBackButton);

       
        if (index == 2) RefreshSlots(); 
        
        RefreshAllUI();
    }
    
    

    public void Back()
    {
        if (pageHistory.Count > 0) OpenPageByIndex(pageHistory.Pop(), false);
    }

    
    public void OnCandidateSelected(Characters profile)
    {
        tempCandidate = profile; 
        
        foreach (var img in onaylaImages)
        {
            if (img != null)
            {
                if (profile.portraitImage != null) img.sprite = profile.portraitImage; 
                else img.sprite = profile.profileIcon; 
            }
            img.preserveAspect = true;
        }
        CloseProfileImages();
        
        if (profile == IoData)
        {
            if (TamIoName != null) TamIoName.SetActive(true);
            if (TamIoList != null) TamIoList.SetActive(true);
            if (TamIoInfo != null) TamIoInfo.SetActive(true);
            
            if (SmallIoName != null) SmallIoName.SetActive(true);
            if (SmallIoList != null) SmallIoList.SetActive(true);
            if (SmallIoInfo != null) SmallIoInfo.SetActive(true);
        }
        else if (profile == ElroiData)
        {
            if (TamElroiName != null) TamElroiName.SetActive(true);
            if (TamElroiList != null) TamElroiList.SetActive(true);
            if (TamElroiInfo != null) TamElroiInfo.SetActive(true);
            
            if (SmallElroiName != null) SmallElroiName.SetActive(true);
            if (SmallElroiList != null) SmallElroiList.SetActive(true);
            if (SmallElroiInfo != null) SmallElroiInfo.SetActive(true);
        }

        // if (NameText != null) 
        //NameText.text = profile.characterName;

        // if (ListsText != null) 
        //ListsText.text = profile.profileList;

        //   if (InfoText != null) 
        //InfoText.text = profile.info;
        
        OpenPageByIndex(3);
    }
    
    void CloseProfileImages()
    {
        
        if (TamIoName != null) TamIoName.SetActive(false);
        if (TamIoList != null) TamIoList.SetActive(false);
        if (TamIoInfo != null) TamIoInfo.SetActive(false);
        
        if (TamElroiName != null) TamElroiName.SetActive(false);
        if (TamElroiList != null) TamElroiList.SetActive(false);
        if (TamElroiInfo != null) TamElroiInfo.SetActive(false);

       
        if (SmallIoName != null) SmallIoName.SetActive(false);
        if (SmallIoList != null) SmallIoList.SetActive(false);
        if (SmallIoInfo != null) SmallIoInfo.SetActive(false);
        
        if (SmallElroiName != null) SmallElroiName.SetActive(false);
        if (SmallElroiList != null) SmallElroiList.SetActive(false);
        if (SmallElroiInfo != null) SmallElroiInfo.SetActive(false);
    }

    
    public void OnConfirmSelection()
    {
        
        if (activeSlotToFill == 0)
        {
            selectedLeft = tempCandidate;
           
            foreach (var img in leftHeartImages)
            {
                if (img != null)
                {
                    img.sprite = tempCandidate.profileIcon;
                    img.color = Color.white;
                }
            }
            activeSlotToFill = 1;
        }
        else
        {
            selectedRight = tempCandidate;
          
            
            foreach (var img in rightHeartImages)
            {
                if (img != null)
                {
                    img.sprite = tempCandidate.profileIcon;
                    img.color = Color.white;
                }
            }
        }

        OpenPageByIndex(4);
    }

    public void OnHeartClicked(int slotIndex)
    {
     
        activeSlotToFill = slotIndex;

       
        if (slotIndex == 0) selectedLeft = null;
        else selectedRight = null;
        
        RefreshAllUI();

        OpenPageByIndex(2); 
    }
    
    
    public void OnDatePlacesClicked()
    {
        OpenPageByIndex(5); 
    }
    
    
    
    void RefreshSlots()
    {
        foreach (var slot in allSlots)
        {
            slot.UpdateSlotState(selectedLeft, selectedRight);
        }
    }
    
    void RefreshAllUI()
    {
       
        foreach (var img in leftHeartImages)
        {
            if (img == null) continue;

            if (selectedLeft != null)
            {
                
                img.sprite = selectedLeft.profileIcon;
                img.color = Color.white; // Görünür yap
            }
            else
            {
               
                if (defaultIcon != null) /*img.sprite = defaultIcon*/;
                img.color = Color.white; // Görünür yap
            }
        }

      
        foreach (var img in rightHeartImages)
        {
            if (img == null) continue;

            if (selectedRight != null)
            {
                img.sprite = selectedRight.profileIcon;
                img.color = Color.white;
            }
            else
            {
                
                if (defaultIcon != null) img.sprite = defaultIcon;
                img.color = Color.white; 
            }
        }
        
        RefreshSlots();
    }
    
   
    public GameObject warningPanelSmall; 
    public GameObject warningPanelBig;   
    public float warningDuration = 2.0f;

   
    public void ShowWarningPopup()
    {
        StopCoroutine("HideWarningRoutine"); 
        StartCoroutine("HideWarningRoutine");
    }

   
    System.Collections.IEnumerator HideWarningRoutine()
    {
        
        if (warningPanelSmall != null) warningPanelSmall.SetActive(true);
        if (warningPanelBig != null) warningPanelBig.SetActive(true);
        
        
        yield return new WaitForSeconds(warningDuration); 
        
        if (warningPanelSmall != null) warningPanelSmall.SetActive(false);
        if (warningPanelBig != null) warningPanelBig.SetActive(false);
    }
    
    
    public void TryStartDate()
    {
        
        if (selectedLeft != null && selectedRight != null)
        {
            SceneManager.LoadScene("Loading");
        }
       
       
    }
    
    
    
    
    
    public void OnLoginClicked() => OpenPageByIndex(1);
    public void OnMatchButtonClicked() => OpenPageByIndex(2);
}