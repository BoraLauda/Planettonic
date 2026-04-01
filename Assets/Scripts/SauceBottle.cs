using UnityEngine;
using System.Collections;

public class SauceBottle : MonoBehaviour
{
    public GameObject sauceEffect; 
    
    [Header("Sos Ayarları")]
    public string sosTuru = "Kirmizi"; 

    [Header("Animasyon Ayarları")]
    public float moveSpeed = 4f; 
    public float pourWaitTime = 0.8f; 
    
    private DraggableItem draggableItem;
    private RectTransform rectTransform;
    private Quaternion startRotation;

    void Awake()
    {
        draggableItem = GetComponent<DraggableItem>();
        rectTransform = GetComponent<RectTransform>();
        startRotation = rectTransform.localRotation; 
        
        if (sauceEffect != null) 
            sauceEffect.SetActive(false);
    }
    
    public void StartAutomaticPour(Transform shakerTransform)
    {
        StartCoroutine(PourSequence(shakerTransform));
    }

    IEnumerator PourSequence(Transform shakerTransform)
    {
        Vector2 baslangicNoktasi = rectTransform.anchoredPosition;
        Quaternion baslangicAcisi = rectTransform.localRotation;
        
        rectTransform.position = shakerTransform.position;
       
        Vector2 hedefNokta = rectTransform.anchoredPosition + new Vector2(-50f, 100f); 
        rectTransform.anchoredPosition = baslangicNoktasi; 
        
        Quaternion hedefAci = Quaternion.Euler(0, 0, -135f); 

        float t = 0;
        
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            rectTransform.anchoredPosition = Vector2.Lerp(baslangicNoktasi, hedefNokta, t);
            rectTransform.localRotation = Quaternion.Lerp(baslangicAcisi, hedefAci, t);
            yield return null;
        }

        rectTransform.anchoredPosition = hedefNokta;
        rectTransform.localRotation = hedefAci;
        
        yield return new WaitForSeconds(0.2f);

        if (sauceEffect != null) sauceEffect.SetActive(true);
        
        if (KokteylManager.Instance != null)
        {
            KokteylManager.Instance.EklenenSosuKaydet(sosTuru);
            Debug.Log(sosTuru + " sos bardağa eklendi!");
        }
       
       
        yield return new WaitForSeconds(pourWaitTime);

        if (sauceEffect != null) sauceEffect.SetActive(false);
        yield return new WaitForSeconds(0.2f);

        rectTransform.localRotation = startRotation;
        rectTransform.anchoredPosition = draggableItem.startPosition; 
        draggableItem.isLocked = false;
    }
}