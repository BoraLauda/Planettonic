using UnityEngine;
using System.Collections;
public class Peluslar : MonoBehaviour
{
    public float speed = 400f;        
    public float maxRise = 100f;      
    public float waitTimeAtPeak = 0.5f; 
    
    [HideInInspector] public PelusSpawner spawner;
    [HideInInspector] public int benimDelikIndexim = -1;

    private Vector2 startPos;
    private bool returning = false;
    private bool isWaiting = false; 
    private RectTransform rt;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        startPos = rt.anchoredPosition;
    }

    void Update()
    {
        if (transform.parent != null && transform.parent.name == "Claw") return;

        if (isWaiting) return;

        if (!returning)
        {
            rt.anchoredPosition += Vector2.up * speed * Time.deltaTime;
            if (rt.anchoredPosition.y >= startPos.y + maxRise) 
            {
                StartCoroutine(WaitRoutine());
            }
        }
        else
        {
            rt.anchoredPosition += Vector2.down * speed * Time.deltaTime;
            if (rt.anchoredPosition.y <= startPos.y) 
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator WaitRoutine()
    {
        isWaiting = true; 
        yield return new WaitForSeconds(waitTimeAtPeak); 
        returning = true; 
        isWaiting = false; 
    }

    void OnDestroy()
    {
        if (spawner != null && benimDelikIndexim != -1)
        {
            spawner.KapiniAc(benimDelikIndexim);
        }
    }
}
