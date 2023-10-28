using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Important: the names in the enum value should be the same as the scene you're trying to load
public enum SceneName : byte
{
    Bootstrap,
    Menu,
    CharacterSelection,
    Controls,
    Gameplay,
    Victory,
    Defeat,

};

public class LoadingSceneManager : SingletonPersistent<LoadingSceneManager>
{
    public SceneName SceneActive => m_sceneActive;

    private SceneName m_sceneActive;

    // After running the menu scene, which initiates this manager, we subscribe to these events
    // due to the fact that when a network session ends it cannot longer listen to them.

    public void LoadScene(SceneName sceneToLoad, bool isNetworkSessionActive = true)
    {
        StartCoroutine(Loading(sceneToLoad, isNetworkSessionActive));
    }

    // Coroutine for the loading effect. It use an alpha in out effect
    private IEnumerator Loading(SceneName sceneToLoad, bool isNetworkSessionActive)
    {
        LoadingFadeEffect.Instance.FadeIn();

        // Here the player still sees the black screen
        yield return new WaitUntil(() => LoadingFadeEffect.s_canLoad);
        LoadSceneLocal(sceneToLoad);

        // Because the scenes are not heavy we can just wait a second and continue with the fade.
        // In case the scene is heavy instead we should use additive loading to wait for the
        // scene to load before we continue
        yield return new WaitForSeconds(1f);

        LoadingFadeEffect.Instance.FadeOut();
    }

    // Load the scene using the regular SceneManager, use this if there's no active network session
    private void LoadSceneLocal(SceneName sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad.ToString());
        switch (sceneToLoad)
        {
            case SceneName.Menu:
                if (AudioManager.Instance != null)
                    AudioManager.Instance.SwitchMusic(AudioManager.Instance.introMusic);
                break;

            case SceneName.Gameplay:
                if (AudioManager.Instance != null)
                    AudioManager.Instance.SwitchMusic(AudioManager.Instance.gameplayMusic);
                break;
        }
    }



}