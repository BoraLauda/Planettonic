using UnityEngine;

public class TabPanelSentinel : MonoBehaviour
{
    void OnEnable()
    {
        APPler app = FindFirstObjectByType<APPler>();
        if (app != null)
        {
            int myIndex = -1;
            
          
            for (int i = 0; i < app.smallTabPanels.Length; i++)
            {
                if (app.smallTabPanels[i] == gameObject || app.largeTabPanels[i] == gameObject)
                {
                    myIndex = i;
                    break;
                }
            }

          
            if (myIndex != -1 && myIndex != app.currentActiveTab)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
