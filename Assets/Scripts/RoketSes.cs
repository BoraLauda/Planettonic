using UnityEngine;

public class RoketSes : MonoBehaviour
{
    private AudioSource motorSesi;

    void Awake()
    {
        motorSesi = GetComponent<AudioSource>();
    }

    
    void OnEnable()
    {
        if (motorSesi != null)
        {
            motorSesi.Play();
        }
    }

   
    void OnDisable()
    {
        if (motorSesi != null)
        {
            motorSesi.Stop();
        }
    }
}
