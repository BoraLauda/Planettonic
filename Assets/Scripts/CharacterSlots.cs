using UnityEngine;
using UnityEngine.UI;

public class CharacterSlots : MonoBehaviour
{
    public Characters myProfile;
    public Image myImage;
    public Button myButton;
    
    [Header("UI Görsel Bileşenleri")]
    public GameObject planetIcon; 

    private GameObject currentLockedUI; 

    
    public void UpdateSlotState(Characters selectedLeft, Characters selectedRight, bool isUnlocked, GameObject lockedPrefab, bool isGrayedOut = false)
    {
        if (currentLockedUI != null) Destroy(currentLockedUI);

        if (!isUnlocked)
        {
            myButton.interactable = false;
            myImage.enabled = false; 

            if (planetIcon != null) planetIcon.SetActive(false); 

            if (lockedPrefab != null)
            {
                currentLockedUI = Instantiate(lockedPrefab, transform);
                
                RectTransform rt = currentLockedUI.GetComponent<RectTransform>();
                if(rt != null)
                {
                    rt.anchoredPosition = Vector2.zero;
                    rt.localPosition = Vector3.zero;
                    rt.localScale = Vector3.one; 
                }
            }
            return;
        }
        
        myButton.interactable = !isGrayedOut;
        myImage.enabled = true; 
        
        if (myProfile != null) myImage.sprite = myProfile.profileIcon;
        
        myImage.color = isGrayedOut ? Color.gray : Color.white;

        if (planetIcon != null) planetIcon.SetActive(true); 

        if (myProfile != null && (myProfile == selectedLeft || myProfile == selectedRight))
        {
            myButton.interactable = false;
            myImage.color = Color.gray;
        }
    }

    public void OnClicked()
    {
        if (myProfile != null)
        {
            FindObjectOfType<APPler>().OnCandidateSelected(myProfile);
        }
    }
}