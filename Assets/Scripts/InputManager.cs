using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField]
    private LineRenderer debugLine;

    public Vector3 dragDirection;
    public float percentageOfMaxDragDistance = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) == true)
        {
            this.CastRay();
        }
    }

    private void CastRay()
    {
        Ray ray = this.frameCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, this.clickDetectableMask))
        {
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
            yield return new WaitForSeconds(0.1f);
            Vector3 newMousePosition = this.frameCamera.ScreenToViewportPoint(Input.mousePosition);

            //Debug.LogError("Initial: " + initialMousePosition + " New: " + newMousePosition);

            if (this.GetDistance(initialMousePosition, newMousePosition) >= this.minDragDistance)
            {
                this.currentDragDistance = Vector3.Distance(this.clickStartingPosition, newMousePosition);

                //Debug.LogError("CurrentDragDistance: " + this.currentDragDistance);

                if (this.currentDragDistance > this.maxDragDistance)
                {
                    this.currentDragDistance = this.maxDragDistance;
                }

                Debug.LogError("CurrentDragDistance: " + this.currentDragDistance);
            }

            this.dragDirection = (newMousePosition - this.clickStartingPosition).normalized;
            this.percentageOfMaxDragDistance = this.currentDragDistance / this.maxDragDistance;
            //Vector3 terminatingPoint = this.clickStartingPosition +(this.dragDirection * this.currentDragDistance);

            //this.debugLine.SetPosition(0, Camera.main.ViewportToWorldPoint(this.clickStartingPosition));
            //this.debugLine.SetPosition(1, Camera.main.ViewportToWorldPoint(terminatingPoint));
        }

        this.dragDirection = Vector3.zero;
        this.percentageOfMaxDragDistance = 0.0f;
        this.clickStartingPosition = Vector3.zero;
        this.currentDragDistance = 0;

        this.dragCoroutine = null;
    }
}
