using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LiquidSpawn : MonoBehaviour
{
    public Image liquidImage; 
    public float fillSpeed = 0.5f; 
    
    [Range(0, 1)] public float targetFill = 0f; 
    private float currentFill = 0f;
    
    private bool hasStartedPouring = false; 
    private float idleTimer = 0f;           
    private bool isFinished = false;        

    void OnEnable()
    {
        if (liquidImage == null) liquidImage = GetComponent<Image>();
        
        currentFill = 0f;
        targetFill = 0f;
        
        hasStartedPouring = false;
        idleTimer = 0f;
        isFinished = false;
        
        if (liquidImage != null) 
        {
            liquidImage.fillAmount = 0f; 
            
            if (KokteylManager.Instance != null)
            {
                liquidImage.color = KokteylManager.Instance.GetKokteylRengi();
            }
        }
    }

    void Update()
    {
        if (isFinished) return;

      
        if (Input.GetMouseButton(0))
        {
            hasStartedPouring = true;
            idleTimer = 0f; 
            targetFill += 0.25f * Time.deltaTime;
        }
        else if (hasStartedPouring)
        {
            
            idleTimer += Time.deltaTime;
            
            if (idleTimer >= 3f)
            {
                FinishPouring();
            }
        }

        targetFill = Mathf.Clamp(targetFill, 0f, 1f);

        if (currentFill < targetFill)
        {
            currentFill += fillSpeed * Time.deltaTime;
            liquidImage.fillAmount = currentFill;
        }
    }

    private void FinishPouring()
    {
        isFinished = true;

        if (KokteylManager.Instance != null)
        {
            HangiTarifYapildiKontrolEt();
            
            KokteylManager.Instance.NextPhase(); 
        }
    }

    private void HangiTarifYapildiKontrolEt()
    {
        string sonucMesaji = "Uyan tarif yok!";
        
        foreach (KokteylTarifi tarif in KokteylManager.Instance.tumTarifler)
        {
          
            if (KokteylManager.Instance.eklenenBuzSayisi == tarif.istenenBuzSayisi &&
                KokteylManager.Instance.eklenenLimonSayisi == tarif.istenenLimonSayisi &&
                KokteylManager.Instance.isStirred == tarif.karistirilmaliMi &&
                KokteylManager.Instance.isShaken == tarif.calkalanmaliMi)
            {
                
                if (KokteylManager.Instance.eklenenSoslar.Count == tarif.istenenSoslar.Count)
                {
                    List<string> kopyaEklenenler = new List<string>(KokteylManager.Instance.eklenenSoslar);
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
                        sonucMesaji = tarif.tarifAdi + " yapıldı!";
                        break; 
                    }
                }
            }
        }

        Debug.Log("Dökme İşlemi Bitti! -> " + sonucMesaji);
    }
}