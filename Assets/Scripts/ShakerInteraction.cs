using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

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

    [Header("Masa Kontrolü")]
    public bool masadaMi = false; 
    public RectTransform masaMerkezi; 
    public float kabulEdilebilirMesafe = 50f; 

    [Header("UI Uyarı Sistemi")]
    public TMP_Text emptyWarningText; 
    public float warningDuration = 1.5f; 
    private Coroutine warningCoroutine;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    
        originalScale = transform.localScale;
    }

    void Start()
    {
        ResetShaker(); 
        if (emptyWarningText != null) emptyWarningText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (masaMerkezi != null)
        {
            float mesafe = Vector2.Distance(GetComponent<RectTransform>().anchoredPosition, masaMerkezi.anchoredPosition);
            masadaMi = (mesafe < kabulEdilebilirMesafe);
        }
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

    
    public void ShowEmptyWarning()
    {
        if (warningCoroutine != null) StopCoroutine(warningCoroutine);
        warningCoroutine = StartCoroutine(HideWarningRoutine());
    }

    IEnumerator HideWarningRoutine()
    {
        if (emptyWarningText != null)
        {
            emptyWarningText.text = "Shaker is empty!";
            emptyWarningText.gameObject.SetActive(true);
            yield return new WaitForSeconds(warningDuration);
            emptyWarningText.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!masadaMi)
        {
            Debug.Log("Shaker henüz masanın ortasında değil! Kapak açılamaz.");
            return;
        }

        
        if (kapakAcikMi && KokteylManager.Instance != null && KokteylManager.Instance.IsShakerEmpty())
        {
            ShowEmptyWarning();
            return;
        }

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
        if (!masadaMi)
        {
            Debug.Log("Shaker henüz masanın ortasında değil! İçine bir şey atılamaz.");
            return;
        }

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
                if (draggedItem.itemName == "Buz" || draggedItem.itemName == "Portakal" || 
                    draggedItem.itemName == "Nane" || draggedItem.itemName == "Cilek" || 
                    draggedItem.itemName == "Zeytin" || draggedItem.itemName == "Limon")
                {
                    draggedItem.isLocked = true;
                    
                    if (KokteylManager.Instance != null)
                    {
                        if (draggedItem.itemName == "Buz") KokteylManager.Instance.BuzEkle();
                        else if (draggedItem.itemName == "Portakal") KokteylManager.Instance.PortakalEkle();
                        else if (draggedItem.itemName == "Nane") KokteylManager.Instance.NaneEkle();
                        else if (draggedItem.itemName == "Cilek") KokteylManager.Instance.CilekEkle();
                        else if (draggedItem.itemName == "Zeytin") KokteylManager.Instance.ZeytinEkle();
                        else if (draggedItem.itemName == "Limon") KokteylManager.Instance.LimonEkle(); 
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
               
                else if (draggedItem.itemName == "Kasik")
                {
                    if (KokteylManager.Instance != null)
                    {
                        if (KokteylManager.Instance.IsShakerEmpty())
                        {
                            draggedItem.ForceTurn(); 
                            ShowEmptyWarning(); 
                            return; 
                        }

                        if (!KokteylManager.Instance.isStirred)
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