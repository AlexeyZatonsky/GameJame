using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CutsceneManager : SingletonManager<CutsceneManager>
{
    [Header("Video Settings")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private string nextSceneName;

    private void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("No VideoPlayer.");
            return;
        }

        videoPlayer.loopPointReached += OnVideoFinished; 
        videoPlayer.Play();
    }

    private void Update()
    {
        if (InputManager.Instance.IsSkip())
        {
            SkipCutscene();
        }
    }

    private void SkipCutscene()
    {
        //videoPlayer.Stop();
        LoadNextSceneAsync();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("CutScene Completed.NewScene load...");
        LoadNextSceneAsync();
    }

    private void LoadNextSceneAsync()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadSceneAsync(nextSceneName);
        }
        else
        {
            Debug.LogError("No Name for next Scene");
        }
    }
}