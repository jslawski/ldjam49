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

    public float verticalViewportThreshold = 0.1f;
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

    void LateUpdate()
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
