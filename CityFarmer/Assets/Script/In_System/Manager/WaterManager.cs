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

    private void OnApplicationFocus(bool focus)
    {
      
        if (focus)
        {
            DateTime lastTime = InfoManager.Instance.Water.LastDateWater;
            _timeSpan = DateTime.Now - lastTime;
            if ((int)_timeSpan.TotalSeconds + _water.CurrentWater < _water.MaxWater)
            {
                _water.CurrentWater += (int)_timeSpan.TotalSeconds;
            }
            else
            {
                _water.CurrentWater = _water.MaxWater;
            }
        }
        InfoManager.Instance.Water.LastDateWater = DateTime.Now;
        InfoManager.Instance.UpdateSQL(InfoManager.Instance.WaterUpdateString());
    }
    private void OnApplicationQuit()
    {
        InfoManager.Instance.Water.LastDateWater = DateTime.Now;
        InfoManager.Instance.UpdateSQL(InfoManager.Instance.WaterUpdateString());
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
