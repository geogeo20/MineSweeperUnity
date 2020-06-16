using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private const string TIME_1 = "0{0}:0{1}";
    private const string TIME_2 = "0{0}:{1}";
    private const string TIME_3 = "{0}:0{1}";
    private const string TIME_4 = "{0}:{1}";
    private int currentTime = -1;
    private int min;
    private int sec;

    [SerializeField]
    private TextMeshProUGUI timerText = null;
    private MasterController masterController;

    private void Start()
    {
        masterController = MasterController.Instace;
    }

    private void Update()
    {
        UpdateTimer((int)masterController.Timer);
    }

    public void UpdateTimer(int newTime)
    {
        if(newTime != currentTime)
        {
            min = newTime / 60;
            sec = newTime % 60;

            if(min > 9)
            {
                if(sec > 9)
                {
                    timerText.text = string.Format(TIME_4, min, sec);
                }
                else
                {
                    timerText.text = string.Format(TIME_3, min, sec);
                }
            }
            else
            {
                if (sec > 9)
                {
                    timerText.text = string.Format(TIME_2, min, sec);
                }
                else
                {
                    timerText.text = string.Format(TIME_1, min, sec);
                }
            }
            currentTime = newTime;
        }
    }
}
