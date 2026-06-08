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
}

public class KokteylManager : MonoBehaviour
{
    public static KokteylManager Instance; 

    public enum GamePhase { Preparation, Jigger, Stirring, Shaking, Pouring, Finished }
    public GamePhase currentPhase = GamePhase.Preparation;

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
        ResetMasayiVeDegiskenleri(); 
    }

    private void ResetMasayiVeDegiskenleri()
    {
        currentPhase = GamePhase.Preparation;
        
        eklenenBuzSayisi = 0;
        eklenenLimonSayisi = 0;
        eklenenPortakalSayisi = 0; 
        eklenenNaneSayisi = 0;     
        eklenenCilekSayisi = 0;    
        eklenenZeytinSayisi = 0;   
        eklenenSoslar.Clear(); 
        
        SivilariTemizle();
        if (masadakiBardakSivilari != null)
        {
            foreach (Image sivi in masadakiBardakSivilari)
            {
                if (sivi != null) { sivi.fillAmount = 0f; sivi.color = Color.white; }
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

        StartPhase(GamePhase.Preparation);
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
        float enYuksekBasariOrani = 0f;
        string yapilanKokteylAdi = "Bilinmeyen Karışım";

        foreach (KokteylTarifi tarif in tumTarifler)
        {
            float dogruPuan = 0;
            // DİKKAT: Eski 8 puanı 6'ya düşürdük!
            float toplamKriter = 6f + tarif.istenenSoslar.Count;

            if (eklenenBuzSayisi == tarif.istenenBuzSayisi) dogruPuan++;
            if (eklenenLimonSayisi == tarif.istenenLimonSayisi) dogruPuan++;
            if (eklenenPortakalSayisi == tarif.istenenPortakalSayisi) dogruPuan++;
            if (eklenenNaneSayisi == tarif.istenenNaneSayisi) dogruPuan++;
            if (eklenenCilekSayisi == tarif.istenenCilekSayisi) dogruPuan++;
            if (eklenenZeytinSayisi == tarif.istenenZeytinSayisi) dogruPuan++;

            int dogruSosSayisi = 0;
            List<string> kopyaEklenenler = new List<string>(eklenenSoslar);
            foreach (string istenenSos in tarif.istenenSoslar)
            {
                if (kopyaEklenenler.Contains(istenenSos))
                {
                    dogruSosSayisi++;
                    kopyaEklenenler.Remove(istenenSos); 
                }
            }
            dogruPuan += dogruSosSayisi;

            int yanlisSosSayisi = kopyaEklenenler.Count;
            dogruPuan -= (yanlisSosSayisi * 0.5f); 

            float oran = Mathf.Clamp01(dogruPuan / toplamKriter);
            
            if (oran > enYuksekBasariOrani)
            {
                enYuksekBasariOrani = oran;
                yapilanKokteylAdi = tarif.tarifAdi; 
            }
        }

        bool solSevdi = false;
        bool sagSevdi = false;

        if (enYuksekBasariOrani >= 1f)
        {
            if (DateSettings.leftChar != null && yapilanKokteylAdi == DateSettings.leftChar.sevdigiKokteyl)
                solSevdi = true;
                
            if (DateSettings.rightChar != null && yapilanKokteylAdi == DateSettings.rightChar.sevdigiKokteyl)
                sagSevdi = true;
        }

        float kazanilanYildiz = 0f;
        int kazanilanKalp = 0;
        TargetCharacter hedefKarakter = TargetCharacter.Both;
        
        List<DialogueDataları> reactionSequence = new List<DialogueDataları>();
        brainDate bd = FindFirstObjectByType<brainDate>(); 

        if (solSevdi && sagSevdi)
        {
            kazanilanYildiz = 2f; 
            kazanilanKalp = 80;
            hedefKarakter = TargetCharacter.Both;
            
            if (bd != null)
            {
                foreach (var outcome in bd.coupleKokteylOutcomes)
                {
                    if ((DateSettings.leftChar == outcome.characterA && DateSettings.rightChar == outcome.characterB) ||
                        (DateSettings.leftChar == outcome.characterB && DateSettings.rightChar == outcome.characterA))
                    {
                        if (outcome.ortakSevmeSenaryosu != null)
                            reactionSequence.Add(outcome.ortakSevmeSenaryosu);
                        break;
                    }
                }
            }
        }
        else
        {
            if (solSevdi)
            {
                kazanilanYildiz = 1f; kazanilanKalp = 40; hedefKarakter = TargetCharacter.Left;
            }
            else if (sagSevdi)
            {
                kazanilanYildiz = 1f; kazanilanKalp = 40; hedefKarakter = TargetCharacter.Right;
            }

            if (!solSevdi && DateSettings.leftChar != null && DateSettings.leftChar.kokteylSevmediDiyalogu != null)
            {
                reactionSequence.Add(DateSettings.leftChar.kokteylSevmediDiyalogu);
            }

            if (!sagSevdi && DateSettings.rightChar != null && DateSettings.rightChar.kokteylSevmediDiyalogu != null)
            {
                reactionSequence.Add(DateSettings.rightChar.kokteylSevmediDiyalogu);
            }
        }

        if (bd != null) 
        {
            DialogueDataları savedScenario = bd.GetSavedMainScenario();
            if (savedScenario != null && savedScenario.nextScenario != null)
            {
                reactionSequence.Add(savedScenario.nextScenario);
            }
        }

        if (bd != null) 
        {
            bd.EndBartendingGame(kazanilanYildiz, kazanilanKalp, hedefKarakter, reactionSequence); 
        }
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
        if (bd != null) bd.EndBartendingGame(1f, 40, TargetCharacter.Both, new List<DialogueDataları>()); 
        MasadakiBardagiGuncelle(1f);
        TemizlikYap();
    }

    private string MevcutTarifiBul()
    {
        float enYuksekBasariOrani = 0f;
        string yapilanKokteylAdi = "";

        foreach (KokteylTarifi tarif in tumTarifler)
        {
            float dogruPuan = 0;
            // DİKKAT: Burada da eski 8 puanı 6'ya düşürdük!
            float toplamKriter = 6f + tarif.istenenSoslar.Count;

            if (eklenenBuzSayisi == tarif.istenenBuzSayisi) dogruPuan++;
            if (eklenenLimonSayisi == tarif.istenenLimonSayisi) dogruPuan++;
            if (eklenenPortakalSayisi == tarif.istenenPortakalSayisi) dogruPuan++;
            if (eklenenNaneSayisi == tarif.istenenNaneSayisi) dogruPuan++;
            if (eklenenCilekSayisi == tarif.istenenCilekSayisi) dogruPuan++;
            if (eklenenZeytinSayisi == tarif.istenenZeytinSayisi) dogruPuan++;

            int dogruSosSayisi = 0;
            List<string> kopyaEklenenler = new List<string>(eklenenSoslar);
            foreach (string istenenSos in tarif.istenenSoslar)
            {
                if (kopyaEklenenler.Contains(istenenSos))
                {
                    dogruSosSayisi++;
                    kopyaEklenenler.Remove(istenenSos); 
                }
            }
            dogruPuan += dogruSosSayisi;
            dogruPuan -= (kopyaEklenenler.Count * 0.5f); 

            float oran = Mathf.Clamp01(dogruPuan / toplamKriter);
            
            if (oran > enYuksekBasariOrani)
            {
                enYuksekBasariOrani = oran;
                yapilanKokteylAdi = tarif.tarifAdi; 
            }
        }

        if (enYuksekBasariOrani >= 1f)
        {
            return yapilanKokteylAdi;
        }
        
        return "";
    }
    
    public bool IsShakerEmpty()
    {
        return eklenenBuzSayisi == 0 &&
               eklenenLimonSayisi == 0 &&
               eklenenPortakalSayisi == 0 &&
               eklenenNaneSayisi == 0 &&
               eklenenCilekSayisi == 0 &&
               eklenenZeytinSayisi == 0 &&
               eklenenSoslar.Count == 0;
    }

    public Color GetKokteylRengi()
    {
        Color sonucRengi = Color.white; 
        string yapilanTarif = MevcutTarifiBul().ToLower();

        if (yapilanTarif.Contains("cosmopolitan"))
        {
            ColorUtility.TryParseHtmlString("#E9ADAD", out sonucRengi);
        }
        else if (yapilanTarif.Contains("martini"))
        {
            ColorUtility.TryParseHtmlString("#F8BF9E", out sonucRengi);
        }
        else if (yapilanTarif.Contains("neptun") || yapilanTarif.Contains("neptün"))
        {
            ColorUtility.TryParseHtmlString("#BDF39D", out sonucRengi);
        }
        else if (eklenenSoslar.Count > 0)
        {
            string anaSos = eklenenSoslar[0];
            switch (anaSos)
            {
                case "Mavi": sonucRengi = new Color(0.2f, 0.6f, 1f); break;
                case "Pembe": sonucRengi = new Color(1f, 0.4f, 0.7f); break;
                case "Sari": sonucRengi = Color.yellow; break;
                case "Turuncu": sonucRengi = new Color(1f, 0.5f, 0f); break;
                case "Kirmizi": sonucRengi = Color.red; break;
                case "Yesil": sonucRengi = Color.green; break;
            }
        }

        sonucRengi.a = 180f / 255f; 
        return sonucRengi;
    }
    
    public void IptalEtVeHazirligaDon()
    {
        if (currentPhase == GamePhase.Shaking && ShakerInteraction.Instance != null)
        {
            ShakerInteraction.Instance.kapakAcikMi = true;
            if (ShakerInteraction.Instance.kapaliShakerObjesi != null) ShakerInteraction.Instance.kapaliShakerObjesi.SetActive(false);
            if (ShakerInteraction.Instance.acikShakerObjesi != null) ShakerInteraction.Instance.acikShakerObjesi.SetActive(true);
        }
        
        StartPhase(GamePhase.Preparation);
    }
}