using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPhysics : MonoBehaviour
{
    private float impulseForceMagnitude = 90f;
    private float minImpulseNeeded = 30f;
    private InputManager currentInput;

    private void Start()
    {
        this.currentInput = GameObject.Find("GameConsole").GetComponent<InputManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (this.currentInput.objectClicked != "ConsoleScreen")
        {
            return;
        }

        if (collision.collider.gameObject.tag == "Table" && Mathf.Abs(collision.impulse.x) > this.minImpulseNeeded)
        {
            if (this.transform.position.x < collision.gameObject.transform.position.x)
            {
                Debug.LogError("LEFT PULSE: " + collision.impulse.x);
                collision.rigidbody.AddForce(-this.transform.right * this.impulseForceMagnitude * this.currentInput.percentageOfMaxXDistance, ForceMode.Impulse);
            }
            else
            {                
                collision.rigidbody.AddForce(this.transform.right * this.impulseForceMagnitude * this.currentInput.percentageOfMaxXDistance, ForceMode.Impulse);
            }
        }
    }
}
