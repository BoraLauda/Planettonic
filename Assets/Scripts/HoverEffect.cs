using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float buyumeMiktari = 1.2f; 
    public float hiz = 10f; 

    private Vector3 orjinalBoyut;
    private Vector3 hedefBoyut;
    
    private Button buton; 

    void Start()
    {
        orjinalBoyut = transform.localScale;
        hedefBoyut = orjinalBoyut;
        
        buton = GetComponent<Button>(); 
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, hedefBoyut, Time.deltaTime * hiz);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buton != null && buton.interactable == false)
        {
            return;
        }
        
        hedefBoyut = orjinalBoyut * buyumeMiktari;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hedefBoyut = orjinalBoyut; 
    }
}