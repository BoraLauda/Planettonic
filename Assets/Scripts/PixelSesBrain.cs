using UnityEngine;

public class PixelSesBrain : MonoBehaviour
{
    [Header("Ses Kaynakları (Hoparlörler)")]
    public AudioSource arkaPlanMuzigi; 
    public AudioSource sfxKaynagi;     

    [Header("Ses Efektleri (Klipler)")]
    public AudioClip hitSesi;
    public AudioClip olumSesi;

    void OnEnable()
    {
       

        if (arkaPlanMuzigi != null)
        {
            arkaPlanMuzigi.Play();
        }
    }
    
    void OnDisable()
    {
        

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