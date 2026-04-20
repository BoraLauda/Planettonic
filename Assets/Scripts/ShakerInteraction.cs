using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ShakerInteraction : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public int currentIceCount = 0;
    public int requiredIce = 2;

    [Header("Kapak Ayarları")]
    public bool kapakAcikMi = false;
    
    public GameObject kapaliShakerObjesi; 
    public GameObject acikShakerObjesi;   

    void Start()
    {
        if (kapaliShakerObjesi != null) kapaliShakerObjesi.SetActive(true);
        if (acikShakerObjesi != null) acikShakerObjesi.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        bool oncekiDurum = kapakAcikMi; 
        kapakAcikMi = !kapakAcikMi; 
        
        if (kapaliShakerObjesi != null) kapaliShakerObjesi.SetActive(!kapakAcikMi);
        if (acikShakerObjesi != null) acikShakerObjesi.SetActive(kapakAcikMi);
        
        if (oncekiDurum == true && kapakAcikMi == false)
        {
            if (KokteylManager.Instance != null)
            {
                if (!KokteylManager.Instance.isShaken)
                {
                    Debug.Log("Kapak kapandı");
                    StartCoroutine(GecikmeliFazaGec(KokteylManager.GamePhase.Shaking, 0.5f));
                }
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!kapakAcikMi)
        {
            Debug.Log("Shaker kapalı! Eşya atılamaz.");
            return; 
        }

        if (eventData.pointerDrag != null)
        {
            DraggableItem draggedItem = eventData.pointerDrag.GetComponent<DraggableItem>();

            if (draggedItem != null)
            {
                if (draggedItem.itemName == "Buz")
                {
                    draggedItem.isLocked = true;
                    currentIceCount++;
                    Debug.Log("Shaker içindeki buz: " + currentIceCount);
                    
                    if (KokteylManager.Instance != null) KokteylManager.Instance.BuzEkle();
                    
                    Destroy(draggedItem.gameObject); 
                }
                
               
                else if (draggedItem.itemName == "Portakal" || draggedItem.itemName == "Nane" || 
                         draggedItem.itemName == "Cilek" || draggedItem.itemName == "Zeytin")
                {
                    draggedItem.isLocked = true;
                    
                    if (KokteylManager.Instance != null)
                    {
                        if (draggedItem.itemName == "Portakal") KokteylManager.Instance.PortakalEkle();
                        else if (draggedItem.itemName == "Nane") KokteylManager.Instance.NaneEkle();
                        else if (draggedItem.itemName == "Cilek") KokteylManager.Instance.CilekEkle();
                        else if (draggedItem.itemName == "Zeytin") KokteylManager.Instance.ZeytinEkle();
                    }
                    
                    Debug.Log(draggedItem.itemName + " shaker'a atıldı!");
                    Destroy(draggedItem.gameObject); 
                }
              

                else if (draggedItem.itemName == "Sos")
                {
                    draggedItem.isLocked = true; 
                    SauceBottle sauce = draggedItem.GetComponent<SauceBottle>();
                    if (sauce != null) sauce.StartAutomaticPour(this.transform); 
                }
                else if (draggedItem.itemName == "Limon")
                {
                    draggedItem.isLocked = true;
                    draggedItem.GetComponent<RectTransform>().position = transform.position;
                    Lemon squeeze = draggedItem.GetComponent<Lemon>();
                    if (squeeze != null) squeeze.canSqueeze = true;
                }
                else if (draggedItem.itemName == "Kasik")
                {
                    if (KokteylManager.Instance != null && !KokteylManager.Instance.isStirred)
                    {
                        draggedItem.ForceTurn();
                        StartCoroutine(GecikmeliFazaGec(KokteylManager.GamePhase.Stirring, 0.4f));
                    }
                    else
                    {
                        draggedItem.ForceTurn();
                    }
                }
                
                else if (draggedItem.itemName.StartsWith("Bardak_"))
                {
                    string indexString = draggedItem.itemName.Substring(draggedItem.itemName.Length - 1);
                    int bIndex = int.Parse(indexString);
                    
                    if (KokteylManager.Instance != null)
                    {
                        KokteylManager.Instance.secilenBardakIndex = bIndex;
                    }

                    draggedItem.ForceTurn();
                    StartCoroutine(GecikmeliFazaGec(KokteylManager.GamePhase.Pouring, 0.4f));
                }
            }
        }
    }
    
    private IEnumerator GecikmeliFazaGec(KokteylManager.GamePhase hedefFaz, float beklemeSuresi)
    {
        yield return new WaitForSeconds(beklemeSuresi);
        KokteylManager.Instance.StartPhase(hedefFaz);
    }
}