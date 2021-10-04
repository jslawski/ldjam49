using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchbookCollectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError("Collected!");

        if (other.gameObject.tag == "Table")
        {
            GameManager.instance.CollectMatchbook();
            Destroy(this.gameObject);
        }
    }
}
