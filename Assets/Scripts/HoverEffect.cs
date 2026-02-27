using UnityEngine;
using UnityEngine.EventSystems;
public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    public float buyumeMiktari = 1.2f; 
    public float hiz = 10f; 

    private Vector3 orjinalBoyut;
    private Vector3 hedefBoyut;

    void Start()
    {
        orjinalBoyut = transform.localScale;
        hedefBoyut = orjinalBoyut;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, hedefBoyut, Time.deltaTime * hiz);
    }

  
    public void OnPointerEnter(PointerEventData eventData)
    {
        hedefBoyut = orjinalBoyut * buyumeMiktari;
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        hedefBoyut = orjinalBoyut; 
    }
}
