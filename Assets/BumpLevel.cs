using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpLevel : MonoBehaviour
{
    private Vector3 targetPosition;
    private Vector3 originalPosition;

    public Rigidbody rb;

    public float moveSpeed = 0.01f;

    private void Start()
    {
        this.originalPosition = this.rb.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            StartCoroutine(this.Bump());
        }
    }

    private IEnumerator Bump()
    {
        this.targetPosition = new Vector3(this.originalPosition.x + 10, this.originalPosition.y, this.originalPosition.z);

        while (Mathf.Abs(this.rb.position.x - this.targetPosition.x) > 0.5)
        {
            this.rb.MovePosition(Vector3.Lerp(this.rb.position, this.targetPosition, this.moveSpeed));
            yield return new WaitForFixedUpdate();
        }

        this.rb.position = this.targetPosition;
        this.targetPosition = this.originalPosition;

        while (Mathf.Abs(this.rb.position.x - this.targetPosition.x) > 0.5)
        {
            this.rb.MovePosition(Vector3.Lerp(this.rb.position, this.targetPosition, this.moveSpeed));
            yield return new WaitForFixedUpdate();
        }

        this.rb.position = this.originalPosition;
    }

    private void ResetPosition()
    {
        this.targetPosition = this.originalPosition;
    }
}
