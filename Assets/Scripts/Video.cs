using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class VideoTransition : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject coverImage; 
    public string targetSceneName = "Date"; 

    void Start()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();
        if (coverImage != null) coverImage.SetActive(true);

        
        videoPlayer.playOnAwake = false; 
        videoPlayer.Prepare();

        
        videoPlayer.prepareCompleted += OnVideoReady; 
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    
    void OnVideoReady(VideoPlayer vp)
    {
        videoPlayer.Play();
        if (coverImage != null) coverImage.SetActive(false);
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(targetSceneName);
    }
}