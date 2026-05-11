using UnityEngine;
using System.Collections;

public class TabGroupRefresher : MonoBehaviour
{
    private void OnEnable()
    {
     
        StartCoroutine(RefreshAfterStartDöngüsü());
    }

    private IEnumerator RefreshAfterStartDöngüsü()
    {
        yield return new WaitForEndOfFrame();

        APPler app = FindFirstObjectByType<APPler>();
        if (app != null)
        {
            app.SwitchTab(app.currentActiveTab);
        }
    }
}