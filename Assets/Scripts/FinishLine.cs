using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Table")
        {
            GameManager.instance.currentState = GameState.GameOver;
            GameManager.instance.gameTimer.Pause();
            GameManager.instance.gameTimer.gameObject.SetActive(false);
            GameManager.instance.ShowResultsScreen();
        }
    }

}
