using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameState { Cutscene, Title, MainGame, GameOver }

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int matchbooksCollected = 0;
    private int lettersCollected = 0;

    private float score;

    public GameState currentState = GameState.Cutscene;

    //UI Stuff
    public TextMeshProUGUI matchbookText;
    public TextMeshProUGUI letterText;
    public GameObject resultsScreen;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void CollectMatchbook()
    {
        this.matchbooksCollected++;
        StartCoroutine(this.DisplayMatchbookCollected());
    }

    public void CollectLetter(string letter)
    {
        this.lettersCollected++;
        StartCoroutine(this.DisplayLetterCollected(letter));
    }

    public IEnumerator DisplayLetterCollected(string letter)
    {
        this.matchbookText.text = ("LETTER\n" + letter + "\nCOLLECTED!");

        yield return new WaitForSeconds(2.0f);

        this.matchbookText.text = "";
    }

    private IEnumerator DisplayMatchbookCollected()
    {
        this.matchbookText.text = ("MATCHBOOK COLLECTED!");

        yield return new WaitForSeconds(2.0f);

        this.matchbookText.text = "";
    }

    public void ShowResultsScreen()
    {
        this.resultsScreen.SetActive(true);
    }

    public void RestartGame()
    {
        this.currentState = GameState.MainGame;

        this.matchbooksCollected = 0;
        this.lettersCollected = 0;
        this.score = 0;
    }
}
