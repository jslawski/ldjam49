using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterCollectible : MonoBehaviour
{
    public string letter = "";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Table")
        {
            GameManager.instance.CollectLetter(this.letter);
            Destroy(this.gameObject);
        }
    }
}
