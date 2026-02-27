using UnityEngine;
using UnityEngine.UI;

public class MailItemController : MonoBehaviour
{
    public GameObject bodyObj;

   
    public void ToggleMail()
    {
        
        bool isActive = !bodyObj.activeSelf;
        bodyObj.SetActive(isActive);
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
    }
}