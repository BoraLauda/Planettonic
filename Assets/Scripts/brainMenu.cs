using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class brainMenu : MonoBehaviour
{
    [Header("Karakter İkonları")]
    public Image solKarakterIkonu;
    public Image sagKarakterIkonu;

    [Header("Slot ve Buton Ayarları")]
    public List<DropSlots> allSlots;
    public Button confirmButton;
    public Button clearButton;
    
    [Header("Ses Ayarları")]
    public AudioSource audioSource;
    public AudioClip pickUpSound; 
    public AudioClip dropSound;
    
    void Start()
    {
        SetButtonsInteractable(false);
        
        if (clearButton != null)
        {
            clearButton.onClick.AddListener(OnClearAllClicked);
        }
        
        if(allSlots == null || allSlots.Count == 0)
        {
            Debug.LogError("HATA");
        }

        if (DateSettings.leftChar != null && solKarakterIkonu != null)
        {
            if (DateSettings.leftChar.menuMinigameIkonu != null)
                solKarakterIkonu.sprite = DateSettings.leftChar.menuMinigameIkonu;
            else
                solKarakterIkonu.sprite = DateSettings.leftChar.profileIcon; 
        }

        if (DateSettings.rightChar != null && sagKarakterIkonu != null)
        {
            if (DateSettings.rightChar.menuMinigameIkonu != null)
                sagKarakterIkonu.sprite = DateSettings.rightChar.menuMinigameIkonu;
            else
                sagKarakterIkonu.sprite = DateSettings.rightChar.profileIcon; 
        }
    }
    
    public void PlayPickUpSound()
    {
        if (audioSource != null && pickUpSound != null)
            audioSource.PlayOneShot(pickUpSound);
    }

    public void PlayDropSound()
    {
        if (audioSource != null && dropSound != null)
            audioSource.PlayOneShot(dropSound);
    }

    public void CheckAllSlots()
    {
        int filledCount = 0;
        foreach (DropSlots slot in allSlots)
        {
            if (slot == null) continue;
            if (slot.isFull)
            {
                filledCount++;
            }
        }
        
        Debug.Log("Toplam Slot: " + allSlots.Count + " | Dolu Olan: " + filledCount);
        
        if (filledCount == 4)
        {
            SetButtonsInteractable(true);
            Debug.Log("Slotlar doldu butonlar gözküküsn ");
        }
        else
        {
            SetButtonsInteractable(false);
        }
    }
    
    void SetButtonsInteractable(bool isInteractable)
    {
        if (confirmButton != null) confirmButton.interactable = isInteractable;
        if (clearButton != null) clearButton.interactable = isInteractable;
    }
    
    public void OnClearAllClicked()
    {
        foreach (DropSlots slot in allSlots)
        {
            slot.ResetSlot();
        }
        
        CheckAllSlots();
    }
}