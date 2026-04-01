using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class APPler : MonoBehaviour
{
    public GameObject backButtonSmall; 
    public GameObject backButtonLarge;
    
    [Header("Sayfalar")]
    public int currentPageSayı = 0;
    private Stack<int> pageHistory = new Stack<int>();
    
    public Characters IoData;    
    public Characters ElroiData;

    
    
    public Image ortakTamName;
    public Image ortakTamList;
    public Image ortakTamInfo;
    
  
    public Image ortakSmallName;
    public Image ortakSmallList;
    public Image ortakSmallInfo;
    

    public GameObject confirmButton;

    
    public List<Image> onaylaImages; 
    public List<Image> leftHeartImages;   
    public List<Image> rightHeartImages;
    
    public Sprite defaultIcon;

    public List<CharacterSlots> allSlots; 

    private Characters tempCandidate;
    private Characters selectedLeft;  
    private Characters selectedRight;
    
    private int activeSlotToFill = 0; 
    
    public List<GameObject> smallPages; 
    public List<GameObject> largePages; 

    
    public List<MatchScenario> coupleScenarios; 
    public DialogueDataları defaultScenario;

    public GameObject warningPanelSmall; 
    public GameObject warningPanelBig;   
    public float warningDuration = 2.0f;

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
        
        bool shouldShowBackButton = (index != 4 && index != 0); 

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
        
      
        if (ortakTamName != null) ortakTamName.sprite = profile.nameImage;
        if (ortakSmallName != null) ortakSmallName.sprite = profile.nameImage;

        if (ortakTamList != null) ortakTamList.sprite = profile.listImage;
        if (ortakSmallList != null) ortakSmallList.sprite = profile.listImage;

        if (ortakTamInfo != null) ortakTamInfo.sprite = profile.infoImage;
        if (ortakSmallInfo != null) ortakSmallInfo.sprite = profile.infoImage;

        OpenPageByIndex(3);
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
                img.color = Color.white;
            }
            else
            {
                img.color = Color.white; 
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
          
            DateSettings.leftChar = selectedLeft;
            DateSettings.rightChar = selectedRight;
            DateSettings.selectedScenario = defaultScenario; 

           
            foreach (var match in coupleScenarios)
            {
                if ((selectedLeft == match.characterA && selectedRight == match.characterB) ||
                    (selectedLeft == match.characterB && selectedRight == match.characterA))
                {
                    DateSettings.leftChar = match.characterA; 
                    DateSettings.rightChar = match.characterB;
                    
                    string char1 = match.characterA.characterName;
                    string char2 = match.characterB.characterName;
                    string coupleKey = string.Compare(char1, char2) < 0 ? 
                        "DateLevel_" + char1 + "_" + char2 : 
                        "DateLevel_" + char2 + "_" + char1;

                    int currentLevel = PlayerPrefs.GetInt(coupleKey, 0);
                    
                    if (currentLevel < match.dateLevels.Count)
                    {
                        DateSettings.selectedScenario = match.dateLevels[currentLevel];
                    }
                    else
                    {
                        DateSettings.selectedScenario = match.dateLevels[match.dateLevels.Count - 1];
                    }

                    break; 
                }
            }
            SceneManager.LoadScene("Loading");
        }
        else
        {
            ShowWarningPopup(); 
        }
    }
    
    public void OnLoginClicked() => OpenPageByIndex(1);
    public void OnMatchButtonClicked() => OpenPageByIndex(2);
}

[System.Serializable]
public class MatchScenario
{
    public Characters characterA;
    public Characters characterB;
    public List<DialogueDataları> dateLevels; 
}