using UnityEngine;

public class EdgeSlide : MonoBehaviour
{
 public static bool isDraggingItem = false;    
    
    [Header("UI Referansları")]
    public RectTransform viewWindow; 
    public RectTransform contentToPan; 
    
    [Header("Kaydırma Ayarları")]
    public float panSpeed = 800f; 
    public float edgeThreshold = 100f; 
    public float resetSpeed = 10f;
    
    [Header("Göz Kararı Sınırlar (Limitler)")]
    [Tooltip("Barı farenle EN SOLA (en son şişeye) çektiğinde Inspector'da yazan Pos X değeri")]
    public float solLimitX = -1500f; 
    
    [Tooltip("Barı farenle EN SAĞA çektiğinde Inspector'da yazan Pos X değeri")]
    public float sagLimitX = 1500f;  

    private Canvas parentCanvas;
    private float baslangicX;

    void Start()
    {
        if (viewWindow != null)
        {
            parentCanvas = viewWindow.GetComponentInParent<Canvas>();
        }
        
        if (contentToPan != null)
        {
            baslangicX = contentToPan.anchoredPosition.x;
        }
    }

    void Update()
    {
        if (contentToPan == null || viewWindow == null || parentCanvas == null) return;
        
        // Faz kontrolü (Hazırlık aşamasında değilsek merkeze dön)
        if (KokteylManager.Instance != null && KokteylManager.Instance.currentPhase != KokteylManager.GamePhase.Preparation)
        {
            Vector2 targetPos = new Vector2(baslangicX, contentToPan.anchoredPosition.y);
            contentToPan.anchoredPosition = Vector2.Lerp(contentToPan.anchoredPosition, targetPos, Time.deltaTime * resetSpeed);
            return; 
        }

        if (isDraggingItem) return; 
        
        Vector2 mousePos = Input.mousePosition;
        Camera cam = (parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : parentCanvas.worldCamera;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(viewWindow, mousePos, cam, out Vector2 localMousePos))
        {
            Rect rect = viewWindow.rect;
            
            // Farenin Y ekseninde pencere içinde olduğundan emin ol
            if (localMousePos.y >= rect.yMin && localMousePos.y <= rect.yMax)
            {
                Vector2 currentPos = contentToPan.anchoredPosition;
                bool isTryingToMove = false;

                // Faremiz soldaysa
                if (localMousePos.x < rect.xMin + edgeThreshold)
                {
                    currentPos.x += panSpeed * Time.deltaTime;
                    isTryingToMove = true;
                }
                // Faremiz sağdaysa
                else if (localMousePos.x > rect.xMax - edgeThreshold)
                {
                    currentPos.x -= panSpeed * Time.deltaTime;
                    isTryingToMove = true;
                }

                if (isTryingToMove)
                {
                    // --- MANUEL SINIRLAMA ---
                    // Eğer sol limit ve sağ limit ters girildiyse hata çıkmasın diye min ve max'ı güvene alıyoruz
                    float gercekMin = Mathf.Min(solLimitX, sagLimitX);
                    float gercekMax = Mathf.Max(solLimitX, sagLimitX);
                    
                    currentPos.x = Mathf.Clamp(currentPos.x, gercekMin, gercekMax);
                    contentToPan.anchoredPosition = currentPos;
                }
            }
        }
    }
}
