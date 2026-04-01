using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
  
    public string startSceneName = "Desktop"; 
    
   
    public StarAnimation starAnimator; 
    
    
    public RectTransform buttonsContainer; 
    public float buttonExitOffsetY = -1000f; 
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float entranceDuration = 1.0f;
    
    public RectTransform titleContainer; 
    public float titleExitOffsetY = 1000f;

   
    public Button loadGameButton; 

    void Start()
    {
        if (loadGameButton != null)
        {
            bool hasSave = (PlayerPrefs.GetInt("HasSave", 0) == 1);
            loadGameButton.interactable = hasSave; 
            
            CanvasGroup cg = loadGameButton.GetComponent<CanvasGroup>();
            if (cg == null) 
            {
                cg = loadGameButton.gameObject.AddComponent<CanvasGroup>();
            }
            
           
            cg.alpha = hasSave ? 1f : 0.8f; 
           
        }
        
        if (buttonsContainer != null)
        {
            StartCoroutine(AnimateButtonsIn(entranceDuration));
        }
        
        if (titleContainer != null)
        {
            StartCoroutine(AnimateTitleIn(entranceDuration));
        }
    }

    public void NewGame()
    {
        Debug.Log("Yeni Oyun");
        
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("HasSave", 1); 
        PlayerPrefs.Save();

        StartTransition(); 
    }

    public void LoadGame()
    {
        Debug.Log("Kayıt");
        if (PlayerPrefs.GetInt("HasSave", 0) == 1)
        {
            StartTransition();
        }
    }
    
    private void StartTransition()
    {
        float beklemeSuresi = 1.5f;

        if (starAnimator != null)
        {
            starAnimator.PlayExitAnimation();
            beklemeSuresi = starAnimator.duration; 
        }

        if (buttonsContainer != null)
        {
            StartCoroutine(AnimateButtonsOut(beklemeSuresi));
        }

        if (titleContainer != null)
        {
            StartCoroutine(AnimateTitleOut(beklemeSuresi));
        }

       
        StartCoroutine(WaitAndLoadScene(startSceneName, beklemeSuresi));
    }
    
    IEnumerator AnimateTitleIn(float duration)
    {
        Vector2 targetPos = titleContainer.anchoredPosition;
        Vector2 startPos = targetPos + new Vector2(0, titleExitOffsetY); 
        
        titleContainer.anchoredPosition = startPos;
        
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float curveValue = moveCurve.Evaluate(t);
            
            titleContainer.anchoredPosition = Vector2.Lerp(startPos, targetPos, curveValue);
            yield return null;
        }
        titleContainer.anchoredPosition = targetPos;
    }

    IEnumerator AnimateTitleOut(float duration)
    {
        Vector2 startPos = titleContainer.anchoredPosition;
        Vector2 targetPos = startPos + new Vector2(0, titleExitOffsetY); 
        
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float curveValue = moveCurve.Evaluate(t);
            
            titleContainer.anchoredPosition = Vector2.Lerp(startPos, targetPos, curveValue);
            yield return null;
        }
        titleContainer.anchoredPosition = targetPos;
    }
    
    IEnumerator AnimateButtonsIn(float duration)
    {
        Vector2 targetPos = buttonsContainer.anchoredPosition;
        Vector2 startPos = targetPos + new Vector2(0, buttonExitOffsetY); 
        
        buttonsContainer.anchoredPosition = startPos;
        
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float curveValue = moveCurve.Evaluate(t);
            
            buttonsContainer.anchoredPosition = Vector2.Lerp(startPos, targetPos, curveValue);
            yield return null;
        }
        
        buttonsContainer.anchoredPosition = targetPos;
    }

    IEnumerator AnimateButtonsOut(float duration)
    {
        Vector2 startPos = buttonsContainer.anchoredPosition;
        Vector2 targetPos = startPos + new Vector2(0, buttonExitOffsetY); 
        
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float curveValue = moveCurve.Evaluate(t);
            
            buttonsContainer.anchoredPosition = Vector2.Lerp(startPos, targetPos, curveValue);
            yield return null;
        }
        buttonsContainer.anchoredPosition = targetPos;
    }

    IEnumerator WaitAndLoadScene(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    public void OpenOptions()
    {
        Debug.Log("Ayarlar menüsü açılacak");
    }

    public void QuitGame()
    {
        Debug.Log("Oyundan çıkılıyor");
        Application.Quit(); 
    }
}