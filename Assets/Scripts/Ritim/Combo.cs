using UnityEngine;
using TMPro;

public class Combo : MonoBehaviour
{
    public static Combo Instance;

    public TextMeshProUGUI comboText;
    private int currentCombo = 0;

    [Header("Görsel Ayarlar")]
    public float popScale = 1.5f; 
    public float shrinkSpeed = 5f; 

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (comboText != null) comboText.alpha = 0;
    }

    void Update()
    {
        
        if (comboText != null && comboText.transform.localScale.x > 1f)
        {
            comboText.transform.localScale = Vector3.Lerp(
                comboText.transform.localScale, 
                Vector3.one, 
                Time.deltaTime * shrinkSpeed
            );
        }
    }

    public void AddCombo()
    {
        currentCombo++;
        
        if (currentCombo >= 2)
        {
            UpdateComboUI();
        }
    }

    public void ResetCombo()
    {
        currentCombo = 0;
        if (comboText != null) comboText.alpha = 0; 
    }

    void UpdateComboUI()
    {
        if (comboText == null) return;

        comboText.text = "COMBO " + currentCombo + "x";
        comboText.alpha = 1;
        
        comboText.transform.localScale = new Vector3(popScale, popScale, 1f);
        
        if (currentCombo > 10) comboText.color = Color.yellow;
        else if (currentCombo > 5) comboText.color = Color.cyan;
        else comboText.color = Color.white;
    }
}