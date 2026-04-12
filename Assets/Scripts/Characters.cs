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
    
    [Header("GörselBüyük")]
    public Sprite portraitImage; 
        
    //[Header("Detay Metinleri")]
    //[TextArea(3, 10)] 
    //public string profileList;
    
    public List<LocationPreference> locationPreferences;
    
    //[TextArea(3, 10)] 
    //public string info;
    
    public Sprite nameImage;  
    public Sprite listImage; // boy kilo vs.
    public Sprite infoImage; //özet
    
    [Header("Sevilmeyen Yemekler")]
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
