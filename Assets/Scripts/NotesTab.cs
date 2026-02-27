using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NotesTab : MonoBehaviour
{
    
    public List<GameObject> categoryPages; 
    
    public List<Button> tabButtons; 

    public Color activeTabColor = Color.white; 
    public Color inactiveTabColor = Color.gray; 

    void Start()
    {
        OpenTab(0);
    }
    
    public void OpenTab(int tabIndex)
    {
        for (int i = 0; i < categoryPages.Count; i++)
        {
            if (categoryPages[i] != null)
            {
                categoryPages[i].SetActive(i == tabIndex);
            }
        }
        
        for (int i = 0; i < tabButtons.Count; i++)
        {
            if (tabButtons[i] != null)
            {
                Image btnImage = tabButtons[i].GetComponent<Image>();
                if (btnImage != null)
                {
                    btnImage.color = (i == tabIndex) ? activeTabColor : inactiveTabColor;
                }
            }
        }
    }
}
