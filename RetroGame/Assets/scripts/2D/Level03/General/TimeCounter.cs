using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class TimeCounter : MonoBehaviour
{

    [SerializeField] private TMP_Text txtTime;
    public float timer;

    void Start()
    {
        InvokeRepeating("DecreaseTime", 0f, 1f);
    }

    private void DecreaseTime()
    {
        if (timer < 0) return;

        if(timer > 0f)
        {
            timer--;
        }

        else
        {
            timer = 0f;
        }

        DisplayTime(timer);

        //if(timer <= 0)
        //{
            
        //    //RoundSingleton.Instance.

        //}
    }

    private void DisplayTime(float timeToDisplay)
    {
        if(timeToDisplay < 0f)
        {
            timeToDisplay = 0f;
        }

         float seconds = Mathf.FloorToInt(timeToDisplay);

        txtTime.text = seconds.ToString();
    }
}
