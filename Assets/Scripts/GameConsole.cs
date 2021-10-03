using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameConsole : MonoBehaviour
{
    Vector3 pointToMoveTo;
    public Camera frameCamera;
    [SerializeField]
    private float maxYDiff = 0.18f;
    [SerializeField]
    private float maxXDiff = 0.08f;
    [SerializeField]
    private float maxRotation = 20f;

    [SerializeField]
    private InputManager currentInput;

    private Vector3 centerPoint;

    private float minXViewport;
    private float maxXViewport;
    private float minYViewport;
    private float maxYViewport;

    public float moveSpeed = 0.5f;

    private Vector3 velocity = Vector3.zero;
    private float angleVelocity = 0;
    public float damp = 5f;

    private float smoothTime = 0.1f;
    private Vector3 tempVel = Vector3.zero;

    private LevelManipulator levelGeometry;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(this.LoadGameScene());

        this.maxXViewport = 0.5f + this.maxXDiff;
        this.maxYViewport = 0.5f + this.maxYDiff;
        this.centerPoint = this.frameCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, this.frameCamera.nearClipPlane));
    }

    private IEnumerator LoadGameScene()
    {
        SceneManager.LoadScene("GameplayScene", LoadSceneMode.Additive);

        while (SceneManager.sceneCount < 2)
        {
            Debug.LogError("Iterate");
            yield return null;
        }

        yield return null;

        //GameObject test = GameObject.FindGameObjectWithTag("LevelParent");

        this.levelGeometry = GameObject.Find("LevelPivot").GetComponent<LevelManipulator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.currentInput.objectClicked == "ConsoleScreen")
        {
            this.TranslateConsole();
            this.ResetRotation();
        }
        else if (this.currentInput.objectClicked == "TiltButton")
        {
            this.RotateConsole();
        }
    }

    private void TranslateConsole()
    {
        Vector3 moveDirection = new Vector3(this.maxXDiff * this.currentInput.dragDirection.x,
            this.maxYDiff * this.currentInput.dragDirection.y, this.frameCamera.nearClipPlane) + new Vector3(0.5f, 0.5f, 0f);

        moveDirection = this.frameCamera.ViewportToWorldPoint(moveDirection);

        Vector3 targetPosition = Vector3.LerpUnclamped(this.centerPoint, moveDirection,
            this.currentInput.percentageOfMaxDragDistance);

        targetPosition = new Vector3(targetPosition.x, targetPosition.y, this.transform.position.z);

        this.velocity += targetPosition - this.transform.position;
        this.velocity -= (this.velocity * this.damp * Time.deltaTime);

        this.transform.position += (this.velocity * Time.deltaTime);

        this.levelGeometry.TranslateLevel(this.velocity);
    }

    private void RotateConsole()
    {
        float tiltDirection = (int)this.currentInput.tiltButtonClicked * this.currentInput.dragDirection.y;

        float targetAngle = 0.0f;

        if (tiltDirection > 0)
        {
            tiltDirection = 1;
        }
        else
        {
            tiltDirection = -1;
        }

        targetAngle = Mathf.Lerp(0, this.maxRotation, this.currentInput.percentageOfMaxYDistance) * tiltDirection;

        //Debug.LogError("Target Angle: " + targetAngle);

        Vector3 targetRotation = new Vector3(this.transform.rotation.x,
            this.transform.rotation.y,
            this.transform.rotation.z + targetAngle);

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, 
            Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z), 
            10.0f * Time.deltaTime);

        /*if (Mathf.Abs(this.transform.rotation.eulerAngles.z - targetRotation.z) < 0.01f)
        {
            this.transform.rotation = Quaternion.Euler(targetRotation.x, targetRotation.y, 0f);
           
            if (this.levelGeometry.levelPivotObject != null)
            {
                Debug.LogError("TRIGGERED");
                this.levelGeometry.RotateLevel(targetRotation);
                this.levelGeometry.DestroyPivot();
            }
            
            return;
        }*/

        this.levelGeometry.RotateLevel(targetRotation);
    }

    private void ResetRotation()
    {
        /*if (this.transform.rotation.eulerAngles.z <= 0.001f)
        {
            return;
        }*/

        float tiltDirection = this.transform.rotation.eulerAngles.z;

        float targetAngle = 0.0f;

        if (tiltDirection > 0)
        {
            tiltDirection = 1;
        }
        else
        {
            tiltDirection = -1;
        }

        Vector3 targetRotation = new Vector3(this.transform.rotation.x,
            this.transform.rotation.y,
            this.transform.rotation.z + targetAngle);

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
            Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z),
            10.0f * Time.deltaTime);

        /*if (Mathf.Abs(this.transform.rotation.eulerAngles.z - targetRotation.z) < 0.01f)
        {
            this.transform.rotation = Quaternion.Euler(targetRotation.x, targetRotation.y, 0f);

            if (this.levelGeometry.levelPivotObject != null)
            {
                Debug.LogError("TRIGGERED IN RESET");
                this.levelGeometry.RotateLevel(targetRotation);
                this.levelGeometry.DestroyPivot();
            }

            return;
        }
        */
        this.levelGeometry.RotateLevel(targetRotation);
    }
}
