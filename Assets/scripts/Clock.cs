using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class Clock : MonoBehaviour {

    public Transform hoursTransform, minutesTransform;
   
    public int startingMinute;
    public int startingHour;
    [Range(1.1f,100f)]
    public int millisecondRatio = 10;
    public int hourToAlert = -1;
    public int hoursToEndOn = -1;
    public bool paused;
    public bool clockEnabled = false;
    public AudioClip timesUpWarningClip;
    public AudioClip timesUpClip;

    private int minutes;
    private int hours;
    private int millisecondCounter;
    private bool canIncreaseHour = false;
    private int lastMillisecond;
    private bool playTimesUpWarning;
    private bool playedTimesUpWarning;
    private bool playedTimesUp;
    private bool playTimesUp;
    private AudioSource audioSource;

    private const float
        hoursToDegrees = 360f / 12f,
        minutesToDegrees = 360f / 60f;

    void Start()
    {
        minutes = startingMinute;
        hours = startingHour;
        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        if (!clockEnabled)
            return;
        if (paused)
            return;
        DateTime time = DateTime.Now;
        if (millisecondCounter != time.Millisecond)
        {
            millisecondCounter = time.Millisecond;
            lastMillisecond += 1;
        }
        if (lastMillisecond == millisecondRatio)
        {
            lastMillisecond = 0;
            minutes++;
            canIncreaseHour = true;

        }
        if (minutes % 60 == 0 && canIncreaseHour)
        {
            minutes = 0;
            canIncreaseHour = false;
            hours++;
        }
        if (hours == 12)
        {
            hours = 0;
        }

        if (hours == hourToAlert && playedTimesUpWarning == false)
        {
            Debug.Log("Should be playing stuff");
            playTimesUpWarning = true;
            
        }
        if (playTimesUpWarning) {
            playedTimesUpWarning = true;
            playTimesUpWarning = false;
            audioSource.clip = timesUpWarningClip;
            audioSource.Play();
        }
        if (hours == hoursToEndOn && playedTimesUp == false)
        {
            playTimesUp = true;
        }

        if (playTimesUp)
        {
            Debug.Log("times up!");
            playedTimesUp = true;
            playTimesUp = false;
            StartCoroutine(playAlarmSound());
            endDate();
        }
       
        hoursTransform.localRotation = Quaternion.Euler(0f, 0f, hours * -hoursToDegrees);
        minutesTransform.localRotation = Quaternion.Euler(0f, 0f, minutes * -minutesToDegrees);
    }

    IEnumerator playAlarmSound()
    {
        audioSource.clip = timesUpClip;
        audioSource.Play();
        paused = true;
        yield return new WaitForSeconds(audioSource.clip.length);
        
    }

    private void endDate()
    {
        SceneManager.LoadScene(1);
    }

    public void updateTime(int minutes)
    {
        Debug.Log("in update time with " + minutes);
        this.minutes += minutes;
        if (this.minutes >= 60 )
        {
            this.minutes = 0;
            lastMillisecond = 0;
            canIncreaseHour = false;
            hours++;
        }
    }
}
