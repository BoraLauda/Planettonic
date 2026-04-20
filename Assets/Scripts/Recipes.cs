using UnityEngine;

public class Recipes : MonoBehaviour
{
    public GameObject openBookPanel; 
    public GameObject[] recipePages; 

    void Start()
    {
       
        if (openBookPanel != null) openBookPanel.SetActive(false);
    }

    public void ToggleBook()
    {
        if (openBookPanel == null) return;
        
        bool currentlyActive = openBookPanel.activeSelf;
        openBookPanel.SetActive(!currentlyActive);
        
        if (!currentlyActive)
        {
            ShowPage(0);
        }
    }

    public void ShowPage(int index)
    {
        if (recipePages == null) return;

        for (int i = 0; i < recipePages.Length; i++)
        {
            if (recipePages[i] != null)
                recipePages[i].SetActive(i == index);
        }
    }
}