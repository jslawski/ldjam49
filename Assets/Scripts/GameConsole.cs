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
   
    public static float maxRotation = 20f;

    [SerializeField]
    private InputManager currentInput;

    private Vector3 centerPoint;

    private float minXViewport;
    private float maxXViewport;
    private float minYViewport;
    private float maxYViewport;

    private Vector3 velocity = Vector3.zero;
    private float angleVelocity = 0;
    public float damp = 5f;

    private float smoothTime = 0.1f;
    private Vector3 tempVel = Vector3.zero;

    private LevelManipulator levelGeometry;

    private float rotateSpeed = 10f;

    [SerializeField]
    private MeshRenderer gameScreen;
    [SerializeField]
    private Material cutsceneRenderTexture;
    [SerializeField]
    private Material gameplayRenderTexture;

    private bool cutsceneFinished = false;

    [SerializeField]
    private Sprite[] consoleSprites;
    [SerializeField]
    private SpriteRenderer currentSprite;

    //Cutscene stuff
    private Coroutine transitionCoroutine;
    private float timeBeforeZoomOut = 4.0f;
    private float zoomOutSpeed = 3.0f;
    private float cutsceneCameraSize = 4.4f;
    private float gameplayCameraSize = 6.0f;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        this.maxXViewport = 0.5f + this.maxXDiff;
        this.maxYViewport = 0.5f + this.maxYDiff;
        this.centerPoint = this.frameCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, this.frameCamera.nearClipPlane));

        if (GameManager.instance.currentState == GameState.Cutscene)
        {
            this.gameScreen.material = this.cutsceneRenderTexture;
            this.frameCamera.orthographicSize = this.cutsceneCameraSize;
            StartCoroutine(this.TransitionFromCutscene());
        }
    }

    public void ReloadLevel()
    {
        StartCoroutine(this.LoadGameScene());
    }

    private IEnumerator TransitionFromCutscene()
    {
        yield return new WaitForSeconds(this.timeBeforeZoomOut);

        while (Mathf.Abs(this.frameCamera.orthographicSize - this.gameplayCameraSize) > 0.01f)
        {
            this.frameCamera.orthographicSize = Mathf.Lerp(
                this.frameCamera.orthographicSize, 
                this.gameplayCameraSize, 
                this.zoomOutSpeed * Time.deltaTime);

            yield return null;
        }

        this.frameCamera.orthographicSize = this.gameplayCameraSize;
        StartCoroutine(this.LoadGameScene());
        yield return null;
    }

    private IEnumerator LoadGameScene()
    {
        SceneManager.LoadScene(this.currentInput.sceneToLoadName, LoadSceneMode.Additive);

        while (SceneManager.sceneCount < 2)
        {
            Debug.LogError("Iterate");
            yield return null;
        }

        yield return null;

        this.levelGeometry = GameObject.Find("LevelPivot").GetComponent<LevelManipulator>();
        CameraFollow.instance.playerCharacter = GameObject.Find("Table");

        CameraFollow.instance.transform.position = new Vector3(
            CameraFollow.instance.playerCharacter.transform.position.x,
            CameraFollow.instance.playerCharacter.transform.position.y,
            CameraFollow.instance.transform.position.z);

        CameraFollow.instance.playerRb = CameraFollow.instance.playerCharacter.GetComponent<Rigidbody>();

        this.gameScreen.material = this.gameplayRenderTexture;
    }

    private void UpdateVisualState()
    {
        this.currentSprite.sprite = this.consoleSprites[0];

        if (this.currentInput.objectHovered == "ConsoleScreen")
        {
            this.currentSprite.sprite = this.consoleSprites[3];
        }

        if (this.currentInput.objectHovered == "TiltButton")
        {
            if (this.currentInput.tiltButtonHovered == TiltButton.Left)
            {
                this.currentSprite.sprite = this.consoleSprites[1];
            }
            else
            {
                this.currentSprite.sprite = this.consoleSprites[5];
            }
        }

        if (this.currentInput.objectUIClicked == "ConsoleScreen")
        {
            this.currentSprite.sprite = this.consoleSprites[4];
        }

        if (this.currentInput.objectUIClicked == "TiltButton")
        {
            if (this.currentInput.tiltButtonHovered == TiltButton.Left)
            {
                this.currentSprite.sprite = this.consoleSprites[2];
            }
            else
            {
                this.currentSprite.sprite = this.consoleSprites[6];
            }
        }
    }

    private void Update()
    {
        if (GameManager.instance.currentState == GameState.MainGame)
        {
            this.UpdateVisualState();
        }
    }

    void FixedUpdate()
    {
        if (GameManager.instance.currentState != GameState.MainGame && 
            GameManager.instance.currentState != GameState.GameOver)
        {
            return;
        }

        else if (this.currentInput.objectClicked == "TiltButton")
        {
            this.RotateConsole();
        }
        else
        {
            this.TranslateConsole();
            this.ResetRotation();
        }
    }

    private void TranslateConsole()
    {
        if (this.levelGeometry == null)
        {
            return;
        }

        Vector3 moveDirection = new Vector3(this.maxXDiff * this.currentInput.dragDirection.x,
            this.maxYDiff * this.currentInput.dragDirection.y, this.frameCamera.nearClipPlane) + new Vector3(0.5f, 0.5f, 0f);

        moveDirection = this.frameCamera.ViewportToWorldPoint(moveDirection);

        Vector3 targetPosition = Vector3.LerpUnclamped(this.centerPoint, moveDirection,
            this.currentInput.percentageOfMaxDragDistance);

        targetPosition = new Vector3(targetPosition.x, targetPosition.y, this.transform.position.z);

        this.velocity += 10f * (targetPosition - this.transform.position);
        this.velocity -= (this.velocity * this.damp * Time.fixedDeltaTime);

        this.transform.position += (this.velocity * Time.fixedDeltaTime);

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

        targetAngle = Mathf.Lerp(0, maxRotation, this.currentInput.percentageOfMaxYDistance) * tiltDirection;

        //Debug.LogError("Target Angle: " + targetAngle);
 
        Vector3 targetRotation = new Vector3(this.transform.rotation.x,
            this.transform.rotation.y,
            this.transform.rotation.z + targetAngle);

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, 
            Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z), 
            this.rotateSpeed * Time.fixedDeltaTime);

        this.levelGeometry.RotateLevel(targetRotation);
    }

    private void ResetRotation()
    {
        if (this.levelGeometry == null)
        {
            return;
        }
        
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
            this.rotateSpeed * Time.fixedDeltaTime);

        this.levelGeometry.RotateLevel(targetRotation);
    }
}
