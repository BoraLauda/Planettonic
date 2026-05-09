using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System.Collections.Generic;

[System.Serializable]
public class NoteItem
{
    public string subtitleName; 
    public Sprite noteContentSprite; 
}

[System.Serializable]
public class NoteCategory
{
    public string categoryName; 
    public List<NoteItem> noteItems; 
}

public class NotesManager : MonoBehaviour
{
    [Header("KÜÇÜK EKRAN (SMALL) PANELLERİ")]
    public GameObject panel_SubtitleList_Small;
    public GameObject panel_NoteReading_Small;
    public Image image_NoteContent_Small; 
    public GameObject btn_Back_Small; 
    public ScrollRect noteScrollRect_Small; 
    public Transform subtitleContainer_Small; 
    public GameObject subtitleButtonPrefab_Small; 
    public TMP_Text categoryTitleText_Small; 

    [Header("BÜYÜK EKRAN (LARGE) PANELLERİ")]
    public GameObject panel_SubtitleList_Large;
    public GameObject panel_NoteReading_Large;
    public Image image_NoteContent_Large; 
    public GameObject btn_Back_Large; 
    public ScrollRect noteScrollRect_Large; 
    public Transform subtitleContainer_Large; 
    public GameObject subtitleButtonPrefab_Large; 
    public TMP_Text categoryTitleText_Large; 

    [Header("Not Veritabanı")]
    public List<NoteCategory> allCategories;

    void Start()
    {
        if(panel_SubtitleList_Small) panel_SubtitleList_Small.SetActive(false);
        if(panel_NoteReading_Small) panel_NoteReading_Small.SetActive(false);
        if(btn_Back_Small) btn_Back_Small.SetActive(false);

        if(panel_SubtitleList_Large) panel_SubtitleList_Large.SetActive(false);
        if(panel_NoteReading_Large) panel_NoteReading_Large.SetActive(false);
        if(btn_Back_Large) btn_Back_Large.SetActive(false);
    }

    public void OpenCategory(int categoryIndex)
    {
        if (categoryIndex < 0 || categoryIndex >= allCategories.Count) return;

        if (subtitleContainer_Small != null)
        {
            foreach (Transform child in subtitleContainer_Small) Destroy(child.gameObject);
        }
        if (subtitleContainer_Large != null)
        {
            foreach (Transform child in subtitleContainer_Large) Destroy(child.gameObject);
        }

        NoteCategory selectedCategory = allCategories[categoryIndex];
        
        if (categoryTitleText_Small != null) 
            categoryTitleText_Small.text = selectedCategory.categoryName;
            
        if (categoryTitleText_Large != null) 
            categoryTitleText_Large.text = selectedCategory.categoryName;

        foreach (NoteItem item in selectedCategory.noteItems)
        {
            if (subtitleButtonPrefab_Small != null && subtitleContainer_Small != null)
            {
                GameObject newBtnSmall = Instantiate(subtitleButtonPrefab_Small, subtitleContainer_Small);
                TMP_Text btnTextSmall = newBtnSmall.GetComponentInChildren<TMP_Text>();
                if (btnTextSmall != null) btnTextSmall.text = item.subtitleName;

                Button btnSmall = newBtnSmall.GetComponent<Button>();
                btnSmall.onClick.AddListener(() => OpenNote(item.noteContentSprite));
            }

            if (subtitleButtonPrefab_Large != null && subtitleContainer_Large != null)
            {
                GameObject newBtnLarge = Instantiate(subtitleButtonPrefab_Large, subtitleContainer_Large);
                TMP_Text btnTextLarge = newBtnLarge.GetComponentInChildren<TMP_Text>();
                if (btnTextLarge != null) btnTextLarge.text = item.subtitleName;

                Button btnLarge = newBtnLarge.GetComponent<Button>();
                btnLarge.onClick.AddListener(() => OpenNote(item.noteContentSprite));
            }
        }

        if(panel_SubtitleList_Small) panel_SubtitleList_Small.SetActive(true);
        if(panel_NoteReading_Small) panel_NoteReading_Small.SetActive(false);
        if(btn_Back_Small) btn_Back_Small.SetActive(false);

        if(panel_SubtitleList_Large) panel_SubtitleList_Large.SetActive(true);
        if(panel_NoteReading_Large) panel_NoteReading_Large.SetActive(false);
        if(btn_Back_Large) btn_Back_Large.SetActive(false);
    }

    public void OpenNote(Sprite noteSprite)
    {
        if (noteSprite == null) return;

        if (image_NoteContent_Small != null)
        {
            image_NoteContent_Small.sprite = noteSprite;
            AdjustAspectRatio(image_NoteContent_Small, noteSprite);
        }

        if (image_NoteContent_Large != null)
        {
            image_NoteContent_Large.sprite = noteSprite;
            AdjustAspectRatio(image_NoteContent_Large, noteSprite);
        }

        if(panel_SubtitleList_Small) panel_SubtitleList_Small.SetActive(false);
        if(panel_NoteReading_Small) panel_NoteReading_Small.SetActive(true);
        if(btn_Back_Small) btn_Back_Small.SetActive(true);

        if(panel_SubtitleList_Large) panel_SubtitleList_Large.SetActive(false);
        if(panel_NoteReading_Large) panel_NoteReading_Large.SetActive(true);
        if(btn_Back_Large) btn_Back_Large.SetActive(true);

        Canvas.ForceUpdateCanvases(); 
        if (noteScrollRect_Small != null) noteScrollRect_Small.verticalNormalizedPosition = 1f; 
        if (noteScrollRect_Large != null) noteScrollRect_Large.verticalNormalizedPosition = 1f; 
    }

    public void OnBackButtonClicked()
    {
        if(panel_SubtitleList_Small) panel_SubtitleList_Small.SetActive(true);
        if(panel_NoteReading_Small) panel_NoteReading_Small.SetActive(false);
        if(btn_Back_Small) btn_Back_Small.SetActive(false);

        if(panel_SubtitleList_Large) panel_SubtitleList_Large.SetActive(true);
        if(panel_NoteReading_Large) panel_NoteReading_Large.SetActive(false);
        if(btn_Back_Large) btn_Back_Large.SetActive(false);
    }

    private void AdjustAspectRatio(Image img, Sprite spr)
    {
        AspectRatioFitter arf = img.GetComponent<AspectRatioFitter>();
        if (arf == null) 
        {
            arf = img.gameObject.AddComponent<AspectRatioFitter>();
        }
        arf.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
        arf.aspectRatio = spr.rect.width / spr.rect.height;
    }
}