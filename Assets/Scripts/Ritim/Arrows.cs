using UnityEngine;

public class Arrows : MonoBehaviour
{
    private RectTransform rectTransform;
    public float fallSpeed = 500f; 

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.anchoredPosition += Vector2.down * fallSpeed * Time.deltaTime;
    }
}