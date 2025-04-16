using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class StudioIntro : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(1); // ou o nome da sua cena
    }
}
