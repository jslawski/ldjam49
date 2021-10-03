using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TiltButton { Left = -1, Right = 1, None = 0 }

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera frameCamera;
    private Vector3 clickStartingPosition;
    private float currentDragDistance = 0;
    [SerializeField]
    private LayerMask clickDetectableMask;

    private Coroutine dragCoroutine = null;

    private float minDragDistance = 0.01f;
    private float maxDragDistance = 0.3f;
    private float maxYDistance = 0.3f;
    private float maxXDistance = 0.3f;

    [SerializeField]
    private LineRenderer debugLine;

    public Vector3 dragDirection;
    public float percentageOfMaxDragDistance = 0.0f;
    public float percentageOfMaxYDistance = 0.0f;
    public float percentageOfMaxXDistance = 0.0f;

    public string objectClicked = "";

    public TiltButton tiltButtonClicked = TiltButton.None;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) == true)
        {
            this.CastRay();
        }

        if (Input.GetMouseButtonUp(0) == true)
        {
            this.objectClicked = "";
        }
    }

    private void CastRay()
    {
        Ray ray = this.frameCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            this.objectClicked = hit.collider.gameObject.tag;

            if (this.objectClicked == "TiltButton")
            {
                this.tiltButtonClicked = this.frameCamera.ScreenToViewportPoint(Input.mousePosition).x < 0.5f ? TiltButton.Left : TiltButton.Right;
                //Debug.LogError("Tilt button clicked: " + this.tiltButtonClicked);
            }

            //Debug.LogError("Object: " + hit.collider.gameObject.name + " Tag: " + this.objectClicked);

            if (this.dragCoroutine == null)
            {
                StartCoroutine(this.UpdateDrag());
            }
        }
    }

    private float GetDistance(Vector3 initialPoint, Vector3 newPoint)
    {
        return Mathf.Sqrt((Mathf.Pow(newPoint.x - initialPoint.x, 2) + Mathf.Pow(newPoint.y - initialPoint.y, 2)));
    }

    private IEnumerator UpdateDrag()
    {
        this.clickStartingPosition = this.frameCamera.ScreenToViewportPoint(Input.mousePosition);

        while (Input.GetMouseButton(0))
        {
            Vector3 initialMousePosition = this.frameCamera.ScreenToViewportPoint(Input.mousePosition);
            yield return new WaitForSeconds(0.01f);
            Vector3 newMousePosition = this.frameCamera.ScreenToViewportPoint(Input.mousePosition);

            if (this.GetDistance(initialMousePosition, newMousePosition) >= this.minDragDistance)
            {
                this.currentDragDistance = Vector3.Distance(this.clickStartingPosition, newMousePosition);

                if (this.currentDragDistance > this.maxDragDistance)
                {
                    this.currentDragDistance = this.maxDragDistance;
                }

                //Debug.LogError("CurrentDragDistance: " + this.currentDragDistance);
            }

            this.dragDirection = (newMousePosition - this.clickStartingPosition).normalized;
            this.percentageOfMaxDragDistance = this.currentDragDistance / this.maxDragDistance;
            this.percentageOfMaxYDistance = Mathf.Abs((newMousePosition - this.clickStartingPosition).y) / this.maxYDistance;
            this.percentageOfMaxXDistance = Mathf.Abs((newMousePosition - this.clickStartingPosition).x) / this.maxXDistance;

            //Debug.LogError("Percentage: " + this.percentageOfMaxYDistance);
        }

        this.dragDirection = Vector3.zero;
        this.percentageOfMaxDragDistance = 0.0f;
        this.percentageOfMaxYDistance = 0.0f;
        this.percentageOfMaxXDistance = 0.0f;
        this.clickStartingPosition = Vector3.zero;
        this.currentDragDistance = 0;

        this.dragCoroutine = null;
    }
}
