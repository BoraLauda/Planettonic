using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
    
public class brainDate : MonoBehaviour
{
    
    public TMP_Text introLocationText; 
    public Image backgroundImage;
    
    public GameObject introPanel;
    
    public TMP_Text introLeftNameText;  
    public TMP_Text introRightNameText;  
    public Transform introLeftStarsCont; 
    public Transform introRightStarsCont;
    
    public GameObject bartendingMiniGameObj; 
    private bool isBartendingMode = false;
    
    public TutorialPopup tutorialPopup; 
    
    public List<Sprite> menuTutorialSprites; 
    public List<Sprite> iceBreakerTutorialSprites;
    public List<Sprite> dodgeTutorialSprites;
    
    public string desktopScene = "Desktop";
    
    public GameObject dateSuccessPanel; 
    public GameObject dateFailPanel;
    
    public GameObject BGblur;
    
    public Transform leftStarsCont;     
    public Transform rightStarsCont;
    public Transform successLeftStars;     
    public Transform successRightStars;  
    public Transform failLeftStars;        
    public Transform failRightStars;
    
    public float leftStars = 0;
    public float rightStars = 0;
    
    public GameObject dateEndedObject;    
    public DialogueDataları startingScenario;
    public DialogueDataları failScenario;
    private DialogueDataları savedMainScenario;
    
    public brainDODGE dodgeScript;
    private bool isDodgeMode = false;
    
    public Image leftDaterImage;          
    public GameObject leftDialoguePanel; 
    public TMP_Text leftNameText;            
    public TMP_Text leftBodyText;
    
    public GameObject leftOptionsPanel;  
    public Button[] leftButtons;
    
    public Transform stars;  
    public GameObject fullStarPrefab; 
    public GameObject halfStarPrefab;

    public Image rightDaterImage;         
    public GameObject rightDialoguePanel;
    public TMP_Text rightNameText;
    public TMP_Text rightBodyText;
    
    public GameObject rightOptionsPanel; 
    public Button[] rightButtons;
    
    public IceBreaker iceBreakerScript; 
    private bool isIceBreakerMode = false;
   
    public GameObject chancellorPanel;
    public TMP_Text chancellorNameText;     
    public TMP_Text chancellorBodyText;
    
    public GameObject chaperonPanel;
    public TMP_Text chaperonNameText;       
    public TMP_Text chaperonBodyText;
    
    // KALP YILDIZ
    public float starThreshold = 6f;
    public float currentStars = 0;   
    public int totalHearts = 0;
    public TMP_Text heartUI; 
    public TMP_Text starUI;

    public GameObject menuMiniGameObj;
    
    public DialogueDataları menuTutorialScenario;
    
    public float typeSpeed = 0.04f;
    
    private DialogueDataları currentScenario;
    private Queue<DialogueDataları> scenarioQueue = new Queue<DialogueDataları>();
    
    private int lineIndex = 0;
    private bool isTyping = false;
    private string currentFullSentence = "";

    private bool isEventTriggered = false;
    
    public float focusSpeed = 10f;
    private Vector3 leftTargetScale = Vector3.one;
    private Vector3 rightTargetScale = Vector3.one;
    
    private bool isMenuMode = false;
    
    public GameObject pixelMiniGameObj;
    
    public GameObject cyberchicsButton;
    private bool isCyberchicsUsed = false;
    
    public GameObject heartOfCircuitButton; 
    private bool isHeartOfCircuitActive = false;
    private bool isHeartOfCircuitUsedThisDate = false;

