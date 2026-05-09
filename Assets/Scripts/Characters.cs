using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewProfile", menuName = "Matchmaker/Character Profile")]
public class Characters : ScriptableObject
{
    [Header("Kimlik")]
    public string characterName; 

    [Header("Görsel")]
    public Sprite profileIcon;
    public Sprite dodgeTheQuestionIkonu;
    public Sprite menuMinigameIkonu; 
    public Sprite portraitImage; 
        
    [Header("Profil Ekranı Metinleri")]
    public string fullName;             
    public string demographicsInfo;      
    public string relationshipGoals;    
    public string hobbies;               
    public string foodPreferenceText;    
    
    [TextArea(3, 5)] 
    public string quoteText;         
    
    [Header("Mekan ve Yemek Tercihleri")]
    public List<LocationPreference> locationPreferences;
    public List<FoodReaction> hatedFoods; 

    [Header("Ice Breaker Soruları")]
    public List<DialogueDataları> iceBreakerGood;
    public List<DialogueDataları> iceBreakerMid;
    public List<DialogueDataları> iceBreakerBad;

    [Header("Dodge Oyunu Soruları")]
    public List<DialogueDataları> dodgeQuestions;
}

[System.Serializable]
public class FoodReaction
{
    public FoodType food; 
    public DialogueDataları reactionScenario; 
}

[System.Serializable]
public class LocationPreference
{
    public string locationName; 
    public float bonusStars;    
}