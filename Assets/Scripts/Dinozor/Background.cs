using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Background : MonoBehaviour
{
    [Header("Ayarlar")]
    public float kaymaHizi = 200f;
    public RectTransform baslangicArkaplan; 
    public GameObject donguPrefab;
    public Transform donguKonteyner; 

    [Header("Matematik ve Test Ayarları")]
    public float donguGenisligi = 873.78f; 
    public float ormanaBaglamaKaydirmasi = 0f; 
    public float kapanisOfseti = -500f; 
    public float klonKapanisOfseti = -800f; 

    [Header("Boşluk Kapatıcı")]
    public float icIceGecmePayi = 2f; 

    private List<RectTransform> aktifDonguler = new List<RectTransform>();
    private bool baslangicBitti = false;

    void Start()
    {
        float ormanSagKenar = 0;
        if (baslangicArkaplan != null)
        {
            ormanSagKenar = baslangicArkaplan.anchoredPosition.x + (baslangicArkaplan.sizeDelta.x * (1f - baslangicArkaplan.pivot.x));
        }
        
        for (int i = 0; i < 3; i++)
        {
            GameObject yeniDongu = Instantiate(donguPrefab, donguKonteyner);
            RectTransform rt = yeniDongu.GetComponent<RectTransform>();
            
            float ilkKlonX = ormanSagKenar + ormanaBaglamaKaydirmasi;
            
            rt.anchoredPosition = new Vector2(ilkKlonX + (i * (donguGenisligi - icIceGecmePayi)), baslangicArkaplan != null ? baslangicArkaplan.anchoredPosition.y : 0);
            
            aktifDonguler.Add(rt);
        }
    }

    void Update()
    {
        float mesafe = kaymaHizi * Time.deltaTime;
        
        if (!baslangicBitti && baslangicArkaplan != null)
        {
            baslangicArkaplan.anchoredPosition += Vector2.left * mesafe;
            
            float anlikOrmanSag = baslangicArkaplan.anchoredPosition.x + (baslangicArkaplan.sizeDelta.x * (1f - baslangicArkaplan.pivot.x));
            
            if (anlikOrmanSag <= kapanisOfseti) 
            {
                baslangicBitti = true;
                baslangicArkaplan.gameObject.SetActive(false);
            }
        }
        
     
        for (int i = 0; i < aktifDonguler.Count; i++)
        {
            aktifDonguler[i].anchoredPosition += Vector2.left * mesafe;

            float klonSagKenar = aktifDonguler[i].anchoredPosition.x + (donguGenisligi * (1f - aktifDonguler[i].pivot.x));
            
            if (klonSagKenar <= klonKapanisOfseti) 
            {
                float enSagX = -99999f;
                foreach (var d in aktifDonguler)
                {
                    float dSag = d.anchoredPosition.x + (donguGenisligi * (1f - d.pivot.x));
                    if (dSag > enSagX) enSagX = dSag;
                }
                
                float yeniX = enSagX + (donguGenisligi * aktifDonguler[i].pivot.x) - icIceGecmePayi;
                
                aktifDonguler[i].anchoredPosition = new Vector2(yeniX, aktifDonguler[i].anchoredPosition.y);
            }
        }
    }
}