    void Start()
    {
        // PlayerPrefs.SetString("SavedEquippedItems", "The Cyberchics,Heart of the Circuit,");// SONRADAN SİL
        
        if(leftDialoguePanel) leftDialoguePanel.SetActive(false);
        if(leftOptionsPanel) leftOptionsPanel.SetActive(false);
        if(chancellorPanel) chancellorPanel.SetActive(false);
        if(chaperonPanel) chaperonPanel.SetActive(false);
        
        if(rightDialoguePanel) rightDialoguePanel.SetActive(false);
        if(rightOptionsPanel) rightOptionsPanel.SetActive(false);
        
        if(BGblur) BGblur.SetActive(false);

        if(menuMiniGameObj) menuMiniGameObj.SetActive(false);
        if(dateEndedObject) dateEndedObject.SetActive(false);
        
        
        PrepareSceneData();

        if (introPanel != null)
        {
            introPanel.SetActive(true);
        }
        else
        {
            StartTheDate(); 
        }
    }
    
   
    void PrepareSceneData()
    {
        DialogueDataları playThis = DateSettings.selectedScenario != null ? DateSettings.selectedScenario : startingScenario;
        
        if (playThis != null)
        {
            if (introLocationText != null && !string.IsNullOrEmpty(playThis.locationName))
            {
                introLocationText.text = playThis.locationName; 
            }

            if (backgroundImage != null && playThis.locationBackground != null)
            {
                backgroundImage.sprite = playThis.locationBackground; 
            }
            
            Sprite foundLeft = null;
            Sprite foundRight = null;

           
            foreach (var line in playThis.allLines)
            {
                if (foundLeft == null && line.side == SpeakerSide.Left && line.characterSprite != null)
                    foundLeft = line.characterSprite;
                    
                if (foundRight == null && line.side == SpeakerSide.Right && line.characterSprite != null)
                    foundRight = line.characterSprite;

                if (foundLeft != null && foundRight != null) break; 
            }

            
            if (foundLeft != null && leftDaterImage != null) 
            {
                leftDaterImage.sprite = foundLeft;
            }
            if (foundRight != null && rightDaterImage != null) 
            {
                rightDaterImage.sprite = foundRight;
            }
        }

       
        string currentLocation = "";
        if (playThis != null && !string.IsNullOrEmpty(playThis.locationName))
        {
            currentLocation = playThis.locationName;
        }


        if (DateSettings.leftChar != null)
        {
            if (introLeftNameText != null) introLeftNameText.text = DateSettings.leftChar.characterName; 
            
            foreach (var pref in DateSettings.leftChar.locationPreferences)
            {
                if (pref.locationName == currentLocation)
                {
                    leftStars += pref.bonusStars;
                    break; 
                }
            }
        }
        
        if (DateSettings.rightChar != null)
        {
            if (introRightNameText != null) introRightNameText.text = DateSettings.rightChar.characterName;
            
            foreach (var pref in DateSettings.rightChar.locationPreferences)
            {
                if (pref.locationName == currentLocation)
                {
                    rightStars += pref.bonusStars;
                    break; 
                }
            }
        }
        
        if (introLeftStarsCont != null) UpdateBar(introLeftStarsCont, leftStars);
        if (introRightStarsCont != null) UpdateBar(introRightStarsCont, rightStars);

        UpdateScoreUI();
        
        CheckHeartOfCircuitAvailability();
    }
    
    void Update()
    {
        if (leftDaterImage != null)
        {
            leftDaterImage.transform.localScale = Vector3.Lerp(leftDaterImage.transform.localScale, leftTargetScale, Time.deltaTime * focusSpeed);
        }

        if (rightDaterImage != null)
        {
            rightDaterImage.transform.localScale = Vector3.Lerp(rightDaterImage.transform.localScale, rightTargetScale, Time.deltaTime * focusSpeed);
        }
    }

    public void StartScenario(DialogueDataları scenario)
    {
        currentScenario = scenario;
        lineIndex = 0;
        DisplayLine();
    }
    
    public void QueueScenarios(List<DialogueDataları> scenariosToPlay)
    {
        scenarioQueue.Clear(); 
        foreach (var sc in scenariosToPlay)
        {
            scenarioQueue.Enqueue(sc);
        }
        PlayNextInQueue();
    }
    
