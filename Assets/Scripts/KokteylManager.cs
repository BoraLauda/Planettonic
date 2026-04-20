using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class KokteylTarifi
{
    public string tarifAdi = "Yeni Tarif";
    public Sprite kitapIkonu; 
    
    [Header("İçerikler")]
    public int istenenBuzSayisi = 0;
    public int istenenLimonSayisi = 0;
    
    public Sprite siviIkonu; 
    public int istenenSiviMiktari = 1; 
    
    public List<string> istenenSoslar = new List<string>();
    
    [Header("Yapılış")]
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

    [Header("Kitap ve Masa UI Bağlantısı")]
    public GameObject kitapButonu; 
    public Image[] masadakiBardakSivilari; 
    
    [Header("Oyuncunun Ekledikleri (Hafıza)")]
    public int eklenenBuzSayisi = 0;
    public int eklenenLimonSayisi = 0;
    public List<string> eklenenSoslar = new List<string>();
    
    [Header("Tarif Sistemi (Kayıtlı Tarifler)")]
    public List<KokteylTarifi> tumTarifler = new List<KokteylTarifi>(); 
  
    public int secilenBardakIndex = 0; 
    public GameObject[] pouringBardakObjeleri;

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
                
                for (int i = 0; i < pouringBardakObjeleri.Length; i++)
                {
                    if (pouringBardakObjeleri[i] != null)
                    {
                        pouringBardakObjeleri[i].SetActive(i == secilenBardakIndex);
                    }
                }
                break;
            case GamePhase.Finished:
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
        
        if (masadakiBardakSivilari != null)
        {
            foreach (Image sivi in masadakiBardakSivilari)
            {
                if (sivi != null) sivi.fillAmount = 0f;
            }
        }

        if (kitapButonu != null) kitapButonu.SetActive(true);
    }
    
    public void SivilariTemizle()
    {
        GameObject[] sivilar = GameObject.FindGameObjectsWithTag("Liquid");
        foreach (GameObject sivi in sivilar) Destroy(sivi);
    }
    
    private void SadeceMasayiBirakVeSifirla()
    {
        if (phase1_Jigger != null) phase1_Jigger.SetActive(false);
        if (phase2_Stirring != null) phase2_Stirring.SetActive(false);
        if (phase3_Shaking != null) phase3_Shaking.SetActive(false);
        if (phase4_Pouring != null) phase4_Pouring.SetActive(false);
        
        if (pouringBardakObjeleri != null)
        {
            foreach (GameObject bardak in pouringBardakObjeleri)
            {
                if (bardak != null) bardak.SetActive(false);
            }
        }
        
        if (phase0_Preparation != null) phase0_Preparation.SetActive(true);
        if (kitapButonu != null) kitapButonu.SetActive(true);
        
        eklenenBuzSayisi = 0;
        eklenenLimonSayisi = 0;
        eklenenSoslar.Clear();
        isShaken = false;
        isStirred = false;
        
        currentPhase = GamePhase.Preparation;
    }
    
    public void MasadakiBardagiGuncelle(float dolulukOrani)
    {
        if (masadakiBardakSivilari != null && secilenBardakIndex < masadakiBardakSivilari.Length)
        {
            Image hedefSivi = masadakiBardakSivilari[secilenBardakIndex];
            if (hedefSivi != null)
            {
                hedefSivi.color = GetKokteylRengi();
                hedefSivi.fillAmount = dolulukOrani;
            }
        }
    }

    private void HileyleBitir()
    {
        brainDate bd = FindFirstObjectByType<brainDate>(); 
        if (bd == null) bd = FindObjectOfType<brainDate>(); 
        if (bd != null) bd.EndPixelGame(1f, 1, TargetCharacter.Both); 
        
        MasadakiBardagiGuncelle(1f);
        SadeceMasayiBirakVeSifirla();
    }
    
    public void EklenenSosuKaydet(string sosAdi)
    {
        eklenenSoslar.Add(sosAdi);
    }

    public void BuzEkle() { eklenenBuzSayisi++; }
    public void LimonEkle() { eklenenLimonSayisi++; }
    
    private void PuanHesaplaVeBitir()
    {
        KokteylTarifi yapilanTarif = null;

        foreach (KokteylTarifi tarif in tumTarifler)
        {
            if (eklenenBuzSayisi == tarif.istenenBuzSayisi &&
                eklenenLimonSayisi == tarif.istenenLimonSayisi &&
                isStirred == tarif.karistirilmaliMi &&
                isShaken == tarif.calkalanmaliMi)
            {
                if (eklenenSoslar.Count == tarif.istenenSoslar.Count)
                {
                    List<string> kopyaEklenenler = new List<string>(eklenenSoslar);
                    bool soslarDogruMu = true;

                    foreach (string istenenSos in tarif.istenenSoslar)
                    {
                        if (kopyaEklenenler.Contains(istenenSos))
                        {
                            kopyaEklenenler.Remove(istenenSos);
                        }
                        else
                        {
                            soslarDogruMu = false;
                            break;
                        }
                    }

                    if (soslarDogruMu)
                    {
                        yapilanTarif = tarif;
                        break; 
                    }
                }
            }
        }

        float basariOrani = 0f;
        int kalpKazanildi = 0;

        if (yapilanTarif != null)
        {
            basariOrani = 1f; 
            kalpKazanildi = 1;
        }
        else
        {
            basariOrani = 0f; 
            kalpKazanildi = 0;
        }
        
        brainDate bd = FindFirstObjectByType<brainDate>(); 
        if (bd == null) bd = FindObjectOfType<brainDate>(); 
        if (bd != null) bd.EndPixelGame(basariOrani, kalpKazanildi, TargetCharacter.Both); 
        
        SadeceMasayiBirakVeSifirla();
    }
    
    public Color GetKokteylRengi()
    {
        if (eklenenSoslar.Count == 0) return Color.white; 
        
        string anaSos = eklenenSoslar[0];

        switch (anaSos)
        {
            case "Mavi": return new Color(0.2f, 0.6f, 1f);
            case "Pembe": return new Color(1f, 0.4f, 0.7f);
            case "Sari": return Color.yellow;
            case "Turuncu": return new Color(1f, 0.5f, 0f);
            case "Kirmizi": return Color.red;
            case "Yesil": return Color.green;
            default: return Color.white;
        }
    }
}