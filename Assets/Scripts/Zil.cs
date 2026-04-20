using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ZilButonu : MonoBehaviour, IPointerClickHandler
{
    [Header("Animasyon Ayarları")]
    public float inmeMiktari = 15f; 
    public float animasyonHizi = 0.1f; 
    
    private Vector3 orjinalPozisyon;
    private bool basildiMi = false;

    void Start()
    {
        orjinalPozisyon = transform.localPosition;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (basildiMi) return; 
        StartCoroutine(ZilAnimasyonuVeBitir());
    }

    private IEnumerator ZilAnimasyonuVeBitir()
    {
        basildiMi = true;

       
        Vector3 hedefPozisyon = orjinalPozisyon - new Vector3(0, inmeMiktari, 0);
        yield return StartCoroutine(MoveZil(orjinalPozisyon, hedefPozisyon, animasyonHizi));
        
        yield return StartCoroutine(MoveZil(hedefPozisyon, orjinalPozisyon, animasyonHizi));
        
        if (KokteylManager.Instance != null)
        {
            KokteylManager.Instance.StartPhase(KokteylManager.GamePhase.Finished);
        }

        basildiMi = false;
    }

    private IEnumerator MoveZil(Vector3 start, Vector3 end, float time)
    {
        float elapsed = 0f;
        while (elapsed < time)
        {
            transform.localPosition = Vector3.Lerp(start, end, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = end;
    }
}