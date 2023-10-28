using UnityEngine;

// This scene is show for a moment before gameplay
public class ControlsManager : MonoBehaviour
{
    [SerializeField]
    private int m_waitingTime;

    [SerializeField]
    private SceneName m_sceneName;

    private void Start()
    {
        // Invoke the next scene, waiting some time
        Invoke(nameof(LoadNextScene), m_waitingTime);

    }

    private void LoadNextScene()
    {
        // Safety check
        if (LoadingSceneManager.Instance != null)
        {
            // Tell the clients to start the loading effect
            Load();

            // Loading scene on server
            LoadingSceneManager.Instance.LoadScene(m_sceneName);
        }
    }

    private void Load()
    {

        LoadingFadeEffect.Instance.FadeAll();
    }
}
