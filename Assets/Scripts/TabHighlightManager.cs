using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class TabHighlightManager : MonoBehaviour
{
    [Header("Ayarlar")]
    public RectTransform highlightBox;
    public RectTransform[] tabButtons; 
    public float animSpeed = 15f;      
    public float returnDelay = 0.15f;  

    private int activeTabIndex = 0;   
    private Vector2 targetPosition;  
    private Coroutine returnCoroutine; 

    void Start()
    {
        if (tabButtons.Length > 0)
        {
          
            targetPosition = tabButtons[activeTabIndex].anchoredPosition;
            highlightBox.anchoredPosition = targetPosition;
        }

        for (int i = 0; i < tabButtons.Length; i++)
        {
            int index = i; 

            EventTrigger trigger = tabButtons[i].gameObject.GetComponent<EventTrigger>();
            if (trigger == null) trigger = tabButtons[i].gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry enterEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
            enterEntry.callback.AddListener((data) => { OnHoverEnter(index); });
            trigger.triggers.Add(enterEntry);

           
            EventTrigger.Entry exitEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
            exitEntry.callback.AddListener((data) => { OnHoverExit(); });
            trigger.triggers.Add(exitEntry);

           
            Button btn = tabButtons[i].GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => { SetActiveTab(index); });
            }
        }
    }

    void Update()
    {
      
        if (highlightBox != null)
        {
            highlightBox.anchoredPosition = Vector2.Lerp(highlightBox.anchoredPosition, targetPosition, Time.deltaTime * animSpeed);
        }
    }

    private void OnHoverEnter(int targetIndex)
    {
       
        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
            returnCoroutine = null;
        }
        
        
        targetPosition = tabButtons[targetIndex].anchoredPosition;
    }

    private void OnHoverExit()
    {
        
        if (gameObject.activeInHierarchy)
        {
            returnCoroutine = StartCoroutine(ReturnToActiveDelayed());
        }
    }

    private IEnumerator ReturnToActiveDelayed()
    {
        
        yield return new WaitForSeconds(returnDelay);
        
        targetPosition = tabButtons[activeTabIndex].anchoredPosition;
    }

   
    public void SetActiveTab(int newIndex)
    {
        activeTabIndex = newIndex;
        OnHoverEnter(activeTabIndex); 
        
    }
}