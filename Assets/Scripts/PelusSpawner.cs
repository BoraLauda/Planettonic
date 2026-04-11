using UnityEngine;
using System.Collections.Generic;

public class PelusSpawner : MonoBehaviour
{
    [Header("Referanslar")]
    public GameObject bearPrefab;
    public GameObject bunnyPrefab;
    public GameObject foxPrefab;
    public RectTransform[] spawnHoles; 
    public RectTransform pelusContainer; 
    
    public Claw clawScript; 

    [Header("Ayarlar")]
    public float spawnInterval = 1.5f; 
    public float spawnY_Offset = -50f; 

    private float timer = 0f;
    private bool isGameActive = true; 

    [HideInInspector] public bool[] doluDelikler;
    [HideInInspector] public List<RectTransform> activeToys = new List<RectTransform>();

    void Start()
    {
        if (spawnHoles != null)
        {
            doluDelikler = new bool[spawnHoles.Length];
        }
    }

    void Update()
    {
        if (!isGameActive) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SpawnToy();
            timer = spawnInterval;
        }

        activeToys.RemoveAll(item => item == null);
    }

    void SpawnToy()
    {
        if (spawnHoles == null || pelusContainer == null || clawScript == null) return;

      
        List<int> bosDelikler = new List<int>();
        for (int i = 0; i < doluDelikler.Length; i++)
        {
            if (!doluDelikler[i]) bosDelikler.Add(i);
        }
        if (bosDelikler.Count == 0) return;

        List<GameObject> uygunPrefablar = new List<GameObject>();

        if (clawScript.bearCount == 0 && !IsToyAlreadyInGame("bear")) 
            uygunPrefablar.Add(bearPrefab);
            
        if (clawScript.bunnyCount == 0 && !IsToyAlreadyInGame("bunny")) 
            uygunPrefablar.Add(bunnyPrefab);
            
        if (clawScript.foxCount == 0 && !IsToyAlreadyInGame("fox")) 
            uygunPrefablar.Add(foxPrefab);

       
        if (uygunPrefablar.Count == 0) return;

       
        GameObject secilenPrefab = uygunPrefablar[Random.Range(0, uygunPrefablar.Count)];
        int secilenHoleIndex = bosDelikler[Random.Range(0, bosDelikler.Count)];
        
        doluDelikler[secilenHoleIndex] = true;

        GameObject newToy = Instantiate(secilenPrefab, pelusContainer);
        RectTransform toyRect = newToy.GetComponent<RectTransform>();

     
        var logic = newToy.GetComponent<Peluslar>();
        if (logic != null)
        {
            logic.spawner = this;
            logic.benimDelikIndexim = secilenHoleIndex;
        }

        Vector3 localPos = pelusContainer.InverseTransformPoint(spawnHoles[secilenHoleIndex].position);
        toyRect.anchoredPosition = new Vector2(localPos.x, localPos.y + spawnY_Offset);
        
        activeToys.Add(toyRect);
    }
    
    bool IsToyAlreadyInGame(string typeName)
    {
        foreach (var toy in activeToys)
        {
            if (toy != null && toy.gameObject.name.ToLower().Contains(typeName))
                return true;
        }
        return false;
    }

    public void KapiniAc(int index)
    {
        if (index >= 0 && index < doluDelikler.Length)
            doluDelikler[index] = false;
    }

    public void StopSpawning() { isGameActive = false; }
}