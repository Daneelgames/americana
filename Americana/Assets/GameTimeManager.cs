using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager instance;
    public enum TimeType
    {
        american, world
    }

    public TimeType timeType = TimeType.world;

    public int currentHour = 12;
    public int currentMinutes = 0;
    public int currentDay = 1;

    public TextMeshProUGUI timeFeedbackField;
    public Animator timeFeedbackAnim;
    private string updateString = "Update";

    void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        StartCoroutine(TimeCount());
    }

    IEnumerator TimeCount()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            AddMinute();
        }
    }

    void AddMinute()
    {
        currentMinutes++;
        if (currentMinutes >= 60)
        {
            currentMinutes = 0;
            AddHour();
        }
        else
        {
            // update text and anim here
            UpdateTimeCounter();
        }
    }
    public void AddHour()
    {
        currentHour++;
        if (currentHour >= 24)
        {
            currentHour = 0;
            AddDay();
        }
        else
        {
            // update text and anim here
            timeFeedbackAnim.SetTrigger(updateString);
            UpdateTimeCounter();
        }
    }

    void AddDay()
    {
        currentDay++;
        
        // update text and anim here
        timeFeedbackAnim.SetTrigger(updateString);
        UpdateTimeCounter();
    }

    void UpdateTimeCounter()
    {
        string timeString = currentDay + ". ";
        if (currentHour < 10)
            timeString += "0" + currentHour + ":";
        else
            timeString += currentHour + ":";
        
        if (currentMinutes < 10)
            timeString += "0" + currentMinutes;
        else
            timeString += currentMinutes;

        timeFeedbackField.text = timeString;
    }
}
