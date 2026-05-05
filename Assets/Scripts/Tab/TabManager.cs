using UnityEngine;

public class TabManager : MonoBehaviour
{
    [Header("Sekme Sayfaları (Empty Object'ler)")]
    public GameObject pageProfiles;
    public GameObject pagePlaces;
    public GameObject pageReviews;

    [Header("APPler Bağlantısı")]
    public APPler applerScript; 

    void Start()
    {
        OpenProfilesTab();
    }

    public void OpenProfilesTab()
    {
        if(pageProfiles != null) pageProfiles.SetActive(true);
        if(pagePlaces != null) pagePlaces.SetActive(false);
        if(pageReviews != null) pageReviews.SetActive(false);
    }

    public void OpenPlacesTab()
    {
        if(pageProfiles != null) pageProfiles.SetActive(false);
        if(pagePlaces != null) pagePlaces.SetActive(true);
        if(pageReviews != null) pageReviews.SetActive(false);
    }

    public void OpenReviewsTab()
    {
        if(pageProfiles != null) pageProfiles.SetActive(false);
        if(pagePlaces != null) pagePlaces.SetActive(false);
        if(pageReviews != null) pageReviews.SetActive(true);
        
        if(applerScript != null)
        {
            applerScript.LoadReviews();
        }
        else
        {
            Debug.LogWarning("TabManager içinde APPler scripti bağlı değil");
        }
    }
}