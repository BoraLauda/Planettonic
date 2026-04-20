using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro; 

public class BrainKuzu : MonoBehaviour
{
    public static BrainKuzu Instance;

    [Header("Kalp UI Ayarları")]
    public Image[] kalpImajlari; 

    [Header("Zamanlayıcı Ayarları")]
    public float gameDuration = 20f;
    public TextMeshProUGUI sureText;
    private float gameTimer;
    private bool isGameOver = false;

    [Header("Oyun Mantığı")]
    public int maksimumKalp = 5; 
    
    private int mevcutYarimKalpPuan = 0; 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        gameTimer = gameDuration;
        isGameOver = false;
        KalpleriGuncelle();
    }

    private void Update()
    {
        if (isGameOver) return;

        gameTimer -= Time.deltaTime;

        if (sureText != null)
        {
            int kalanSaniye = Mathf.CeilToInt(Mathf.Max(0, gameTimer));
            sureText.text = kalanSaniye.ToString();

            if (gameTimer <= 5f && gameTimer > 0f)
            {
                sureText.color = Color.red;
                
                float saniyeKusurati = gameTimer % 1f;
                float pulseScale = 1f + (saniyeKusurati * 0.4f); 
                sureText.transform.localScale = new Vector3(pulseScale, pulseScale, 1f);
            }
            else
            {
                sureText.color = Color.white;
                sureText.transform.localScale = Vector3.one;
            }
        }

        if (gameTimer <= 0)
        {
            MinigameBitti(false);
        }
    }

    public void KalpToplandi()
    {
        if (isGameOver) return;

        int maksimumYarimPuan = maksimumKalp * 2; 

        if (mevcutYarimKalpPuan < maksimumYarimPuan)
        {
            mevcutYarimKalpPuan++;
            KalpleriGuncelle();
            
            if (mevcutYarimKalpPuan == maksimumYarimPuan)
            {
                MinigameBitti(true);
            }
        }
    }

    public void KirikKalbeCarpildi()
    {
        if (isGameOver) return;

        if (mevcutYarimKalpPuan > 0)
        {
            mevcutYarimKalpPuan--; 
            KalpleriGuncelle();
        }
    }
    
    private void KalpleriGuncelle()
    {
        
        for (int i = 0; i < kalpImajlari.Length; i++)
        {
            if (kalpImajlari[i] != null)
            {
                float kalbinPayi = (mevcutYarimKalpPuan - (i * 2)) / 2f;
                
                kalpImajlari[i].fillAmount = Mathf.Clamp(kalbinPayi, 0f, 1f);
            }
        }
    }
    
    private void MinigameBitti(bool basariliMi)
    {
        isGameOver = true;
        Debug.Log(basariliMi ? "TEBRİKLER: Tüm kalpler toplandı!" : "SÜRE BİTTİ!");

        brainDate bd = FindFirstObjectByType<brainDate>();
        if (bd != null)
        {
            float kazanilanYildiz = basariliMi ? 1f : 0.5f; 
            int kazanilanKalp = Mathf.RoundToInt(mevcutYarimKalpPuan * 2.5f); 

            bd.EndRunnerGame(kazanilanYildiz, kazanilanKalp, TargetCharacter.Both);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}