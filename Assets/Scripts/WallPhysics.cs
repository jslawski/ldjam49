using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPhysics : MonoBehaviour
{
    private float impulseForceMagnitude = 50f;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.LogError("RigidBody: " + collision.rigidbody.gameObject.name);

        if (collision.collider.gameObject.tag == "Table")
        {
            if (collision.impulse.x <= -30f)
            {
                Debug.LogError("LEFT PULSE: " + collision.impulse.x);
                collision.rigidbody.AddForce(-this.transform.right * this.impulseForceMagnitude, ForceMode.Impulse);
                //Apply right pulse
            }
            else if (collision.impulse.x > 30f)
            {
                Debug.LogError("RIGHT PULSE: " + collision.impulse.x);
                
                collision.rigidbody.AddForce(this.transform.right * this.impulseForceMagnitude, ForceMode.Impulse);
                //Apply left pulse
            }
        }
    }
}
