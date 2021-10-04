using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public delegate void TimerEndedHandler();
    public event TimerEndedHandler OnTimerEnded;

    public float TimerValueInSeconds { get; private set; }
    public bool IsPaused = true;

    [SerializeField]
    private Text TimerText;

    void Update()
    {
        if (IsPaused == true)
        {
            UpdateTimerText();
            return;
        }

        TimerValueInSeconds += Time.deltaTime;

        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        string minutes = Mathf.Floor(TimerValueInSeconds / 60).ToString("0");
        string seconds = Mathf.Floor(TimerValueInSeconds % 60).ToString("00");
        TimerText.text = string.Format("{0}:{1}", minutes, seconds);
    }

    public void Pause()
    {
        IsPaused = true;
    }

    public void SetTime(float intervalTimerValueInSeconds)
    {
        TimerValueInSeconds = intervalTimerValueInSeconds;
    }

    public void StartTimer(float initialTimerValueInSeconds)
    {
        TimerValueInSeconds = initialTimerValueInSeconds;
        IsPaused = false;
    }

    public void ReduceTimerBy(float seconds)
    {
        TimerValueInSeconds -= seconds;
    }
}