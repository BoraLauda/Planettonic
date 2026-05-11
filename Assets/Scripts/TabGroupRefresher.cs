using UnityEngine;

public class TabGroupRefresher : MonoBehaviour
{
    private void OnEnable()
    {
        APPler app = FindFirstObjectByType<APPler>();
        if (app != null)
        {
            
            app.SwitchTab(app.currentActiveTab);
        }
    }
}