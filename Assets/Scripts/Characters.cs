using UnityEngine;


[CreateAssetMenu(fileName = "NewProfile", menuName = "Matchmaker/Character Profile")]
public class Characters : ScriptableObject
{
    [Header("Kimlik")]
    public string characterName; 
    

    [Header("Görsel")]
    public Sprite profileIcon;
    
    [Header("GörselBüyük")]
    public Sprite portraitImage; 
        
    [Header("Detay Metinleri")]
    [TextArea(3, 10)] 
    public string profileList;
    
    
    [TextArea(3, 10)] 
    public string info;
    
    
}