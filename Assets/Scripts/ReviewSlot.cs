using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReviewSlot : MonoBehaviour
{
    [Header("Yazılar")]
    public TMP_Text char1NameText;
    public TMP_Text char2NameText;
    public TMP_Text char1CommentText;
    public TMP_Text char2CommentText;
    public TMP_Text resultText; 
    
    [Header("Görseller")]
    public Image char1Image;
    public Image char2Image;
    
    [Header("Yıldız Sistemi")]
    public Transform char1StarsContainer;
    public Transform char2StarsContainer;
    public GameObject fullStarPrefab;
    public GameObject halfStarPrefab;
    public GameObject emptyStarPrefab; 

    public void Setup(DateReview data, Sprite profile1Icon, Sprite profile2Icon)
    {
        char1NameText.text = data.char1Name;
        char2NameText.text = data.char2Name;
        char1CommentText.text = "\"" + data.char1Comment + "\"";
        char2CommentText.text = "\"" + data.char2Comment + "\"";

        if (profile1Icon != null) char1Image.sprite = profile1Icon;
        if (profile2Icon != null) char2Image.sprite = profile2Icon;

        if (data.isSuccess)
        {
            resultText.text = "SUCCESS";
           
        }
        else
        {
            resultText.text = "FAILED";
        }

        DrawStars(char1StarsContainer, data.char1Stars);
        DrawStars(char2StarsContainer, data.char2Stars);
    }

    void DrawStars(Transform container, float starCount)
    {
        foreach (Transform child in container) Destroy(child.gameObject);
        
        int fullCount = Mathf.FloorToInt(starCount); 
        bool needsHalf = (starCount - fullCount) >= 0.5f;

       
        for (int i = 0; i < 5; i++) 
        {
            if (i < fullCount)
            {
               
                Instantiate(fullStarPrefab, container);
            }
            else if (i == fullCount && needsHalf)
            {
                
                Instantiate(halfStarPrefab, container);
            }
            else
            {
               
                if (emptyStarPrefab != null)
                {
                    Instantiate(emptyStarPrefab, container);
                }
            }
        }
    }
}