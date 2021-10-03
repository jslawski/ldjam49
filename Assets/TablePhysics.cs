using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TablePhysics : MonoBehaviour
{
    private InputManager currentInput;

    private float maxSpeed = 90f;

    [SerializeField]
    private LevelManipulator levelGeometry;

    private Rigidbody tableRb;

    [SerializeField]
    private CapsuleCollider tableLeg;

    private bool customGravityOn = false;
    private float customGravity = 75f;

    private float impulseForceDistance = 0.5f;
    private float impulseForceMagnitude = 50f;
    private float minSpeedForForceTrigger = 50f;
    private bool shouldImpulseForce = false;
    private Vector3 impulseDirection = Vector3.zero;

    private bool isTilted = false;
    private float tiltDirection = 0f;
    private float maxTiltAcceleration = 20f;
    private float currentTiltAcceleration = 0f;

    // Start is called before the first frame update
    void Start()
    {
        this.tableRb = this.gameObject.GetComponent<Rigidbody>();

        this.currentInput = GameObject.Find("GameConsole").GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (this.tableRb.SweepTest(-Vector3.up, out hit, this.tableLeg.height * 3.0f) == false)
        {
            this.customGravityOn = true;
            Debug.LogError("AIRBORNE");
        }
        else
        {
            this.customGravityOn = false;
        }

        if (this.currentInput.objectClicked == "TiltButton")
        {
            this.isTilted = true;

            this.tiltDirection = (int)this.currentInput.tiltButtonClicked * this.currentInput.dragDirection.y;

            if (this.tiltDirection < 0.0f)
            {
                this.tiltDirection = 1.0f;
            }
            else
            {
                this.tiltDirection = -1.0f;
            }

            this.currentTiltAcceleration = Mathf.Lerp(0, this.maxTiltAcceleration, this.currentInput.percentageOfMaxYDistance);
        }
        else
        {
            this.isTilted = false;
        }

        /*if (this.tableRb.SweepTest(Vector3.left, out hit, this.impulseForceDistance) && 
            this.tableRb.velocity.x > this.minSpeedForForceTrigger)
        {
            this.shouldImpulseForce = true;
            this.impulseDirection = Vector3.right;

            Debug.LogError("Velocity: " + this.tableRb.velocity);
        }*/

        /*if (this.tableRb.SweepTest(Vector3.right, out hit, this.impulseForceDistance) &&
            Mathf.Abs(this.tableRb.velocity.x) > this.minSpeedForForceTrigger)
        {
            this.shouldImpulseForce = true;
            this.impulseDirection = Vector3.left;

            Debug.LogError("Velocity: " + this.tableRb.velocity);
        }*/
    }

    private void FixedUpdate()
    {
        if (this.customGravityOn == true)
        {
            this.tableRb.AddForce(-Vector3.up * this.customGravity, ForceMode.Acceleration);
        }

        /*if (this.shouldImpulseForce == true)
        {
            this.shouldImpulseForce = false;
            this.tableRb.AddForce(this.impulseDirection * this.impulseForceMagnitude, ForceMode.Impulse);
        }
        */
        if (this.isTilted)
        {
            this.tableRb.AddForce(this.levelGeometry.transform.right * this.tiltDirection * this.currentTiltAcceleration, ForceMode.Acceleration); ;
        }

        if (this.tableRb.velocity.magnitude > this.maxSpeed)
        {
            Debug.LogError("LIMITED");
            this.tableRb.velocity = this.tableRb.velocity.normalized * this.maxSpeed;
        }
    }
}
