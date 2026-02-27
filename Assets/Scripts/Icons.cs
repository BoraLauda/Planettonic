using UnityEngine;
using UnityEngine.UI;

public class Icons : MonoBehaviour
{
    public enum IconType { Normal, Market } 

    [Header("AYARLAR")]
    public IconType iconType = IconType.Normal; 

    [Header("HANGİ PENCERE")]
    public GameObject linkedWindow;

    public void OpenWindow()
    {
     
        if (iconType == IconType.Market)
        {
           
            if (DesktopManager.instance != null && !DesktopManager.instance.isMarketUnlocked)
            {
               
                DesktopManager.instance.ShowLockedWarning();
                return; 
            }
        }

        

        if (linkedWindow == null) return;

        // e ztn açık
        if (linkedWindow.activeSelf)
        {
            linkedWindow.transform.SetAsLastSibling();
        }
        else
        {
            linkedWindow.SetActive(true);
            linkedWindow.transform.SetAsLastSibling();
        }
    }
}
