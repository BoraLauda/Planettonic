using UnityEngine;
using UnityEngine.UI;
public class ButtonEffects : MonoBehaviour
{
    
    public Button[] buttons; 
    
    public float fadeSpeed = 10f;
    public float passiveAlpha = 0.5f; 

    private int selectedIndex = 0; 
    
    public void SelectButton(int index)
    {
        selectedIndex = index;
    }

    void Update()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] == null || buttons[i].image == null) continue;
            
            float targetAlpha = (i == selectedIndex) ? 1f : passiveAlpha;
            
            Color currentColor = buttons[i].image.color;
            
            float newAlpha = Mathf.Lerp(currentColor.a, targetAlpha, Time.deltaTime * fadeSpeed);
            
            buttons[i].image.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
        }
    }
}
