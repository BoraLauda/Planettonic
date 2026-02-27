using UnityEngine;
using UnityEngine.UI;

public class CharacterSlots : MonoBehaviour
{
    [Header("Veri")]
    public Characters myProfile; 

    [Header("Görsel Bileşenler")]
    public Image myImage;     
    public Button myButton;   

    
    void Start()
    {
        if (myProfile != null)
        {
            myImage.sprite = myProfile.profileIcon;
        }
    }

    
    public void UpdateSlotState(Characters selectedLeft, Characters selectedRight)
    {
        // Eğer ben zaten seçiliysem
        if (myProfile != null && (myProfile == selectedLeft || myProfile == selectedRight))
        {
            myButton.interactable = false; // Tıklanamaz ol
            myImage.color = Color.gray;    // Gri ol
        }
        else
        {
            myButton.interactable = true;  // Normal ol
            myImage.color = Color.white;   // Beyaz ol
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