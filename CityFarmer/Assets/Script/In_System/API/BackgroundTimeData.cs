using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BackgroundTimeData : MonoBehaviour
{
    private TimeSpan _timeSpan { get; set; }

    private void OnApplicationPause(bool pause)
    {
       if(pause)
        {
            PlayerPrefs.SetString("LastTime", Convert.ToString(DateTime.Now));
        }
        if (!pause)
        {
            DateTime lastTime = Convert.ToDateTime(PlayerPrefs.GetString("LastTime"));
            _timeSpan = DateTime.Now - lastTime;
        }
    }
}
