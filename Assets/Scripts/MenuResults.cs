using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CoupleMenuOutcome
{
    public string ciftAdi = "Yeni Çift"; // Inspector'da karışmasın diye
    public Characters characterA;
    public Characters characterB;
    
    [Header("Bu Çifte Özel Senaryolar")]
    public DialogueDataları successScenario;      // Hatasız sipariş verirlerse
    public DialogueDataları continuationScenario; // Yemekten sonraki asıl devam diyaloğu
}

public class MenuResults : MonoBehaviour
{
    public float delayBetweenEffects = 0.8f; 
    public float turnDelay = 1.0f;          
    public float waitBeforeFinish = 1.5f;
    
    [Header("Feedback Objeleri (Mutlu Efektler)")]
    public GameObject[] leftFeedbacks;    
    public GameObject[] rightFeedbacks;   

    [Header("Slotlar (Tabaklar)")]
    public Image[] leftSlots;     
    public Image[] rightSlots;   
    
    public Foods[] allMenuFoods;
    
    public float starReward = 0.5f; 
    public int heartReward = 10;
    
    [Header("Çiftlere Özel Menü Sonuçları")]
    public List<CoupleMenuOutcome> coupleOutcomes; 
    
    private brainDate dateManager;
    private bool isProcessing = false;
    
    void Start()
    {
        dateManager = FindFirstObjectByType<brainDate>();
        
        foreach(var obj in leftFeedbacks) if(obj) obj.SetActive(false);
        foreach(var obj in rightFeedbacks) if(obj) obj.SetActive(false);
    }

    public void ChecktheOrdersFinish()
    {
        if (AreSlotsEmpty(leftSlots) || AreSlotsEmpty(rightSlots))
        {
            return;
        }

        StartCoroutine(ProcessResultsWithEffects());
    }

    IEnumerator ProcessResultsWithEffects()
    {
        isProcessing = true; 
      
        for (int i = 0; i < rightSlots.Length; i++)
        {
            FoodType type = GetFoodTypeFromSprite(rightSlots[i].sprite);
            
            if (!IsFoodHated(DateSettings.rightChar, type))
            {
                if (rightFeedbacks != null && i < rightFeedbacks.Length && rightFeedbacks[i] != null) 
                {
                    rightFeedbacks[i].SetActive(true);
                }
            }
            
            yield return new WaitForSecondsRealtime(delayBetweenEffects);
        }

        yield return new WaitForSecondsRealtime(turnDelay);

        for (int i = 0; i < leftSlots.Length; i++)
        {
            FoodType type = GetFoodTypeFromSprite(leftSlots[i].sprite);

            if (!IsFoodHated(DateSettings.leftChar, type))
            {
                if (leftFeedbacks != null && i < leftFeedbacks.Length && leftFeedbacks[i] != null) 
                {
                    leftFeedbacks[i].SetActive(true);
                }
            }
            
            yield return new WaitForSecondsRealtime(delayBetweenEffects);
        }

        yield return new WaitForSecondsRealtime(waitBeforeFinish); 

        FinalizeLogic();
    }

    void FinalizeLogic()
    {
        CalculateRewards();

        List<DialogueDataları> finalSequence = new List<DialogueDataları>();
        bool anyMistake = false;

        // Sağdaki karakterin nefret ettiği yemek var mı?
        foreach (Image slot in rightSlots)
        {
            FoodType type = GetFoodTypeFromSprite(slot.sprite);
            DialogueDataları failReaction = GetHatedFoodReaction(DateSettings.rightChar, type);
            
            if (failReaction != null) 
            {
                if (!finalSequence.Contains(failReaction)) 
                {
                    finalSequence.Add(failReaction);
                    anyMistake = true;
                }
            }
        }

        // Soldaki karakterin nefret ettiği yemek var mı?
        foreach (Image slot in leftSlots)
        {
            FoodType type = GetFoodTypeFromSprite(slot.sprite);
            DialogueDataları failReaction = GetHatedFoodReaction(DateSettings.leftChar, type);
            
            if (failReaction != null)
            {
                if (!finalSequence.Contains(failReaction))
                {
                    finalSequence.Add(failReaction);
                    anyMistake = true;
                }
            }
        }

        // MASADAKİ ÇİFTİ BUL VE ONLARA ÖZEL SENARYOLARI ÇEK
        DialogueDataları activeSuccess = null;
        DialogueDataları activeContinuation = null;

        foreach (var outcome in coupleOutcomes)
        {
            if ((DateSettings.leftChar == outcome.characterA && DateSettings.rightChar == outcome.characterB) ||
                (DateSettings.leftChar == outcome.characterB && DateSettings.rightChar == outcome.characterA))
            {
                activeSuccess = outcome.successScenario;
                activeContinuation = outcome.continuationScenario;
                break;
            }
        }

        // Hata yoksa başarı diyaloğunu ekle
        if (!anyMistake && activeSuccess != null)
        {
            finalSequence.Add(activeSuccess);
        }
        
        // Her halükarda devam diyaloğunu ekle
        if (activeContinuation != null)
        {
            finalSequence.Add(activeContinuation);
        }

        FinishGame(finalSequence);
    }
   
    void CalculateRewards()
    {
        float leftStars = 0;
        int leftHearts = 0;
        foreach (Image slot in leftSlots)
        {
            FoodType type = GetFoodTypeFromSprite(slot.sprite);
            if (!IsFoodHated(DateSettings.leftChar, type))
            {
                leftStars += starReward;
                leftHearts += heartReward;
            }
        }
        if(dateManager != null && (leftStars > 0 || leftHearts > 0))
            dateManager.AddReward(leftStars, leftHearts, TargetCharacter.Left); 

        float rightStars = 0;
        int rightHearts = 0;
        foreach (Image slot in rightSlots)
        {
            FoodType type = GetFoodTypeFromSprite(slot.sprite);
            if (!IsFoodHated(DateSettings.rightChar, type))
            {
                rightStars += starReward;
                rightHearts += heartReward;
            }
        }
        if(dateManager != null && (rightStars > 0 || rightHearts > 0))
            dateManager.AddReward(rightStars, rightHearts, TargetCharacter.Right);
    }
    
    bool IsFoodHated(Characters character, FoodType food)
    {
        if (character == null || character.hatedFoods == null) return false;
        
        foreach (FoodReaction fr in character.hatedFoods)
        {
            if (fr.food == food) return true;
        }
        return false;
    }
    
    DialogueDataları GetHatedFoodReaction(Characters character, FoodType food)
    {
        if (character == null || character.hatedFoods == null) return null;
        
        foreach (FoodReaction fr in character.hatedFoods)
        {
            if (fr.food == food) return fr.reactionScenario;
        }
        return null;
    }
    
    FoodType GetFoodTypeFromSprite(Sprite slotSprite)
    {
        foreach (Foods food in allMenuFoods)
        {
            if (food.GetComponent<Image>().sprite == slotSprite)
            {
                return food.type;
            }
        }
        return FoodType.SafeFood;
    }

    bool AreSlotsEmpty(Image[] slots)
    {
        foreach (Image slot in slots)
        {
            if (slot.sprite == null || GetFoodTypeFromSprite(slot.sprite) == FoodType.SafeFood)
            {
                if(slot.sprite == null) return true; 
            }
        }
        return false;
    }

    void FinishGame(List<DialogueDataları> sequence)
    {
        if (dateManager != null)
        {
            dateManager.ResumeFromMiniGame(sequence);
        }
    }
}