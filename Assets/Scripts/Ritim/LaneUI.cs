using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LaneUI : MonoBehaviour
{
    [Header("Takım ve Tuş Ayarları")]
    public string teamName; 
    public KeyCode myKey; 
    
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

    void Start()
    {
        originalScale = hitTarget.localScale;
        originalPos = hitTarget.anchoredPosition;
        
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
                Debug.Log("MISS! Ok akıp gitti.");
                HitNote(targetNote, false); 
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
                Debug.Log("PERFECT! Combo x1 - Mesafe: " + distance);
                HitNote(targetNote, true); 
            }
            else if (distance <= goodHitTolerance)
            {
                Debug.Log("GOOD! - Mesafe: " + distance);
                HitNote(targetNote, true); 
            }
            else
            {
                Debug.Log("MISS! Çok erken veya geç bastın. Mesafe: " + distance);
                HitNote(targetNote, false); 
            }
        }
        else
        {
            TriggerMissFeedback();
        }
    }

    void HitNote(Arrows noteToHit, bool isSuccess)
    {
        if (isSuccess)
        {
            hitTarget.localScale = originalScale * popScale;
            
            if (Combo.Instance != null) 
            {
                Combo.Instance.AddCombo();
            }
        }
        else
        {
            TriggerMissFeedback(); 
        }

        activeNotes.Remove(noteToHit);
        Destroy(noteToHit.gameObject);
    }

    void TriggerMissFeedback()
    {
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