    void PlayNextInQueue()
    {
        if (scenarioQueue.Count > 0)
        {
            DialogueDataları next = scenarioQueue.Dequeue();
            StartScenario(next);
        }
        else
        {
            float totalScore = leftStars + rightStars; 
            
            if(leftDialoguePanel) leftDialoguePanel.SetActive(false);
            if(rightDialoguePanel) rightDialoguePanel.SetActive(false);
            if(chancellorPanel) chancellorPanel.SetActive(false);
            if(chaperonPanel) chaperonPanel.SetActive(false);

            if (dateEndedObject != null) dateEndedObject.SetActive(true); 

            if (totalScore >= starThreshold) 
            {
                if(dateSuccessPanel != null) dateSuccessPanel.SetActive(true);
            }
            else
            {
                if(dateFailPanel != null) dateFailPanel.SetActive(true);
            }
        }
    }
    
    public void AddReward(float stars, int hearts, TargetCharacter target)
    {
        if (isHeartOfCircuitActive && hearts > 0)
        {
            int roll = Random.Range(0, 100);
            if (roll < 30) 
            {
                hearts *= 2;
                Debug.Log("Heart of the Circuit tutu! Kalp ikiye katlandı: " + hearts);
            }
        }
        
        totalHearts += hearts;

        if (target == TargetCharacter.Left)
        {
            leftStars += stars;
            Debug.Log($"SOL KAZANDI: {stars} Yıldız.");
        }
        else if (target == TargetCharacter.Right)
        {
            rightStars += stars;
            Debug.Log($"SAĞ KAZANDI: {stars} Yıldız.");
        }
        else if (target == TargetCharacter.Both)
        {
            leftStars += stars;
            rightStars += stars;
            Debug.Log($"İKİSİ DE KAZANDI: {stars} Yıldız.");
        }
        
        UpdateScoreUI();
    }
    
    void DisplayLine()
    {
        if (lineIndex >= currentScenario.allLines.Count)
        {
            if (isMenuMode) 
            {
                if(leftDialoguePanel) leftDialoguePanel.SetActive(false);
                if(rightDialoguePanel) rightDialoguePanel.SetActive(false);
                if(chancellorPanel) chancellorPanel.SetActive(false);
                if(chaperonPanel) chaperonPanel.SetActive(false);

                return; 
            }
            
            if (isDodgeMode) 
            { 
                if(leftDialoguePanel) leftDialoguePanel.SetActive(false);
                if(rightDialoguePanel) rightDialoguePanel.SetActive(false);
                
                leftTargetScale = Vector3.one;
                rightTargetScale = Vector3.one;
                
                if (dodgeScript != null) dodgeScript.ResumeAfterDialogue(); 
                return; 
            }

            if (isIceBreakerMode)
            {
                leftTargetScale = Vector3.one;
                rightTargetScale = Vector3.one;
                if (iceBreakerScript != null) iceBreakerScript.ResumeGame(); return;
            }
            
            if (currentScenario.nextScenario != null) { StartScenario(currentScenario.nextScenario); return; }
            PlayNextInQueue(); 
            return;
        }

        DialogueLine line = currentScenario.allLines[lineIndex];
        UpdateCharacterFocus(line.side);

        if (line.side == SpeakerSide.Left)
        {
            ActivatePanel(SpeakerSide.Left);
            HandleUI(leftOptionsPanel, leftButtons, leftDaterImage, leftNameText, leftBodyText, line);
        }
        else if (line.side == SpeakerSide.Right)
        {
            ActivatePanel(SpeakerSide.Right);
            HandleUI(rightOptionsPanel, rightButtons, rightDaterImage, rightNameText, rightBodyText, line);
        }
        else if (line.side == SpeakerSide.Counselor)
        {
            ActivatePanel(SpeakerSide.Counselor); 
            HandleMentorDirect(chancellorNameText, chancellorBodyText, line);
        }
        else if (line.side == SpeakerSide.Chaperon)
        {
            ActivatePanel(SpeakerSide.Chaperon);
            HandleMentorDirect(chaperonNameText, chaperonBodyText, line);
        }
    }
    
