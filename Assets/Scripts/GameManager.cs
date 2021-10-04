using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int matchbooksCollected = 0;
    private int totalMatchbooks = 0;

    private float score;

    //UI Stuff
    public TextMeshProUGUI matchbookText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    /*public void CountObjects()
    {
        this.totalMatchbooks = GameObject.FindGameObjectsWithTag("Matchbook").Length;
    }
    */

    public void CollectMatchbook()
    {
        this.matchbooksCollected++;
        StartCoroutine(this.DisplayMatchbookCollected());
    }

    private IEnumerator DisplayMatchbookCollected()
    {
        this.matchbookText.text = ("MATCHBOOK COLLECTED!");

        yield return new WaitForSeconds(2.0f);

        this.matchbookText.text = "";
    }
}
