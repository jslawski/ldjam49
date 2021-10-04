using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int matchbooksCollected = 0;
    private int lettersCollected = 0;

    private float score;

    //UI Stuff
    public TextMeshProUGUI matchbookText;
    public TextMeshProUGUI letterText;

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
}
