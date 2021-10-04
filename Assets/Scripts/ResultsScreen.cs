using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsScreen : MonoBehaviour
{
    [SerializeField]
    private GameConsole gameConsole;
    [SerializeField]
    private InputManager currentInput;

    private float timeBeforeAwaitingInput = 3.0f;

    private void OnEnable()
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

        SceneManager.UnloadSceneAsync("GameplayScene");
        this.gameConsole.ReloadLevel();
        GameManager.instance.RestartGame();
        this.gameObject.SetActive(false);
    }
}
