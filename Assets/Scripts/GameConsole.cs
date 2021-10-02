using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConsole : MonoBehaviour
{
    Vector3 pointToMoveTo;
    public Camera frameCamera;
    [SerializeField]
    private float maxYDiff = 0.18f;
    [SerializeField]
    private float maxXDiff = 0.08f;
    [SerializeField]
    private float maxRotation = 20f;

    [SerializeField]
    private InputManager currentInput;

    private Vector3 centerPoint;

    private float minXViewport;
    private float maxXViewport;
    private float minYViewport;
    private float maxYViewport;

    public float moveSpeed = 0.5f;

    private Vector3 velocity = Vector3.zero;
    private float angleVelocity = 0;
    public float damp = 5f;

    private float smoothTime = 0.1f;
    private Vector3 tempVel = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        this.maxXViewport = 0.5f + this.maxXDiff;
        this.maxYViewport = 0.5f + this.maxYDiff;
        this.centerPoint = this.frameCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, this.frameCamera.nearClipPlane));
    }

    // Update is called once per frame
    void Update()
    {
        if (this.currentInput.objectClicked == "ConsoleScreen")
        {
            this.TranslateConsole();
            this.ResetRotation();
        }
        else if (this.currentInput.objectClicked == "TiltButton")
        {
            this.RotateConsole();
        }
    }

    private void TranslateConsole()
    {
        Vector3 moveDirection = new Vector3(this.maxXDiff * this.currentInput.dragDirection.x,
            this.maxYDiff * this.currentInput.dragDirection.y, this.frameCamera.nearClipPlane) + new Vector3(0.5f, 0.5f, 0f);

        moveDirection = this.frameCamera.ViewportToWorldPoint(moveDirection);

        Vector3 targetPosition = Vector3.LerpUnclamped(this.centerPoint, moveDirection,
            this.currentInput.percentageOfMaxDragDistance);

        targetPosition = new Vector3(targetPosition.x, targetPosition.y, this.transform.position.z);

        this.velocity += targetPosition - this.transform.position;
        this.velocity -= (this.velocity * this.damp * Time.deltaTime);

        this.transform.position += (this.velocity * Time.deltaTime);
    }

    private void RotateConsole()
    {
        float tiltDirection = (int)this.currentInput.tiltButtonClicked * this.currentInput.dragDirection.y;

        float targetAngle = 0.0f;

        if (tiltDirection > 0)
        {
            tiltDirection = 1;
        }
        else
        {
            tiltDirection = -1;
        }

        targetAngle = Mathf.Lerp(0, this.maxRotation, this.currentInput.percentageOfMaxYDistance) * tiltDirection;

        Debug.LogError("Target Angle: " + targetAngle);

        Vector3 targetRotation = new Vector3(this.transform.rotation.x,
            this.transform.rotation.y,
            this.transform.rotation.z + targetAngle);

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, 
            Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z), 
            10.0f * Time.deltaTime);

        //this.transform.eulerAngles = Mathf.SmoothDampAngle(this.transform.eulerAngles, targetRotation, ref tempVel, this.smoothTime);

        /*Debug.LogError("Target Angle: " + targetAngle);

        float unsignedAngleVelocity = (targetAngle - this.transform.eulerAngles.z);

        this.angleVelocity += ((targetAngle * Time.deltaTime) * tiltDirection);
        this.angleVelocity -= (this.angleVelocity * 5f * Time.deltaTime);

        if (this.angleVelocity < 0)
        {
            Debug.LogError("NEGATIVE");
        }

        Vector3 targetRotation = new Vector3(this.transform.rotation.x, 
            this.transform.rotation.y, 
            this.transform.rotation.z * (angleVelocity * Time.deltaTime));

        this.transform.eulerAngles = new Vector3(this.transform.rotation.eulerAngles.x,
            this.transform.rotation.eulerAngles.y,
            this.transform.rotation.eulerAngles.z + (this.angleVelocity * Time.deltaTime));
        */
    }

    private void ResetRotation()
    {
        float tiltDirection = this.transform.rotation.eulerAngles.z;

        float targetAngle = 0.0f;

        if (tiltDirection > 0)
        {
            tiltDirection = 1;
        }
        else
        {
            tiltDirection = -1;
        }

        Vector3 targetRotation = new Vector3(this.transform.rotation.x,
            this.transform.rotation.y,
            this.transform.rotation.z + targetAngle);

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
            Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z),
            10.0f * Time.deltaTime);
    }
}
