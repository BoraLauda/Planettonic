using System.Collections;
using UnityEngine;

public class StarAnimation : MonoBehaviour
{
    public float duration = 1.5f; 
    public float startOffsetX = -1500f; 
    public float endOffsetX = 1500f;
    public float randomDelayMax = 0.5f;
    
    
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); 

    private RectTransform rectTransform;
    private Vector2 targetPosition;

    void Start()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        
        rectTransform = GetComponent<RectTransform>();
        
        targetPosition = rectTransform.anchoredPosition;
        
        Vector2 startPosition = targetPosition + new Vector2(startOffsetX, 0);
        rectTransform.anchoredPosition = startPosition;
        
        StartCoroutine(FlyIn(startPosition, targetPosition));
    }

    IEnumerator FlyIn(Vector2 startPos, Vector2 targetPos)
    {
        
        float delay = Random.Range(0f, randomDelayMax);
        yield return new WaitForSeconds(delay);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration; 
            
            float curveValue = moveCurve.Evaluate(t);
            
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, curveValue);
            
            yield return null; 
        }
        
        rectTransform.anchoredPosition = targetPos;
    }
    
    public void PlayExitAnimation()
    {
        StopAllCoroutines(); 
        Vector2 currentPos = rectTransform.anchoredPosition;
        Vector2 exitPosition = targetPosition + new Vector2(endOffsetX, 0); 
        
        StartCoroutine(FlyOut(currentPos, exitPosition));
    }

    IEnumerator FlyOut(Vector2 startPos, Vector2 targetPos)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration; 
            float curveValue = moveCurve.Evaluate(t);
            
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, curveValue);
            yield return null; 
        }
        rectTransform.anchoredPosition = targetPos;
    }
}