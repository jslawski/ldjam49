using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterCollectible : MonoBehaviour
{
    public string letter = "";
    public AudioSource soundEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Table")
        {
            GameManager.instance.CollectLetter(this.letter);
            this.soundEffect.Play();
            Destroy(this.gameObject);
        }
    }
}
