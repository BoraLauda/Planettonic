using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class brainMenu : MonoBehaviour

{
    public List<DropSlots> allSlots;
    
    public Button confirmButton;
    public Button clearButton;
    
    public AudioSource audioSource;
    public AudioClip pickUpSound; 
    public AudioClip dropSound;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
            if (slot == null) continue;//DENEME AMAÇLI SİL
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
