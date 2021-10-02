using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Fix the distance, it's way to big!  Why?

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera frameCamera;
    private Vector3 clickStartingPosition;
    private float currentDragDistance = 0;
    [SerializeField]
    private LayerMask clickDetectableMask;

    private Coroutine dragCoroutine = null;

    private float minDragDistance = 0.5f;
    private float maxDragDistance = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) == true)
        {
            Debug.LogError("Click Detected!");
            this.CastRay();
        }
    }

    /*private void OnMouseDown()
    {
        Debug.LogError("Click Detected!");

        if (this.dragCoroutine == null)
        {
            this.clickStartingPosition = Input.mousePosition;
            this.dragCoroutine = StartCoroutine(this.UpdateDrag());
        }
    }*/

    private void CastRay()
    {
        Ray ray = this.frameCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Debug.LogError("Casting Ray");

        if (Physics.Raycast(ray, out hit, this.clickDetectableMask))
        {
            Debug.LogError("Ray hit");

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
        while (Input.GetMouseButton(0))
        {
            Debug.LogError("Dragging");

            Vector3 initialMousePosition = Input.mousePosition;
            yield return new WaitForSeconds(0.1f);
            Vector3 newMousePosition = Input.mousePosition;

            if (this.GetDistance(initialMousePosition, newMousePosition) >= this.minDragDistance)
            {
                this.currentDragDistance = Vector3.Distance(this.clickStartingPosition, newMousePosition);

                Debug.LogError("CurrentDragDistance: " + this.currentDragDistance);

                if (this.currentDragDistance > this.maxDragDistance)
                {
                    this.currentDragDistance = this.maxDragDistance;
                }

                //Debug.LogError("CurrentDragDistance: " + this.currentDragDistance);
            }

            Vector3 terminatingPoint = this.clickStartingPosition +
                (((newMousePosition - this.clickStartingPosition).normalized) * this.maxDragDistance);

            Debug.DrawLine(this.frameCamera.ScreenToWorldPoint(this.clickStartingPosition), this.frameCamera.ScreenToWorldPoint(terminatingPoint), Color.red, 0.1f);
        }

        this.dragCoroutine = null;
    }
}
