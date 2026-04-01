using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour
{
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(() => 
        {
            if (Audio.Instance != null)
            {
                Audio.Instance.PlayClick();
            }
        });
    }
}