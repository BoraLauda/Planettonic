using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class IceBreaker : MonoBehaviour
{
    public GameObject pauseImage; 
    public GameObject startImage;
    
    public int heartPerPink = 5;       
    public int heartPerYellow = 20;    

    public int bonusHeartOnWin = 20;   
    public float totalStarsOnWin = 2f;

    [Header("Görsel")]
    public RectTransform needle;      
    public RectTransform zonePink;
    public RectTransform zonePink1;
    public RectTransform zoneYellow; 
    
    [Header("Buz")]
    public Image iceImage;           
    public Sprite[] iceSprites; 
    
    public Image[] SnowImages; 
    private int currentHearts = 4;
    
    public GameObject piecePrefab; 
    public Transform pieceSpawnPoint;  
    public List<Sprite> pieceSprites; 
    public float explosion = 800f;
    

    [Header("Ayarlar")]
    public float speed = 1200f;        
    private int currentIceLevel = 0; 
    public float shakeStrength = 5f;
    public float strikeDistance = 30f; 
    public float strikeSpeed = 15f; 
    
    private bool movingUp = true;     
    private float topLimit;           
    private float bottomLimit;
    
    public GameObject successVisuals;
    private bool isGameActive = false;
    private bool isStriking = false;
    
    private bool isLeftTurn = true;
    
    private int pendingDamage = 0;
    
    private brainDate dateManager;

    void Start()
    {
       dateManager = FindFirstObjectByType<brainDate>();

        if (needle != null && needle.parent != null)
        {
            RectTransform barRect = needle.parent.GetComponent<RectTransform>();
            float barHeight = barRect.rect.height;
            topLimit = barHeight / 2f;
            bottomLimit = -barHeight / 2f;
        }
    }
    
    public void StartGame()
    {
        gameObject.SetActive(true);
        isGameActive = true;
        currentHearts = 4;
        currentIceLevel = 0;
        isLeftTurn = true; 
        speed = 1200f; 

        foreach (var h in SnowImages) h.gameObject.SetActive(true);
        if (iceSprites.Length > 0) iceImage.sprite = iceSprites[0];
        iceImage.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!isGameActive || isStriking) return;
        
        MoveNeedle();
        
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            CheckHit();
        }
    }
    
    void MoveNeedle()
    {
        Vector3 pos = needle.anchoredPosition;
        if (movingUp) {
            pos.y += speed * Time.deltaTime;
            if (pos.y >= topLimit) movingUp = false; 
        } else {
            pos.y -= speed * Time.deltaTime;
            if (pos.y <= bottomLimit) movingUp = true; 
        }
        needle.anchoredPosition = pos;
    }
    
    void CheckHit()
    {
        float currentY = needle.anchoredPosition.y;
        DialogueDataları selectedDialogue = null;
        pendingDamage = 0; 

      
        TargetCharacter currentTarget = isLeftTurn ? TargetCharacter.Left : TargetCharacter.Right;
        Characters currentCharData = isLeftTurn ? DateSettings.leftChar : DateSettings.rightChar;

        
        if (ZoneDetect(currentY, zoneYellow))
        {
            Debug.Log("SARI");
            pendingDamage = 2; 
        
            if(dateManager != null) 
                dateManager.AddReward(0, heartPerYellow, currentTarget);

            if (currentCharData != null)
                selectedDialogue = GetRandomQuestion(currentCharData.iceBreakerGood);
        }
        
        else if (ZoneDetect(currentY, zonePink) || ZoneDetect(currentY, zonePink1))
        {
            Debug.Log("PEMBE");
            pendingDamage = 1; 

            if(dateManager != null) 
                dateManager.AddReward(0, heartPerPink, currentTarget);
            
            if (currentCharData != null)
                selectedDialogue = GetRandomQuestion(currentCharData.iceBreakerMid);
        }
       
        else
        {
            Debug.Log("BOŞA");
            pendingDamage = 0; 
        
            if (currentCharData != null)
                selectedDialogue = GetRandomQuestion(currentCharData.iceBreakerBad);
        }

        StartCoroutine(StrikeSequence(selectedDialogue));
    }
    
    IEnumerator StrikeSequence(DialogueDataları dialogToPlay)
    {
        isStriking = true; 
        isGameActive = false; 

        Vector3 originalPos = needle.anchoredPosition;
        Vector3 targetPos = originalPos + new Vector3(-strikeDistance, 0, 0); 

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * strikeSpeed;
            needle.anchoredPosition = Vector3.Lerp(originalPos, targetPos, t);
            yield return null;
        }
    
        if (pendingDamage > 0)
        {
            ApplyIceDamage(pendingDamage);
            SpawnShards(); 
            yield return new WaitForSeconds(1.0f); 
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
        }
    
        if (dialogToPlay != null && dateManager != null)
        {
            if(pauseImage != null) pauseImage.SetActive(true);
            if(startImage != null) startImage.SetActive(false);
            
            dateManager.PlayIceBreakerDialogue(dialogToPlay);
        }
        else
        {
            ResumeGame();
        }

        needle.anchoredPosition = originalPos;
    }
    
    void SpawnShards()
    {
        if (piecePrefab == null || pieceSpawnPoint == null) return;

        int count = Random.Range(3, 6); 

        for (int i = 0; i < count; i++)
        {
            GameObject shard = Instantiate(piecePrefab, pieceSpawnPoint.position, Quaternion.identity, pieceSpawnPoint.parent);
            
            if (pieceSprites.Count > 0)
                shard.GetComponent<Image>().sprite = pieceSprites[Random.Range(0, pieceSprites.Count)];

            Rigidbody2D rb = shard.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 dir = new Vector2(Random.Range(-1f, 1f), Random.Range(0.2f, 1f)).normalized;
                rb.AddForce(dir * explosion);
                rb.AddTorque(Random.Range(-200f, 200f)); 
            }
            Destroy(shard, 2.0f);
        }
    }

    public void ResumeGame()
    {
        StartCoroutine(ResumeSequence());
    }
    
    IEnumerator ResumeSequence()
    {
        if(pauseImage != null) pauseImage.SetActive(false);
        if(startImage != null) startImage.SetActive(true);
        
        yield return new WaitForSeconds(1.0f);

        if(startImage != null) startImage.SetActive(false);
        
        LoseHeart(); 
    
        if (currentIceLevel >= iceSprites.Length) 
        {
            StartCoroutine(Finishing(true));
        }
        else if (currentHearts <= 0) 
        {
            StartCoroutine(Finishing(false));
        }
        else
        {
            isLeftTurn = !isLeftTurn; 
            isStriking = false;
            isGameActive = true;  
        }
    }
    
    bool ZoneDetect(float needleY, RectTransform zone)
    {
        if (zone == null) return false;
        float zoneY = zone.anchoredPosition.y;   
        float halfHeight = zone.rect.height / 2f;
        return (needleY <= zoneY + halfHeight && needleY >= zoneY - halfHeight);
    }
    
    void ApplyIceDamage(int amount)
    {
        currentIceLevel += amount;

        if (speed < 1500) speed = 1500;
        else if (speed < 1800) speed = 1800;
        else speed = 2250;
        
        int indexToShow = Mathf.Clamp(currentIceLevel, 0, iceSprites.Length - 1);
        if (indexToShow < iceSprites.Length)
        {
            iceImage.sprite = iceSprites[indexToShow];
        }
    }

    void LoseHeart()
    {
        currentHearts--;
        if (currentHearts >= 0 && currentHearts < SnowImages.Length)
        {
            SnowImages[currentHearts].gameObject.SetActive(false);
        }
    }

    IEnumerator Finishing(bool isSuccess)
    {
        if (isSuccess)
        {
            dateManager.AddReward(totalStarsOnWin, bonusHeartOnWin, TargetCharacter.Both);
            
            float duration = 1.0f;
            float elapsed = 0f;
            Vector3 originalPos = iceImage.rectTransform.anchoredPosition;
            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * shakeStrength;
                float y = Random.Range(-1f, 1f) * shakeStrength;
                iceImage.rectTransform.anchoredPosition = originalPos + new Vector3(x, y, 0);
                elapsed += Time.deltaTime;
                yield return null; 
            }
            iceImage.rectTransform.anchoredPosition = originalPos;
            iceImage.gameObject.SetActive(false); 
            if (successVisuals != null) successVisuals.SetActive(true);
        }

        yield return new WaitForSeconds(1.5f);

        if (dateManager != null)
        {
            dateManager.EndIceBreaker(isSuccess);
        }
        
        gameObject.SetActive(false); 
    }
    
    DialogueDataları GetRandomQuestion(List<DialogueDataları> list)
    {
        if (list == null || list.Count == 0) 
        {
            Debug.LogWarning("Daha soru kalmadı(Bunun olmaması lazım..)");
            return null;
        }
        
        int randomIndex = Random.Range(0, list.Count);
        DialogueDataları selectedQ = list[randomIndex];
        list.RemoveAt(randomIndex);
        return selectedQ;
    }
}