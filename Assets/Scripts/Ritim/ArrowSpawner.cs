using UnityEngine;
using System.Collections.Generic;

public class NoteSpawner : MonoBehaviour
{
    public GameObject blueNotePrefab;
    public GameObject pinkNotePrefab;
    public RectTransform[] spawnPoints;
    public LaneUI[] lanes;

    public float initialSpawnInterval = 1f;
    public float baseFallSpeed = 400f; 
    public float gameDuration = 20f;

    private float timer;
    private float gameTimer;
    private bool isGameOver = false;

    void Start()
    {
        gameTimer = gameDuration;
        timer = initialSpawnInterval;
    }

    void Update()
    {
        if (isGameOver) return;

        gameTimer -= Time.deltaTime;
        
        float progress = (gameDuration - gameTimer) / gameDuration;
        float spawnDifficulty = 1f + (progress * 2.0f); 
        float speedDifficulty = 1f + (progress * 0.3f); 

        if (gameTimer <= 0)
        {
            GameOver();
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnRandomNote(speedDifficulty);
            timer = initialSpawnInterval / spawnDifficulty;
        }
    }

    void SpawnRandomNote(float currentSpeedScale)
    {
        int randomLane = Random.Range(0, 4);
        GameObject noteToSpawn = (randomLane == 0 || randomLane == 3) ? blueNotePrefab : pinkNotePrefab;

        GameObject newNote = Instantiate(noteToSpawn, spawnPoints[randomLane]);
        RectTransform noteRect = newNote.GetComponent<RectTransform>();
        noteRect.anchoredPosition = Vector2.zero;

        float rotationZ = 0f;
        switch (randomLane)
        {
            case 0: rotationZ = 180f; break;
            case 1: rotationZ = 180f; break;
            case 2: rotationZ = 0f; break;
            case 3: rotationZ = 0f; break;
        }

        noteRect.localRotation = Quaternion.Euler(0, 0, rotationZ);

        Arrows noteScript = newNote.GetComponent<Arrows>();
        noteScript.fallSpeed = baseFallSpeed * currentSpeedScale;
        lanes[randomLane].activeNotes.Add(noteScript);
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("SÜRE BİTTİ! OYUN TAMAMLANDI.");
    }
}