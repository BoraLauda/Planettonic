using UnityEngine;
using System.Collections;

public class Claw : MonoBehaviour
{
    [Header("Kanca Ayarları")]
    public float horizontalSpeed = 300f;
    public float verticalSpeed = 500f;
    public float dropDistance = 400f;
    
    public float toleranceY = 60f; 
    public float toleranceX = 60f;
    
    [Header("Yakalama Merkezi (YENİ)")]
    public Transform grabPoint;
    
    public float catchOffsetY = 0f; 

    [Header("Kanca Animasyon (Parmaklar)")]
    public RectTransform leftClaw;  
    public RectTransform rightClaw; 
    public float openAngle = 30f;   
    public float closeAngle = 0f;   
    public float clawAnimSpeed = 10f; 

    [Header("Referanslar")]
    public PelusSpawner spawner; 

    [Header("Koleksiyon (Kazanma Şartı)")]
    public int bearCount = 0;
    public int bunnyCount = 0;
    public int foxCount = 0;
    
    private RectTransform rectTransform;
    private Vector2 startPos;
    
    private bool isMoving = true;
    private bool isDropping = false;
    private bool isGameWon = false;
    
    private float movementTimer = 0f; 
    

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
        SetClawAngles(openAngle);
    }

    void Update()
    {
        if (isGameWon) return;

        if (isMoving && !isDropping)
        {
            movementTimer += Time.deltaTime; 
            float newX = startPos.x + Mathf.PingPong(movementTimer * horizontalSpeed, 800) - 400; 
            rectTransform.anchoredPosition = new Vector2(newX, startPos.y);
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(DropSequence());
            }
        }
    }

    IEnumerator DropSequence()
    {
        isDropping = true;
        isMoving = false;

        Vector2 targetDropPos = new Vector2(rectTransform.anchoredPosition.x, startPos.y - dropDistance);

        while (rectTransform.anchoredPosition.y > targetDropPos.y)
        {
            rectTransform.anchoredPosition += Vector2.down * verticalSpeed * Time.deltaTime;
            yield return null;
        }

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * clawAnimSpeed;
            float currentLeft = Mathf.Lerp(openAngle, closeAngle, t);
            float currentRight = Mathf.Lerp(-openAngle, -closeAngle, t);
            
            if (leftClaw != null) leftClaw.localRotation = Quaternion.Euler(0, 0, currentLeft);
            if (rightClaw != null) rightClaw.localRotation = Quaternion.Euler(0, 0, currentRight);
            yield return null;
        }

      
        RectTransform caughtToy = TryCatchToy();

        if (caughtToy != null)
        {
            
            caughtToy.SetParent(grabPoint);
            
            caughtToy.anchoredPosition = new Vector2(0, catchOffsetY); 
            
            Peluslar moleLogic = caughtToy.GetComponent<Peluslar>();
            if (moleLogic != null) 
            {
                moleLogic.enabled = false; 
            }
        }

        yield return new WaitForSeconds(0.2f);

        while (rectTransform.anchoredPosition.y < startPos.y)
        {
            rectTransform.anchoredPosition += Vector2.up * verticalSpeed * Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, startPos.y);
        
        if (caughtToy != null)
        {
            ProcessCaughtToy(caughtToy);
        }
        
        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * clawAnimSpeed;
            float currentLeft = Mathf.Lerp(closeAngle, openAngle, t);
            float currentRight = Mathf.Lerp(-closeAngle, -openAngle, t);
            
            if (leftClaw != null) leftClaw.localRotation = Quaternion.Euler(0, 0, currentLeft);
            if (rightClaw != null) rightClaw.localRotation = Quaternion.Euler(0, 0, currentRight);
            yield return null;
        }

        isDropping = false;
        isMoving = true;
    }
    
    void SetClawAngles(float angle)
    {
        if (leftClaw != null) leftClaw.localRotation = Quaternion.Euler(0, 0, angle);
        if (rightClaw != null) rightClaw.localRotation = Quaternion.Euler(0, 0, -angle);
    }

    RectTransform TryCatchToy()
    {
        
        if (grabPoint == null) return null;

        foreach (RectTransform toy in spawner.activeToys)
        {
            if (toy == null) continue;

            Vector2 localPos = grabPoint.InverseTransformPoint(toy.position);
           
            Debug.Log($"[Hedef: {toy.name}] GrabPoint'e uzaklığı -> X: {Mathf.Abs(localPos.x)}, Y: {Mathf.Abs(localPos.y)}");

            if (Mathf.Abs(localPos.x) < toleranceX && Mathf.Abs(localPos.y) < toleranceY)
            {
                Debug.Log($"*** YAKALANDI! ({toy.name}) ***");
                return toy;
            }
        }
        return null; 
    }

    void ProcessCaughtToy(RectTransform toy)
    {
        string toyName = toy.gameObject.name.ToLower();

        if (toyName.Contains("bear")) bearCount++;
        else if (toyName.Contains("bunny")) bunnyCount++;
        else if (toyName.Contains("fox")) foxCount++;

        Destroy(toy.gameObject);
        CheckWinCondition();
    }

    void CheckWinCondition()
    {
        if (bearCount > 0 && bunnyCount > 0 && foxCount > 0)
        {
            isGameWon = true;
            isMoving = false;
            spawner.StopSpawning();
        }
    }
}