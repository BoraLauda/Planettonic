using UnityEngine;

public class HeartSpawner : MonoBehaviour
{
    [Header("Prefab'lar")] 
    public GameObject kalpPrefab;
    public GameObject kirikKalpPrefab;

    [Header("Göz Kararı Spawn Noktaları")] 
    public Transform kalpSpawnNoktasi;
    public Transform kirikKalpSpawnNoktasi;

    [Header("60 Saniyelik Oyun - Hız Ayarları")] 
    public float baslangicSpawnAraligi = 0.8f; 
    public float tabanKaymaHizi = 900f;        
    public float hizlanmaKatsayisi = 0.05f; 
    public float maxHizCarpani = 3.5f; 

    public static float globalHizCarpani = 1f;
    public static float AnlikHiz; 
    
    
    public static bool isGameActive = true;

    private float timer;

    void OnEnable()
    {
      
        globalHizCarpani = 1f;
        timer = 0f;
        AnlikHiz = tabanKaymaHizi;
        isGameActive = true; 
    }

    void Update()
    {
       
        if (!isGameActive) return;

        if (globalHizCarpani < maxHizCarpani)
        {
            globalHizCarpani += hizlanmaKatsayisi * Time.deltaTime;
        }

        AnlikHiz = tabanKaymaHizi * globalHizCarpani;

        float guncelSpawnAraligi = baslangicSpawnAraligi / globalHizCarpani;

        timer += Time.deltaTime;
        if (timer >= guncelSpawnAraligi)
        {
            Spawnla();
            timer = 0;
        }
    }

    void Spawnla()
    {
        if (kalpSpawnNoktasi == null || kirikKalpSpawnNoktasi == null) return;

        bool isKalp = Random.value > 0.55f;
        GameObject secilenPrefab = isKalp ? kalpPrefab : kirikKalpPrefab;
        Transform secilenNokta = isKalp ? kalpSpawnNoktasi : kirikKalpSpawnNoktasi;

        GameObject yeniItem = Instantiate(secilenPrefab, secilenNokta.parent);
        yeniItem.transform.position = secilenNokta.position;
        
        yeniItem.AddComponent<ItemHareketi>();
    }

    public class ItemHareketi : MonoBehaviour
    {
        private RectTransform myRect;

        void Start()
        {
            myRect = GetComponent<RectTransform>();
        }

        void Update()
        {
           
            if (!HeartSpawner.isGameActive) return;

            myRect.anchoredPosition += Vector2.left * HeartSpawner.AnlikHiz * Time.deltaTime;
            
            if (myRect.anchoredPosition.x < -2000f) 
            {
                Destroy(gameObject);
            }
        }
    }
}