using UnityEngine;

public class Audio : MonoBehaviour
{
    public static Audio Instance;

    
    public AudioSource sfxSource; 
    
    public AudioClip defaultClickSound; 

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

   
    public void PlayClick()
    {
        if (sfxSource != null && defaultClickSound != null)
        {
            sfxSource.PlayOneShot(defaultClickSound);
        }
    }

    
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}