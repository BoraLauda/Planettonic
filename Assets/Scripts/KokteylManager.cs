using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class KokteylTarifi
{
    public string tarifAdi = "Yeni Tarif";
    public int istenenBuzSayisi = 0;
    public int istenenLimonSayisi = 0;
    public List<string> istenenSoslar = new List<string>();
    public bool calkalanmaliMi = false;
    public bool karistirilmaliMi = false;
}


public class KokteylManager : MonoBehaviour
{
    public static KokteylManager Instance; 

    public enum GamePhase { Preparation, Jigger, Stirring, Shaking, Pouring, Finished }
    public GamePhase currentPhase = GamePhase.Preparation;
    
    public bool isShaken = false;
    public bool isStirred = false;

    [Header("Aşama UI Kutuları")]
    public GameObject phase0_Preparation;
    public GameObject phase1_Jigger;
    public GameObject phase2_Stirring;
    public GameObject phase3_Shaking;
    public GameObject phase4_Pouring;

    public KeyCode hileTusu = KeyCode.F10; 
    public GameObject miniGameAnaObje;   
    
    [Header("Oyuncunun Ekledikleri (Hafıza)")]
    public int eklenenBuzSayisi = 0;
    public int eklenenLimonSayisi = 0;
    public List<string> eklenenSoslar = new List<string>();
    
    [Header("Tarif Sistemi (Müşterinin İstedikleri)")]
    public List<KokteylTarifi> tumTarifler = new List<KokteylTarifi>(); 
    public KokteylTarifi aktifTarif; 
  

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        StartPhase(GamePhase.Preparation);
    }
    
    void Update()
    {
        if (currentPhase == GamePhase.Preparation && Input.GetKeyDown(KeyCode.Return))
        {
            NextPhase();
        }

        if (Input.GetKeyDown(hileTusu))
        {
            HileyleBitir();
        }
    }

    public void StartPhase(GamePhase newPhase)
    {
        currentPhase = newPhase;

        if (phase0_Preparation) phase0_Preparation.SetActive(false);
        if (phase1_Jigger) phase1_Jigger.SetActive(false);
        if (phase2_Stirring) phase2_Stirring.SetActive(false);
        if (phase3_Shaking) phase3_Shaking.SetActive(false);
        if (phase4_Pouring) phase4_Pouring.SetActive(false);

        switch (currentPhase)
        {
            case GamePhase.Preparation:
                if (phase0_Preparation) phase0_Preparation.SetActive(true);
                break;
            case GamePhase.Jigger:
                if (phase1_Jigger) phase1_Jigger.SetActive(true);
                break;
            case GamePhase.Stirring:
                if (phase2_Stirring) phase2_Stirring.SetActive(true);
                break;
            case GamePhase.Shaking:
                if (phase3_Shaking) phase3_Shaking.SetActive(true);
                break;
            case GamePhase.Pouring:
                if (phase4_Pouring) phase4_Pouring.SetActive(true);
                break;
            case GamePhase.Finished:
                Debug.Log("Kokteyl hazır! Puan hesaplanıyor...");
                PuanHesaplaVeBitir();
                break;
        }
    }

    public void NextPhase()
    {
        StartPhase(currentPhase + 1);
    }
    
    void OnDisable()
    {
        SivilariTemizle();
    }
    
    void OnEnable()
    {
        currentPhase = GamePhase.Preparation;
        
        isShaken = false; 
        isStirred = false;
        
        eklenenBuzSayisi = 0;
        eklenenLimonSayisi = 0;
        eklenenSoslar.Clear(); 
        
        SivilariTemizle();
        
        if (tumTarifler.Count > 0)
        {
            int rastgeleIndex = Random.Range(0, tumTarifler.Count);
            aktifTarif = tumTarifler[rastgeleIndex];
            Debug.Log("Yeni Müşteri Geldi! İstenen Kokteyl: " + aktifTarif.tarifAdi);
        }
        else
        {
            Debug.LogWarning("Hiç tarif girmedin");
        }
    }
    
    public void SivilariTemizle()
    {
        GameObject[] sivilar = GameObject.FindGameObjectsWithTag("Liquid");
        foreach (GameObject sivi in sivilar) Destroy(sivi);
    }
    
    private void HileyleBitir()
    {
        brainDate bd = FindFirstObjectByType<brainDate>(); 
        if (bd == null) bd = FindObjectOfType<brainDate>(); 
        
        if (bd != null) bd.EndPixelGame(1f, 1, TargetCharacter.Both); 
        
        if (miniGameAnaObje != null) miniGameAnaObje.SetActive(false); 
        else gameObject.SetActive(false);
    }
    
    public void EklenenSosuKaydet(string sosAdi)
    {
        eklenenSoslar.Add(sosAdi);
        Debug.Log("Manager'a kaydedilen sos: " + sosAdi);
    }

    public void BuzEkle() { eklenenBuzSayisi++; }
    public void LimonEkle() { eklenenLimonSayisi++; }
    
    private void PuanHesaplaVeBitir()
    {
        if (aktifTarif == null) return;

        float toplamPuan = 0f;
        float maxPuan = 5f;
        
        if (eklenenBuzSayisi == aktifTarif.istenenBuzSayisi) toplamPuan += 1f;
        
        if (eklenenLimonSayisi == aktifTarif.istenenLimonSayisi) toplamPuan += 1f;
        
        if (isStirred == aktifTarif.karistirilmaliMi) toplamPuan += 1f;
        
        if (isShaken == aktifTarif.calkalanmaliMi) toplamPuan += 1f;

        bool soslarDogruMu = true;
        if (eklenenSoslar.Count != aktifTarif.istenenSoslar.Count) 
        {
            soslarDogruMu = false;
        }
        else
        {
            foreach (string istenenSos in aktifTarif.istenenSoslar)
            {
                if (!eklenenSoslar.Contains(istenenSos)) soslarDogruMu = false;
            }
        }
        
        if (soslarDogruMu) toplamPuan += 1f;
        
        float basariOrani = toplamPuan / maxPuan;
        Debug.Log("Kokteyl Bitti! Başarı Oranı: " + basariOrani);
        
        brainDate bd = FindFirstObjectByType<brainDate>(); 
        if (bd == null) bd = FindObjectOfType<brainDate>(); 
        
        if (bd != null)
        {
            int kalpKazanildi = (basariOrani >= 0.8f) ? 1 : 0;
            bd.EndPixelGame(basariOrani, kalpKazanildi, TargetCharacter.Both); 
        }
        
        if (miniGameAnaObje != null) miniGameAnaObje.SetActive(false); 
        else gameObject.SetActive(false);
    }
}