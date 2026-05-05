using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    
    [Header("Büyük Ekran Yazıları")]
    public TMP_Text ortakTamFullName;
    public TMP_Text ortakTamDemographics;
    public TMP_Text ortakTamRelationship;
    public TMP_Text ortakTamHobbies;
    public TMP_Text ortakTamFoodPrefs;
    public TMP_Text ortakTamQuote;
    
    [Header("Küçük Ekran Yazıları")]
    public TMP_Text ortakSmallFullName;
    public TMP_Text ortakSmallDemographics;
    public TMP_Text ortakSmallRelationship;
    public TMP_Text ortakSmallHobbies;
    public TMP_Text ortakSmallFoodPrefs;
    public TMP_Text ortakSmallQuote;

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

    [Header("Mekan Butonları Küçük")]
    public Button btnRestoranSmall;
    public Button btnBarSmall;
    public Button btnEvSmall;
    public Button btnArcadeSmall;

    [Header("Mekan Butonları Büyük")]
    public Button btnRestoranLarge;
    public Button btnBarLarge;
    public Button btnEvLarge;
    public Button btnArcadeLarge;

    [Header("Reviews (Değerlendirmeler) Ekranı")]
    public Transform reviewsContentContainerSmall; 
    public Transform reviewsContentContainerLarge; 
    public GameObject reviewSlotPrefabSmall;       // YENİ: Küçük Ekran Prefabı
    public GameObject reviewSlotPrefabLarge;       // YENİ: Büyük Ekran Prefabı

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
        
        bool shouldShowBackButton = (index != 3 && index != 0); 

        if (backButtonSmall != null) backButtonSmall.SetActive(shouldShowBackButton);
        if (backButtonLarge != null) backButtonLarge.SetActive(shouldShowBackButton);

        if (index == 1) RefreshSlots(); 
        
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
        
        if (ortakTamFullName != null) ortakTamFullName.text = profile.fullName;
        if (ortakTamDemographics != null) ortakTamDemographics.text = profile.demographicsInfo;
        if (ortakTamRelationship != null) ortakTamRelationship.text = profile.relationshipGoals;
        if (ortakTamHobbies != null) ortakTamHobbies.text = profile.hobbies;
        if (ortakTamFoodPrefs != null) ortakTamFoodPrefs.text = profile.foodPreferenceText;
        if (ortakTamQuote != null) ortakTamQuote.text = profile.quoteText;

        if (ortakSmallFullName != null) ortakSmallFullName.text = profile.fullName;
        if (ortakSmallDemographics != null) ortakSmallDemographics.text = profile.demographicsInfo;
        if (ortakSmallRelationship != null) ortakSmallRelationship.text = profile.relationshipGoals;
        if (ortakSmallHobbies != null) ortakSmallHobbies.text = profile.hobbies;
        if (ortakSmallFoodPrefs != null) ortakSmallFoodPrefs.text = profile.foodPreferenceText;
        if (ortakSmallQuote != null) ortakSmallQuote.text = profile.quoteText;

        OpenPageByIndex(2);
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

        OpenPageByIndex(3);
    }

    public void OnHeartClicked(int slotIndex)
    {
        activeSlotToFill = slotIndex;
        if (slotIndex == 0) selectedLeft = null;
        else selectedRight = null;
        
        RefreshAllUI();
        
        OpenPageByIndex(1); 
    }
    
    public void OnDatePlacesClicked()
    {
        OpenPageByIndex(4); 
    }
    
    void RefreshSlots()
    {
       if (IoData == null || ElroiData == null) return;

        string char1 = IoData.characterName;
        string char2 = ElroiData.characterName;
        
        string dateKey = string.Compare(char1, char2) < 0 ? 
            "DateLevel_" + char1 + "_" + char2 : 
            "DateLevel_" + char2 + "_" + char1;

        int ioElroiLevel = PlayerPrefs.GetInt(dateKey, 0);
        bool isFirstDateDone = ioElroiLevel > 0;

        for (int i = 0; i < allSlots.Count; i++)
        {
            if (allSlots[i] == null || allSlots[i].myProfile == null)
            {
                if (allSlots[i] != null) allSlots[i].UpdateSlotState(selectedLeft, selectedRight, false, lockedPrefab, false);
                continue;
            }

            bool isUnlocked = false;
            bool isGrayedOut = false; 

            if (allSlots[i].myProfile == IoData || allSlots[i].myProfile == ElroiData)
            {
                isUnlocked = true; 
            }
            else
            {
                isUnlocked = isFirstDateDone; 
            }

            string cName = allSlots[i].myProfile.characterName;

            if (PlayerPrefs.GetInt("PlayedCouple_Ary_Jetty", 0) == 1)
            {
                if (cName == "Jetty")
                {
                    bool isArySelected = (selectedLeft != null && selectedLeft.characterName == "Ary") || 
                                         (selectedRight != null && selectedRight.characterName == "Ary");
                    if (isArySelected) isGrayedOut = true;
                }
                else if (cName == "Ary")
                {
                    bool isJettySelected = (selectedLeft != null && selectedLeft.characterName == "Jetty") || 
                                           (selectedRight != null && selectedRight.characterName == "Jetty");
                    if (isJettySelected) isGrayedOut = true;
                }
            }

            allSlots[i].UpdateSlotState(selectedLeft, selectedRight, isUnlocked, lockedPrefab, isGrayedOut);
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
        UpdateLocationButtons();
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
            OpenPageByIndex(4); 
        }
        else
        {
            ShowWarningPopup(); 
        }
    }

    public void UpdateLocationButtons()
    {
        if (selectedLeft == null || selectedRight == null) 
        {
            SetButtonState(btnRestoranSmall, btnRestoranLarge, false);
            SetButtonState(btnArcadeSmall, btnArcadeLarge, false);
            SetButtonState(btnBarSmall, btnBarLarge, false);
            SetButtonState(btnEvSmall, btnEvLarge, false);
            return; 
        }

        string char1 = selectedLeft.characterName;
        string char2 = selectedRight.characterName;
        
        string dateKey = string.Compare(char1, char2) < 0 ? 
            "DateLevel_" + char1 + "_" + char2 : 
            "DateLevel_" + char2 + "_" + char1;

        int currentLevel = PlayerPrefs.GetInt(dateKey, 0);

        bool isIoAndElroi = (selectedLeft == IoData && selectedRight == ElroiData) || 
                            (selectedLeft == ElroiData && selectedRight == IoData);

        if (currentLevel == 0)
        {
            if (isIoAndElroi)
            {
                SetButtonState(btnRestoranSmall, btnRestoranLarge, true);
                SetButtonState(btnArcadeSmall, btnArcadeLarge, false);
                SetButtonState(btnBarSmall, btnBarLarge, false);
                SetButtonState(btnEvSmall, btnEvLarge, false);
            }
            else
            {
                SetButtonState(btnRestoranSmall, btnRestoranLarge, true);
                SetButtonState(btnArcadeSmall, btnArcadeLarge, true);
                SetButtonState(btnBarSmall, btnBarLarge, false);
                SetButtonState(btnEvSmall, btnEvLarge, false);
            }
        }
        else if (currentLevel == 1)
        {
            if (isIoAndElroi)
            {
                SetButtonState(btnRestoranSmall, btnRestoranLarge, false);
                SetButtonState(btnArcadeSmall, btnArcadeLarge, false);
                SetButtonState(btnBarSmall, btnBarLarge, true);  
                SetButtonState(btnEvSmall, btnEvLarge, false);   
            }
            else
            {
                SetButtonState(btnRestoranSmall, btnRestoranLarge, false);
                SetButtonState(btnArcadeSmall, btnArcadeLarge, false);
                SetButtonState(btnBarSmall, btnBarLarge, true);
                SetButtonState(btnEvSmall, btnEvLarge, true);
            }
        }
        else 
        {
            SetButtonState(btnRestoranSmall, btnRestoranLarge, false);
            SetButtonState(btnArcadeSmall, btnArcadeLarge, false);
            SetButtonState(btnBarSmall, btnBarLarge, false);
            SetButtonState(btnEvSmall, btnEvLarge, false);
        }
    }

    private void SetButtonState(Button btnSmall, Button btnLarge, bool isActive)
    {
        if (btnSmall != null) btnSmall.interactable = isActive;
        if (btnLarge != null) btnLarge.interactable = isActive;
    }

    public void StartDateWithLocation(string secilenMekan)
    {
        if (selectedLeft == null || selectedRight == null) return; 
        
        DateSettings.leftChar = selectedLeft;
        DateSettings.rightChar = selectedRight;
        DateSettings.selectedScenario = defaultScenario; 

        if (selectedLeft != null) PlayerPrefs.SetInt("Played_" + selectedLeft.characterName, 1);
        if (selectedRight != null) PlayerPrefs.SetInt("Played_" + selectedRight.characterName, 1);
        PlayerPrefs.Save();

        if (selectedLeft != null && selectedRight != null)
        {
            string c1 = selectedLeft.characterName;
            string c2 = selectedRight.characterName;
            string playedCoupleKey = string.Compare(c1, c2) < 0 ? 
                "PlayedCouple_" + c1 + "_" + c2 : 
                "PlayedCouple_" + c2 + "_" + c1;
            
            PlayerPrefs.SetInt(playedCoupleKey, 1);
            PlayerPrefs.Save();
        }

        foreach (var match in coupleScenarios)
        {
            if ((selectedLeft == match.characterA && selectedRight == match.characterB) ||
                (selectedLeft == match.characterB && selectedRight == match.characterA))
            {
                DateSettings.leftChar = match.characterA;
                DateSettings.rightChar = match.characterB;

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
    
    public void OnMatchButtonClicked() => OpenPageByIndex(1);

    public void LoadReviews()
    {
        if (reviewsContentContainerSmall != null)
        {
            foreach (Transform child in reviewsContentContainerSmall)
            {
                Destroy(child.gameObject);
            }
        }

        if (reviewsContentContainerLarge != null)
        {
            foreach (Transform child in reviewsContentContainerLarge)
            {
                Destroy(child.gameObject);
            }
        }

        string jsonLoad = PlayerPrefs.GetString("SavedReviewsDB", "");
        if (string.IsNullOrEmpty(jsonLoad)) return; 

        ReviewDatabase db = JsonUtility.FromJson<ReviewDatabase>(jsonLoad);

        db.allPastDates.Reverse();

        foreach (var reviewData in db.allPastDates)
        {
            Sprite icon1 = GetProfileIconByName(reviewData.char1Name);
            Sprite icon2 = GetProfileIconByName(reviewData.char2Name);
            
            if (reviewsContentContainerSmall != null && reviewSlotPrefabSmall != null)
            {
                GameObject newSlotSmall = Instantiate(reviewSlotPrefabSmall, reviewsContentContainerSmall);
                ReviewSlot slotScriptSmall = newSlotSmall.GetComponent<ReviewSlot>();
                if (slotScriptSmall != null)
                {
                    slotScriptSmall.Setup(reviewData, icon1, icon2);
                }
            }

            if (reviewsContentContainerLarge != null && reviewSlotPrefabLarge != null)
            {
                GameObject newSlotLarge = Instantiate(reviewSlotPrefabLarge, reviewsContentContainerLarge);
                ReviewSlot slotScriptLarge = newSlotLarge.GetComponent<ReviewSlot>();
                if (slotScriptLarge != null)
                {
                    slotScriptLarge.Setup(reviewData, icon1, icon2);
                }
            }
        }
    }

    private Sprite GetProfileIconByName(string charName)
    {
        if (IoData != null && IoData.characterName == charName) return IoData.profileIcon;
        if (ElroiData != null && ElroiData.characterName == charName) return ElroiData.profileIcon;
        if (JettyData != null && JettyData.characterName == charName) return JettyData.profileIcon;
        if (LinusData != null && LinusData.characterName == charName) return LinusData.profileIcon;
        return defaultIcon;
    }
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