using UnityEngine;
using UnityEngine.UI;
using System.Collections; 

public class KokteylSpam : MonoBehaviour
{
    public Image progressBar;
    public RectTransform shakerToShake; 
    
    [Header("Oyun Modu")]
    public bool buEkranStirringMi = false;
    
    [Header("İlerleme Ayarları")]
    public float progressPerPress = 10f;
    public float maxProgress = 100f;
    public float decayRate = 15f; 

    [Header("Sallanma/Dönme Ayarları")]
    public float shakeIntensity = 30f;  
    public float shakeSmoothness = 15f; 

    [Header("Görsel Efektler (Kod Parçacık Sistemi)")]
    public GameObject yildizPrefab;     
    public Transform efektMerkezi;     
    public int pariltiSayisi = 15;     
    public float patlamaGucu = 400f;    
    
    private KeyCode key1; 
    private KeyCode key2;
    private KeyCode altKey1; 
    private KeyCode altKey2; 

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

        if (buEkranStirringMi)
        {
            key1 = KeyCode.LeftArrow;
            key2 = KeyCode.RightArrow;
            altKey1 = KeyCode.A;
            altKey2 = KeyCode.D;
        }
        else
        {
            key1 = KeyCode.UpArrow;
            key2 = KeyCode.DownArrow;
            altKey1 = KeyCode.W;
            altKey2 = KeyCode.S;
        }
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

        if (Input.GetKeyDown(key1) || Input.GetKeyDown(altKey1))
        {
            if (lastPressedKey != key1)
            {
                currentProgress += progressPerPress;
                lastPressedKey = key1;
                targetShakeOffset = shakeIntensity; 
                timeSinceLastPress = 0f;
            }
        }
        else if (Input.GetKeyDown(key2) || Input.GetKeyDown(altKey2))
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
            StartCoroutine(BasariVeGecisRoutine());
        }
    }

    private IEnumerator BasariVeGecisRoutine()
    {
        isFinished = true;
        currentProgress = maxProgress;
        if (progressBar != null) progressBar.fillAmount = 1f;
        
        if (shakerToShake != null) 
        {
            shakerToShake.anchoredPosition = initialShakerPosition;
            shakerToShake.localRotation = initialRotation;
        }

       
        if (yildizPrefab != null && efektMerkezi != null)
        {
            for (int i = 0; i < pariltiSayisi; i++)
            {
                GameObject yildiz = Instantiate(yildizPrefab, efektMerkezi.position, Quaternion.identity, efektMerkezi.parent);
                StartCoroutine(YildizUcurRoutine(yildiz));
            }
        }

        yield return new WaitForSeconds(1.0f);
        
        if (buEkranStirringMi) KokteylManager.Instance.isStirred = true;
        else KokteylManager.Instance.isShaken = true;

        KokteylManager.Instance.StartPhase(KokteylManager.GamePhase.Preparation);
    }

    
    private IEnumerator YildizUcurRoutine(GameObject yildiz)
    {
        RectTransform rt = yildiz.GetComponent<RectTransform>();
        Image img = yildiz.GetComponent<Image>();

        Vector2 rastgeleYon = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        
       
        float rastgeleHiz = Random.Range(patlamaGucu * 0.8f, patlamaGucu * 1.5f); 
        float rastgeleDonusHizi = Random.Range(-300f, 300f);

       
        float animasyonSuresi = 1.2f;
        float gecenSure = 0f;

        while (gecenSure < animasyonSuresi)
        {
            gecenSure += Time.deltaTime;
            float oran = gecenSure / animasyonSuresi; 

            rt.anchoredPosition += rastgeleYon * rastgeleHiz * Time.deltaTime;
            
         
            rastgeleHiz = Mathf.Lerp(rastgeleHiz, 0f, Time.deltaTime * 2.5f); 

            rt.Rotate(0, 0, rastgeleDonusHizi * Time.deltaTime);

            if (img != null)
            {
                Color c = img.color;
                
                c.a = Mathf.Lerp(1f, 0f, Mathf.Pow(oran, 2f)); 
                img.color = c;
            }

            yield return null;
        }

        Destroy(yildiz);
    }
}