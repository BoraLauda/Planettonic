using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "DatingGame/Market Item")]
public class ItemData : ScriptableObject
{
    public string itemName; 
    public int price;      
    public Sprite cardImage; 
}