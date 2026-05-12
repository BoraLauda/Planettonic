using UnityEngine;
using System.Collections.Generic;


public enum GenderType { Kadin, Erkek }

[CreateAssetMenu(fileName = "NewProfile", menuName = "Matchmaker/Character Profile")]
public class Characters : ScriptableObject
{
    [Header("Kimlik")]
    public string characterName; 
    public GenderType cinsiyet; 

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
    
    public List<Sprite> profileRatingIcons;
    
    [TextArea(3, 5)] 
    public string quoteText;         
    
    [Header("Mekan ve Yemek Tercihleri")]
    public List<LocationPreference> locationPreferences;
    public List<FoodReaction> hatedFoods; 
    
    public string sevdigiKokteyl;
    
    public DialogueDataları kokteylSevmediDiyalogu;

    [Header("Ice Breaker Good (Hedefe Özel)")]
    public List<TargetedDialogue> iceBreakerGood;

    [Header("Ice Breaker Mid (Hedefe Özel)")]
    public List<TargetedDialogue> iceBreakerMid;

    [Header("Ice Breaker Bad (Hedefe Özel)")]
    public List<TargetedDialogue> iceBreakerBad;

    [Header("Dodge Questions (Hedefe Özel)")]
    public List<TargetedDialogue> dodgeQuestions;
    
    [Header("2. Dodge Questions (Hedefe Özel)")]
    public List<TargetedDialogue> curveDodgeQuestions;
}

[System.Serializable]
public class TargetedDialogue
{
    [Header("Kime Söylenecek/Sorulacak?")]
    public Characters targetCharacter; 

    [Header("Diyalog Dosyası")]
    public DialogueDataları diyalogDosyasi; 
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