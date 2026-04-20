using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefablar; 
    public float spawnHizi = 2f;
    public float hareketHizi = 200f;
    public Transform spawnNoktası; 

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnHizi)
        {
            SpawnItem();
            timer = 0;
        }
    }

    void SpawnItem()
    {
        int rand = Random.Range(0, itemPrefablar.Length);
        GameObject yeniItem = Instantiate(itemPrefablar[rand], spawnNoktası.position, Quaternion.identity, transform);
        
        StartCoroutine(MoveItem(yeniItem));
    }

    System.Collections.IEnumerator MoveItem(GameObject item)
    {
        while (item != null)
        {
            item.transform.Translate(Vector3.left * hareketHizi * Time.deltaTime);
            if (item.transform.localPosition.x < -1000) Destroy(item);
            yield return null;
        }
    }
}