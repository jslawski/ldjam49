using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManipulator : MonoBehaviour
{
    /*[SerializeField]
    private Rigidbody levelRb;
    */
    //[SerializeField]
    //public GameObject levelPivotObject;

    //private GameObject levelPivotInstance;
    private Rigidbody levelPivotRb;

    private float translateAmplifier = 1.5f;
    private float rotateAmplifier = 1f;
    private float rotateSpeedAmplifier = 3f;

    [SerializeField]
    private Camera gameCamera;

    [SerializeField]
    private GameObject levelParent;

    public void Start()
    {
        this.gameCamera = GameObject.Find("GameCamera").GetComponent<Camera>();
        this.levelPivotRb = this.GetComponent<Rigidbody>();
    }

    public void LateUpdate()
    {        
        /*if (this.transform.eulerAngles.z > 0.01f)
        {
            return;
        }

        Vector3 destinationVector = new Vector3(CameraFollow.instance.playerCharacter.transform.position.x, this.transform.position.y, this.transform.position.z);

        Vector3 positionDirection = destinationVector - this.transform.position;

        this.transform.position += positionDirection;
        this.levelParent.transform.position -= positionDirection; */
    }

    public void TranslateLevel(Vector3 velocity)
    {
        /*if (this.levelRb != null)
        {
            Vector3 targetPosition = this.levelRb.position + (this.translateAmplifier * velocity * Time.deltaTime);
            this.levelRb.MovePosition(targetPosition);
            return;
        }
        else if (this.levelPivotRb != null)
        {
            Vector3 targetPosition = this.levelPivotRb.position + (this.translateAmplifier * velocity * Time.deltaTime);
            this.levelPivotRb.MovePosition(targetPosition);
            return;
        }
        */

        Vector3 targetPosition = this.levelPivotRb.position + (this.translateAmplifier * velocity * Time.deltaTime);
        this.levelPivotRb.MovePosition(targetPosition);
        /*return;

        Debug.LogError("ERROR!  NO RIGIDBODY FOUND!");*/
    }

    public void RotateLevel(Vector3 targetRotation)
    {
        Vector3 rotationPoint = new Vector3(
            this.gameCamera.transform.position.x, 
            this.gameCamera.transform.position.y, 
            this.transform.position.z);

        float rotationAngle = targetRotation.z;

        Quaternion rotationQ = Quaternion.Euler(new Vector3(0,0,Mathf.DeltaAngle(this.levelParent.transform.eulerAngles.z, rotationAngle) * this.rotateAmplifier) * this.rotateSpeedAmplifier * Time.fixedDeltaTime);
        Vector3 translationVector = this.levelPivotRb.transform.position - rotationPoint;

        this.levelPivotRb.MovePosition(rotationQ * translationVector + rotationPoint);
        this.levelPivotRb.MoveRotation(this.levelPivotRb.transform.rotation * rotationQ);
    }

    /*private void SetupPivot()
    {
        Debug.LogError("SetupCalled");
        this.levelPivotInstance = Instantiate(levelPivotObject, this.gameCamera.transform.position, new Quaternion());

        this.levelPivotInstance.AddComponent<Rigidbody>();
        this.levelPivotRb = this.levelPivotInstance.GetComponent<Rigidbody>();
        this.levelPivotRb.useGravity = false;
        this.levelPivotRb.isKinematic = true;
        this.levelPivotRb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

        Destroy(this.levelRb);

        this.gameObject.transform.SetParent(levelPivotInstance.transform);
    }

    public void DestroyPivot()
    {
        Debug.LogError("DestroyCalled");

        this.gameObject.transform.SetParent(null);

        this.gameObject.AddComponent<Rigidbody>();
        this.levelRb = this.gameObject.GetComponent<Rigidbody>();
        this.levelRb.useGravity = false;
        this.levelRb.isKinematic = true;
        this.levelRb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

        Destroy(this.levelPivotInstance);
        this.levelPivotInstance = null;
    }*/
}
