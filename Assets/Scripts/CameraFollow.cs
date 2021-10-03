/*using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;

    public GameObject playerCharacter;
    public Rigidbody playerRb;

    private Camera thisCamera;
    private Transform cameraTransform;
    private float cameraDistance;

    private float verticalViewportThreshold = 0.3f;
    private float horizontalViewportThreshold = 0.3f;

    void Awake()
    {
        CameraFollow.instance = this;
        this.thisCamera = this.gameObject.GetComponentInChildren<Camera>();
        this.cameraTransform = this.gameObject.transform;
        this.cameraDistance = this.cameraTransform.position.y;
    }

    private bool IsPlayerPastHorizontalThreshold(float playerViewportXPosition)
    {
        return (playerViewportXPosition > (1.0f - this.horizontalViewportThreshold)) ||
            (playerViewportXPosition < (0.0f + this.horizontalViewportThreshold));
    }

    void LateUpdate()
    {
        if (this.playerCharacter == null)
        {
            return;
        }

        Vector3 playerViewportPosition = thisCamera.WorldToViewportPoint(this.playerCharacter.gameObject.transform.position);

        if (this.playerCharacter != null && playerViewportPosition.y > this.verticalViewportThreshold)
        {
            this.UpdateCameraVerticalPosition();
        }

        if (this.IsPlayerPastHorizontalThreshold(playerViewportPosition.x))
        {
            this.UpdateCameraHorizontalPosition();
        }
    }

    private void UpdateCameraVerticalPosition()
    {
        if (this.playerRb.velocity.y <= 0)
        {
            return;
        }

        Vector3 worldSpaceCenteredPosition = this.thisCamera.ViewportToWorldPoint(new Vector3(0.5f, this.verticalViewportThreshold, this.cameraDistance));

        Vector3 shiftVector = new Vector3(0, this.playerCharacter.transform.position.y - worldSpaceCenteredPosition.y, 0);

        //if (this.playerCharacter.oldWayEngaged == true)
        //{
        //    this.cameraTransform.Translate(shiftVector.normalized * this.playerCharacter.playerVelocity.magnitude * Time.deltaTime);
        //}
        //else
        //{
        //    this.cameraTransform.Translate(shiftVector.normalized * this.playerCharacter.Body.velocity.magnitude * Time.deltaTime);
        //}

        this.cameraTransform.Translate(shiftVector.normalized * this.playerRb.velocity.y * Time.deltaTime);
    }

    private void UpdateCameraHorizontalPosition()
    {
        Vector3 worldSpaceCenteredPosition = this.thisCamera.ViewportToWorldPoint(new Vector3(0.5f, this.verticalViewportThreshold, this.cameraDistance));

        Vector3 shiftVector = new Vector3(this.playerCharacter.transform.position.x - worldSpaceCenteredPosition.x, 0, 0);

        this.cameraTransform.Translate(shiftVector.normalized * Mathf.Abs(this.playerRb.velocity.x) * Time.deltaTime);
    }
}
*/


using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;

    public GameObject playerCharacter;
    public Rigidbody playerRb;

    private Camera thisCamera;
    private Transform cameraTransform;
    private float cameraDistance;

    public float verticalViewportThreshold = 0.5f;
    public float horizontalViewportThreshold = 0.5f;

    void Awake()
    {
        instance = this;
        this.thisCamera = this.gameObject.GetComponentInChildren<Camera>();
        this.cameraTransform = this.gameObject.transform;
        this.cameraDistance = this.cameraTransform.position.y;
    }

    private bool IsPlayerPastHorizontalThreshold(float playerViewportXPosition)
    {
        return (playerViewportXPosition >= (1.0f - this.horizontalViewportThreshold)) ||
            (playerViewportXPosition <= (0.0f + this.horizontalViewportThreshold));
    }

    private bool IsPlayerPastVerticalThreshold(float playerViewportYPosition)
    {
        return (playerViewportYPosition >= (1.0f - this.verticalViewportThreshold)) ||
            (playerViewportYPosition <= (0.0f + this.verticalViewportThreshold));
    }

    void FixedUpdate()
    {
        if (this.playerCharacter == null)
        {
            return;
        }

        Vector3 playerViewportPosition = thisCamera.WorldToViewportPoint(this.playerCharacter.gameObject.transform.position);

        if (this.playerCharacter != null && this.IsPlayerPastVerticalThreshold(playerViewportPosition.y))
        {
            this.UpdateCameraVerticalPosition();
        }
        
        if (this.IsPlayerPastHorizontalThreshold(playerViewportPosition.x))
        {
            this.UpdateCameraHorizontalPosition();
        }
    }

    private void UpdateCameraVerticalPosition()
    {
        Vector3 worldSpaceCenteredPosition = this.thisCamera.ViewportToWorldPoint(new Vector3(0.5f, this.verticalViewportThreshold, this.cameraDistance));

        Vector3 shiftVector = new Vector3(0, this.playerCharacter.transform.position.y - worldSpaceCenteredPosition.y, 0);

        this.cameraTransform.Translate(shiftVector.normalized * Mathf.Abs(this.playerRb.velocity.y) * Time.deltaTime);
    }

    private void UpdateCameraHorizontalPosition()
    {
        Vector3 worldSpaceCenteredPosition = this.thisCamera.ViewportToWorldPoint(new Vector3(0.5f, this.verticalViewportThreshold, this.cameraDistance));

        Vector3 shiftVector = new Vector3(this.playerCharacter.transform.position.x - worldSpaceCenteredPosition.x, 0, 0);

        this.cameraTransform.Translate(shiftVector.normalized * Mathf.Abs(this.playerRb.velocity.x) * Time.deltaTime);
    }
}
