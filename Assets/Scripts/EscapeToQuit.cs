using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeToQuit : MonoBehaviour
{
    [SerializeField]
    private GameConsole gameConsole;
    [SerializeField]
    private InputManager currentInput;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.UnloadSceneAsync(this.currentInput.sceneToLoadName);
            this.gameConsole.ReloadLevel();
            GameManager.instance.RestartGame();
        }
    }
}
