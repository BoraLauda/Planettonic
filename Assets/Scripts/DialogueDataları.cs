using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "New Scenario", menuName = "Date System")]
public class DialogueDataları : ScriptableObject
{

    [Header("Mekan Bilgileri")]
    public string locationName; 
    public Sprite locationBackground;

    [Header("Senaryo Akışı")]
    public List<DialogueLine> allLines;
    public DialogueDataları nextScenario;
}

[System.Serializable]
public class DialogueLine
{
   
    public SpeakerSide side; 

    public string characterName;    
    public Sprite characterSprite;  

    [TextArea(3, 10)] 
    public string sentence;         

    public string eventTrigger;
    
    [Header("Seçimler (Opsiyonel)")]
    public List<ChoiceOption> choices; 
}

[System.Serializable]
public class ChoiceOption
{
    public string choices;      
    public DialogueDataları nextScenario; 
    public TargetCharacter target;
    public float starReward = 0; 
    public int heartReward = 0;
}


public enum SpeakerSide
{
    Left,       // Io
    Right,      // Elroi
    Counselor, 
    Chaperon    
}

public enum TargetCharacter
{
    Left,
    Right,
    Both,
    
    Io, //SONRA SİL    
    Elroi //SONRA SİL  
}
    
    

