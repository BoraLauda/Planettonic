using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class DropDown : MonoBehaviour
{

    public TMP_Dropdown dropdown;
    public TextMeshProUGUI label;
  
  
    void Start()
    {
        dropdown.onValueChanged.AddListener(OnDropdownChanged);
        Invoke(nameof(SetTitle), 0f);
        
    }

    void SetTitle()
    {
        label.text = "Planets";
    }

    void OnDropdownChanged(int index)
    {
        label.text = "Planets";

    }
}
