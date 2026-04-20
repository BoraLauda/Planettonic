using UnityEngine;
using UnityEngine.UI; 

public class BrainKuzu : MonoBehaviour
{
    public static BrainKuzu Instance;

    [Header("Kalp UI Ayarları")]
    public Image[] kalpImajlari; 
    
    public Sprite doluKalpSprite;
    
    public Sprite bosKalpSprite;

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
        KalpleriGuncelle();
    }

    public void KalpToplandi()
    {
        if (mevcutKalp < maksimumKalp)
        {
            mevcutKalp++;
            KalpleriGuncelle();
            Debug.Log("Kalp doldu! Mevcut: " + mevcutKalp);

           
            if (mevcutKalp == maksimumKalp)
            {
                MinigameBitti();
            }
        }
    }

  
    public void KirikKalbeCarpildi()
    {
        if (mevcutKalp > 0)
        {
            mevcutKalp--;
            KalpleriGuncelle();
            Debug.Log("Kalp kırıldı! Mevcut: " + mevcutKalp);
        }
        else
        {
            Debug.Log("Zaten hiç kalbin yok, daha fazla düşemez!");
           
        }
    }
    
    private void KalpleriGuncelle()
    {
        for (int i = 0; i < kalpImajlari.Length; i++)
        {
            if (i < mevcutKalp)
            {
                kalpImajlari[i].sprite = doluKalpSprite;
            }
            else
            {
                kalpImajlari[i].sprite = bosKalpSprite;
            }
        }
    }
    
    private void MinigameBitti()
    {
        Debug.Log("OYUN BİTTİ!");
    }
}