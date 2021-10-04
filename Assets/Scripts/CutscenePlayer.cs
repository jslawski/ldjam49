using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutscenePlayer : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer cutscenePlayer;

    private bool hasStarted = false;

    private void Awake()
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "introCutscene.mp4");
        this.cutscenePlayer.url = filePath;

        this.cutscenePlayer.renderMode = VideoRenderMode.RenderTexture;
        this.cutscenePlayer.targetCameraAlpha = 1.0f;
        this.cutscenePlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.cutscenePlayer.isPlaying && this.hasStarted == false)
        {
            this.hasStarted = true;
        }

        if (!this.cutscenePlayer.isPlaying && this.hasStarted == true)
        {
            GameManager.instance.currentState = GameState.Title;
            this.gameObject.SetActive(false);
        }
    }
}
