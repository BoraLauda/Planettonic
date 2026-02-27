using UnityEngine;

public class WindowSwapper : MonoBehaviour
{
    [Header("Pencereler")]
    public GameObject smallWindow;
    public GameObject bigWindow;
    
    public void OpenBigMode()
    {
        smallWindow.SetActive(false);
        bigWindow.SetActive(true);
    }

    public void OpenSmallMode()
    {
        bigWindow.SetActive(false);
        smallWindow.SetActive(true);
    }
}