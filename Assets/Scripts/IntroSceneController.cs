using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroSceneController : MonoBehaviour
{
    [Header("Ayarlar")]
    public VideoPlayer videoPlayer;
    public string nextSceneName = "Desktop"; 

    void Start()
    {
        if (videoPlayer != null)
        {
            
            videoPlayer.loopPointReached += GoToNextScene;
        }
    }

    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.S))
        {
            GoToNextScene(videoPlayer);
        }
    }

    void GoToNextScene(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}