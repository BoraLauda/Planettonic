using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MenuResults : MonoBehaviour
{
    public float delayBetweenEffects = 0.8f; 
    public float turnDelay = 1.0f;          
    public float waitBeforeFinish = 1.5f;
    
    [Header("Feedback Objeleri")]
    public GameObject[] ioFeedbacks;    
    public GameObject[] elroiFeedbacks; 

    [Header("Slotlar")]
    public Image[] ioSlots;    
    public Image[] elroiSlots;
    
    public Foods[] allMenuFoods;
    
    public float starReward = 0.5f; 
    public int heartReward = 10;
    
    [Header("Senaryolar")]
    public DialogueDataları scenarioSuccess;      
    public DialogueDataları scenarioFail_Seafood;  
    public DialogueDataları scenarioFail_Wine;     
    public DialogueDataları scenarioFail_IoMeat;
    public DialogueDataları scenarioContinuation;
    
    private brainDate dateManager;
    private bool isProcessing = false;
    
    void Start()
    {
        dateManager = FindFirstObjectByType<brainDate>();
        
        foreach(var obj in ioFeedbacks) if(obj) obj.SetActive(false);
        foreach(var obj in elroiFeedbacks) if(obj) obj.SetActive(false);
    }

    public void ChecktheOrdersFinish()
    {
        if (AreSlotsEmpty(ioSlots) || AreSlotsEmpty(elroiSlots))
        {
            return;
        }

        StartCoroutine(ProcessResultsWithEffects());
    }

    IEnumerator ProcessResultsWithEffects()
    {
        isProcessing = true; 
        
        for (int i = 0; i < elroiSlots.Length; i++)
        {
            FoodType type = GetFoodTypeFromSprite(elroiSlots[i].sprite);
            
          
            if (type != FoodType.Seafood && type != FoodType.Wine && type != FoodType.Salad)
            {
                if (elroiFeedbacks != null && i < elroiFeedbacks.Length && elroiFeedbacks[i] != null) 
                {
                    elroiFeedbacks[i].SetActive(true);
                }
            }
            
            
            yield return new WaitForSecondsRealtime(delayBetweenEffects);
        }

        
        yield return new WaitForSecondsRealtime(turnDelay);

        
        for (int i = 0; i < ioSlots.Length; i++)
        {
            FoodType type = GetFoodTypeFromSprite(ioSlots[i].sprite);

            if (type != FoodType.Meat && type != FoodType.Seafood)
            {
                if (ioFeedbacks != null && i < ioFeedbacks.Length && ioFeedbacks[i] != null) 
                {
                    ioFeedbacks[i].SetActive(true);
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

        foreach (Image slot in elroiSlots)
        {
            FoodType type = GetFoodTypeFromSprite(slot.sprite);
            if (type == FoodType.Seafood)
            {
                if (!finalSequence.Contains(scenarioFail_Seafood))
                {
                    finalSequence.Add(scenarioFail_Seafood);
                    anyMistake = true;
                }
            }
            else if (type == FoodType.Wine)
            {
                if (!finalSequence.Contains(scenarioFail_Wine))
                {
                    finalSequence.Add(scenarioFail_Wine);
                    anyMistake = true;
                }
            }
        }

        foreach (Image slot in ioSlots)
        {
            FoodType type = GetFoodTypeFromSprite(slot.sprite);
            if (type == FoodType.Meat || type == FoodType.Seafood)
            {
                if (!finalSequence.Contains(scenarioFail_IoMeat))
                {
                    finalSequence.Add(scenarioFail_IoMeat);
                    anyMistake = true;
                }
            }
        }

        if (!anyMistake)
        {
            finalSequence.Add(scenarioSuccess);
        }
        
        if (scenarioContinuation != null)
        {
            finalSequence.Add(scenarioContinuation);
        }

        FinishGame(finalSequence);
    }
   
    void CalculateRewards()
    {
        float ioStars = 0;
        int ioHearts = 0;
        foreach (Image slot in ioSlots)
        {
            FoodType type = GetFoodTypeFromSprite(slot.sprite);
            if (type != FoodType.Meat && type != FoodType.Seafood)
            {
                ioStars += starReward;
                ioHearts += heartReward;
            }
        }
        if(dateManager != null && (ioStars > 0 || ioHearts > 0))
            dateManager.AddReward(ioStars, ioHearts, TargetCharacter.Io); 

        float elroiStars = 0;
        int elroiHearts = 0;
        foreach (Image slot in elroiSlots)
        {
            FoodType type = GetFoodTypeFromSprite(slot.sprite);
            if (type != FoodType.Seafood && type != FoodType.Wine && type != FoodType.Salad)
            {
                elroiStars += starReward;
                elroiHearts += heartReward;
            }
        }
        if(dateManager != null && (elroiStars > 0 || elroiHearts > 0))
            dateManager.AddReward(elroiStars, elroiHearts, TargetCharacter.Elroi);
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
