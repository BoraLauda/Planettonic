using UnityEngine;
using System.Collections; 

public class Dragon : MonoBehaviour
{
    public GameObject alevTopuPrefab;
    public Transform atesNoktasi;    
    public float saldiriAraligi = 3f;
    
    public int can = 2;

    private Animator anim;
    private float sayac;
    
    private SpriteRenderer sr;
    private Color orijinalRenk;
    private Rigidbody2D rb;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>(); 
        
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            orijinalRenk = sr.color;
        }

        sayac = saldiriAraligi; 
    }

    void Update()
    {
        sayac -= Time.deltaTime;
        
        if (sayac <= 0f)
        {
            anim.SetTrigger("Attack"); 
            sayac = saldiriAraligi;
        }
    }

    public void AgizAcildiOlayi()
    {
        anim.speed = 0f;
        
        AlevFirlat();
        
        StartCoroutine(AlevSonrasiBekle());
    }

    IEnumerator AlevSonrasiBekle()
    {
        yield return new WaitForSeconds(0.4f);
        
        anim.speed = 1f;
    }

    void AlevFirlat()
    {
        Instantiate(alevTopuPrefab, atesNoktasi.position, atesNoktasi.rotation, transform.parent);
    }

    public void HasarAl()
    {
        if (can <= 0) return;

        can--;
        Debug.Log("Ejderha hasar aldı Kalan can: " + can);

        if (can <= 0)
        {
            Debug.Log("Ejderha Öldü");
            StartCoroutine(OlumEfekti());
        }
        else
        {
            StartCoroutine(HasarEfekti());
        }
    }
    
   IEnumerator HasarEfekti()
    {
        if (sr != null) sr.color = new Color(1f, 0.4f, 0.4f);
        
        float sekmeYonu = Mathf.Sign(transform.localScale.x) * 1f; 
        
        Vector3 baslangicKordinati = transform.position;
        Vector3 hedefKordinat = baslangicKordinati + new Vector3(sekmeYonu * 0.2f, 0.3f, 0f);
       
        float gecenSure = 0f;
        while(gecenSure < 0.1f)
        {
            gecenSure += Time.deltaTime;
            transform.position = Vector3.Lerp(baslangicKordinati, hedefKordinat, gecenSure / 0.1f);
            yield return null;
        }
        
        yield return new WaitForSeconds(0.1f);
      
        if (sr != null) sr.color = orijinalRenk;
        
        transform.position = baslangicKordinati; 
    }

    IEnumerator OlumEfekti()
    {
        this.enabled = false;
        
        if (sr != null) sr.color = new Color(1f, 0.2f, 0.2f); 
        
        Vector3 baslangic = transform.position;
        Vector3 hedef = baslangic + new Vector3(0f, 0.5f, 0f);
        
        float gecenSure = 0;
        Vector3 baslangicBoyutu = transform.localScale;
        
        while(gecenSure < 0.2f)
        {
            gecenSure += Time.deltaTime;
            float t = gecenSure / 0.2f;
            
            transform.position = Vector3.Lerp(baslangic, hedef, t);
            transform.localScale = Vector3.Lerp(baslangicBoyutu, Vector3.zero, t);
            
            yield return null;
        }

        Destroy(gameObject);
    }
}