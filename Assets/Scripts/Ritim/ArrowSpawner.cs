using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro; 

public class ArrowSpawner : MonoBehaviour
{
    public GameObject blueNotePrefab;
    public GameObject pinkNotePrefab;
    public RectTransform[] spawnPoints;
    public LaneUI[] lanes;

    public float initialSpawnInterval = 1f;
    public float baseFallSpeed = 400f; 
    public float gameDuration = 20f;
    
    public TextMeshProUGUI sureText;

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
        
        if (sureText != null)
        {
            int kalanSaniye = Mathf.CeilToInt(Mathf.Max(0, gameTimer));
            sureText.text = kalanSaniye.ToString();
            
            if (gameTimer <= 5f && gameTimer > 0f)
            {
                sureText.color = Color.red;
                
                float saniyeKusurati = gameTimer % 1f;
                
                float palseScale = 1f + (saniyeKusurati * 2f); 
                
                sureText.transform.localScale = new Vector3(palseScale, palseScale, 1f);
            }
            else
            {
                sureText.color = Color.white; 
                sureText.transform.localScale = Vector3.one; 
            }
        }

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

        if (sureText != null)
        {
            sureText.transform.localScale = Vector3.one;
            sureText.text = "0";
        }

        Debug.Log("SÜRE BİTTİ! OYUN TAMAMLANDI.");
        
        brainDate bd = FindFirstObjectByType<brainDate>();
        if (bd != null)
        {
          
            float kazanilanYildiz = 1f;
            int kazanilanKalp = 10;

            bd.EndRhythmGame(kazanilanYildiz, kazanilanKalp, TargetCharacter.Both);
        }
        else
        {
            
            transform.parent.gameObject.SetActive(false);
        }
    }
}