    void HandleMentorDirect(TMP_Text targetNameText, TMP_Text targetBodyText, DialogueLine line)
    {
        targetNameText.text = line.characterName;

        if (line.choices != null && line.choices.Count > 0)
        {
            targetBodyText.text = line.sentence;
            leftOptionsPanel.SetActive(true); 
            SetupButtons(leftButtons, line.choices);
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(TypeSentence(targetBodyText, line.sentence));
        }
    }
   
    void HandleUI(GameObject activeOptionsPanel, Button[] activeButtons,
        Image charImg, TMP_Text nameTxt, TMP_Text bodyTxt, 
        DialogueLine line)
    {
        if (line.characterSprite != null) charImg.sprite = line.characterSprite;
        nameTxt.text = line.characterName;

        bool hasChoices = (line.choices != null && line.choices.Count > 0);

        if (hasChoices)
        {
            bodyTxt.gameObject.SetActive(false);
            activeOptionsPanel.SetActive(true);
            SetupButtons(activeButtons, line.choices);
        }
        else
        {
            activeOptionsPanel.SetActive(false);
            bodyTxt.gameObject.SetActive(true);

            StopAllCoroutines();
            StartCoroutine(TypeSentence(bodyTxt, line.sentence));
        }
    }

    void HandleUI(GameObject activeDialoguePanel, GameObject inactiveDialoguePanel, 
        GameObject activeOptionsPanel, Button[] activeButtons,
        Image charImg, TMP_Text nameTxt, TMP_Text bodyTxt, 
        DialogueLine line)
    {
        inactiveDialoguePanel.SetActive(false);
        
        if (leftOptionsPanel.activeSelf) leftOptionsPanel.SetActive(false);
        if (rightOptionsPanel.activeSelf) rightOptionsPanel.SetActive(false);

        activeDialoguePanel.SetActive(true);

        if (line.characterSprite != null) charImg.sprite = line.characterSprite;
        nameTxt.text = line.characterName;

        bool hasChoices = (line.choices != null && line.choices.Count > 0);

        if (hasChoices)
        {
            bodyTxt.gameObject.SetActive(false);
            activeOptionsPanel.SetActive(true);
            SetupButtons(activeButtons, line.choices);
        }
        else
        {
            activeOptionsPanel.SetActive(false);
            bodyTxt.gameObject.SetActive(true);

            StopAllCoroutines();
            StartCoroutine(TypeSentence(bodyTxt, line.sentence));
        }
    }
    
    void ActivatePanel(SpeakerSide activeSide)
    { 
        if(leftDialoguePanel) leftDialoguePanel.SetActive(false);
        if(rightDialoguePanel) rightDialoguePanel.SetActive(false);
        
        if(chancellorPanel) chancellorPanel.SetActive(false);
        if(chaperonPanel) chaperonPanel.SetActive(false);

        if (activeSide == SpeakerSide.Left) 
            leftDialoguePanel.SetActive(true);
        else if (activeSide == SpeakerSide.Right) 
            rightDialoguePanel.SetActive(true);
        else if (activeSide == SpeakerSide.Counselor)
            chancellorPanel.SetActive(true); 
        else if (activeSide == SpeakerSide.Chaperon)
            chaperonPanel.SetActive(true);   
    }
    
    void SetupButtons(Button[] buttons, List<ChoiceOption> choices)
    {
        foreach (var btn in buttons) btn.gameObject.SetActive(false);

        for (int i = 0; i < choices.Count; i++)
        {
            if (i >= buttons.Length) break; 

            buttons[i].gameObject.SetActive(true);
            
            TMP_Text btnText = buttons[i].GetComponentInChildren<TMP_Text>();
            if (btnText != null) btnText.text = choices[i].choices;

            int index = i; 
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => OnOptionSelected(choices[index]));
        }
        
