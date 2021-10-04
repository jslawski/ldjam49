using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchbookCollectible : MonoBehaviour
{
    public AudioSource soundEffect;

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError("Collected!");

        if (other.gameObject.tag == "Table")
        {
            GameManager.instance.CollectMatchbook();
            this.soundEffect.Play();
            Destroy(this.gameObject);
        }
    }
}
