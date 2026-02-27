using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DesktopManager : MonoBehaviour
{
    public static DesktopManager instance;
    
    private static bool isGameSessionStarted = false; 

    [Header("Veriler")]
    public TMP_Text heartDisplay;    
    public GameObject lockedPanel;  
    public string dateSceneName = "DateScene";

    private int totalHearts;
    public bool isMarketUnlocked; 

    void Awake()
    {
        instance = this; 
        
        
        if (!isGameSessionStarted)
        {
            
            PlayerPrefs.DeleteAll();
            
            
            isGameSessionStarted = true;
        }
       
    }

    void Start()
    {
        if(lockedPanel) lockedPanel.SetActive(false); 
        LoadData();
    }

    void LoadData()
    {
        totalHearts = PlayerPrefs.GetInt("SavedHearts", 0);
        
       
        isMarketUnlocked = PlayerPrefs.GetInt("IsMarketUnlocked", 0) == 1;

        if (heartDisplay) heartDisplay.text = totalHearts.ToString();
    }

   
    public void ShowLockedWarning()
    {
        if(lockedPanel) 
        {
            lockedPanel.SetActive(true);
            lockedPanel.transform.SetAsLastSibling(); 
        }
    }
    
   
    public void GoToDateScene()
    {
        SceneManager.LoadScene(dateSceneName);
    }

    
    public void CloseLockedPanel()
    {
        if(lockedPanel) lockedPanel.SetActive(false);
    }
}