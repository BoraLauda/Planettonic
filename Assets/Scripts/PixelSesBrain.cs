using UnityEngine;

public class PixelSesBrain : MonoBehaviour
{
  [Header("Ses Kaynakları (Hoparlörler)")]
    public AudioSource arkaPlanMuzigi; 
    public AudioSource sfxKaynagi;     

    [Header("Ses Efektleri (Klipler)")]
    public AudioClip hitSesi;
    public AudioClip olumSesi;

    private brainDate mainDateScript;

   
    void OnEnable()
    {
        
        mainDateScript = FindFirstObjectByType<brainDate>();
        if (mainDateScript != null)
        {
            if (mainDateScript.bgmSource != null) mainDateScript.bgmSource.Pause();
            if (mainDateScript.bgsSource != null) mainDateScript.bgsSource.Pause();
        }

       
        if (arkaPlanMuzigi != null)
        {
            arkaPlanMuzigi.Play();
        }
    }

    
    void OnDisable()
    {
        if (mainDateScript != null)
        {
            if (mainDateScript.bgmSource != null) mainDateScript.bgmSource.UnPause();
            if (mainDateScript.bgsSource != null) mainDateScript.bgsSource.UnPause();
        }

       
        if (arkaPlanMuzigi != null)
        {
            arkaPlanMuzigi.Stop();
        }
    }

 
    public void HitSesiCal()
    {
        if (sfxKaynagi != null && hitSesi != null)
        {
            sfxKaynagi.PlayOneShot(hitSesi); 
        }
    }

  
    public void OlumSesiCal()
    {
        if (sfxKaynagi != null && olumSesi != null)
        {
           
            if (arkaPlanMuzigi != null) arkaPlanMuzigi.Stop(); 
            
            sfxKaynagi.PlayOneShot(olumSesi);
        }
    }
}
