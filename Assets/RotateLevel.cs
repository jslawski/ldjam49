using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLevel : MonoBehaviour
{
    private float targetRotation;
    private float originalRotation;

    public Rigidbody rb;

    public float moveSpeed = 0.1f;

    private void Start()
    {
        this.originalRotation = this.rb.rotation.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            StartCoroutine(this.Rotate());
        }
    }

    private IEnumerator Rotate()
    {
        this.targetRotation = -30;

        while (Mathf.Abs((360 - this.transform.rotation.eulerAngles.z) + this.targetRotation) > 0.5f)
        {
            Debug.LogError("Iterating 1: " + Mathf.Abs((360 - this.transform.rotation.eulerAngles.z) + this.targetRotation));
            this.rb.MoveRotation(Quaternion.Lerp(this.rb.rotation, Quaternion.Euler(0f, 0f, this.targetRotation), this.moveSpeed));
            yield return new WaitForFixedUpdate();
        }

        this.rb.MoveRotation(Quaternion.Euler(0f, 0f, this.targetRotation));
        this.targetRotation = 30;

        Debug.LogError("Waiting start");

        yield return new WaitForSeconds(2.0f);

        Debug.LogError("Finished Waiting");

        while (Mathf.Abs((360 - this.transform.rotation.eulerAngles.z) + this.targetRotation) > 0.5f)
        {
            Debug.LogError("Iterating 2");
            this.rb.MoveRotation(Quaternion.Lerp(this.rb.rotation, Quaternion.Euler(0f, 0f, this.targetRotation), this.moveSpeed));
            yield return new WaitForFixedUpdate();
        }

        Debug.LogError("Donezo");

        this.rb.MoveRotation(Quaternion.Euler(0f, 0f, this.targetRotation));
    }
}
