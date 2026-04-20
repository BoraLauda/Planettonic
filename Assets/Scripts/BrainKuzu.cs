using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro; 

public class BrainKuzu : MonoBehaviour
{
    public static BrainKuzu Instance;

    [Header("Kalp UI Ayarları")]
    public Image[] kalpImajlari; 
    public Sprite doluKalpSprite;
    public Sprite bosKalpSprite;

    [Header("Zamanlayıcı Ayarları")]
    public float gameDuration = 20f;
    public TextMeshProUGUI sureText;
    private float gameTimer;
    private bool isGameOver = false;

    [Header("Oyun Mantığı")]
    public int maksimumKalp = 5;
    private int mevcutKalp = 0; 

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

        if (mevcutKalp < maksimumKalp)
        {
            mevcutKalp++;
            KalpleriGuncelle();
            
            if (mevcutKalp == maksimumKalp)
            {
                MinigameBitti(true);
            }
        }
    }

    public void KirikKalbeCarpildi()
    {
        if (isGameOver) return;

        if (mevcutKalp > 0)
        {
            mevcutKalp--;
            KalpleriGuncelle();
        }
    }
    
    private void KalpleriGuncelle()
    {
        for (int i = 0; i < kalpImajlari.Length; i++)
        {
            if (i < mevcutKalp)
                kalpImajlari[i].sprite = doluKalpSprite;
            else
                kalpImajlari[i].sprite = bosKalpSprite;
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
            int kazanilanKalp = mevcutKalp * 5;

            bd.EndRunnerGame(kazanilanYildiz, kazanilanKalp, TargetCharacter.Both);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}