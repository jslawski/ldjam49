using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAndBob : MonoBehaviour
{
    private Rigidbody objectRb;
    private float startY;

    public float rotationSpeed = 5f;
    public float bobIntensity = 1f;
    public float bobFrequency = 1f;

    // Start is called before the first frame update
    void Start()
    {
        this.objectRb = this.GetComponent<Rigidbody>();
        this.startY = this.transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.objectRb.position = new Vector3(
            this.transform.position.x,
            this.startY + (this.bobIntensity * (Mathf.Sin(this.bobFrequency * Time.fixedTime))),
            this.transform.position.z);

        Quaternion rotation = Quaternion.Euler(0, rotationSpeed * Time.fixedTime, 0);

        this.objectRb.MoveRotation(rotation);
    }
}