        DialogueLine currentLine = currentScenario.allLines[lineIndex];
        CheckCyberchicsAvailability(currentLine);
    }
    
    void OnOptionSelected(ChoiceOption option)
    {
        if(leftOptionsPanel) leftOptionsPanel.SetActive(false);
        if(rightOptionsPanel) rightOptionsPanel.SetActive(false);
        
        if (option.starReward > 0 || option.heartReward > 0)
        {
            AddReward(option.starReward, option.heartReward, option.target);
        }

        if (option.nextScenario != null)
        {
            scenarioQueue.Clear();
            StartScenario(option.nextScenario);
        }
        else
        {
            NextLine();
        }
    }
    
    IEnumerator TypeSentence(TMP_Text textObj, string sentence)
    {
        isTyping = true;
        currentFullSentence = sentence;
        textObj.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            textObj.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }
        isTyping = false;
    }

    public void OnScreenClick()
    {
        if (isMenuMode) return; 

        if (currentScenario == null) return;
        if (lineIndex >= currentScenario.allLines.Count) return;
        if (isEventTriggered) return;

        DialogueLine line = currentScenario.allLines[lineIndex];

        if (line.choices != null && line.choices.Count > 0) return;

        if (isTyping)
        {
            StopAllCoroutines();
            if (line.side == SpeakerSide.Left) leftBodyText.text = currentFullSentence;
            else if (line.side == SpeakerSide.Right) rightBodyText.text = currentFullSentence;
            else if (line.side == SpeakerSide.Counselor) chancellorBodyText.text = currentFullSentence;
            else if (line.side == SpeakerSide.Chaperon) chaperonBodyText.text = currentFullSentence;
            
            isTyping = false;
            return;
        }

        if (!string.IsNullOrEmpty(line.eventTrigger))
        {
            TriggerEvent(line.eventTrigger);
            return; 
        }
        NextLine();
    }

    void TriggerEvent(string eventName)
    {
      if (eventName == "StartMenuGame")
      {
          if(leftDialoguePanel) leftDialoguePanel.SetActive(false);
          if(rightDialoguePanel) rightDialoguePanel.SetActive(false);
          
          if (BGblur) BGblur.SetActive(true);
          if (menuMiniGameObj != null) menuMiniGameObj.SetActive(true);
          
          bool isIoAndElroi = false;
          if (DateSettings.leftChar != null && DateSettings.rightChar != null)
          {
              string char1 = DateSettings.leftChar.characterName;
              string char2 = DateSettings.rightChar.characterName;
               
              if ((char1 == "Io" && char2 == "Elroi") || (char1 == "Elroi" && char2 == "Io"))
              {
                  isIoAndElroi = true;
              }
          }

          if (isIoAndElroi && menuTutorialScenario != null)
          {
              StartScenario(menuTutorialScenario);
          }
         
      }
      else if (eventName == "MenüMiniGame")
      {
          isEventTriggered = true;

          if(leftDialoguePanel) leftDialoguePanel.SetActive(false);
          if(rightDialoguePanel) rightDialoguePanel.SetActive(false);
          if(chancellorPanel) chancellorPanel.SetActive(false);
          if(chaperonPanel) chaperonPanel.SetActive(false);
            
          tutorialPopup.OpenTutorial("MENU", menuTutorialSprites, () =>
          {
              isEventTriggered = false; 
              isMenuMode = true;
          });
      }
       
      else if (eventName == "StartPixelGame")
      {
          savedMainScenario = currentScenario; 
          isEventTriggered = true;
           
          if(leftDialoguePanel) leftDialoguePanel.SetActive(false);
          if(rightDialoguePanel) rightDialoguePanel.SetActive(false);
          if(chancellorPanel) chancellorPanel.SetActive(false);
          if(chaperonPanel) chaperonPanel.SetActive(false);
    
          if (BGblur) BGblur.SetActive(true);
           
          if (pixelMiniGameObj != null) 
          {
              pixelMiniGameObj.SetActive(true);
          }
      }
       
      else if (eventName == "StartIceBreaker")
      {
          savedMainScenario = currentScenario;
          isEventTriggered = true;

          if (leftDialoguePanel) leftDialoguePanel.SetActive(false);
          if (rightDialoguePanel) rightDialoguePanel.SetActive(false);
          if(chancellorPanel) chancellorPanel.SetActive(false);
          if(chaperonPanel) chaperonPanel.SetActive(false);
            
          if (BGblur) BGblur.SetActive(true);

          tutorialPopup.OpenTutorial("ICE BREAKER", iceBreakerTutorialSprites, () =>
          {
              if (iceBreakerScript != null) iceBreakerScript.StartGame();
          });
      }
       
      else if (eventName == "StartBartending")
      {
          savedMainScenario = currentScenario;
          isEventTriggered = true;
          isBartendingMode = true;
           
          if(leftDialoguePanel) leftDialoguePanel.SetActive(false);
          if(rightDialoguePanel) rightDialoguePanel.SetActive(false);
          if(chancellorPanel) chancellorPanel.SetActive(false);
          if(chaperonPanel) chaperonPanel.SetActive(false);
            
          if (BGblur) BGblur.SetActive(true);

          
          if (bartendingMiniGameObj != null) 
          {
              bartendingMiniGameObj.SetActive(true);
          }
      }
       
      else if (eventName == "StartDodgeGame")
      {
          savedMainScenario = currentScenario; 
          isEventTriggered = true;
            
          if(leftDialoguePanel) leftDialoguePanel.SetActive(false);
          if(rightDialoguePanel) rightDialoguePanel.SetActive(false);
          if(chancellorPanel) chancellorPanel.SetActive(false);
          if(chaperonPanel) chaperonPanel.SetActive(false);
            
          if (BGblur) BGblur.SetActive(true);
            
          tutorialPopup.OpenTutorial("DODGE THE QUESTION", dodgeTutorialSprites, () =>
          {
              if (dodgeScript != null) dodgeScript.StartGame();
          });
      }
    }

    public void ResumeFromMiniGame(List<DialogueDataları> results)
    {
        isMenuMode = false;
        isEventTriggered = false;
        
        if (menuMiniGameObj != null) menuMiniGameObj.SetActive(false);
        if (BGblur) BGblur.SetActive(false);
        
        QueueScenarios(results);
    }
    
    public void PlayIceBreakerDialogue(DialogueDataları scenario)
    {
        isIceBreakerMode = true; 
        isEventTriggered = false;
        StartScenario(scenario);
    }

    public void EndIceBreaker(bool success)
    {
        isIceBreakerMode = false;
        if(iceBreakerScript != null) iceBreakerScript.gameObject.SetActive(false);
        isEventTriggered = false;
       
        if (BGblur) BGblur.SetActive(false);
        
        if (savedMainScenario != null && savedMainScenario.nextScenario != null)
        {
            StartScenario(savedMainScenario.nextScenario);
        }
        else
        {
            Debug.LogWarning("Ice Breaker bitti devamına senaryo bağla datadan");
        }
    }
    
    public void PlayDodgeDialogue(DialogueDataları scenario)
    {
        isDodgeMode = true;
        isEventTriggered = false; 
        StartScenario(scenario);
    }

    public void EndDodgeGame()
    {
        isDodgeMode = false;
        if(dodgeScript != null) dodgeScript.gameObject.SetActive(false);
        isEventTriggered = false;
        
        if (BGblur) BGblur.SetActive(false);

        UpdateCharacterFocus((SpeakerSide)(-1));
        
        Debug.Log("Dodge Game Bitti");
        
        float totalScore = leftStars + rightStars;
        
        if (totalScore >= starThreshold) 
        {
            if (savedMainScenario != null && savedMainScenario.nextScenario != null)
            {
                StartScenario(savedMainScenario.nextScenario);
            }
        }
        else
        {
            if (failScenario != null)
            {
                StartScenario(failScenario);
            }
        }
    }
    
    public void UseHeartOfCircuit()
    {
        if (isHeartOfCircuitActive) return;

        isHeartOfCircuitActive = true;
        isHeartOfCircuitUsedThisDate = true;

        if (heartOfCircuitButton != null) heartOfCircuitButton.SetActive(false);
        
        PlayerPrefs.SetInt("HeartCooldown", 2); 
        PlayerPrefs.Save();

        Debug.Log("Heart of the Circuit Aktif! Artık her kalp %30 şansla 2x olabilir.");
    }
    
    public void UseCyberchics()
    {
        if (isCyberchicsUsed) return; 
        isCyberchicsUsed = true;

        if (cyberchicsButton != null) cyberchicsButton.SetActive(false);

        DialogueLine currentLine = currentScenario.allLines[lineIndex];
        
        int worstIndex = -1;
        float lowestScore = float.MaxValue;

        for (int i = 0; i < currentLine.choices.Count; i++)
        {
            float score = currentLine.choices[i].starReward + currentLine.choices[i].heartReward;
            if (score < lowestScore)
            {
                lowestScore = score;
                worstIndex = i;
            }
        }
        
        if (worstIndex != -1)
        {
            if (leftOptionsPanel.activeSelf) leftButtons[worstIndex].gameObject.SetActive(false);
            else if (rightOptionsPanel.activeSelf) rightButtons[worstIndex].gameObject.SetActive(false);
        }
        
        string owned = PlayerPrefs.GetString("SavedOwnedItems", "");
        string equipped = PlayerPrefs.GetString("SavedEquippedItems", "");
        
        owned = owned.Replace("The Cyberchics,", "");
        equipped = equipped.Replace("The Cyberchics,", "");
        
        PlayerPrefs.SetString("SavedOwnedItems", owned);
        PlayerPrefs.SetString("SavedEquippedItems", equipped);
        PlayerPrefs.Save();
    }
    
    void UpdateScoreUI()
    {
        if (heartUI != null) heartUI.text = totalHearts.ToString();
        
        UpdateBar(leftStarsCont, leftStars);
        UpdateBar(rightStarsCont, rightStars);

        if (successLeftStars != null) UpdateBar(successLeftStars, leftStars);
        if (successRightStars != null) UpdateBar(successRightStars, rightStars);
        
        if (failLeftStars != null) UpdateBar(failLeftStars, leftStars);
        if (failRightStars != null) UpdateBar(failRightStars, rightStars);
    }
    
    void UpdateBar(Transform container, float starCount)
    {
        if (container == null || fullStarPrefab == null || halfStarPrefab == null) return;
     
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
      
        int fullCount = Mathf.FloorToInt(starCount); 
        bool needsHalf = (starCount - fullCount) >= 0.5f;

        for (int i = 0; i < fullCount; i++)
        {
            Instantiate(fullStarPrefab, container);
        }
        
        if (needsHalf)
        {
            Instantiate(halfStarPrefab, container);
        }
    }
    
    void UpdateCharacterFocus(SpeakerSide activeSide)
    {
        Vector3 focusScale = new Vector3(1.06f, 1.06f, 1f); 
        Vector3 normalScale = Vector3.one;                
        
        if (activeSide == SpeakerSide.Left)
        {
            leftTargetScale = focusScale;
            rightTargetScale = normalScale;
        }
        else if (activeSide == SpeakerSide.Right)
        {
            leftTargetScale = normalScale;
            rightTargetScale = focusScale;
        }
        else 
        {
            leftTargetScale = normalScale;
            rightTargetScale = normalScale;
        }
    }
    
    public void RestartDate()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToDesktop()
    {
        if (!isHeartOfCircuitUsedThisDate)
        {
            int currentCD = PlayerPrefs.GetInt("HeartCooldown", 0);
            if (currentCD > 0)
            {
                PlayerPrefs.SetInt("HeartCooldown", currentCD - 1);
            }
        }
        
        float totalScore = leftStars + rightStars; 
    
        if (totalScore >= starThreshold && DateSettings.leftChar != null && DateSettings.rightChar != null) 
        {
            string char1 = DateSettings.leftChar.characterName;
            string char2 = DateSettings.rightChar.characterName;
            string coupleKey = string.Compare(char1, char2) < 0 ? 
                "DateLevel_" + char1 + "_" + char2 : 
                "DateLevel_" + char2 + "_" + char1;
            
            int currentLevel = PlayerPrefs.GetInt(coupleKey, 0);
            PlayerPrefs.SetInt(coupleKey, currentLevel + 1);
        }
        
        int currentBank = PlayerPrefs.GetInt("SavedHearts", 0); 
        int newTotal = currentBank + totalHearts; 
        PlayerPrefs.SetInt("SavedHearts", newTotal);
        
        PlayerPrefs.SetInt("IsMarketUnlocked", 1);
        
        PlayerPrefs.SetInt("HasSave", 1); 

        PlayerPrefs.Save();
        
        if (!string.IsNullOrEmpty(desktopScene))
        {
            SceneManager.LoadScene(desktopScene);
        }
    }
    
    public void StartTheDate() 
    {
        if (introPanel != null)
        {
            introPanel.SetActive(false); 
        }
        
        if (DateSettings.selectedScenario != null)
        {
            StartScenario(DateSettings.selectedScenario); 
        }
        else if (startingScenario != null)
        {
            StartScenario(startingScenario); 
        }
    }
    
    public void EndBartendingGame(float earnedStars, int earnedHearts, TargetCharacter target)
    {
        isBartendingMode = false;
        isEventTriggered = false;
        
        if (bartendingMiniGameObj != null) bartendingMiniGameObj.SetActive(false);
        if (BGblur) BGblur.SetActive(false);
        
        AddReward(earnedStars, earnedHearts, target);
        
        
        if (savedMainScenario != null && savedMainScenario.nextScenario != null)
        {
            StartScenario(savedMainScenario.nextScenario);
        }
    }
    
    public void EndPixelGame(float earnedStars, int earnedHearts, TargetCharacter target)
    {
        isEventTriggered = false;
    
        if (pixelMiniGameObj != null) pixelMiniGameObj.SetActive(false);
        if (BGblur) BGblur.SetActive(false);
    
        AddReward(earnedStars, earnedHearts, target);
        
        if (savedMainScenario != null && savedMainScenario.nextScenario != null)
        {
            StartScenario(savedMainScenario.nextScenario);
        }
    }
    
    public void CheckCyberchicsAvailability(DialogueLine line)
    {
        if (cyberchicsButton == null) return;
        
        string equippedItems = PlayerPrefs.GetString("SavedEquippedItems", "");
        
        if (equippedItems.Contains("The Cyberchics") && !isCyberchicsUsed && line.choices != null && line.choices.Count > 2)
        {
            cyberchicsButton.SetActive(true);
        }
        else
        {
            cyberchicsButton.SetActive(false);
        }
    }
    
    public void CheckHeartOfCircuitAvailability()
    {
        if (heartOfCircuitButton == null) return;

        string equippedItems = PlayerPrefs.GetString("SavedEquippedItems", "");
        int cooldown = PlayerPrefs.GetInt("HeartCooldown", 0);
        
        if (equippedItems.Contains("Heart of the Circuit") && cooldown <= 0)
        {
            heartOfCircuitButton.SetActive(true);
        }
        else
        {
            heartOfCircuitButton.SetActive(false);
        }
    }
    
    
    void NextLine()
    {
        lineIndex++;
        DisplayLine();
    }
}