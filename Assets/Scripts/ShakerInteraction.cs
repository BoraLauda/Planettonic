using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ShakerInteraction : MonoBehaviour, IDropHandler, IPointerClickHandler
{
   
    public static ShakerInteraction Instance;
    

    public int currentIceCount = 0;
    public int requiredIce = 2;

    [Header("Kapak Ayarları")]
    public bool kapakAcikMi = false;
    
    public GameObject kapaliShakerObjesi; 
    public GameObject acikShakerObjesi;   

    [Header("Blup (Ölçek) Efekti")]
    public float bumpUpScale = 1.15f; 
    public float bumpDuration = 0.1f; 
    private Vector3 originalScale;   

    void Awake()
    {
      
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    

        originalScale = transform.localScale;
    }

    void Start()
    {
        ResetShaker(); 
    }

    public void ResetShaker()
    {
        currentIceCount = 0;
        kapakAcikMi = false; 
        
        if (kapaliShakerObjesi != null) kapaliShakerObjesi.SetActive(true);
        if (acikShakerObjesi != null) acikShakerObjesi.SetActive(false);
    }
    
    void OnEnable()
    {
        ResetShaker();
    }

  
    public void TriggerBlup()
    {
        if (gameObject.activeInHierarchy)
        {
            StopCoroutine("BumpScale"); 
            StartCoroutine("BumpScale");
        }
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
                    Debug.Log("Kapak kapandı, Çalkalama (Shake) başlıyor!");
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
                // Katı malzemeler atıldığında (Blup içeriden tetikleniyor)
                if (draggedItem.itemName == "Buz" || draggedItem.itemName == "Portakal" || 
                    draggedItem.itemName == "Nane" || draggedItem.itemName == "Cilek" || draggedItem.itemName == "Zeytin")
                {
                    draggedItem.isLocked = true;
                    
                    if (KokteylManager.Instance != null)
                    {
                        if (draggedItem.itemName == "Buz") KokteylManager.Instance.BuzEkle();
                        else if (draggedItem.itemName == "Portakal") KokteylManager.Instance.PortakalEkle();
                        else if (draggedItem.itemName == "Nane") KokteylManager.Instance.NaneEkle();
                        else if (draggedItem.itemName == "Cilek") KokteylManager.Instance.CilekEkle();
                        else if (draggedItem.itemName == "Zeytin") KokteylManager.Instance.ZeytinEkle();
                    }

                    if (draggedItem.itemName == "Buz") currentIceCount++;
                    
                    
                    TriggerBlup(); 
                 

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
                        Debug.Log("Kaşık atıldı, Karıştırma (Stir) başlıyor!");
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

    IEnumerator BumpScale()
    {
        Vector3 targetScale = new Vector3(originalScale.x * bumpUpScale, originalScale.y * bumpUpScale, originalScale.z);
        
        float elapsed = 0f;
        while (elapsed < bumpDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / bumpDuration);
            elapsed += Time.deltaTime;
            yield return null; 
        }
        
        transform.localScale = targetScale; 
        yield return new WaitForSeconds(0.02f);

        elapsed = 0f;
        while (elapsed < bumpDuration)
        {
            transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / bumpDuration);
            elapsed += Time.deltaTime;
            yield return null; 
        }
        
        transform.localScale = originalScale; 
    }
    
    private IEnumerator GecikmeliFazaGec(KokteylManager.GamePhase hedefFaz, float beklemeSuresi)
    {
        yield return new WaitForSeconds(beklemeSuresi);
        KokteylManager.Instance.StartPhase(hedefFaz);
    }
}