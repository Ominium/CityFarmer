using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class WaterManager : MonoBehaviour
{
    private TimeSpan _timeSpan { get; set; }
    private Water _water;
    private float _deltatime = 0.0f;

    private void Start()
    {
        _water = InfoManager.Instance.Water;
    }

    private void OnApplicationFocus(bool pause)
    {
      
        if (!pause)
        {
            PlayerPrefs.SetString("LastTime", Convert.ToString(DateTime.Now));
        }
        if (pause)
        {
            DateTime lastTime = Convert.ToDateTime(PlayerPrefs.GetString("LastTime"));
            _timeSpan = DateTime.Now - lastTime;
            if((int)_timeSpan.TotalSeconds + _water.CurrentWater < _water.MaxWater)
            {
                _water.CurrentWater += (int)_timeSpan.TotalSeconds;
            }
            else
            {
                _water.CurrentWater = _water.MaxWater;
            }
            
        }
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("LastTime", Convert.ToString(DateTime.Now));
    }
    private void Update()
    {
        if(_water.CurrentWater + 1 < _water.MaxWater)
        {
            _deltatime += Time.deltaTime;
            if (_deltatime >= 1.0f)
            {
                _water.CurrentWater++;
                _deltatime = 0.0f;
            }
        }       
    }
}
