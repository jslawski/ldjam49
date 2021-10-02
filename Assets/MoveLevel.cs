using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLevel : MonoBehaviour
{
    public Rigidbody rb;
   
    private void FixedUpdate()
    {
        this.rb.MovePosition(new Vector3(this.rb.position.x, this.rb.position.y + (0.5f * (Mathf.Sin(5 * Time.time))), this.rb.position.z));     
    }
}
