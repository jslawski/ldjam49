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
    private float rotateAmplifier = 0.2f;
    private float rotateSpeedAmplifier = 3f;

    [SerializeField]
    private Camera gameCamera;

    [SerializeField]
    private GameObject levelParent;

    //private Rigidbody backgroundLayers;

    public void Start()
    {
        this.gameCamera = GameObject.Find("GameCamera").GetComponent<Camera>();
        this.levelPivotRb = this.GetComponent<Rigidbody>();
        //this.backgroundLayers = GameObject.Find("ParallaxBackground").GetComponent<Rigidbody>();
    }

    public void TranslateLevel(Vector3 velocity)
    {
        Vector3 targetPosition = this.levelPivotRb.position + (this.translateAmplifier * velocity * Time.deltaTime);
        this.levelPivotRb.MovePosition(targetPosition);
    }

    public void RotateLevel(Vector3 targetRotation)
    {
        Vector3 rotationPoint = new Vector3(
            this.gameCamera.transform.position.x, 
            this.gameCamera.transform.position.y, 
            this.transform.position.z);

        float rotationAngle = targetRotation.z;

        Quaternion rotationQ = Quaternion.Euler(new Vector3(0,0,Mathf.DeltaAngle(this.levelParent.transform.eulerAngles.z, rotationAngle * this.rotateAmplifier)) * this.rotateSpeedAmplifier * Time.fixedDeltaTime);
        Vector3 translationVector = this.levelPivotRb.transform.position - rotationPoint;

        this.levelPivotRb.MovePosition(rotationQ * translationVector + rotationPoint);
        this.levelPivotRb.MoveRotation(this.levelPivotRb.transform.rotation * rotationQ);

        //this.backgroundLayers.MovePosition(rotationQ * translationVector + rotationPoint);
        //this.backgroundLayers.MoveRotation(this.levelPivotRb.transform.rotation * rotationQ);
    }
}
