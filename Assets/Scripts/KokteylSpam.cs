using UnityEngine;
using UnityEngine.UI;

public class KokteylSpam : MonoBehaviour
{
    public Image progressBar;
    public RectTransform shakerToShake; 
    
    [Header("Oyun Modu")]
    public bool buEkranStirringMi = false;
    
    [Header("Tuş Ayarları")]
    public KeyCode key1 = KeyCode.W; 
    public KeyCode key2 = KeyCode.S; 
    
    [Header("İlerleme Ayarları")]
    public float progressPerPress = 10f;
    public float maxProgress = 100f;
    public float decayRate = 15f; 

    [Header("Sallanma/Dönme Ayarları")]
    public float shakeIntensity = 30f;  
    public float shakeSmoothness = 15f; 

    private float currentProgress = 0f;
    private KeyCode lastPressedKey = KeyCode.None;
    private bool isFinished = false;

    private float currentShakeOffset = 0f; 
    private float targetShakeOffset = 0f;  
    private float timeSinceLastPress = 0f; 
    
    private Vector2 initialShakerPosition; 
    private Quaternion initialRotation; 

    void Start()
    {
        if (shakerToShake != null)
        {
            initialShakerPosition = shakerToShake.anchoredPosition;
            initialRotation = shakerToShake.localRotation;
        }
    }
    
    void OnEnable()
    {
        isFinished = false;
        currentProgress = 0f;
        lastPressedKey = KeyCode.None;
        if (progressBar != null) progressBar.fillAmount = 0f;
    }

    void Update()
    {
        if (isFinished) return;

        if (currentProgress > 0)
        {
            currentProgress -= decayRate * Time.deltaTime;
            if (currentProgress < 0) currentProgress = 0;
        }

        timeSinceLastPress += Time.deltaTime;

        if (Input.GetKeyDown(key1))
        {
            if (lastPressedKey != key1)
            {
                currentProgress += progressPerPress;
                lastPressedKey = key1;
                targetShakeOffset = shakeIntensity; 
                timeSinceLastPress = 0f;
            }
        }
        else if (Input.GetKeyDown(key2))
        {
            if (lastPressedKey != key2)
            {
                currentProgress += progressPerPress;
                lastPressedKey = key2;
                targetShakeOffset = -shakeIntensity; 
                timeSinceLastPress = 0f;
            }
        }

        if (timeSinceLastPress > 0.15f)
        {
            targetShakeOffset = 0f;
        }

        if (shakerToShake != null)
        {
            currentShakeOffset = Mathf.Lerp(currentShakeOffset, targetShakeOffset, Time.deltaTime * shakeSmoothness);
            
            if (buEkranStirringMi)
            {
                shakerToShake.localRotation = initialRotation * Quaternion.Euler(0, 0, currentShakeOffset);
            }
            else
            {
                shakerToShake.anchoredPosition = new Vector2(initialShakerPosition.x, initialShakerPosition.y + currentShakeOffset);
            }
        }

        if (progressBar != null)
        {
            progressBar.fillAmount = currentProgress / maxProgress;
        }

        if (currentProgress >= maxProgress)
        {
            isFinished = true;
            currentProgress = maxProgress;
            if (progressBar != null) progressBar.fillAmount = 1f;
            
            if (shakerToShake != null) 
            {
                shakerToShake.anchoredPosition = initialShakerPosition;
                shakerToShake.localRotation = initialRotation;
            }
            
            if (buEkranStirringMi)
            {
                KokteylManager.Instance.isStirred = true;
            }
            else
            {
                KokteylManager.Instance.isShaken = true;
            }

            KokteylManager.Instance.StartPhase(KokteylManager.GamePhase.Preparation);
        }
    }
}