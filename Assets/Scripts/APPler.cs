using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class APPler : MonoBehaviour
{
    public GameObject backButtonSmall; 
    public GameObject backButtonLarge;
    
    public int currentPageSayı = 0;
    private Stack<int> pageHistory = new Stack<int>();
    
    public Characters IoData;    
    public Characters ElroiData;
    
    [Header("Sonradan Açılacak Karakterler")]
    public Characters JettyData;
    public Characters LinusData;
    
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

    [Header("UI Kilitli Prefab (image_6 Prefabı)")]
    public GameObject lockedPrefab; 

    [Header("Mekan Butonları")]
    public Button btnRestoran;
    public Button btnBar;
    public Button btnEv;
    public Button btnArcade;

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
        if (IoData == null || ElroiData == null) return;

        string char1 = IoData.characterName;
        string char2 = ElroiData.characterName;
        string coupleKey = string.Compare(char1, char2) < 0 ? 
            "CoupleLevel_" + char1 + "_" + char2 : 
            "CoupleLevel_" + char2 + "_" + char1;

        int ioElroiLevel = PlayerPrefs.GetInt(coupleKey, 0);
        bool isFirstDateDone = ioElroiLevel > 0;

        for (int i = 0; i < allSlots.Count; i++)
        {
            if (allSlots[i] == null || allSlots[i].myProfile == null)
            {
                if (allSlots[i] != null) allSlots[i].UpdateSlotState(selectedLeft, selectedRight, false, lockedPrefab);
                continue;
            }

            bool isUnlocked = false;

            if (allSlots[i].myProfile == IoData || allSlots[i].myProfile == ElroiData)
            {
                isUnlocked = true; 
            }
            else if (allSlots[i].myProfile == JettyData || allSlots[i].myProfile == LinusData)
            {
                isUnlocked = isFirstDateDone; 
            }
            else
            {
                isUnlocked = false; 
            }

            allSlots[i].UpdateSlotState(selectedLeft, selectedRight, isUnlocked, lockedPrefab);
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
            UpdateLocationButtons();
            OpenPageByIndex(5); 
        }
        else
        {
            ShowWarningPopup(); 
        }
    }

    public void UpdateLocationButtons()
    {
        if (selectedLeft == null || selectedRight == null) return;

        string char1 = selectedLeft.characterName;
        string char2 = selectedRight.characterName;
        string coupleKey = string.Compare(char1, char2) < 0 ? 
            "CoupleLevel_" + char1 + "_" + char2 : 
            "CoupleLevel_" + char2 + "_" + char1;

        int currentLevel = PlayerPrefs.GetInt(coupleKey, 0);

        if ((selectedLeft == IoData && selectedRight == ElroiData || selectedLeft == ElroiData && selectedRight == IoData) && currentLevel == 0)
        {
            SetButtonState(btnRestoran, true);
            SetButtonState(btnArcade, false);
            SetButtonState(btnBar, false);
            SetButtonState(btnEv, false);
            return;
        }

        if (currentLevel == 0)
        {
            SetButtonState(btnRestoran, true);
            SetButtonState(btnArcade, true);
            SetButtonState(btnBar, false);
            SetButtonState(btnEv, false);
        }
        else if (currentLevel == 1)
        {
            SetButtonState(btnRestoran, false);
            SetButtonState(btnArcade, false);
            SetButtonState(btnBar, true);
            SetButtonState(btnEv, true);
        }
        else 
        {
            SetButtonState(btnRestoran, false);
            SetButtonState(btnArcade, false);
            SetButtonState(btnBar, false);
            SetButtonState(btnEv, false);
        }
    }

    private void SetButtonState(Button btn, bool isActive)
    {
        if (btn != null)
        {
            btn.interactable = isActive;
        }
    }

    public void StartDateWithLocation(string secilenMekan)
    {
        DateSettings.leftChar = selectedLeft;
        DateSettings.rightChar = selectedRight;
        DateSettings.selectedScenario = defaultScenario; 

        foreach (var match in coupleScenarios)
        {
            if ((selectedLeft == match.characterA && selectedRight == match.characterB) ||
                (selectedLeft == match.characterB && selectedRight == match.characterA))
            {
                List<DialogueDataları> aranacakListe = null;

                switch (secilenMekan)
                {
                    case "Restoran": aranacakListe = match.restoranSenaryolari; break;
                    case "Ev": aranacakListe = match.evSenaryolari; break;
                    case "Bar": aranacakListe = match.barSenaryolari; break;
                    case "Arcade": aranacakListe = match.arcadeSenaryolari; break;
                }

                if (aranacakListe != null && aranacakListe.Count > 0)
                {
                    string char1 = match.characterA.characterName;
                    string char2 = match.characterB.characterName;
                    
                    string coupleKey = string.Compare(char1, char2) < 0 ? 
                        "CoupleLevel_" + char1 + "_" + char2 : 
                        "CoupleLevel_" + char2 + "_" + char1;

                    int currentLevel = PlayerPrefs.GetInt(coupleKey, 0);
                    
                    DateSettings.selectedScenario = aranacakListe[0];

                    PlayerPrefs.SetInt(coupleKey, currentLevel + 1);
                    PlayerPrefs.Save();
                }
                break; 
            }
        }
        SceneManager.LoadScene("Loading");
    }
    
    public void OnLoginClicked() => OpenPageByIndex(1);
    public void OnMatchButtonClicked() => OpenPageByIndex(2);
}

[System.Serializable]
public class MatchScenario
{
    public Characters characterA;
    public Characters characterB;
    
    public List<DialogueDataları> restoranSenaryolari; 
    public List<DialogueDataları> arcadeSenaryolari;
    public List<DialogueDataları> barSenaryolari;
    public List<DialogueDataları> evSenaryolari;
}