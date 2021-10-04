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

    [SerializeField]
    private SpriteRenderer matchesCounter;
    [SerializeField]
    private Sprite[] matchesImages;
    [SerializeField]
    private GameObject[] tableCounter;

    public Timer gameTimer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void CollectMatchbook()
    {
        this.matchesCounter.sprite = this.matchesImages[this.matchbooksCollected];
        this.matchbooksCollected++;

        this.matchesCounter.gameObject.SetActive(true);
        
        StartCoroutine(this.DisplayMatchbookCollected());
    }

    public void CollectLetter(string letter)
    {
        this.lettersCollected++;
        StartCoroutine(this.DisplayLetterCollected(letter));

        switch (letter)
        {
            case "T":
                this.tableCounter[0].SetActive(true);
                break;
            case "A":
                this.tableCounter[1].SetActive(true);
                break;
            case "B":
                this.tableCounter[2].SetActive(true);
                break;
            case "L":
                this.tableCounter[3].SetActive(true);
                break;
            case "E":
                this.tableCounter[4].SetActive(true);
                break;
        }
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

        for (int i = 0; i < 5; i++)
        {
            this.tableCounter[i].SetActive(false);
        }

        this.matchesCounter.sprite = this.matchesImages[0];
        this.matchesCounter.gameObject.SetActive(false);
    }
}
