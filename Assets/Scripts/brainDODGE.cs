using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic; 
public class brainDODGE : MonoBehaviour
{
    public float roundStartDelay = 2.0f;
    
    public GameObject pauseImage;
    public GameObject startImage;
    
    public int baseHeartReward = 20;  
    public int penaltyPerHit = 5;      
    //public float successStarReward = 2f;
    
    
    public Image fadeImage;
    public float gameSaniye = 20f; 
    
    public Image centerCharacterImage;
    
    public List<DialogueDataları> questionsForIo;
    public List<DialogueDataları> questionsForElroi;
    
    public float startSpawnAralık = 1.5f;
    public float minSpawnAralık = 0.6f;   
    private float currentSpawnAralık;
    
    public float doubleSpawnChance = 0.5f; 
    public float timeToStartDouble = 5f;
    
    
    public float startSpeed = 500f;     
    public float maxSpeed = 1500f;      
    public float hızlanma = 50f;
    
    
    
    private float currentRocketSpeed;
    
    public RectTransform objectShake; 
    public float shakeDuration = 0.2f;  
    public float shakeStrength = 15f;   
    
    public GameObject rocketPrefab; 
    public Transform rocketParent;  
    public Transform[] spawnPoints; 
    public GameObject[] warningSigns; 

    private float timer;
    public bool isGameActive = true;
    private int currentTurn = 0; 
    private bool isLeftDodging = true;
    
    private int hitCount = 0;
    
    private brainDate dateManager;
    private Dictionary<List<DialogueDataları>, List<DialogueDataları>> questionPools = new Dictionary<List<DialogueDataları>, List<DialogueDataları>>();
    private float currentSpawnDelay;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dateManager = FindFirstObjectByType<brainDate>();
        SetAlpha(1f);
        
