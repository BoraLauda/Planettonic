using UnityEngine;

public class HeartSpawner : MonoBehaviour
{
    [Header("Prefab'lar")] public GameObject kalpPrefab;
    public GameObject kirikKalpPrefab;

    [Header("Göz Kararı Spawn Noktaları")] public Transform kalpSpawnNoktasi;

    [Tooltip("Kırık kalplerin yerde doğacağı boş obje")]
    public Transform kirikKalpSpawnNoktasi;

    [Header("Ayarlar")] public float spawnAraligi = 2.5f;
    public float kaymaHizi = 200f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnAraligi)
        {
            Spawnla();
            timer = 0;
        }
    }

    void Spawnla()
    {

        if (kalpSpawnNoktasi == null || kirikKalpSpawnNoktasi == null)
        {
            return;
        }

        bool isKalp = Random.value > 0.5f;

        GameObject secilenPrefab = isKalp ? kalpPrefab : kirikKalpPrefab;
        Transform secilenNokta = isKalp ? kalpSpawnNoktasi : kirikKalpSpawnNoktasi;

        GameObject yeniItem = Instantiate(secilenPrefab, secilenNokta.parent);

        yeniItem.transform.position = secilenNokta.position;
        ItemHareketi hareket = yeniItem.AddComponent<ItemHareketi>();
        hareket.hiz = kaymaHizi;
    }

    public class ItemHareketi : MonoBehaviour
    {
        public float hiz;
        private RectTransform myRect;

        void Start()
        {
            myRect = GetComponent<RectTransform>();
        }

        void Update()
        {

            myRect.anchoredPosition += Vector2.left * hiz * Time.deltaTime;
            if (myRect.anchoredPosition.x < -1500f)
            {
                Destroy(gameObject);
            }
        }
    }
}
