using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class KokteylTarifi
{
    public string tarifAdi = "Yeni Tarif";
    public Sprite kitapIkonu; 
    
    [Header("Icerikler")]
    public int istenenBuzSayisi = 0;
    public int istenenLimonSayisi = 0;
    public int istenenPortakalSayisi = 0;
    public int istenenNaneSayisi = 0;     
    public int istenenCilekSayisi = 0;   
    public int istenenZeytinSayisi = 0;   
    
    public List<string> istenenSoslar = new List<string>();
    
    [Header("Yapilis")]
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

    [Header("Asama UI Kutulari")]
    public GameObject phase0_Preparation;
    public GameObject phase1_Jigger;
    public GameObject phase2_Stirring;
    public GameObject phase3_Shaking;
    public GameObject phase4_Pouring;

    public KeyCode hileTusu = KeyCode.F10; 
    public GameObject miniGameAnaObje;   

    [Header("Kitap, Masa ve Zil UI")]
    public GameObject kitapButonu; 
    public GameObject zilObjesi;
    public Image[] masadakiBardakSivilari; 

    [Header("Yerlesim Ayarlari")]
    public RectTransform[] masadakiBardakObjeleri; 
    public RectTransform shakerObjesi;             
    public RectTransform masaMerkezPozisyonu;      
    
    private Vector2 shakerBaslangicPos;            
    private Vector3[] bardakBaslangicPozisyonlari; 
    
    [HideInInspector] public List<GameObject> masadakiSusler = new List<GameObject>(); 
    
    [Header("Oyuncunun Ekledikleri (Hafiza)")]
    public int eklenenBuzSayisi = 0;
    public int eklenenLimonSayisi = 0;
    public int eklenenPortakalSayisi = 0; 
    public int eklenenNaneSayisi = 0;     
    public int eklenenCilekSayisi = 0;    
    public int eklenenZeytinSayisi = 0;   
    public List<string> eklenenSoslar = new List<string>();
    
    [Header("Tarif Sistemi")]
    public List<KokteylTarifi> tumTarifler = new List<KokteylTarifi>(); 
  
    public int secilenBardakIndex = 0; 
    public GameObject[] pouringBardakObjeleri;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (shakerObjesi != null) shakerBaslangicPos = shakerObjesi.anchoredPosition;
        
        if (masadakiBardakObjeleri != null)
        {
            bardakBaslangicPozisyonlari = new Vector3[masadakiBardakObjeleri.Length];
            for (int i = 0; i < masadakiBardakObjeleri.Length; i++)
            {
                if (masadakiBardakObjeleri[i] != null)
                    bardakBaslangicPozisyonlari[i] = masadakiBardakObjeleri[i].anchoredPosition;
            }
        }
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
    
    void OnEnable()
    {
        currentPhase = GamePhase.Preparation;
        isShaken = false; 
        isStirred = false;
        
        eklenenBuzSayisi = 0;
        eklenenLimonSayisi = 0;
        eklenenPortakalSayisi = 0; 
        eklenenNaneSayisi = 0;     
        eklenenCilekSayisi = 0;    
        eklenenZeytinSayisi = 0;   
        eklenenSoslar.Clear(); 
        
        if (masadakiBardakSivilari != null)
        {
            foreach (Image sivi in masadakiBardakSivilari)
            {
                if (sivi != null) sivi.fillAmount = 0f;
            }
        }
        
        if (kitapButonu != null) kitapButonu.SetActive(true);
        if (zilObjesi != null) zilObjesi.SetActive(false); 
        
        foreach (GameObject sus in masadakiSusler) { if (sus != null) Destroy(sus); }
        masadakiSusler.Clear();

        if (shakerObjesi != null) shakerObjesi.anchoredPosition = shakerBaslangicPos;
        
        if (masadakiBardakObjeleri != null && bardakBaslangicPozisyonlari != null)
        {
            for (int i = 0; i < masadakiBardakObjeleri.Length; i++)
            {
                if (masadakiBardakObjeleri[i] != null)
                    masadakiBardakObjeleri[i].anchoredPosition = bardakBaslangicPozisyonlari[i];
            }
        }
    }

    public void SivilariTemizle()
    {
        GameObject[] sivilar = GameObject.FindGameObjectsWithTag("Liquid");
        foreach (GameObject sivi in sivilar) Destroy(sivi);
    }

    public void BuzEkle() { eklenenBuzSayisi++; }
    public void LimonEkle() { eklenenLimonSayisi++; }
    public void PortakalEkle() { eklenenPortakalSayisi++; }
    public void NaneEkle() { eklenenNaneSayisi++; }
    public void CilekEkle() { eklenenCilekSayisi++; }
    public void ZeytinEkle() { eklenenZeytinSayisi++; }

    public void EklenenSosuKaydet(string sosAdi)
    {
        eklenenSoslar.Add(sosAdi);
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

    private void PuanHesaplaVeBitir()
    {
        KokteylTarifi yapilanTarif = null;

        foreach (KokteylTarifi tarif in tumTarifler)
        {
            if (eklenenBuzSayisi == tarif.istenenBuzSayisi &&
                eklenenLimonSayisi == tarif.istenenLimonSayisi &&
                eklenenPortakalSayisi == tarif.istenenPortakalSayisi &&
                eklenenNaneSayisi == tarif.istenenNaneSayisi &&
                eklenenCilekSayisi == tarif.istenenCilekSayisi &&
                eklenenZeytinSayisi == tarif.istenenZeytinSayisi &&
                isStirred == tarif.karistirilmaliMi &&
                isShaken == tarif.calkalanmaliMi)
            {
                if (eklenenSoslar.Count == tarif.istenenSoslar.Count)
                {
                    List<string> kopyaEklenenler = new List<string>(eklenenSoslar);
                    bool soslarDogruMu = true;
                    foreach (string istenenSos in tarif.istenenSoslar)
                    {
                        if (kopyaEklenenler.Contains(istenenSos)) kopyaEklenenler.Remove(istenenSos);
                        else { soslarDogruMu = false; break; }
                    }
                    if (soslarDogruMu) { yapilanTarif = tarif; break; }
                }
            }
        }

        float basariOrani = (yapilanTarif != null) ? 1f : 0f;
        int kalpKazanildi = (yapilanTarif != null) ? 1 : 0;
        
        brainDate bd = FindFirstObjectByType<brainDate>(); 
        if (bd != null) bd.EndPixelGame(basariOrani, kalpKazanildi, TargetCharacter.Both); 
        
        TemizlikYap();
    }

    public void SadeceMasayiBirakVeSifirla()
    {
        if (phase1_Jigger) phase1_Jigger.SetActive(false);
        if (phase2_Stirring) phase2_Stirring.SetActive(false);
        if (phase3_Shaking) phase3_Shaking.SetActive(false);
        if (phase4_Pouring) phase4_Pouring.SetActive(false);
        
        if (pouringBardakObjeleri != null)
        {
            foreach (GameObject bardak in pouringBardakObjeleri)
            {
                if (bardak != null) bardak.SetActive(false);
            }
        }

        if (shakerObjesi != null) shakerObjesi.anchoredPosition = shakerBaslangicPos;

        if (masadakiBardakObjeleri != null && secilenBardakIndex < masadakiBardakObjeleri.Length)
        {
            RectTransform secilenBardak = masadakiBardakObjeleri[secilenBardakIndex];
            if (secilenBardak != null && masaMerkezPozisyonu != null)
            {
                secilenBardak.anchoredPosition = masaMerkezPozisyonu.anchoredPosition;
            }
        }

        if (kitapButonu) kitapButonu.SetActive(true);
        if (phase0_Preparation) phase0_Preparation.SetActive(true);
        
        if (zilObjesi != null) zilObjesi.SetActive(true);
        
        currentPhase = GamePhase.Preparation;
    }

    private void TemizlikYap()
    {
        if (phase0_Preparation) phase0_Preparation.SetActive(false);
        if (phase1_Jigger) phase1_Jigger.SetActive(false);
        if (phase2_Stirring) phase2_Stirring.SetActive(false);
        if (phase3_Shaking) phase3_Shaking.SetActive(false);
        if (phase4_Pouring) phase4_Pouring.SetActive(false);

        if (pouringBardakObjeleri != null)
        {
            foreach (GameObject bardak in pouringBardakObjeleri)
            {
                if (bardak != null) bardak.SetActive(false);
            }
        }

        if (kitapButonu) kitapButonu.SetActive(false);
        if (zilObjesi) zilObjesi.SetActive(false); 

        if (miniGameAnaObje != null) miniGameAnaObje.SetActive(false); 
        else gameObject.SetActive(false);
    }

    private void HileyleBitir()
    {
        brainDate bd = FindFirstObjectByType<brainDate>(); 
        if (bd != null) bd.EndPixelGame(1f, 1, TargetCharacter.Both); 
        MasadakiBardagiGuncelle(1f);
        TemizlikYap();
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