using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError("TRIGGERED");

        if (other.tag == "Table")
        {
            GameManager.instance.currentState = GameState.GameOver;
            GameManager.instance.ShowResultsScreen();
        }
    }

}
