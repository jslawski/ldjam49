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
    private InputManager currentInput;

    private Vector3 centerPoint;

    private float minXViewport;
    private float maxXViewport;
    private float minYViewport;
    private float maxYViewport;

    public float moveSpeed = 0.5f;

    private Vector3 velocity = Vector3.zero;
    public float damp = 5f;

    // Start is called before the first frame update
    void Start()
    {
        this.centerPoint = this.frameCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, this.frameCamera.nearClipPlane));
    }

    // Update is called once per frame
    void Update()
    {
        this.maxXViewport = 0.5f + this.maxXDiff;
        this.maxYViewport = 0.5f + this.maxYDiff;

        Vector3 moveDirection = new Vector3(this.maxXDiff * this.currentInput.dragDirection.x, 
            this.maxYDiff * this.currentInput.dragDirection.y, this.frameCamera.nearClipPlane) + new Vector3(0.5f, 0.5f, 0f);

        moveDirection = this.frameCamera.ViewportToWorldPoint(moveDirection);

        Vector3 targetPosition = Vector3.LerpUnclamped(this.centerPoint, moveDirection,
            this.currentInput.percentageOfMaxDragDistance);

        this.velocity += targetPosition - this.transform.position;
        this.velocity -= (this.velocity * this.damp * Time.deltaTime);

        targetPosition = new Vector3(targetPosition.x, targetPosition.y, this.frameCamera.nearClipPlane);

        this.transform.position += (this.velocity * Time.deltaTime);

        /*this.transform.position = new Vector3(
            EasingFunction.EaseInOutBack(this.transform.position.x, targetPosition.x, /*SomeTimeVariable),
            EasingFunction.EaseInOutBack(this.transform.position.y, targetPosition.y, /*SomeTimeVariable),
            targetPosition.z);
        

        this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPosition, ref this.velocity, this.smoothTime);
        */
    }

    /*public static Vector3 Spring(Vector3 from, Vector3 to, float time)
    {
        return new Vector3(Spring(from.x, to.x, time), Spring(from.y, to.y, time), Spring(from.z, to.z, time));
    }*/
}
