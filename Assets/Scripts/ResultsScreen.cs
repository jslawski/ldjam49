using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultsScreen : MonoBehaviour
{
    [SerializeField]
    private GameConsole gameConsole;
    [SerializeField]
    private InputManager currentInput;

    private float timeBeforeAwaitingInput = 3.0f;

    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private TextMeshProUGUI tableText;
    [SerializeField]
    private TextMeshProUGUI matchesText;

    private void OnEnable()
    {
        StartCoroutine(this.WaitForTap());

        this.timerText.text = GameManager.instance.gameTimer.TimerText.text;
        this.tableText.text = GameManager.instance.lettersCollected.ToString() + " / 5";
        this.matchesText.text = GameManager.instance.matchbooksCollected.ToString() + " / 20";
    }

    private IEnumerator WaitForTap()
    {
        yield return new WaitForSeconds(this.timeBeforeAwaitingInput);
       
        while (this.currentInput.objectClicked != "ConsoleScreen")
        {            
            yield return null;
        }

        SceneManager.UnloadSceneAsync(this.currentInput.sceneToLoadName);
        this.gameConsole.ReloadLevel();
        GameManager.instance.RestartGame();
        this.gameObject.SetActive(false);
    }
}
