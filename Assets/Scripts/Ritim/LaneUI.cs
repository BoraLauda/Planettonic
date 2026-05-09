using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LaneUI : MonoBehaviour
{
    [Header("Takım ve Tuş Ayarları")]
    public string teamName; 
    public KeyCode myKey; 
    
    [Header("Karakter Yönü")]
    public bool isLeftSideLane = true; 

    [Header("Hedef ve Hassasiyet")]
    public RectTransform hitTarget; 
    public Image targetImage;
    public float perfectHitTolerance = 30f; 
    public float goodHitTolerance = 80f;    
    public float missKayıpGıtmeSiniri = -100f; 

    [Header("Başarı Efektleri")]
    public float popScale = 1.3f; 
    public float popSpeed = 15f;  

    [Header("Hata Efektleri (Miss)")]
    public Color missColor = Color.red;
    public float shakeMagnitude = 10f; 
    public float missFeedbackDuration = 0.3f; 
    public float colorFadeDuration = 0.2f;

    public List<Arrows> activeNotes = new List<Arrows>();
    
    private Vector3 originalScale; 
    private Vector2 originalPos;
    private Color originalColor;
    private Coroutine activeMissCoroutine;
    private ArrowSpawner spawnerReference;

    void Start()
    {
        originalScale = hitTarget.localScale;
        originalPos = hitTarget.anchoredPosition;
        spawnerReference = FindFirstObjectByType<ArrowSpawner>();
        
        if (targetImage != null)
        {
            originalColor = targetImage.color;
        }
    }

    void Update()
    {
        hitTarget.localScale = Vector3.Lerp(hitTarget.localScale, originalScale, Time.deltaTime * popSpeed);

        if (Input.GetKeyDown(myKey))
        {
            CheckHit();
        }

        if (activeNotes.Count > 0)
        {
            Arrows targetNote = activeNotes[0];
            float localY = hitTarget.InverseTransformPoint(targetNote.GetComponent<RectTransform>().position).y;

            if (localY < missKayıpGıtmeSiniri)
            {
                HitNote(targetNote, "Miss"); 
            }
        }
    }

    void CheckHit()
    {
        if (activeNotes.Count > 0)
        {
            Arrows targetNote = activeNotes[0]; 
            float distance = Mathf.Abs(hitTarget.InverseTransformPoint(targetNote.GetComponent<RectTransform>().position).y);

           
            if (distance <= perfectHitTolerance)
            {
                HitNote(targetNote, "Perfect"); 
            }
            else if (distance <= goodHitTolerance)
            {
                HitNote(targetNote, "Good"); 
            }
            else
            {
                HitNote(targetNote, "Miss"); 
            }
        }
        else
        {
            TriggerMissFeedback();
        }
    }

    void HitNote(Arrows noteToHit, string hitQuality)
    {
        if (hitQuality == "Perfect" || hitQuality == "Good")
        {
            hitTarget.localScale = originalScale * popScale;
            
            ArrowSpawner.currentGlobalCombo++;
            if (ArrowSpawner.currentGlobalCombo > ArrowSpawner.maxGlobalCombo)
            {
                ArrowSpawner.maxGlobalCombo = ArrowSpawner.currentGlobalCombo;
            }

            if (Combo.Instance != null) 
            {
                Combo.Instance.AddCombo();
            }

          
            if (spawnerReference != null)
            {
                int basePoints = (hitQuality == "Perfect") ? 10 : 5;
                spawnerReference.AddScore(basePoints, isLeftSideLane);
            }
          
        }
        else // Miss
        {
            TriggerMissFeedback(); 
        }

        activeNotes.Remove(noteToHit);
        Destroy(noteToHit.gameObject);
    }

    void TriggerMissFeedback()
    {
        ArrowSpawner.currentGlobalCombo = 0;

        if (Combo.Instance != null)
        {
            Combo.Instance.ResetCombo();
        }

        if (activeMissCoroutine != null)
        {
            StopCoroutine(activeMissCoroutine);
            hitTarget.anchoredPosition = originalPos;
        }
        activeMissCoroutine = StartCoroutine(MissFeedbackRoutine());
    }

    IEnumerator MissFeedbackRoutine()
    {
        if (targetImage != null) targetImage.color = missColor;

        float elapsed = 0f;

        while (elapsed < missFeedbackDuration)
        {
            float randomX = Random.Range(-shakeMagnitude, shakeMagnitude);
            hitTarget.anchoredPosition = new Vector2(originalPos.x + randomX, originalPos.y);
            
            elapsed += Time.deltaTime;
            yield return null;
        }

        hitTarget.anchoredPosition = originalPos;

        elapsed = 0f;
        while (elapsed < colorFadeDuration)
        {
            if (targetImage != null)
            {
                targetImage.color = Color.Lerp(missColor, originalColor, elapsed / colorFadeDuration);
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (targetImage != null) targetImage.color = originalColor;
    }
}