        if(pauseImage != null) pauseImage.SetActive(false);
        if(startImage != null) startImage.SetActive(false);
        
    }
    
    public void StartGame()
    {
        gameObject.SetActive(true);
        currentTurn = 0; 
        hitCount = 0;
        StartRound();
    }
    
    void StartRound()
    {
        if(pauseImage != null) pauseImage.SetActive(false);
        if(startImage != null) startImage.SetActive(false);
        
        SetAlpha(1f);
        foreach (Transform child in rocketParent)
        {
            Destroy(child.gameObject);
        }
        
        if (warningSigns != null)
        {
            foreach (GameObject sign in warningSigns)
            {
                if (sign != null) sign.SetActive(false);
            }
        }
        
        if (currentTurn % 2 == 0)
        {
            isLeftDodging = true;
            
            if(DateSettings.leftChar != null) 
            {
                
                if(DateSettings.leftChar.dodgeTheQuestionIkonu != null)
                    centerCharacterImage.sprite = DateSettings.leftChar.dodgeTheQuestionIkonu;
                else
                    centerCharacterImage.sprite = DateSettings.leftChar.profileIcon;
            }
            Debug.Log("Sıra: SOL KARAKTER KAÇIYOR (Tur " + (currentTurn+1) + ")");
        }
        else
        {
            isLeftDodging = false;
            
            if(DateSettings.rightChar != null) 
            {
               
                if(DateSettings.rightChar.dodgeTheQuestionIkonu != null)
                    centerCharacterImage.sprite = DateSettings.rightChar.dodgeTheQuestionIkonu;
                else
                    centerCharacterImage.sprite = DateSettings.rightChar.profileIcon;
            }
            Debug.Log("Sıra: SAĞ KARAKTER KAÇIYOR (Tur " + (currentTurn+1) + ")");
        }

        timer = gameSaniye;
        currentRocketSpeed = startSpeed;
        currentSpawnDelay = startSpawnAralık;
        isGameActive = true;

        StopAllCoroutines();
        StartCoroutine(SpawnLoop());
    }
    

    // Update is called once per frame
    void Update()
    {
        if (!isGameActive) return;

       
        timer -= Time.deltaTime;
        
        if (currentRocketSpeed < maxSpeed)
        {
            currentRocketSpeed += hızlanma * Time.deltaTime;
        }
        
        float timePassed = gameSaniye - timer;
        float progress = timePassed / gameSaniye; 
        
        currentSpawnAralık = Mathf.Lerp(startSpawnAralık, minSpawnAralık, progress);
        
        
        if (timer <= 0)
        {
            EndRound(true);
        }
    }

    public void TakeDamage()
    {
        if (!isGameActive) return;
      
        hitCount++;
        if (objectShake != null)
        {
            StopCoroutine("ShakeSequence");
            StartCoroutine("ShakeSequence");
        }
       
        EndRound(false);
    }
    
    
    void EndRound(bool isSuccess)
    {
        isGameActive = false; 
        StopCoroutine("SpawnLoop");
        
        if (warningSigns != null)
        {
            foreach (GameObject sign in warningSigns)
            {
                if (sign != null) sign.SetActive(false);
            }
        }

        
        if (isSuccess)
        {
            Debug.Log("Soru sorulmadan geçiliyo");
            NextTurn();
        }
       
        else
        {
            Debug.Log("VURULDUN");
            
            DialogueDataları questionToAsk = null;

            // --- YENİ SİSTEM: Soruları Karakter Dosyasından Çek ---
            if (isLeftDodging) 
            {
                if(DateSettings.leftChar != null) questionToAsk = GetRandomQuestion(DateSettings.leftChar.dodgeQuestions);
            }
            else 
            {
                if(DateSettings.rightChar != null) questionToAsk = GetRandomQuestion(DateSettings.rightChar.dodgeQuestions);
            }

            if (dateManager != null && questionToAsk != null)
            {
                if(pauseImage != null) pauseImage.SetActive(true);
                if(startImage != null) startImage.SetActive(false);
                
                dateManager.PlayDodgeDialogue(questionToAsk);
            }
            else
            {
                NextTurn();
            }
        }
    }
    
    public void ResumeAfterDialogue()
    {
        StartCoroutine(ResumeSequence());
    }
    
    IEnumerator ResumeSequence()
    {
        if(pauseImage != null) pauseImage.SetActive(false);

        if(startImage != null) startImage.SetActive(true);

        yield return new WaitForSeconds(1.0f);
        
        if(startImage != null) startImage.SetActive(false);

        SetAlpha(1f);
        NextTurn();
    }
    
    void SetAlpha(float alphaValue)
    {
        if (fadeImage != null)
        {
            Color tempColor = fadeImage.color;
            tempColor.a = alphaValue;
            fadeImage.color = tempColor;
        }
    }
    
    void NextTurn()
    {
        currentTurn++;

      
        if (currentTurn >= 4)
        {
            Debug.Log("OYUN BİTTİ");
            
          
            int finalHearts = baseHeartReward - (hitCount * penaltyPerHit);
            if (finalHearts < 0) finalHearts = 0; 

           
            float finalStars = 0;

            if (hitCount <= 1) 
            {
                finalStars = 1.0f; 
            }
            else if (hitCount <= 3) 
            {
                finalStars = 0.5f; 
            }
            else
            {
                finalStars = 0f;
            }
            
            if (dateManager != null)
            {
               
                dateManager.AddReward(finalStars, finalHearts, TargetCharacter.Both);
                
               
                dateManager.EndDodgeGame();
            }
        }
        else
        {
            StartCoroutine(WaitAndNextRound());
        }
    }
    
    IEnumerator WaitAndNextRound()
    {
        yield return new WaitForSeconds(0.5f);
        StartRound();
    }




    IEnumerator ShakeSequence()
    {
        Vector3 originalPos = objectShake.anchoredPosition;
        float elapsed = 0f;

       
        while (elapsed < shakeDuration)
        {
            
            float x = Random.Range(-1f, 1f) * shakeStrength;
            float y = Random.Range(-1f, 1f) * shakeStrength;

            objectShake.anchoredPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null; 
        }
        objectShake.anchoredPosition = originalPos;
    }
    
    

    IEnumerator SpawnLoop()
    {
        
        yield return new WaitForSeconds(roundStartDelay);
        
        while (isGameActive)
        {
            int index1 = Random.Range(0, spawnPoints.Length);
            int index2 = -1;
            float timePassed = gameSaniye - timer;
            if (timePassed > timeToStartDouble && Random.value < doubleSpawnChance)
            {
                
                index2 = GetSafeSecondIndex(index1);
            }

            
            warningSigns[index1].SetActive(true);
            if (index2 != -1) warningSigns[index2].SetActive(true);
            
            yield return new WaitForSeconds(0.5f);
            
            if(!isGameActive)
            {
                warningSigns[index1].SetActive(false);
                if (index2 != -1) warningSigns[index2].SetActive(false);
                break;
            }
            
            
            warningSigns[index1].SetActive(false);
            if (index2 != -1) warningSigns[index2].SetActive(false);

            if(isGameActive) 
            {
                SpawnRocket(index1);
                if (index2 != -1) SpawnRocket(index2);
            }
            
            yield return new WaitForSeconds(currentSpawnAralık);
        }
    }
    
    int GetSafeSecondIndex(int firstIndex)
    {
        int secondIndex;
        int attempts = 0; 

        do
        {
            secondIndex = Random.Range(0, spawnPoints.Length);
            attempts++;
            if (attempts > 100) return -1; 

        } while (IsOpposite(firstIndex, secondIndex) || firstIndex == secondIndex);
        
        return secondIndex;
    }
    
    bool IsOpposite(int a, int b)
    {
       
        bool aIsTop = (a == 0 || a == 1);
        bool bIsBottom = (b == 2 || b == 3);
        if (aIsTop && bIsBottom) return true;

        bool aIsBottom = (a == 2 || a == 3);
        bool bIsTop = (b == 0 || b == 1);
        if (aIsBottom && bIsTop) return true;

       
        
        bool aIsRightSpawner = (a == 4 || a == 5); 
        bool bIsLeftSpawner = (b == 6 || b == 7);  
        if (aIsRightSpawner && bIsLeftSpawner) return true;

        bool aIsLeftSpawner = (a == 6 || a == 7);
        bool bIsRightSpawner = (b == 4 || b == 5);
        if (aIsLeftSpawner && bIsRightSpawner) return true;

        
        return false;
    }




    void SpawnRocket(int index)
    {
        Transform point = spawnPoints[index];
        GameObject rocket = Instantiate(rocketPrefab, point.position, Quaternion.identity, rocketParent);
        
        Vector2 dir = Vector2.zero;
        if (index == 0 || index == 1) dir = Vector2.down;
        if (index == 2 || index == 3) dir = Vector2.up;
        if (index == 4 || index == 5) dir = Vector2.right;
        if (index == 6 || index == 7) dir = Vector2.left;

      
        var rScript = rocket.GetComponent<RocketDODGE>();
        if(rScript != null)
        {
            rScript.Setup(dir, currentRocketSpeed);
        }
    }
    
    DialogueDataları GetRandomQuestion(List<DialogueDataları> originalList)
    {
        if (originalList == null || originalList.Count == 0) return null;
        
        if (!questionPools.ContainsKey(originalList))
        {
            questionPools[originalList] = new List<DialogueDataları>(originalList);
        }
        
        List<DialogueDataları> pool = questionPools[originalList];
        
        if (pool.Count == 0)
        {
            pool.AddRange(originalList);
        }
        
        int rnd = Random.Range(0, pool.Count);
        DialogueDataları selectedQ = pool[rnd];
        
        pool.RemoveAt(rnd);

        return selectedQ;
    }
}
