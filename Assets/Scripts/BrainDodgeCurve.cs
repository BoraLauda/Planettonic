using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BrainDodgeCurve : MonoBehaviour
{
    public float roundStartDelay = 2.0f;
    
    public GameObject pauseImage;
    public GameObject startImage;
    
    public int baseHeartReward = 20;  
    public int penaltyPerHit = 5;      
    
    public Image fadeImage;
    public float gameSaniye = 20f; 
    public Image centerCharacterImage;
    
    [Header("Kavis (Curve) Noktaları")]
    public Transform sagAlt;
    public Transform solAlt;
    public Transform sagUst;
    public Transform solUst;
    public Transform merkezNoktasi; 

    [Header("Uyarı İşaretleri (Sırayla: SağAlt, SolAlt, SağÜst, SolÜst)")]
    public GameObject[] warningSigns;
    
    [Header("Zaman ve Hız Ayarları")]
    public float startSpawnAralık = 1.5f;
    public float minSpawnAralık = 0.6f;   
    private float currentSpawnAralık;
    
    public float doubleSpawnChance = 0.5f; 
    public float timeToStartDouble = 5f;
    
    public float startSpeed = 500f;     
    public float maxSpeed = 1500f;      
    public float hızlanma = 50f;
    private float currentRocketSpeed;
    
    [Header("Efekt ve Prefab")]
    public RectTransform objectShake; 
    public float shakeDuration = 0.2f;  
    public float shakeStrength = 15f;   
    public GameObject rocketPrefab; 
    public Transform rocketParent;  

    private float timer;
    public bool isGameActive = true;
    private int currentTurn = 0; 
    private bool isLeftDodging = true;
    private int hitCount = 0;
    private brainDate dateManager;
    private Dictionary<int, List<DialogueDataları>> questionPools = new Dictionary<int, List<DialogueDataları>>();
    
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
        
        foreach (Transform child in rocketParent) Destroy(child.gameObject);
        if (warningSigns != null) foreach (GameObject sign in warningSigns) if (sign != null) sign.SetActive(false);
        
        if (currentTurn % 2 == 0)
        {
            isLeftDodging = true;
            if(DateSettings.leftChar != null) 
                centerCharacterImage.sprite = DateSettings.leftChar.dodgeTheQuestionIkonu != null ? DateSettings.leftChar.dodgeTheQuestionIkonu : DateSettings.leftChar.profileIcon;
        }
        else
        {
            isLeftDodging = false;
            if(DateSettings.rightChar != null) 
                centerCharacterImage.sprite = DateSettings.rightChar.dodgeTheQuestionIkonu != null ? DateSettings.rightChar.dodgeTheQuestionIkonu : DateSettings.rightChar.profileIcon;
        }

        timer = gameSaniye;
        currentRocketSpeed = startSpeed;
        currentSpawnAralık = startSpawnAralık;
        isGameActive = true;

        StopAllCoroutines();
        StartCoroutine(SpawnLoop());
    }
   
    void Update()
    {
        if (!isGameActive) return;

        timer -= Time.deltaTime;
        if (currentRocketSpeed < maxSpeed) currentRocketSpeed += hızlanma * Time.deltaTime;
        
        float progress = (gameSaniye - timer) / gameSaniye; 
        currentSpawnAralık = Mathf.Lerp(startSpawnAralık, minSpawnAralık, progress);
        
        if (timer <= 0) EndRound(true);
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
        
        if (warningSigns != null) foreach (GameObject sign in warningSigns) if (sign != null) sign.SetActive(false);

        if (isSuccess)
        {
            NextTurn();
        }
        else
        {
            DialogueDataları questionToAsk = null;
            Characters currentCharData = isLeftDodging ? DateSettings.leftChar : DateSettings.rightChar;
            Characters opponentCharData = isLeftDodging ? DateSettings.rightChar : DateSettings.leftChar;

            if (currentCharData != null)
            {
                questionToAsk = GetTargetedQuestion(currentCharData.curveDodgeQuestions, opponentCharData);
            }

            if (dateManager != null && questionToAsk != null)
            {
                if(pauseImage != null) pauseImage.SetActive(true);
                
                
                dateManager.PlayCurveDodgeDialogue(questionToAsk);
            }
            else NextTurn();
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
            int finalHearts = baseHeartReward - (hitCount * penaltyPerHit);
            if (finalHearts < 0) finalHearts = 0; 
            float finalStars = (hitCount <= 1) ? 1.0f : (hitCount <= 3 ? 0.5f : 0f);
            
            if (dateManager != null)
            {
              
                dateManager.EndCurveDodgeGame(finalStars, finalHearts, TargetCharacter.Both);
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
            objectShake.anchoredPosition = originalPos + new Vector3(Random.Range(-1f, 1f) * shakeStrength, Random.Range(-1f, 1f) * shakeStrength, 0);
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
            int yol1 = Random.Range(0, 8); 
            int yol2 = -1;
            
            if ((gameSaniye - timer) > timeToStartDouble && Random.value < doubleSpawnChance)
            {
                yol2 = Random.Range(0, 8);
                if(yol1 == yol2) yol2 = (yol2 + 1) % 8; 
            }
            
            int uyari1 = GetWarningIndex(yol1);
            int uyari2 = yol2 != -1 ? GetWarningIndex(yol2) : -1;

            if(warningSigns.Length > uyari1) warningSigns[uyari1].SetActive(true);
            if(yol2 != -1 && warningSigns.Length > uyari2) warningSigns[uyari2].SetActive(true);
            
            yield return new WaitForSeconds(0.5f);
            
            if(!isGameActive) break;
            
            if(warningSigns.Length > uyari1) warningSigns[uyari1].SetActive(false);
            if(yol2 != -1 && warningSigns.Length > uyari2) warningSigns[uyari2].SetActive(false);

            SpawnKavisliRocket(yol1);
            if (yol2 != -1) SpawnKavisliRocket(yol2);
            
            yield return new WaitForSeconds(currentSpawnAralık);
        }
    }

    int GetWarningIndex(int yolIndex)
    {
        if(yolIndex == 0 || yolIndex == 4) return 0;
        if(yolIndex == 1 || yolIndex == 6) return 1; 
        if(yolIndex == 2 || yolIndex == 5) return 2; 
        if(yolIndex == 3 || yolIndex == 7) return 3; 
        return 0;
    }

    void SpawnKavisliRocket(int yolIndex)
    {
        Transform baslangic = sagAlt;
        Transform bitis = solAlt;
    
        switch(yolIndex) {
            case 0: baslangic = sagAlt; bitis = solAlt; break;
            case 1: baslangic = solAlt; bitis = sagAlt; break;
            case 2: baslangic = sagUst; bitis = solUst; break; 
            case 3: baslangic = solUst; bitis = sagUst; break;
            case 4: baslangic = sagAlt; bitis = sagUst; break; 
            case 5: baslangic = sagUst; bitis = sagAlt; break;
            case 6: baslangic = solAlt; bitis = solUst; break; 
            case 7: baslangic = solUst; bitis = solAlt; break;
        }

        GameObject rocket = Instantiate(rocketPrefab, baslangic.position, Quaternion.identity, rocketParent);
        RocketCurve rScript = rocket.GetComponent<RocketCurve>();
        
        if(rScript != null)
        {
            Vector3 hayaletKontrolNoktasi = (2f * merkezNoktasi.position) - (0.5f * baslangic.position) - (0.5f * bitis.position);

            rScript.Firlat(baslangic.position, hayaletKontrolNoktasi, bitis.position, currentRocketSpeed);
        }
    }
    
    DialogueDataları GetTargetedQuestion(List<TargetedDialogue> originalList, Characters opponent)
    {
        if (originalList == null || originalList.Count == 0 || opponent == null) return null;
        int poolKey = originalList.GetHashCode() ^ opponent.GetInstanceID();
        if (!questionPools.ContainsKey(poolKey)) questionPools[poolKey] = new List<DialogueDataları>();
        
        List<DialogueDataları> pool = questionPools[poolKey];
        if (pool.Count == 0)
        {
            foreach (var item in originalList) if (item.targetCharacter == opponent && item.diyalogDosyasi != null) pool.Add(item.diyalogDosyasi);
        }
        
        if (pool.Count == 0) return null;
        int rnd = Random.Range(0, pool.Count);
        DialogueDataları selectedQ = pool[rnd];
        pool.RemoveAt(rnd);
        return selectedQ;
    }
}