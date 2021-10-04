using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleScreen : MonoBehaviour
{
    [SerializeField]
    private GameConsole gameConsole;
    [SerializeField]
    private InputManager currentInput;

    private float timeBeforeAwaitingInput = 1.0f;

    private AudioSource music;

    private void Start()
    {
        StartCoroutine(this.WaitForTap());

        this.music = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (GameManager.instance.currentState == GameState.Title)
        {
            if (this.music.isPlaying == false)
            {
                this.music.Play();
            }
        }
        else if (this.music.isPlaying == true)
        {
            this.music.Stop();
        }
    }
    private void OnDisable()
    {
        GetComponent<AudioSource>().Stop();
    }

    private IEnumerator WaitForTap()
    {
        yield return new WaitForSeconds(this.timeBeforeAwaitingInput);

        while (this.currentInput.objectClicked != "ConsoleScreen")
        {
            yield return null;
        }

        GameManager.instance.gameMusic.clip = GameManager.instance.skaSongs[Random.Range(0, GameManager.instance.skaSongs.Length)];
        GameManager.instance.gameMusic.Play();

        GameManager.instance.currentState = GameState.MainGame;
        GameManager.instance.gameTimer.StartTimer(0);
        this.gameObject.SetActive(false);
    }
}
