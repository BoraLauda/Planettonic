using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RoketSesi : MonoBehaviour
{
    private AudioSource motorSesi;
    private Vector3 sonPozisyon;

    [Header("Dinamik Ses Ayarları")]
    public float referansHiz = 10f; 
    public float maksimumPitch = 2.5f;
    
    [Header("Yok Olma (Fade) Ayarı")]
   
    public float fadeSuresi = 0.3f;

    void OnEnable()
    {
        motorSesi = GetComponent<AudioSource>();
        sonPozisyon = transform.position;
        
        if (motorSesi != null)
        {
            motorSesi.pitch = 1f; 
            motorSesi.Play();
        }
    }

    void OnDisable()
    {
      
        if (motorSesi != null && motorSesi.isPlaying && gameObject.scene.isLoaded)
        {
            HayaletSesYarat();
        }
    }

    void Update()
    {
        if (motorSesi == null || Time.deltaTime == 0) return;

        float anlikHiz = Vector3.Distance(transform.position, sonPozisyon) / Time.deltaTime;
        sonPozisyon = transform.position; 

        if (anlikHiz > 0.1f)
        {
            float hedefPitch = anlikHiz / referansHiz;
            motorSesi.pitch = Mathf.Clamp(hedefPitch, 0.5f, maksimumPitch);
        }
    }

    private void HayaletSesYarat()
    {
        
        GameObject hayalet = new GameObject("RoketFadeSes");
        hayalet.transform.position = transform.position;

       
        AudioSource kopyaSes = hayalet.AddComponent<AudioSource>();
        kopyaSes.clip = motorSesi.clip;
        kopyaSes.volume = motorSesi.volume;
        kopyaSes.pitch = motorSesi.pitch;
        kopyaSes.time = motorSesi.time; 
        kopyaSes.spatialBlend = motorSesi.spatialBlend;
        kopyaSes.Play();

        
        FadeVeYokEt fader = hayalet.AddComponent<FadeVeYokEt>();
        fader.baslangicSes = motorSesi.volume;
        fader.fadeSuresi = fadeSuresi;
    }
}


public class FadeVeYokEt : MonoBehaviour
{
    [HideInInspector] public float baslangicSes;
    [HideInInspector] public float fadeSuresi;
    private AudioSource audioS;

    void Start()
    {
        audioS = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (audioS != null)
        {
            
            audioS.volume -= (baslangicSes / fadeSuresi) * Time.deltaTime;

           
            if (audioS.volume <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}