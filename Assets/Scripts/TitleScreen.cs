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

    private void Start()
    {
        StartCoroutine(this.WaitForTap());
    }

    private IEnumerator WaitForTap()
    {
        yield return new WaitForSeconds(this.timeBeforeAwaitingInput);

        while (this.currentInput.objectClicked != "ConsoleScreen")
        {
            yield return null;
        }

        GameManager.instance.currentState = GameState.MainGame;
        GameManager.instance.gameTimer.StartTimer(0);
        this.gameObject.SetActive(false);
    }
}
