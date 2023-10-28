using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : MonoBehaviour
{
    public AudioClip startSFX;
    [SerializeField] private CharacterDataSO[] characterDataSOs;
    private bool gameStarted;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in characterDataSOs)
        {
            item.EmptyData();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && !gameStarted)
        {
            LoadingSceneManager.Instance.LoadScene(SceneName.CharacterSelection);
            AudioManager.Instance.PlaySoundEffect(startSFX);
            gameStarted = true;
        }
    }
}
