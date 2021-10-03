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

    private float translateAmplifier = 10f;
    private float rotateAmplifier = 1.5f;
    private float rotateSpeedAmplifier = 15f;

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
        Vector3 positionDirection = this.gameCamera.transform.position - this.transform.position;

        this.transform.position += positionDirection;
        this.levelParent.transform.position -= positionDirection; 
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
        /*if (this.levelPivotInstance == null)
        {
            this.SetupPivot();
        }*/

        this.levelPivotRb.MoveRotation(Quaternion.Slerp(this.transform.rotation,
            Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z),
            this.rotateSpeedAmplifier * Time.deltaTime));

        /*if (Mathf.Abs(this.levelPivotRb.rotation.eulerAngles.z - targetRotation.z) < 0.1f)
        {
            this.levelPivotRb.rotation = Quaternion.Euler(targetRotation.x, targetRotation.y, 0f);
        }*/
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
