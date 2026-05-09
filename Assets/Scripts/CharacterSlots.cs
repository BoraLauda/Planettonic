using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharacterSlots : MonoBehaviour
{
    public Characters myProfile;
    public Image myImage;
    public Button myButton;
    public GameObject overrideLockedPrefab; 
    public List<GameObject> hideWhenLocked; 
    public GameObject planetIcon; 

    private GameObject currentLockedUI; 

    public void UpdateSlotState(Characters selectedLeft, Characters selectedRight, bool isUnlocked, GameObject defaultLockedPrefab, bool isGrayedOut = false)
    {
        if (currentLockedUI != null) Destroy(currentLockedUI);

        if (!isUnlocked)
        {
            myButton.interactable = false; 
            
            if (myImage != null) myImage.enabled = false; 
            
            foreach (GameObject obj in hideWhenLocked)
            {
                if (obj != null) obj.SetActive(false);
            }

            if (planetIcon != null) planetIcon.SetActive(false); 

            GameObject finalPrefabToUse = overrideLockedPrefab != null ? overrideLockedPrefab : defaultLockedPrefab;

            if (finalPrefabToUse != null)
            {
                currentLockedUI = Instantiate(finalPrefabToUse, transform);
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
        
        if (myImage != null)
        {
            myImage.enabled = true; 
            myImage.color = isGrayedOut ? Color.gray : Color.white;
            if (myProfile != null) myImage.sprite = myProfile.profileIcon; 
        }

        foreach (GameObject obj in hideWhenLocked)
        {
            if (obj != null) obj.SetActive(true);
        }

        if (planetIcon != null) planetIcon.SetActive(true); 

        if (myProfile != null && (myProfile == selectedLeft || myProfile == selectedRight))
        {
            myButton.interactable = false;
            if (myImage != null) myImage.color = Color.gray;
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