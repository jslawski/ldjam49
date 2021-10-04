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

    [SerializeField]
    private GameObject centerOfMassObject;

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

    private bool shouldFlip = false;
    private float flipMagnitude = 10f;

    private float upwardVelocityNeededToFlip = 10f;

    public bool isAirborne = false;

    private Vector3 originalCenterOfMass;

    private bool newTouch = false;

    // Start is called before the first frame update
    void Start()
    {
        this.tableRb = this.gameObject.GetComponent<Rigidbody>();
        this.tableRb.maxAngularVelocity = 5f;
        this.currentInput = GameObject.Find("GameConsole").GetComponent<InputManager>();

        this.originalCenterOfMass = this.tableRb.centerOfMass;

        //this.centerOfMassObject.transform.localPosition = this.originalCenterOfMass;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (this.newTouch == false)
        {
            this.newTouch = true;
            this.tableRb.angularVelocity = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentState == GameState.GameOver)
        {
            this.tableRb.velocity = Vector3.zero;
            this.tableRb.angularVelocity = Vector3.zero;
            this.tableRb.useGravity = false;
            return;
        }
        
        //this.tableRb.centerOfMass = this.centerOfMassObject.transform.localPosition;

        RaycastHit hit;

        this.isAirborne = !this.tableRb.SweepTest(-Vector3.up, out hit, this.tableLeg.gameObject.transform.lossyScale.x * this.tableLeg.height * 2.0f);

        if (this.isAirborne == true)
        {
            Debug.LogError("I'm airborne!");

            this.customGravityOn = true;

            this.newTouch = false;
        }
        else
        {
            this.customGravityOn = false;
        }

        if (this.currentInput.objectClicked == "TiltButton" && Input.GetMouseButton(0))
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

            if (this.isAirborne == false)
            {
                this.currentTiltAcceleration = Mathf.Lerp(0, this.maxTiltAcceleration, this.currentInput.percentageOfMaxYDistance);
            }
        }
        else
        {
            this.isTilted = false;
        }
    }

    private void FixedUpdate()
    {
        if (this.customGravityOn == true)
        {
            this.tableRb.AddForce(-Vector3.up * this.customGravity, ForceMode.Acceleration);
        }
       
        if (this.isTilted == true)
        {
            if (this.isAirborne == false)
            {
                this.tableRb.AddForce(this.levelGeometry.transform.right * this.tiltDirection * this.currentTiltAcceleration, ForceMode.Acceleration); ;
            }
            else
            {
                Vector3 forceDirection = Vector3.right * this.tiltDirection;
                Vector3 forcePosition = new Vector3(this.transform.position.x, this.transform.position.y + 5.0f, this.transform.position.z);

                //Debug.LogError("Adding for to " + tiltDirection + " side");

                this.tableRb.AddForceAtPosition(forceDirection * this.flipMagnitude, forcePosition);
            }
        }

        if (this.tableRb.velocity.magnitude > this.maxSpeed)
        {
            this.tableRb.velocity = this.tableRb.velocity.normalized * this.maxSpeed;
        }
    }
}
