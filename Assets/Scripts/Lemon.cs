using UnityEngine;
using UnityEngine.EventSystems;

public class Lemon : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Sıkma Ayarları")]
    public float currentJuice = 0f;      
    public float requiredJuice = 2f;     
    
    public bool canSqueeze = false;

    private bool isSqueezing = false;
    private bool isFinished = false;

    void Update()
    {
        if (isSqueezing && !isFinished && canSqueeze)
        {
            currentJuice += Time.deltaTime;

            float shrinkAmount = Mathf.Lerp(1f, 0.6f, currentJuice / requiredJuice);
            transform.localScale = new Vector3(shrinkAmount, shrinkAmount, 1f);

            if (currentJuice >= requiredJuice)
            {
                isFinished = true;
                isSqueezing = false;
                
                if (KokteylManager.Instance != null)
                {
                    KokteylManager.Instance.LimonEkle();
                }
                
                gameObject.SetActive(false);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isFinished && canSqueeze)
        {
            isSqueezing = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isSqueezing = false;
    }
}