using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System.Collections.Generic;
using System; 

[RequireComponent(typeof(CanvasGroup))] 
public class TutorialPopup : MonoBehaviour
{
    [Header("UI Elemanları")]
    public TMP_Text titleText;       
    public Image displayImage;       
    public Button leftButton;        
    public Button rightButton;       
    public Button readyButton;       
    
    private List<Sprite> currentPages; 
    private int pageIndex = 0;
    private Action onReadyCallback;     
    
    private CanvasGroup canvasGroup; 

    void Awake()
    {
        
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

       
        leftButton.onClick.RemoveAllListeners(); 
        leftButton.onClick.AddListener(PrevPage);

        rightButton.onClick.RemoveAllListeners();
        rightButton.onClick.AddListener(NextPage);

        readyButton.onClick.RemoveAllListeners();
        readyButton.onClick.AddListener(OnReadyClicked);
        
        
        HidePanel();
    }

    public void OpenTutorial(string title, List<Sprite> pages, Action onComplete)
    {
        if (pages == null || pages.Count == 0)
        {
            onComplete?.Invoke();
            return;
        }

        if (titleText != null) titleText.text = title;

        currentPages = pages;
        onReadyCallback = onComplete; 
        
        pageIndex = 0; 
        
        
        ShowPanel();
        
        UpdateUI();
    }

    void UpdateUI()
    {
        if(currentPages != null && currentPages.Count > pageIndex)
        {
            displayImage.sprite = currentPages[pageIndex];
            displayImage.preserveAspect = true; 
        }
        
        if(leftButton) leftButton.gameObject.SetActive(pageIndex > 0);
        if(rightButton) rightButton.gameObject.SetActive(pageIndex < currentPages.Count - 1);
        if(readyButton) readyButton.gameObject.SetActive(pageIndex == currentPages.Count - 1);
    }

    void NextPage()
    {
        if (pageIndex < currentPages.Count - 1)
        {
            pageIndex++;
            UpdateUI();
        }
    }

    void PrevPage()
    {
        if (pageIndex > 0)
        {
            pageIndex--;
            UpdateUI();
        }
    }

    void OnReadyClicked()
    {
        
        if (onReadyCallback != null)
        {
            onReadyCallback.Invoke();
        }

       
        HidePanel();
    }

 
    private void ShowPanel()
    {
       
        canvasGroup.alpha = 1f; 
        canvasGroup.blocksRaycasts = true; 
        canvasGroup.interactable = true; 
        
      
        transform.SetAsLastSibling();
    }

    private void HidePanel()
    {
        canvasGroup.alpha = 0f; 
        canvasGroup.blocksRaycasts = false; 
        canvasGroup.interactable = false; 
    }
}